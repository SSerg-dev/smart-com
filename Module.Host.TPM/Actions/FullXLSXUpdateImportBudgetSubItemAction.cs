using Core.Data;
using Core.Extensions;
using Core.Settings;
using Interfaces.Implementation.Action;
using Interfaces.Implementation.Import.FullImport;
using Looper.Parameters;
using Module.Host.TPM.Util;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using NLog;
using Persist;
using Persist.ScriptGenerator;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utility.FileWorker;
using Utility.Import;

namespace Module.Host.TPM.Actions
{
    class FullXLSXUpdateImportBudgetSubItemAction : BaseAction {
        private readonly Guid UserId;
        private readonly Guid RoleId;
        private readonly FileModel ImportFile;
        private readonly Type ImportType;
        private readonly Type ModelType;
        private readonly string Separator;
        private readonly string Quote;
        private readonly bool HasHeader;


        private bool AllowPartialApply { get; set; }
        private readonly Logger logger;

        private string ResultStatus { get; set; }
        private bool HasErrors { get; set; }

        private ScriptGenerator Generator { get; set; }

        public FullXLSXUpdateImportBudgetSubItemAction(FullImportSettings settings) {
            UserId = settings.UserId;
            RoleId = settings.RoleId;
            ImportFile = settings.ImportFile;
            ImportType = settings.ImportType;
            ModelType = settings.ModelType;
            Separator = settings.Separator;
            Quote = settings.Quote;
            HasHeader = settings.HasHeader;

            AllowPartialApply = false;
            logger = LogManager.GetCurrentClassLogger();
        }

        public override void Execute() {
            logger.Trace("Begin");
            try {
                ResultStatus = null;
                HasErrors = false;

                var sourceRecords = ParseImportFile();

                int successCount;
                int warningCount;
                int errorCount;

                var resultFilesModel = ApplyImport(sourceRecords, out successCount, out warningCount, out errorCount);

                // Сохранить выходные параметры
                Results["ImportSourceRecordCount"] = sourceRecords.Count();
                Results["ImportResultRecordCount"] = successCount;
                Results["ErrorCount"] = errorCount;
                Results["WarningCount"] = warningCount;
                Results["ImportResultFilesModel"] = resultFilesModel;

            } catch (Exception e) {
                HasErrors = true;
                string message = String.Format("FullImportAction failed: {0}", e.ToString());
                logger.Error(message);

                if (e.IsUniqueConstraintException()) {
                    message = "This entry already exists in the database.";
                } else {
                    message = e.ToString();
                }

                Errors.Add(message);
                ResultStatus = ImportUtility.StatusName.ERROR;
            } finally {
                // информация о том, какой долен быть статус у задачи
                Results["ImportResultStatus"] = ResultStatus;
                logger.Debug("Finish");
            }
        }

        private IList<IEntity<Guid>> ParseImportFile() {
            var fileDispatcher = new FileDispatcher();
            string importDir = AppSettingsManager.GetSetting<string>("IMPORT_DIRECTORY", "ImportFiles");
            string importFilePath = Path.Combine(importDir, ImportFile.Name);
            if (!fileDispatcher.IsExists(importDir, ImportFile.Name))
            {
                throw new Exception("Import File not found");
            }

            var builder = ImportModelFactory.GetCSVImportModelBuilder(ImportType);
            var validator = ImportModelFactory.GetImportValidator(ImportType);
            int sourceRecordCount;
            List<string> errors;
            IList<Tuple<string, string>> buildErrors;
            IList<Tuple<IEntity<Guid>, string>> validateErrors;
            logger.Trace("before parse file");
            IList<IEntity<Guid>> records = ImportUtilityTPM.ParseXLSXFile(importFilePath, null, builder, validator, Separator, Quote, HasHeader, out sourceRecordCount, out errors, out buildErrors, out validateErrors);
            logger.Trace("after parse file");

            // Обработать ошибки
            foreach (string err in errors) {
                Errors.Add(err);
            }
            if (errors.Any()) {
                HasErrors = true;
                throw new ImportException("An error occurred while parsing the import file.");
            }

            return records;
        }

