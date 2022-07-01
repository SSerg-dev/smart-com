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
    public class FullXLSXCOGSUpdateImportAction : BaseAction {

        public FullXLSXCOGSUpdateImportAction(FullImportSettings settings, int year, string importDestination) {
            UserId = settings.UserId;
            RoleId = settings.RoleId;
            ImportFile = settings.ImportFile;
            ImportType = settings.ImportType;
            ModelType = settings.ModelType;
            Separator = settings.Separator;
            Quote = settings.Quote;
            HasHeader = settings.HasHeader;
            Year = year;
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

                IList<BrandTech> brandTeches = context.Set<BrandTech>().Where(z => !z.Disabled).ToList();
                IList<Tuple<String, Guid?>> brandTechesTuples = brandTeches.Select(y => new Tuple<String, Guid?>(y.BrandsegTechsub, y.Id)).ToList();

                //Присваивание ID
                Parallel.ForEach(sourceRecords, item => {
                    int objId = ((ImportCOGS) item).ClientTreeObjectId;
                    String btName = ((ImportCOGS) item).BrandsegTechsub;
                    if (existedexistedClientTreesIds.Contains(objId)) {
                        var finden = existedClientTreesTuples.FirstOrDefault(y => y.Item2 == objId);
                        if (finden != null) {
                            ((ImportCOGS) item).ClientTreeId = finden.Item1;
                        }
                    }
                    var bt = brandTechesTuples.FirstOrDefault(y => y.Item1 == btName);
                    ((ImportCOGS) item).BrandTechId = bt == null ? null : bt.Item2;
                });

                IList<Tuple<int, Guid?, DateTimeOffset?, DateTimeOffset?>> existedCOGSsTimes =
                    this.GetQuery(context).Where(x => !x.Disabled).Select(y => new Tuple<int, Guid?, DateTimeOffset?, DateTimeOffset?>(y.ClientTreeId, y.BrandTechId, y.StartDate, y.EndDate)).ToList();

                IList<Tuple<int, Guid?, DateTimeOffset?, DateTimeOffset?>> importedCOGSsTimes =
                    sourceRecords.Select(y => new Tuple<int, Guid?, DateTimeOffset?, DateTimeOffset?>(((ImportCOGS) y).ClientTreeId, ((ImportCOGS) y).BrandTechId, ((ImportCOGS) y).StartDate, ((ImportCOGS) y).EndDate)).ToList();

                var importCOGSes = sourceRecords.Cast<ImportCOGS>().Where(x => x.StartDate.HasValue && x.EndDate.HasValue);

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
                    } else if (!IsFilterSuitable(rec, importCOGSes, existedexistedClientTreesIds, brandTechesTuples, context, out validationErrors)) {
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
                    // Закончить импорт
                    IEnumerable<IEntity<Guid>> items = BeforeInsert(records, context).ToList();
                    if (this.ImportDestination == "COGS")
                    {
                        resultRecordCount = InsertCOGSDataToDatabase(items, context);
                    }
                    else if (this.ImportDestination == "ActualCOGS")
                    {
                        resultRecordCount = InsertActualCOGSDataToDatabase(items, context);
                    }
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
        protected virtual bool IsFilterSuitable(IEntity<Guid> rec, IEnumerable<ImportCOGS> importCOGSes, IList<int> existedObjIds, IList<Tuple<String, Guid?>> brandTechesTuples, DatabaseContext context, out IList<string> errors) {
            errors = new List<string>();
            bool isError = false;

            ImportCOGS importObj = (ImportCOGS) rec;
            //Проверка по существующим активным ClientTree для пользователя
            if (!existedObjIds.Contains(importObj.ClientTreeObjectId)) {
                isError = true;
                errors.Add(importObj.ClientTreeObjectId.ToString() + " not in user's active ClientTree list");
            }

            //Проверка StartDate, EndDate
            if (importObj.StartDate == null || importObj.EndDate == null) {
                isError = true;
                errors.Add(" StartDate and EndDate must be fullfilled");
            } else {

                if (importObj.StartDate > importObj.EndDate) {
                    isError = true;
                    errors.Add(" StartDate must be before EndDate");
                }

            }

            if (importObj.StartDate.HasValue && importObj.StartDate.Value.Year != this.Year)
            {
                isError = true;
                errors.Add($"({importObj.ClientTreeObjectId}, {importObj.BrandsegTechsub}) Start Date year must be equal {this.Year}.");
            }

            if (importObj.EndDate.HasValue && importObj.EndDate.Value.Year != this.Year)
            {
                isError = true;
                errors.Add($"({importObj.ClientTreeObjectId}, {importObj.BrandsegTechsub}) End Date year must be equal {this.Year}.");
            }

            var intersectDatesCOGSes = importCOGSes.Where(x => 
                importObj.ClientTreeObjectId == x.ClientTreeObjectId && importObj.BrandsegTechsub == x.BrandsegTechsub && importObj.StartDate >= x.StartDate && importObj.StartDate <= x.EndDate || 
                importObj.ClientTreeObjectId == x.ClientTreeObjectId && importObj.BrandsegTechsub == x.BrandsegTechsub && importObj.EndDate >= x.StartDate && importObj.EndDate <= x.EndDate);

            if (intersectDatesCOGSes.Count() > 1)
            {
                isError = true;
                errors.Add($"({importObj.ClientTreeObjectId}, {importObj.BrandsegTechsub}) there can not be two COGS of client and BrandTech in some Time.");
            }

            ////Проверка BrandTech
            if (!String.IsNullOrEmpty(importObj.BrandsegTechsub)
                && !brandTechesTuples.Any(y => y.Item1 == importObj.BrandsegTechsub)) {
                isError = true;
                errors.Add(importObj.BrandsegTechsub + " is not active BrandTech's Name");
            }

            // LSV percent не больше 100 процентов
            if (importObj.LSVpercent > 100 || importObj.LSVpercent < 0) {
                isError = true;
                errors.Add("LSV percent must be in percentage 0 up to 100");
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
        /// Запись в базу аналогично изменению COGS из интерфейса через контекст
        /// </summary>
        /// <param name="sourceRecords"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected int InsertCOGSDataToDatabase(IEnumerable<IEntity<Guid>> sourceRecords, DatabaseContext context)
        {
            IList<COGS> toCreate = new List<COGS>();
            var query = GetQuery(context).ToList();

            var COGSChangeIncidents = new List<COGS>();

            var settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
            var statusesSetting = settingsManager.GetSetting<string>("NOT_CHECK_PROMO_STATUS_LIST", "Draft,Cancelled,Deleted,Closed");
            var notCheckPromoStatuses = statusesSetting.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            var promoes = context.Set<Promo>().Where(x => !x.Disabled && x.DispatchesStart.HasValue && x.DispatchesStart.Value.Year == this.Year)
                .Select(x => new PromoSimpleCOGS
                {
                    PromoStatusName = x.PromoStatus.Name,
                    Number = x.Number,
                    DispatchesStart = x.DispatchesStart,
                    DispatchesEnd = x.DispatchesEnd,
                    ClientTreeId = x.ClientTree.Id,
                    ClientTreeObjectId = x.ClientTreeId,
                    BrandTechId = x.BrandTechId
                })
                .ToList()
                .Where(x => !notCheckPromoStatuses.Contains(x.PromoStatusName));

            var importCOGS = sourceRecords.Cast<ImportCOGS>().Where(x => x.StartDate.HasValue && x.EndDate.HasValue);
            var allCOGSForCurrentYear = context.Set<COGS>().Where(x => !x.Disabled && x.Year == this.Year);
            var clientTrees = context.Set<ClientTree>().ToList();
            var brandTeches = context.Set<BrandTech>().ToList();

            foreach (var promo in promoes)
            {
                if (!importCOGS.Any(x => x.ClientTreeId == promo.ClientTreeId && (x.BrandTechId == null || x.BrandTechId == promo.BrandTechId) && x.StartDate <= promo.DispatchesStart && x.EndDate >= promo.DispatchesStart))
                {
                    bool existCOGS = false;

                    var clientNode = clientTrees.Where(x => x.ObjectId == promo.ClientTreeObjectId && !x.EndDate.HasValue).FirstOrDefault();
                    if (clientNode == null)
                    {
                        clientNode = clientTrees.Where(x => x.ObjectId == promo.ClientTreeObjectId).FirstOrDefault();
                    }
                    while (!existCOGS && clientNode != null && clientNode.Type != "root")
                    {
                        //промо может быть привязно к удаленному брендтеху(в более общем случае - к бредтеху с другим Id), поэтому сравнение приходится производить по Name, а не по Id
                        var promoBrandTechName = brandTeches.Where(bt => bt.Id == promo.BrandTechId).Select(x => x.BrandsegTechsub).FirstOrDefault();
                        var validBrandTeches = context.Set<BrandTech>().Where(x => x.BrandsegTechsub == promoBrandTechName);

                        existCOGS = importCOGS.Any(x => x.ClientTreeId == clientNode.Id
                                && (x.BrandTechId == null || validBrandTeches.Where(bt => bt.Id == x.BrandTechId).Any())
                                && x.StartDate <= promo.DispatchesStart
                                && x.EndDate >= promo.DispatchesStart);
                        if (!existCOGS)
                        {
                            existCOGS = importCOGS.Any(x => x.ClientTreeFullPath.Split(" > ".ToCharArray()).Last().Equals(clientNode.FullPathName.Split(" > ".ToCharArray()).Last())
                               && (x.BrandTechId == null || validBrandTeches.Where(bt => bt.Id == x.BrandTechId).Any())
                               && x.StartDate <= promo.DispatchesStart
                               && x.EndDate >= promo.DispatchesStart);
                        }
                        clientNode = clientTrees.Where(x => x.ObjectId == clientNode.parentId && !x.EndDate.HasValue).FirstOrDefault();
                        if (clientNode == null)
                        {
                            clientNode = clientTrees.Where(x => x.ObjectId == clientNode.parentId).FirstOrDefault();
                        }
                    }

                    if (!existCOGS)
                    {
                        HasErrors = true;
                        Errors.Add($"Not found interval for promo number {promo.Number}.");
                    }
                }
            }

            if (!HasErrors)
            {
                foreach (var item in allCOGSForCurrentYear)
                {
                    item.Disabled = true;
                    item.DeletedDate = DateTimeOffset.Now;
                }

                foreach (ImportCOGS newRecord in sourceRecords)
                {
                    BrandTech bt = context.Set<BrandTech>().FirstOrDefault(x => x.BrandsegTechsub == newRecord.BrandsegTechsub);
                    COGS toSave = new COGS()
                    {
                        StartDate = newRecord.StartDate,
                        EndDate = newRecord.EndDate,
                        LSVpercent = (float)Math.Round((decimal)newRecord.LSVpercent, 2, MidpointRounding.AwayFromZero),
                        ClientTreeId = newRecord.ClientTreeId,
                        BrandTechId = bt != null ? (Guid?)bt.Id : null,
                        Year = newRecord.StartDate.Value.Year
                    };
                    toCreate.Add(toSave);
                    COGSChangeIncidents.Add(toSave);
                }

                foreach (IEnumerable<COGS> items in toCreate.Partition(100))
                {
                    context.Set<COGS>().AddRange(items);
                }

                // Необходимо выполнить перед созданием инцидентов.
                context.SaveChanges();

                foreach (var cogs in COGSChangeIncidents)
                {
                    var currentCOGS = allCOGSForCurrentYear.FirstOrDefault(x => x.ClientTreeId == cogs.ClientTreeId && x.BrandTechId == cogs.BrandTechId && x.StartDate == cogs.StartDate && x.EndDate == cogs.EndDate && !x.Disabled);
                    if (currentCOGS != null)
                    {
                        context.Set<ChangesIncident>().Add(new ChangesIncident
                        {
                            Id = Guid.NewGuid(),
                            DirectoryName = nameof(COGS),
                            ItemId = currentCOGS.Id.ToString(),
                            CreateDate = DateTimeOffset.Now,
                            Disabled = false
                        });
                    }
                }
            }

            context.SaveChanges();
            return sourceRecords.Count();
        }

        /// <summary>
        /// Запись в базу аналогично изменению ActualCOGS из интерфейса через контекст
        /// </summary>
        /// <param name="sourceRecords"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected int InsertActualCOGSDataToDatabase(IEnumerable<IEntity<Guid>> sourceRecords, DatabaseContext context)
        {
            IList<ActualCOGS> toCreate = new List<ActualCOGS>();

            var settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
            var statusesSetting = settingsManager.GetSetting<string>("ACTUAL_COGSTI_CHECK_PROMO_STATUS_LIST", "Finished, Closed");
            var checkPromoStatuses = statusesSetting.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            var promoes = context.Set<Promo>().Where(x => !x.Disabled && x.DispatchesStart.HasValue && x.DispatchesStart.Value.Year == this.Year)
                .Select(x => new PromoSimpleCOGS
                {
                    PromoStatusName = x.PromoStatus.Name,
                    Number = x.Number,
                    DispatchesStart = x.DispatchesStart,
                    DispatchesEnd = x.DispatchesEnd,
                    ClientTreeId = x.ClientTree.Id,
                    ClientTreeObjectId = x.ClientTreeId,
                    BrandTechId = x.BrandTechId
                })
                .ToList()
                .Where(x => checkPromoStatuses.Contains(x.PromoStatusName));

            var importCOGS = sourceRecords.Cast<ImportCOGS>().Where(x => x.StartDate.HasValue && x.EndDate.HasValue);
            var allActualCOGSForYear = context.Set<ActualCOGS>().Where(x => !x.Disabled && x.Year == this.Year);
            var clientTrees = context.Set<ClientTree>().ToList();
            var brandTeches = context.Set<BrandTech>().ToList();

            foreach (var promo in promoes)
            {
                if (!importCOGS.Any(x => x.ClientTreeId == promo.ClientTreeId && (x.BrandTechId == null || x.BrandTechId == promo.BrandTechId) && x.StartDate <= promo.DispatchesStart && x.EndDate >= promo.DispatchesStart))
                {
                    bool existCOGS = false;

                    var clientNode = clientTrees.Where(x => x.ObjectId == promo.ClientTreeObjectId && !x.EndDate.HasValue).FirstOrDefault();
                    while (!existCOGS && clientNode != null && clientNode.Type != "root")
                    {
                        //промо может быть привязно к удаленному брендтеху(в более общем случае - к бредтеху с другим Id), поэтому сравнение приходится производить по Name, а не по Id
                        var promoBrandTechName = brandTeches.Where(bt => bt.Id == promo.BrandTechId).Select(x => x.BrandsegTechsub).FirstOrDefault();
                        var validBrandTeches = context.Set<BrandTech>().Where(x => x.BrandsegTechsub == promoBrandTechName);

                        existCOGS = importCOGS.Any(x => x.ClientTreeId == clientNode.Id
                                && (x.BrandTechId == null || validBrandTeches.Where(bt => bt.Id == x.BrandTechId).Any())
                                && x.StartDate <= promo.DispatchesStart
                                && x.EndDate >= promo.DispatchesStart);
                        clientNode = clientTrees.Where(x => x.ObjectId == clientNode.parentId && !x.EndDate.HasValue).FirstOrDefault();
                    }

                    if (!existCOGS)
                    {
                        HasErrors = true;
                        Errors.Add($"Not found interval for promo number {promo.Number}.");
                    }
                }
            }

            if (!HasErrors)
            {
                foreach (var item in allActualCOGSForYear)
                {
                    item.Disabled = true;
                    item.DeletedDate = DateTimeOffset.Now;
                }

                foreach (ImportCOGS newRecord in sourceRecords)
                {
                    BrandTech bt = context.Set<BrandTech>().FirstOrDefault(x => x.BrandsegTechsub == newRecord.BrandsegTechsub);
                    ActualCOGS toSave = new ActualCOGS()
                    {
                        StartDate = newRecord.StartDate,
                        EndDate = newRecord.EndDate,
                        LSVpercent = (float)Math.Round((decimal)newRecord.LSVpercent, 2, MidpointRounding.AwayFromZero),
                        ClientTreeId = newRecord.ClientTreeId,
                        BrandTechId = bt != null ? (Guid?)bt.Id : null,
                        Year = newRecord.StartDate.Value.Year,
                        IsCOGSIncidentCreated = false
                    };
                    toCreate.Add(toSave);
                }

                foreach (IEnumerable<ActualCOGS> items in toCreate.Partition(100))
                {
                    context.Set<ActualCOGS>().AddRange(items);
                }

                //инциденты при импорте ActualCOGS не создаются
            }

            context.SaveChanges();
            return sourceRecords.Count();
        }

        protected virtual IEnumerable<IEntity<Guid>> BeforeInsert(IEnumerable<IEntity<Guid>> records, DatabaseContext context) {
            return records;
        }

        private IEnumerable<COGS> GetQuery(DatabaseContext context) {
            IQueryable<COGS> query = context.Set<COGS>().AsNoTracking();
            return query.ToList();
        }
    }

    class PromoSimpleCOGS
    {
        public int? Number { get; set; }
        public DateTimeOffset? DispatchesStart { get; set; }
        public DateTimeOffset? DispatchesEnd { get; set; }
        public int? ClientTreeId { get; set; }
        public int? ClientTreeObjectId { get; set; }
        public Guid? BrandTechId { get; set; }
        public string BrandTechName { get; set; }
        public string PromoStatusName { get; set; }
    }
}