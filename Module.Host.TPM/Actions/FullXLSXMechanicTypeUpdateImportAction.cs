using Core.Data;
using Core.Extensions;
using Core.Settings;
using DocumentFormat.OpenXml;
using Interfaces.Implementation.Action;
using Interfaces.Implementation.Import.FullImport;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using NLog;
using Persist;
using Persist.Model.Import;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utility.FileWorker;
using Utility.Import;
using Utility.Import.Cache;
using Utility.Import.ImportModelBuilder;
using Utility.Import.ModelBuilder;


namespace Module.Host.TPM.Actions
{
    class FullXLSXMechanicTypeUpdateImportAction : BaseAction
    {
        public FullXLSXMechanicTypeUpdateImportAction(FullImportSettings settings)
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
        }

        protected readonly Guid UserId;
        protected readonly Guid RoleId;
        protected readonly FileModel ImportFile;
        protected readonly Type ImportType;
        protected readonly Type ModelType;
        protected readonly string Separator;
        protected readonly string Quote;
        protected readonly bool HasHeader;

        protected bool AllowPartialApply { get; set; }

        protected string ResultStatus { get; set; }
        protected bool HasErrors { get; set; }
        private ScriptGenerator Generator { get; set; }

        protected readonly static Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Выполнить разбор source-данных в импорт-модели и сохранить в БД
        /// </summary>
        public override void Execute()
        {
            logger.Trace("Begin");
            try
            {
                ResultStatus = null;
                HasErrors = false;

                IList<IEntity<Guid>> sourceRecords = ParseImportFile();

                int successCount;
                int warningCount;
                int errorCount;
                ImportResultFilesModel resultFilesModel = ApplyImport(sourceRecords, out successCount, out warningCount, out errorCount);

                if (HasErrors)
                {
                    Fail();
                }
                else
                {
                    Success();
                }

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
                string msg = String.Format("FullImportAction failed: {0}", e.ToString());
                logger.Error(msg);
                string message;
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
                Complete();
            }
        }

        /// <summary>
        /// Выполнить разбор файла импорта
        /// </summary>
        /// <returns></returns>
        private IList<IEntity<Guid>> ParseImportFile()
        {
            var fileDispatcher = new FileDispatcher();
            string importDir = AppSettingsManager.GetSetting<string>("IMPORT_DIRECTORY", "ImportFiles");
            string importFilePath = Path.Combine(importDir, ImportFile.Name);
            if (!fileDispatcher.IsExists(importDir, ImportFile.Name))
            {
                throw new Exception("Import File not found");
            }

            IImportModelBuilder<string[]> builder = ImportModelFactory.GetCSVImportModelBuilder(ImportType);
            IImportValidator validator = ImportModelFactory.GetImportValidator(ImportType);
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
                throw new ImportException("An error occurred while loading the import file.");
            }

