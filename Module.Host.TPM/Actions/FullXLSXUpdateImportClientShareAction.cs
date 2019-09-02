using Core.Data;
using Core.Extensions;
using Core.Settings;
using Interfaces.Implementation.Action;
using Interfaces.Implementation.Import.FullImport;
using Looper.Parameters;
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
                IQueryable<ClientTree> query = context.Set<ClientTree>().AsNoTracking().Where(x => DateTime.Compare(x.StartDate, dtNow) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dtNow) > 0));
                IQueryable<ClientTreeHierarchyView> hierarchy = context.Set<ClientTreeHierarchyView>().AsNoTracking();
                query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

                //Запрос действующих ObjectId
                IList<int> existedObjIds = query.Where(y => y.EndDate == null || y.EndDate > dtNow).Select(y => y.ObjectId).ToList();

                //Повторяющиеся записи
                IList<int> doubledObjIds = sourceRecords
                    .GroupBy(y => ((ImportClientsShare) y).BOI)
                    .Where(z => z.Count() > 1)
                    .Select(t => ((ImportClientsShare) t.FirstOrDefault()).BOI).ToList();

                //Стандартные проверки
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
                    } else if (!IsFilterSuitable(rec, existedObjIds, doubledObjIds, out validationErrors)) {
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

                ////Проверка по сумме
                IList<ImportClientsShare> verifiedList = records.Select(y => (ImportClientsShare) y).ToList();
                IList<int> changedCTObjIds = verifiedList.Select(y => y.BOI).ToList();
                IList<ClientTree> changedCTs = query
                    .Where(y => (y.EndDate == null || y.EndDate > dtNow) && changedCTObjIds.Contains(y.ObjectId))
                    .ToList();
                IList<int> parentIdsOfChangedCT = changedCTs
                    .Select(y => y.parentId).ToList();
                IEnumerable<IGrouping<int, ClientTree>> groups = query
                    .Where(y => (y.EndDate == null || y.EndDate > dtNow) && parentIdsOfChangedCT.Contains(y.parentId))
                    .GroupBy(y => y.parentId);
                var badGroups = groups
                    .Where(y => y
                        .Sum(z => changedCTObjIds.Contains(z.ObjectId)
                            ? verifiedList.FirstOrDefault(t => t.BOI == z.ObjectId).LeafShare
                            : z.Share) > 100 && y.Key != ROOT_CLIENT_OBJECTID);
                IList<int> badGroupsParentIds = badGroups.Select(g => g.Key).ToList();
                if (badGroups.Any()) {
                    HasErrors = true;
                    IList<int> badGroupsObjectIds = new List<int>();
                    foreach (var badGroup in badGroups) {
                        foreach (var item in badGroup) {
                            badGroupsObjectIds.Add(item.ObjectId);
                            var badItem = successList.FirstOrDefault(y => ((ImportClientsShare) y).BOI == item.ObjectId);
                            if (badItem != null) {
                                errorRecords.Add(new Tuple<IEntity<Guid>, string>(badItem, "parent " + item.parentId.ToString() + " can't consist more than 100 percents"));
                            }
                        }
                    }
                    records = new ConcurrentBag<IEntity<Guid>>(records.Where(y => !badGroupsObjectIds.Contains(((ImportClientsShare) y).BOI)));
                    successList = new ConcurrentBag<IEntity<Guid>>(successList.Where(y => !badGroupsObjectIds.Contains(((ImportClientsShare) y).BOI)));
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

        //Кастомная проверка ClientTree
        protected virtual bool IsFilterSuitable(IEntity<Guid> rec, IList<int> existedObjIds, IList<int> doubledObjIds, out IList<string> errors) {
            errors = new List<string>();
            bool isError = false;

            ImportClientsShare importObj = (ImportClientsShare) rec;
            //Проверка по существующим активным ClientTree для пользователя
            if (!existedObjIds.Contains(importObj.BOI)) {
                isError = true;
                errors.Add(importObj.BOI.ToString() + " not in user's active ClientTree list");
            }

            //Проверка дублирования записей
            if (doubledObjIds.Contains(importObj.BOI)) {
                isError = true;
                errors.Add(importObj.BOI.ToString() + " in list more than 1 time");
            }

            // Доля не больше 100 процентов
            if (importObj.LeafShare > 100 || importObj.LeafShare < 0) {
                isError = true;
                errors.Add("Share must be in percentage 0 up to 100");
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
        protected int InsertDataToDatabase(IEnumerable<IEntity<Guid>> sourceRecords, DatabaseContext context) {
            IList<ClientTree> toCreate = new List<ClientTree>();
            IList<ClientTree> toUpdate = new List<ClientTree>();
            var query = GetQuery(context).ToList();

            DateTime dtNow = DateTime.Now;
            foreach (ImportClientsShare newRecord in sourceRecords) {
                ClientTree oldRecord = query.FirstOrDefault(x => x.ObjectId == newRecord.BOI && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dtNow) > 0));
                if (oldRecord != null && (oldRecord.Share != newRecord.LeafShare || oldRecord.DemandCode != newRecord.DemandCode)) {
                    ClientTree oldRecordToSave = new ClientTree()
                    {
                        ObjectId = oldRecord.ObjectId,
                        RetailTypeName = oldRecord.RetailTypeName,
                        Type = oldRecord.Type,
                        Name = oldRecord.Name,
                        FullPathName = oldRecord.FullPathName,
                        Share = oldRecord.Share,
                        ExecutionCode = oldRecord.ExecutionCode,
                        DemandCode = oldRecord.DemandCode,
                        IsBaseClient = oldRecord.IsBaseClient,
                        parentId = oldRecord.parentId,
                        StartDate = oldRecord.StartDate,
                        EndDate = dtNow,
                        depth = oldRecord.depth,
                        IsBeforeStart = oldRecord.IsBeforeStart,
                        DaysStart = oldRecord.DaysStart,
                        IsDaysStart = oldRecord.IsDaysStart,
                        IsBeforeEnd = oldRecord.IsBeforeEnd,
                        DaysEnd = oldRecord.DaysEnd,
                        IsDaysEnd = oldRecord.IsDaysEnd
                    };
                    toCreate.Add(oldRecordToSave);

                    if (oldRecord.Share != newRecord.LeafShare)
                    {
                        ChangesIncident changesIncident = new ChangesIncident
                        {
                            Disabled = false,
                            DeletedDate = null,
                            DirectoryName = "ClientTree",
                            ItemId = oldRecord.Id.ToString(),
                            CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                            ProcessDate = null
                        };
                        context.Set<ChangesIncident>().Add(changesIncident);
                    }

                    oldRecord.Share = (short)newRecord.LeafShare;
                    oldRecord.DemandCode = newRecord.DemandCode;
                    toUpdate.Add(oldRecord);
                }
            }

            foreach (IEnumerable<ClientTree> items in toCreate.Partition(100)) {
                context.Set<ClientTree>().AddRange(items);
            }

            foreach (IEnumerable<ClientTree> items in toUpdate.Partition(10000)) {
                string insertScript = String.Join("", items.Select(y => String.Format("UPDATE ClientTree SET Share = {0}, DemandCode = '{1}' WHERE Id = {2};", y.Share, y.DemandCode, y.Id)));
                context.Database.ExecuteSqlCommand(insertScript);
            }
            context.SaveChanges();
            return sourceRecords.Count();
        }

        protected virtual IEnumerable<IEntity<Guid>> BeforeInsert(IEnumerable<IEntity<Guid>> records, DatabaseContext context) {
            return records;
        }

        private IEnumerable<ClientTree> GetQuery(DatabaseContext context) {
            IQueryable<ClientTree> query = context.Set<ClientTree>().AsNoTracking();
            return query.ToList();
        }

    }
}