        private ImportResultFilesModel ApplyImport(IList<IEntity<Guid>> sourceRecords, out int successCount, out int warningCount, out int errorCount) {

            // Логика переноса данных из временной таблицы в постоянную
            // Получить записи текущего импорта
            using (DatabaseContext context = new DatabaseContext()) {

                var records = new ConcurrentBag<IEntity<Guid>>();
                var successList = new ConcurrentBag<IEntity<Guid>>();
                var errorRecords = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();
                var warningRecords = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();

                // Получить функцию Validate
                var validator = ImportModelFactory.GetImportValidator(ImportType);
                // Получить функцию SetProperty
                var builder = ImportModelFactory.GetModelBuilder(ImportType, ModelType);


                var cacheBuilder = ImportModelFactory.GetImportCacheBuilder(ImportType);
                var cache = cacheBuilder.Build(sourceRecords, context);


                foreach (var item in sourceRecords) {
                    ImportBudgetSubItem typedItem = (ImportBudgetSubItem) item;
                    Budget b = context.Set<Budget>().FirstOrDefault(y => y.Name == typedItem.BudgetName && !y.Disabled);
                    if (b != null) {
                        BudgetItem bi = context.Set<BudgetItem>().FirstOrDefault(y => y.Name == typedItem.BudgetItemName && y.BudgetId == b.Id && !y.Disabled);
                        typedItem.BudgetItemId = bi != null ? (Guid?) bi.Id : null;
                    } else {
                        typedItem.BudgetItemId = null;
                    }

                }


                Parallel.ForEach(sourceRecords, item =>
                {
                    IEntity<Guid> rec;
                    IList<string> warnings;
                    IList<string> validationErrors;

                    if (!validator.Validate(item, out validationErrors)) {
                        HasErrors = true;
                        errorRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", validationErrors)));
                    } else if (!builder.Build(item, cache, context, out rec, out warnings, out validationErrors)) {
                        HasErrors = true;
                        errorRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", validationErrors)));
                        if (warnings.Any()) {
                            warningRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", warnings)));
                        }
                    } else if (!IsFilterSuitable(rec, out validationErrors)) {
                        HasErrors = true;
                        errorRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", validationErrors)));
                    } else {
                        records.Add(rec);
                        successList.Add(item);
                        if (warnings.Any()) {
                            warningRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", warnings)));
                        }
                    }
                });

                logger.Trace("Persist models built");

                int resultRecordCount = 0;

                ResultStatus = GetImportStatus();
                var importModel = ImportUtility.BuildActiveImport(UserId, RoleId, ImportType);
                importModel.Status = ResultStatus;
                context.Imports.Add(importModel);

                bool hasSuccessList = AllowPartialApply || !HasErrors;
                if (hasSuccessList) {
                    // Закончить импорт
                    resultRecordCount = InsertDataToDatabase(records, context);
                }
                logger.Trace("Persist models inserted");
                context.SaveChanges();
                logger.Trace("Data saved");

                errorCount = errorRecords.Count;
                warningCount = warningRecords.Count;
                successCount = successList.Count;
                ImportResultFilesModel resultFilesModel = SaveProcessResultHelper.SaveResultToFile(
                    importModel.Id,
                    hasSuccessList ? successList : null,
                    null,
                    errorRecords,
                    warningRecords);

                if (errorCount > 0 || warningCount > 0)
                {
                    string errorsPath = "/api/File/ImportResultErrorDownload?filename=";
                    string warningsPath = "/api/File/ImportResultWarningDownload?filename=";

                    if (errorCount > 0)
                    {
                        foreach (var record in errorRecords)
                        {
                            Errors.Add($"{ record.Item2 } <a href=\"{ errorsPath + resultFilesModel.TaskId }\">Download</a>");
                        }
                    }
                    if (warningCount > 0)
                    {
                        foreach (var record in warningRecords)
                        {
                            Warnings.Add($"{ record.Item2 } <a href=\"{ warningsPath + resultFilesModel.TaskId }\">Download</a>");
                        }
                    }
                }

                return resultFilesModel;
            }
        }

        private bool IsFilterSuitable(IEntity<Guid> rec, out IList<string> errors) {
            errors = new List<string>();
            bool isSuitable = true;
            ImportBudgetSubItem typedRec = (ImportBudgetSubItem) rec;

            if (typedRec.BudgetItemId == null) {
                isSuitable = false;
                errors.Add("There is no such BudgetItem on base");
            }

            return isSuitable;
        }

        private int InsertDataToDatabase(IEnumerable<IEntity<Guid>> sourceRecords, DatabaseContext context) {
            ScriptGenerator generatorUpdate = new ScriptGenerator(typeof(BudgetSubItem));
            IList<BudgetSubItem> toCreate = new List<BudgetSubItem>();
            IList<BudgetSubItem> toUpdate = new List<BudgetSubItem>();
            foreach (ImportBudgetSubItem newRecord in sourceRecords) {
                BudgetSubItem oldRecord = context.Set<BudgetSubItem>()
                    .FirstOrDefault(bsi => bsi.BudgetItemId == newRecord.BudgetItemId && bsi.Name == newRecord.Name && !bsi.Disabled);

                if (oldRecord == null && newRecord.BudgetItemId.HasValue) {
                    BudgetSubItem toSave = new BudgetSubItem() {
                        BudgetItemId = newRecord.BudgetItemId.Value,
                        Name = newRecord.Name,
                        Description_ru = newRecord.Description_ru
                    };
                    toCreate.Add(toSave);
                }

                if (oldRecord != null && newRecord.BudgetItemId.HasValue)
                {
                    oldRecord.Description_ru = newRecord.Description_ru;
                    toUpdate.Add(oldRecord);
                }
            }

            foreach (IEnumerable<BudgetSubItem> items in toCreate.Partition(100)) {
                context.Set<BudgetSubItem>().AddRange(items);
            }
            foreach (IEnumerable<BudgetSubItem> items in toUpdate.Partition(100))
            {
                string insertScript = generatorUpdate.BuildUpdateScript(items);
                context.Database.ExecuteSqlCommand(insertScript);
            }
            context.SaveChanges();

            return sourceRecords.Count();
        }

        private ScriptGenerator GetScriptGenerator() {
            if (Generator == null) {
                Generator = new ScriptGenerator(ModelType);
            }
            return Generator;
        }

        private IEnumerable<BudgetSubItem> GetQuery(DatabaseContext context) {
            IQueryable<BudgetSubItem> query = context.Set<BudgetSubItem>().AsNoTracking();
            return query.ToList();
        }

        private string GetImportStatus() {
            if (HasErrors) {
                if (AllowPartialApply) {
                    return ImportUtility.StatusName.PARTIAL_COMPLETE;
                } else {
                    return ImportUtility.StatusName.ERROR;
                }
            } else {
                return ImportUtility.StatusName.COMPLETE;
            }
        }
    }
}