            return records;
        }

        /// <summary>
        /// Загрузить импортируемые записи в БД
        /// </summary>
        /// <param name="sourceRecords"></param>
        /// <param name="successCount"></param>
        /// <param name="warningCount"></param>
        /// <param name="errorCount"></param>
        /// <returns></returns>
        private ImportResultFilesModel ApplyImport(IList<IEntity<Guid>> sourceRecords, out int successCount, out int warningCount, out int errorCount)
        {

            // Логика переноса данных из временной таблицы в постоянную
            // Получить записи текущего импорта
            using (DatabaseContext context = new DatabaseContext())
            {

                ConcurrentBag<IEntity<Guid>> records = new ConcurrentBag<IEntity<Guid>>();
                ConcurrentBag<IEntity<Guid>> successList = new ConcurrentBag<IEntity<Guid>>();
                ConcurrentBag<Tuple<IEntity<Guid>, string>> errorRecords = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();
                ConcurrentBag<Tuple<IEntity<Guid>, string>> warningRecords = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();

                // Получить функцию Validate
                IImportValidator validator = ImportModelFactory.GetImportValidator(ImportType);
                // Получить функцию SetProperty
                IModelBuilder builder = ImportModelFactory.GetModelBuilder(ImportType, ModelType);
                IImportCacheBuilder cacheBuilder = ImportModelFactory.GetImportCacheBuilder(ImportType);
                IImportCache cache = cacheBuilder.Build(sourceRecords, context);

                //Стандартные проверки
                Parallel.ForEach(sourceRecords, item =>
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
                    else if (!IsFilterSuitable(rec, out validationErrors))
                    {
                        HasErrors = true;
                        errorRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", validationErrors)));
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
                });

                logger.Trace("Persist models built");

                int resultRecordCount = 0;

                ResultStatus = GetImportStatus();
                Import importModel = ImportUtility.BuildActiveImport(UserId, RoleId, ImportType);
                importModel.Status = ResultStatus;
                context.Imports.Add(importModel);

                bool hasSuccessList = AllowPartialApply || !HasErrors;
                if (hasSuccessList)
                {
                    // Закончить импорт
                    IEnumerable<IEntity<Guid>> items = BeforeInsert(records, context).ToList();
                    resultRecordCount = InsertDataToDatabase(items, context);
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

        //Кастомная проверка
        protected virtual bool IsFilterSuitable(IEntity<Guid> rec, out IList<string> errors)
        {
            errors = new List<string>();
            bool isError = false;

            ImportMechanicType importObj = (ImportMechanicType)rec;

            if (String.IsNullOrEmpty(importObj.Name)) 
            { 
                errors.Add("Name must have a value"); 
                isError = true; 
            }
            if (importObj.Discount < 0)
            { 
                errors.Add("Discount cannot be less than 0"); 
                isError = true; 
            }

            using (DatabaseContext context = new DatabaseContext())
            {               
                if (!String.IsNullOrEmpty(importObj.ClientTreeFullPathName))
                {
                    var baseClient = context.Set<ClientTree>().Where(x => x.FullPathName == importObj.ClientTreeFullPathName).FirstOrDefault();
                    if (baseClient == null) 
                    { 
                        errors.Add("Incorrect client"); 
                        isError = true; 
                    }
                    else if (!baseClient.IsBaseClient)
                    {
                        errors.Add("Client must be base");
                        isError = true;
                    }
                }

                double recDiscount = importObj.Discount.GetValueOrDefault();
                importObj.Discount = Math.Round(recDiscount, 2, MidpointRounding.AwayFromZero);
            }

            return !isError;
        }

        protected virtual void Fail()
        {

        }

        protected virtual void Success()
        {

        }

        protected virtual void Complete()
        {

        }

        /// <summary>
        /// Запись в базу аналогично изменению MechanicType из интерфейса через контекст
        /// </summary>
        /// <param name="sourceRecords"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected int InsertDataToDatabase(IEnumerable<IEntity<Guid>> sourceRecords, DatabaseContext context)
        {
            IList<MechanicType> toCreate = new List<MechanicType>();
            IList<MechanicType> toUpdate = new List<MechanicType>();
            IList<MechanicType> mechanicTypes = context.Set<MechanicType>().ToList();

            foreach (ImportMechanicType newRecord in sourceRecords)
            {
                var tree = context.Set<ClientTree>().Where(x => x.IsBaseClient && x.FullPathName == newRecord.ClientTreeFullPathName).FirstOrDefault();
                newRecord.ClientTreeId = tree?.Id;

                MechanicType oldRecord = mechanicTypes
                   .FirstOrDefault(c
                       => c.Name == newRecord.Name
                           && c.ClientTreeId == newRecord.ClientTreeId
                           && !c.Disabled);

                if (oldRecord == null)
                {
                    MechanicType toSave = new MechanicType()
                    {
                        Id = Guid.NewGuid(),
                        Disabled = false,
                        DeletedDate = null,
                        Name = newRecord.Name,
                        ClientTreeId = newRecord.ClientTreeId,
                        ClientTree = tree,
                        Discount = newRecord.Discount
                    };

                    toCreate.Add(toSave);
                }
                else if (oldRecord.Discount != newRecord.Discount)
                {
                    MechanicType toChange = new MechanicType()
                    {
                        Id = oldRecord.Id,
                        Name = oldRecord.Name,
                        ClientTreeId = oldRecord.ClientTreeId,
                        Discount = newRecord.Discount
                    };

                    toUpdate.Add(toChange);
                }
            }

            foreach (IEnumerable<MechanicType> items in toCreate.Partition(100))
            {
                context.Set<MechanicType>().AddRange(items);
            }

            foreach (IEnumerable<MechanicType> items in toUpdate.Partition(10000))
            {
                String formatStr = "UPDATE [DefaultSchemaSetting].[MechanicType] SET [Discount] = {1} WHERE [Id] ='{0}'";
                string updateScript = String.Join("\n", items.Select(y => String.Format(formatStr, y.Id, y.Discount)).ToList());

                context.ExecuteSqlCommand(updateScript);
            }

            context.SaveChanges();

            return sourceRecords.Count();
        }

        protected virtual IEnumerable<IEntity<Guid>> BeforeInsert(IEnumerable<IEntity<Guid>> records, DatabaseContext context)
        {
            return records;
        }

        private IEnumerable<MechanicType> GetQuery(DatabaseContext context)
        {
            IQueryable<MechanicType> query = context.Set<MechanicType>().AsNoTracking();
            return query.ToList();
        }
    }
}
