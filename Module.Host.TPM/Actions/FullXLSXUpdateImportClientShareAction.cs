using Core.Data;
using Core.Extensions;
using Core.Settings;
using Interfaces.Implementation.Action;
using Interfaces.Implementation.Import.FullImport;
using Looper.Parameters;
using Module.Frontend.TPM.Controllers;
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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utility;
using Utility.Import;
using Utility.Import.Cache;
using Utility.Import.ImportModelBuilder;
using Utility.Import.ModelBuilder;

namespace Module.Host.TPM.Actions {

    /// <summary>
    /// Переопределение Action из ядра приложения
    /// </summary>
    public class FullXLSXUpdateImportClientShareAction : BaseAction {

        public FullXLSXUpdateImportClientShareAction(FullImportSettings settings) {
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

        protected readonly static Logger logger = LogManager.GetCurrentClassLogger();

        const int ROOT_CLIENT_OBJECTID = 5000000;


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
            string importDir = AppSettingsManager.GetSetting<string>("IMPORT_DIRECTORY", "ImportFiles");
            string importFilePath = Path.Combine(importDir, ImportFile.Name);
            if (!File.Exists(importFilePath)) {
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
            foreach (string err in errors) {
                Errors.Add(err);
            }
            if (errors.Any()) {
                HasErrors = true;
                throw new ApplicationException("An error occurred while parsing the import file.");
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
                //здесь должны быть все записи, а не только неудаленные!
                IQueryable<ClientTree> query = context.Set<ClientTree>().AsNoTracking();
                IQueryable<ClientTreeHierarchyView> hierarchy = context.Set<ClientTreeHierarchyView>().AsNoTracking();
                IQueryable<ClientTreeBrandTech> clientTreeBrandTeches = context.Set<ClientTreeBrandTech>().AsNoTracking();
                query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

                var shortClientTreeBrandTech = clientTreeBrandTeches.Select(x => new ShortClientTreeBrandTech(x.ClientTree.ObjectId, x.ParentClientTreeDemandCode));

                IQueryable<ClientTree> actualQuery = context.Set<ClientTree>().AsNoTracking().Where(x => DateTime.Compare(x.StartDate, dtNow) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dtNow) > 0));
                actualQuery = ModuleApplyFilterHelper.ApplyFilter(actualQuery, hierarchy, filters);

                //Запрос ObjectId
                //здесь должны быть все записи, а не только неудаленные!
                List<ClientTree> existingClientTreeIds = query.ToList();

                //здесь должны быть все записи, а не только неудаленные!
                IList<string> existingBrandTechNames = context.Set<BrandTech>().Select(y => y.Name).ToList();

                List<ClientTree> actualExistingClientTreeIds = actualQuery.Where(y => y.EndDate == null || y.EndDate > dtNow).ToList();
                IList<string> actualExistingBrandTechNames = context.Set<BrandTech>().Where(y => y.Disabled == false).Select(y => y.Name).ToList();

                //Повторяющиеся записи
                IList<Tuple<int, string, string>> doubledObjs = sourceRecords
                    .GroupBy(y => new { ((ImportClientsShare)y).ClientTreeId, ((ImportClientsShare) y).DemandCode, ((ImportClientsShare) y).BrandTech })
                    .Where(z => z.Count() > 1)
                    .Select(t => new Tuple<int, string, string>(t.Key.ClientTreeId, t.Key.DemandCode, t.Key.BrandTech)).ToList();

                // Тоьлько актуальные записи.
                IList<ClientTreeBrandTech> changedCTBTs = ClientTreeBrandTechesController.GetActualQuery(context);

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
                    else if (!IsFilterSuitable(changedCTBTs, rec, existingClientTreeIds, shortClientTreeBrandTech, existingBrandTechNames, doubledObjs, out validationErrors))
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

                //Ломается проверка, если есть ошибки в записях
                if (!HasErrors)
                {
                    //Проверка по сумме
                    IList<ImportClientsShare> verifiedList = records.Select(y => (ImportClientsShare)y).ToList();
                    HashSet<string> changedCTBTObjIds = new HashSet<string>(verifiedList.Select(y => y.DemandCode));
                    HashSet<int> changedCTBTclientIds = new HashSet<int>(verifiedList.Select(y => y.ClientTreeId));
                    IList<string> parentDemandCodeOfChangedCTBTs = verifiedList.Select(y => y.DemandCode).ToList();
                    IList<int> clientTreeOfChangedCTBTs = verifiedList.Select(y => y.ClientTreeId).ToList();
                    IList<string> brandtechOfChangedCTBTs = verifiedList.Select(y => y.BrandTech).ToList();

                    var groups = changedCTBTs
                        .GroupBy(y => new { y.ParentClientTreeDemandCode, y.CurrentBrandTechName });
                    var badGroups = groups.Where(y => y
                        .Sum(z => (parentDemandCodeOfChangedCTBTs.Contains(z.ParentClientTreeDemandCode)
                        && clientTreeOfChangedCTBTs.Contains(z.ClientTree.ObjectId) && brandtechOfChangedCTBTs.Contains(z.CurrentBrandTechName))
                        ? Math.Round(verifiedList.FirstOrDefault(t =>
                        t.DemandCode == z.ParentClientTreeDemandCode && t.ClientTreeId == z.ClientTree.ObjectId && t.BrandTech == z.CurrentBrandTechName).LeafShare, 5, MidpointRounding.AwayFromZero)
                        : Math.Round(z.Share, 5, MidpointRounding.AwayFromZero)) > 100.0001);
                    if (badGroups.Any())
                    {
                        HasErrors = true;
                        IList<string> badGroupsObjectIds = new List<string>();
                        foreach (var badGroup in badGroups)
                        {
                            foreach (var item in badGroup)
                                if (item != null)
                                {
                                    badGroupsObjectIds.Add(item.ParentClientTreeDemandCode);
                                    var badItem = successList.FirstOrDefault(y => ((ImportClientsShare)y).DemandCode == item.ParentClientTreeDemandCode && ((ImportClientsShare)y).BrandTech == item.CurrentBrandTechName && ((ImportClientsShare)y).ClientTreeId == item.ClientTree.ObjectId);
                                    if (badItem != null)
                                    {
                                        errorRecords.Add(new Tuple<IEntity<Guid>, string>(badItem,
                                            "Clients with Demand Code " + item.ParentClientTreeDemandCode.ToString() + " and Brand Tech " + item.CurrentBrandTechName + " can't have more than 100 percents in total"));
                                    }
                                }
                        }
                        records = new ConcurrentBag<IEntity<Guid>>(records.Where(y => !badGroupsObjectIds.Contains(((ImportClientsShare)y).DemandCode)));
                        successList = new ConcurrentBag<IEntity<Guid>>(successList.Where(y => !badGroupsObjectIds.Contains(((ImportClientsShare)y).DemandCode)));
                    }
                }


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
                    resultRecordCount = InsertDataToDatabase(items, context, query);
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

        //Кастомная проверка ClientTreeBrandTech
        protected virtual bool IsFilterSuitable(IList<ClientTreeBrandTech> actualClientTreeBrandTeches, IEntity<Guid> rec, List<ClientTree> existingClientTreeIds, IQueryable<ShortClientTreeBrandTech> shortClientTreeBrandTech, IList<string> existingBrandTechNames, IList<Tuple<int, string, string>> doubledObjs, out IList<string> errors) {
            errors = new List<string>();
            bool isError = false;

            var importObj = rec as ImportClientsShare;

            if (importObj != null)
            {
                var testRecord = actualClientTreeBrandTeches.FirstOrDefault();

                var isActualRecord = actualClientTreeBrandTeches.Any(x => x.ClientTree != null && x.BrandTech != null && x.ClientTree.ObjectId == importObj.ClientTreeId &&
                    x.ParentClientTreeDemandCode == importObj.DemandCode && x.CurrentBrandTechName == importObj.BrandTech);

                if (!isActualRecord)
                {
                    isError = true;
                    errors.Add($"Record with Demand Code {importObj.DemandCode} and Brand Tech {importObj.BrandTech} is not actual.");
                }

                //Проверка дублирования записей
                if (doubledObjs.Contains(new Tuple<int, string, string>(importObj.ClientTreeId, importObj.DemandCode, importObj.BrandTech)))
                {
                    isError = true;
                    errors.Add(importObj.DemandCode.ToString() + " " + importObj.ClientTreeId.ToString() + " " + importObj.BrandTech.ToString() + " in list more than 1 time");
                }

                // Доля не больше 100 процентов
                if (importObj.LeafShare > 100 || importObj.LeafShare < 0)
                {
                    isError = true;
                    errors.Add("Share must be in percentage 0 up to 100");
                }
            }
            else
            {
                isError = true;
                errors.Add("Import model is null.");
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
        /// Запись в базу аналогично изменению ClientTree  из интерфейса
        /// </summary>
        /// <param name="sourceRecords"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected int InsertDataToDatabase(IEnumerable<IEntity<Guid>> sourceRecords, DatabaseContext context, IEnumerable<ClientTree> clientTrees) {
            IList<ClientTreeBrandTech> toUpdate = new List<ClientTreeBrandTech>();
            var query = GetQuery(context).ToList();
            var brandtechs = context.Set<BrandTech>().Where(x => !x.Disabled).ToList();

            DateTime dtNow = DateTime.Now;
            foreach (ImportClientsShare newRecord in sourceRecords) {
                ClientTreeBrandTech oldRecord = query.FirstOrDefault(x => !x.Disabled && x.ClientTree.ObjectId == newRecord.ClientTreeId && x.ParentClientTreeDemandCode == newRecord.DemandCode && x.CurrentBrandTechName == newRecord.BrandTech);
                if (oldRecord != null)
                {
                    if (oldRecord.Share != newRecord.LeafShare)
                    {
                        ChangesIncident changesIncident = new ChangesIncident
                        {
                            Disabled = false,
                            DeletedDate = null,
                            DirectoryName = "ClientTreeBrandTech",
                            ItemId = oldRecord.Id.ToString(),
                            CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                            ProcessDate = null
                        };
                        context.Set<ChangesIncident>().Add(changesIncident);
                    }

                    oldRecord.Share = newRecord.LeafShare;
                    oldRecord.ParentClientTreeDemandCode = newRecord.DemandCode;
                    oldRecord.ClientTreeId = clientTrees.First(x => x.ObjectId == newRecord.ClientTreeId).Id;
                    oldRecord.BrandTechId = brandtechs.First(x => x.Name == newRecord.BrandTech).Id;
                    oldRecord.CurrentBrandTechName = newRecord.BrandTech;
                    toUpdate.Add(oldRecord);
                }
            }

            foreach (IEnumerable<ClientTreeBrandTech> items in toUpdate.Partition(10000)) {
                string insertScript = String.Join("", items.Select(y => String.Format("UPDATE ClientTreeBrandTech SET Share = {0}, ParentClientTreeDemandCode = '{1}', ClientTreeId = {2}, BrandTechId = '{3}' WHERE Id = '{4}';", 
                    y.Share, y.ParentClientTreeDemandCode, y.ClientTreeId, y.BrandTechId, y.Id)));
                context.Database.ExecuteSqlCommand(insertScript);
            }
            context.SaveChanges();
            return sourceRecords.Count();
        }

        protected virtual IEnumerable<IEntity<Guid>> BeforeInsert(IEnumerable<IEntity<Guid>> records, DatabaseContext context) {
            return records;
        }

        private IEnumerable<ClientTreeBrandTech> GetQuery(DatabaseContext context) {
            IQueryable<ClientTreeBrandTech> query = context.Set<ClientTreeBrandTech>();
            return query.ToList();
        }

        public class ShortClientTreeBrandTech
        {
            public int ClientTreeId { get; set; }
            public string DemandCode { get; set; }
            public ShortClientTreeBrandTech(int clientTreeId, string demandCode)
            {
                ClientTreeId = clientTreeId;
                DemandCode = demandCode;
            }
        }
    }
}
