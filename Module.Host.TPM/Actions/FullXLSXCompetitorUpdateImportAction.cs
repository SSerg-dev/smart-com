using Core.Data;
using Core.Extensions;
using Core.History;
using Core.MarsCalendar;
using Core.Settings;
using Interfaces.Implementation.Action;
using Interfaces.Implementation.Import.FullImport;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Frontend.TPM.Controllers;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using NLog;
using Persist;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utility.Import;
using Utility.LogWriter;
using Utility.FileWorker;

namespace Module.Host.TPM.Actions
{
    class FullXLSXCompetitorUpdateImportAction : BaseAction
    {
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

        public FullXLSXCompetitorUpdateImportAction(FullImportSettings settings)
        {
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

        public override void Execute()
        {
            logger.Trace("Begin");
            try
            {
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

            }
            catch (Exception e)
            {
                HasErrors = true;
                string message = String.Format("FullImportAction failed: {0}", e.ToString());
                logger.Error(message);

                if (e.IsUniqueConstraintException())
                {
                    message = "This entry already exists in the database.";
                }
                else
                {
                    message = e.ToString();
                }

                Errors.Add(message);
                ResultStatus = ImportUtility.StatusName.ERROR;
            }
            finally
            {
                // информация о том, какой долен быть статус у задачи
                Results["ImportResultStatus"] = ResultStatus;
                logger.Debug("Finish");
            }
        }

        private IList<IEntity<Guid>> ParseImportFile()
        {
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

            IList<IEntity<Guid>> records = ImportUtility.ParseXLSXFile(importFilePath, null, builder, validator, Separator, Quote, HasHeader, out sourceRecordCount, out errors, out buildErrors, out validateErrors);

            logger.Trace("after parse file");

            // Обработать ошибки
            foreach (string err in errors)
            {
                Errors.Add(err);
            }
            if (errors.Any())
            {
                HasErrors = true;
                throw new ImportException("An error occurred while parsing the import file.");
            }

            return records;
        }

        private ImportResultFilesModel ApplyImport(IList<IEntity<Guid>> sourceRecords, out int successCount, out int warningCount, out int errorCount)
        {

            // Логика переноса данных из временной таблицы в постоянную
            // Получить записи текущего импорта
            using (DatabaseContext context = new DatabaseContext())
            {

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

                foreach (var item in sourceRecords)
                {
                    IEntity<Guid> rec;
                    IList<string> warnings;
                    IList<string> validationErrors;

                    if (!validator.Validate(item, out validationErrors))
                    {
                        HasErrors = true;
                        errorRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", validationErrors)));
                    }
                    else if (!builder.Build(item, cache, context, out rec, out warnings, out validationErrors))
                    {
                        HasErrors = true;
                        errorRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", validationErrors)));
                        if (warnings.Any())
                        {
                            warningRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", warnings)));
                        }
                    }
                    else if (!IsFilterSuitable(ref rec, context, out validationErrors))
                    {
                        HasErrors = true;
                        errorRecords.Add(new Tuple<IEntity<Guid>, string>(item, string.Join(", ", validationErrors)));
                    }
                    else
                    {
                        records.Add(rec);
                        successList.Add(item);
                        if (warnings.Any())
                        {
                            warningRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", warnings)));
                        }
                    }
                }

                logger.Trace("Persist models built");


                int resultRecordCount = 0;

                ResultStatus = GetImportStatus();
                var importModel = ImportUtility.BuildActiveImport(UserId, RoleId, ImportType);
                importModel.Status = ResultStatus;
                context.Imports.Add(importModel);

                bool hasSuccessList = AllowPartialApply || !HasErrors;
                if (hasSuccessList)
                {
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

        private bool IsFilterSuitable(ref IEntity<Guid> rec, DatabaseContext context, out IList<string> errors)
        {
            errors = new List<string>();
            bool isSuitable = true;
            int subBrandCode, techCode;


            if (rec == null)
            {
                isSuitable = false;
                errors.Add("There is no such CoefficientSI2SO on base");
            }
            else
            {
                Technology typedRec = (Technology)rec;
                if (String.IsNullOrEmpty(typedRec.Name)) 
                {
                    errors.Add("Name must have a value"); 
                    isSuitable = false; 
                }
                if ((String.IsNullOrEmpty(typedRec.SubBrand) && !String.IsNullOrEmpty(typedRec.SubBrand_code))
                    || !String.IsNullOrEmpty(typedRec.SubBrand) && String.IsNullOrEmpty(typedRec.SubBrand_code))
                {
                    errors.Add("Sub Brand and Sub Brand Code must have a value");
                    isSuitable = false;
                }
                else if (!String.IsNullOrEmpty(typedRec.SubBrand) && !int.TryParse(typedRec.SubBrand_code, out subBrandCode))
                {
                    errors.Add("Only numbers are allowed for Sub Brand Code");
                    isSuitable = false;
                }
                if (!String.IsNullOrEmpty(typedRec.Tech_code) && !int.TryParse(typedRec.Tech_code, out techCode))
                {
                    errors.Add("Only numbers are allowed for Tech Code");
                    isSuitable = false;
                }
                else if (String.IsNullOrEmpty(typedRec.Tech_code))
                {
                    errors.Add("Tech Code must have a value");
                    isSuitable = false;
                }
            }

            return isSuitable;
        }

        private int InsertDataToDatabase(IEnumerable<IEntity<Guid>> sourceRecords, DatabaseContext context)
        {
            NoGuidGeneratingScriptGenerator generatorCreate = new NoGuidGeneratingScriptGenerator(typeof(Technology), false);
            ScriptGenerator generatorUpdate = GetScriptGenerator();
            IList<Technology> toCreate = new List<Technology>();
            IList<Technology> toUpdate = new List<Technology>();
            IList<Technology> technologies = context.Set<Technology>().ToList();

            List<Tuple<IEntity<Guid>, IEntity<Guid>>> toHisCreate = new List<Tuple<IEntity<Guid>, IEntity<Guid>>>();
            List<Tuple<IEntity<Guid>, IEntity<Guid>>> toHisUpdate = new List<Tuple<IEntity<Guid>, IEntity<Guid>>>();

            foreach (Technology newRecord in sourceRecords)
            {
                newRecord.SubBrand_code = String.IsNullOrEmpty(newRecord.SubBrand_code) ? null : newRecord.SubBrand_code;
                Technology oldRecord = technologies
                    .FirstOrDefault(t
                        => (t.Tech_code == newRecord.Tech_code && t.SubBrand_code == newRecord.SubBrand_code && !t.Disabled));

                if (oldRecord == null)
                {
                    newRecord.Id = Guid.NewGuid();
                    toCreate.Add(newRecord);
                    toHisCreate.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(null, newRecord));
                }
                else
                {
                    if (oldRecord.Name != newRecord.Name || oldRecord.SubBrand != newRecord.SubBrand)
                    {
                        toHisUpdate.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(oldRecord, newRecord));

                        var oldTC = oldRecord.Tech_code;
                        var oldTN = oldRecord.Name;
                        var oldSC = oldRecord.SubBrand_code;
                        var oldName = oldRecord.Name;

                        oldRecord.Name = newRecord.Name;
                        oldRecord.Description_ru = newRecord.Description_ru;
                        oldRecord.SubBrand = newRecord.SubBrand;
                        
                        var newName = oldRecord.Name;
                        if (!String.IsNullOrEmpty(oldRecord.SubBrand))
                            newName = String.Format("{0} {1}", oldRecord.Name, oldRecord.SubBrand);

                        Task.Run(() => PromoHelper.UpdateProductHierarchy("Technology", newName, oldName, oldRecord.Id));
                        TechnologiesController.UpdateProductTrees(oldRecord.Id, newName);
                        TechnologiesController.UpdateProducts(oldRecord, oldTC, oldTN, oldSC);

                        toUpdate.Add(oldRecord);
                    } else if (oldRecord.Description_ru != newRecord.Description_ru)
                    {
                        toHisUpdate.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(oldRecord, newRecord));
                        oldRecord.Description_ru = newRecord.Description_ru;
                        toUpdate.Add(oldRecord);
                    }
                }
            }

            foreach (IEnumerable<IEntity<Guid>> items in toCreate.Partition(100))
            {
                string insertScript = generatorCreate.BuildInsertScript(items);
                context.ExecuteSqlCommand(insertScript);
            }

            foreach (IEnumerable<IEntity<Guid>> items in toUpdate.Partition(10000))
            {
                string updateScript = generatorUpdate.BuildUpdateScript(items);
                context.ExecuteSqlCommand(updateScript);
            }

            context.HistoryWriter.Write(toHisCreate, context.AuthManager.GetCurrentUser(), context.AuthManager.GetCurrentRole(), OperationType.Created);
            context.HistoryWriter.Write(toHisUpdate, context.AuthManager.GetCurrentUser(), context.AuthManager.GetCurrentRole(), OperationType.Updated);

            context.SaveChanges();

            return sourceRecords.Count();
        }

        private string GetImportStatus()
        {
            if (HasErrors)
            {
                if (AllowPartialApply)
                {
                    return ImportUtility.StatusName.PARTIAL_COMPLETE;
                }
                else
                {
                    return ImportUtility.StatusName.ERROR;
                }
            }
            else
            {
                return ImportUtility.StatusName.COMPLETE;
            }
        }

        private ScriptGenerator GetScriptGenerator()
        {
            if (Generator == null)
            {
                Generator = new ScriptGenerator(ModelType);
            }
            return Generator;
        }
    }
}
