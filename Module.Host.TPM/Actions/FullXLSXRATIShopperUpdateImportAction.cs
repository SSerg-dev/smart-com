using Core.Data;
using Core.Dependency;
using Core.Extensions;
using Core.Settings;
using DocumentFormat.OpenXml;
using Interfaces.Implementation.Action;
using Interfaces.Implementation.Import.FullImport;
using Looper.Parameters;
using Module.Host.TPM.Util;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using NLog;
using Persist;
using Persist.Model;
using Persist.Model.Import;
using Persist.ScriptGenerator;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utility;
using Utility.FileWorker;
using Utility.Import;
using Utility.Import.Cache;
using Utility.Import.ImportModelBuilder;
using Utility.Import.ModelBuilder;

namespace Module.Host.TPM.Actions
{

    /// <summary>
    /// Переопределение Action из ядра приложения
    /// </summary>
    public class FullXLSXRATIShopperUpdateImportAction : BaseAction {

        public FullXLSXRATIShopperUpdateImportAction(FullImportSettings settings, string importDestination) {
            UserId = settings.UserId;
            RoleId = settings.RoleId;
            ImportFile = settings.ImportFile;
            ImportType = settings.ImportType;
            ModelType = settings.ModelType;
            Separator = settings.Separator;
            Quote = settings.Quote;
            HasHeader = settings.HasHeader;
            ImportDestination = importDestination;

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
        protected readonly int Year;
        protected readonly string ImportDestination;

        protected bool AllowPartialApply { get; set; }

        protected string ResultStatus { get; set; }
        protected bool HasErrors { get; set; }

        protected readonly static Logger logger = LogManager.GetCurrentClassLogger();



        /// <summary>
        /// Выполнить разбор source-данных в импорт-модели и сохранить в БД
        /// </summary>
        public override void Execute() {
            logger.Trace("Begin");
            try {
                ResultStatus = null;
                HasErrors = false;

                IList<IEntity<Guid>> sourceRecords = ParseImportFile();

                int successCount;
                int warningCount;
                int errorCount;
                ImportResultFilesModel resultFilesModel = ApplyImport(sourceRecords, out successCount, out warningCount, out errorCount);

                if (HasErrors) {
                    Fail();
                } else {
                    Success();
                }

                // Сохранить выходные параметры
                Results["ImportSourceRecordCount"] = sourceRecords.Count();
                Results["ImportResultRecordCount"] = successCount;
                Results["ErrorCount"] = errorCount;
                Results["WarningCount"] = warningCount;
                Results["ImportResultFilesModel"] = resultFilesModel;

            } catch (Exception e) {
                HasErrors = true;
                string msg = String.Format("FullImportAction failed: {0}", e.ToString());
                logger.Error(msg);
                string message;
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
                Complete();
            }
        }

        /// <summary>
        /// Выполнить разбор файла импорта
        /// </summary>
        /// <returns></returns>
        private IList<IEntity<Guid>> ParseImportFile() {
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
            IList<IEntity<Guid>> records = ImportUtilityTPM.ParseXLSXFile(importFilePath, null, builder, validator, Separator, Quote, HasHeader, out sourceRecordCount, out errors, out buildErrors, out validateErrors);
            logger.Trace("after parse file");

            // Обработать ошибки
            foreach (string err in errors) {
                Errors.Add(err);
            }
            if (errors.Any()) {
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
        private ImportResultFilesModel ApplyImport(IList<IEntity<Guid>> sourceRecords, out int successCount, out int warningCount, out int errorCount) {

            // Логика переноса данных из временной таблицы в постоянную
            // Получить записи текущего импорта
            using (DatabaseContext context = new DatabaseContext()) {

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

                DateTime dtNow = DateTime.Now;

                //Ограничение пользователя  
                IList<Constraint> constraints = context.Constraints
                    .Where(x => x.UserRole.UserId.Equals(UserId) && x.UserRole.Role.Id.Equals(RoleId))
                    .ToList();
                IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
                IQueryable<ClientTree> ctQuery = context.Set<ClientTree>().AsNoTracking().Where(x => DateTime.Compare(x.StartDate, dtNow) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dtNow) > 0));
                IQueryable<ClientTreeHierarchyView> hierarchy = context.Set<ClientTreeHierarchyView>().AsNoTracking();
                ctQuery = ModuleApplyFilterHelper.ApplyFilter(ctQuery, hierarchy, filters);

                //Запрос действующих ObjectId
                IList<ClientTree> existedClientTrees = ctQuery.Where(y => y.EndDate == null || y.EndDate > dtNow).ToList();
                IList<Tuple<int, int>> existedClientTreesTuples = existedClientTrees.Select(y => new Tuple<int, int>(y.Id, y.ObjectId)).ToList();
                IList<int> existedexistedClientTreesIds = existedClientTreesTuples.Select(y => y.Item2).ToList();

                //Присваивание ID
                Parallel.ForEach(sourceRecords, item => {
                    int objId = ((ImportRATIShopper) item).ClientTreeObjectId;
                    if (existedexistedClientTreesIds.Contains(objId)) {
                        var finden = existedClientTreesTuples.FirstOrDefault(y => y.Item2 == objId);
                        if (finden != null) {
                            ((ImportRATIShopper) item).ClientTreeId = finden.Item1;
                        }
                    }
                });

                IList<Tuple<int, int?>> existedRATIShoppersTimes =
                    this.GetQuery(context).Where(x => !x.Disabled).Select(y => new Tuple<int, int?>(y.ClientTreeId, y.Year)).ToList();

                IList<Tuple<int, int?>> importedRATIShoppersTimes =
                    sourceRecords.Select(y => new Tuple<int,int?>(((ImportRATIShopper) y).ClientTreeId, ((ImportRATIShopper) y).Year)).ToList();

                var importRATIShoppers = sourceRecords.Cast<ImportRATIShopper>();

                //Стандартные проверки
                Parallel.ForEach(sourceRecords, item => {
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
                    } else if (!IsFilterSuitable(rec, importRATIShoppers, existedexistedClientTreesIds, context, out validationErrors)) {
                        HasErrors = true;
                        errorRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", validationErrors)));
                    } else {
                        records.Add(rec);
                        if (warnings.Any()) {
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
                if (hasSuccessList) {
                    IEnumerable<IEntity<Guid>> items = BeforeInsert(records, context).ToList();
                    // Закончить импорт
                    resultRecordCount = InsertRATIShopperDataToDatabase(items, context);
                }
                if (!HasErrors)
                {
                    foreach (var importObj in sourceRecords)
                    {
                        successList.Add(importObj);
                    }

                    logger.Trace("Persist models inserted");
                    context.SaveChanges();
                    logger.Trace("Data saved");
                }

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

        protected ScriptGenerator _generator { get; set; }

        //Кастомная проверка
        protected virtual bool IsFilterSuitable(IEntity<Guid> rec, IEnumerable<ImportRATIShopper> importRATIShoppers, IList<int> existedObjIds, DatabaseContext context, out IList<string> errors) {
            errors = new List<string>();
            bool isError = false;

            ImportRATIShopper importObj = (ImportRATIShopper) rec;
            //Проверка по существующим активным ClientTree для пользователя
            if (!existedObjIds.Contains(importObj.ClientTreeObjectId)) {
                isError = true;
                errors.Add(importObj.ClientTreeObjectId.ToString() + " not in user's active ClientTree list");
            }

            //Проверка StartDate, EndDate
            if (importObj.Year == null ) {
                isError = true;
                errors.Add(" Year must be fullfilled");
            }


            var intersectDatesRATIShoppers = importRATIShoppers.Where(x => 
                importObj.ClientTreeObjectId == x.ClientTreeObjectId && importObj.Year == x.Year);

            if (intersectDatesRATIShoppers.Count() > 1)
            {
                isError = true;
                errors.Add($"({importObj.ClientTreeObjectId}, {importObj.Year}) there can not be two RATIShopper of client in same year.");
            }


            // LSV percent не больше 100 процентов
            if (importObj.RATIShopperPercent > 100 || importObj.RATIShopperPercent < 0) {
                isError = true;
                errors.Add("RA TI Shopper percent must be in percentage 0 up to 100");
            }

            return !isError;
        }

        protected virtual void Fail() {

        }

        protected virtual void Success() {

        }

        protected virtual void Complete() {

        }

        /// <summary>
        /// Запись в базу аналогично изменению RA TI Shopper из интерфейса через контекст
        /// </summary>
        /// <param name="sourceRecords"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected int InsertRATIShopperDataToDatabase(IEnumerable<IEntity<Guid>> sourceRecords, DatabaseContext context)
        {
            IList<RATIShopper> toCreate = new List<RATIShopper>();
            var query = GetQuery(context).ToList();

            var RATIShopperChangeIncidents = new List<ImportRATIShopper>();

            if (!HasErrors)
            {
                foreach (ImportRATIShopper newRecord in sourceRecords)
                {
                    RATIShopper toSave = new RATIShopper()
                    {
                        RATIShopperPercent = (float)Math.Round((decimal)newRecord.RATIShopperPercent, 2, MidpointRounding.AwayFromZero),
                        ClientTreeId = newRecord.ClientTreeId,
                        Year = newRecord.Year
                    };
                    var oldRecord = context.Set<RATIShopper>().SingleOrDefault(x => !x.Disabled && x.Year == newRecord.Year && x.ClientTreeId == newRecord.ClientTreeId);
                    if (oldRecord != null)
                    {
                        oldRecord.RATIShopperPercent = toSave.RATIShopperPercent;
                    }
                    else
                    {
                        toCreate.Add(toSave);
                    }
                    RATIShopperChangeIncidents.Add(newRecord);
                }

                foreach (IEnumerable<RATIShopper> items in toCreate.Partition(100))
                {
                    context.Set<RATIShopper>().AddRange(items);
                }

                // Необходимо выполнить перед созданием инцидентов.
                context.SaveChanges();
                foreach (var ratishopper in RATIShopperChangeIncidents)
                {
                    CreateRATIShopperChangeIncidents(ratishopper, context);
                }
            }

            context.SaveChanges();
            return sourceRecords.Count();
        }

        protected virtual IEnumerable<IEntity<Guid>> BeforeInsert(IEnumerable<IEntity<Guid>> records, DatabaseContext context) {
            return records;
        }

        private IEnumerable<RATIShopper> GetQuery(DatabaseContext context) {
            IQueryable<RATIShopper> query = context.Set<RATIShopper
                
                >().AsNoTracking();
            return query.ToList();
        }

        public void CreateRATIShopperChangeIncidents(ImportRATIShopper model, DatabaseContext context)
        {
            var settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
            var statusesSetting = settingsManager.GetSetting<string>("NOT_CHECK_PROMO_STATUS_LIST", "Draft,Cancelled,Deleted,Closed");
            var checkPromoStatuses = statusesSetting.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            var promoes = context.Set<Promo>()
                .Where(x => !x.Disabled && x.BudgetYear == model.Year && x.ClientTreeId == model.ClientTreeObjectId && x.ClientHierarchy == model.ClientTreeFullPath)
                .Where(x => !checkPromoStatuses.Contains(x.PromoStatus.Name))
                .Select(x => x.Id).ToList();

            foreach (var item in promoes)
            {
                var changesIncident = new ChangesIncident
                {
                    Id = Guid.NewGuid(),
                    DirectoryName = nameof(Promo) + nameof(RATIShopper),
                    ItemId = item.ToString(),
                    CreateDate = DateTimeOffset.Now,
                    Disabled = false
                };

                context.Set<ChangesIncident>().Add(changesIncident);
            }

            context.SaveChanges();
        }
    }
}