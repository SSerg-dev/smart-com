using Castle.Core.Internal;
using Core.Data;
using Core.Extensions;
using Core.Settings;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
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
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
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
    public class FullXLSXNoNegoUpdateImportAction : BaseAction {

        public FullXLSXNoNegoUpdateImportAction(FullImportSettings settings) {
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
                IList<ClientTree> existedClientTrees = ctQuery.Where(y => (y.EndDate == null || y.EndDate > dtNow) && y.depth != 0).ToList();
                IList<Tuple<int, int>> existedClientTreesTuples = existedClientTrees.Select(y => new Tuple<int, int>(y.Id, y.ObjectId)).ToList();
                IList<int> existedClientTreesIds = existedClientTreesTuples.Select(y => y.Item2).ToList();

                IQueryable<ProductTree> ptQuery = context.Set<ProductTree>().AsNoTracking().Where(x => DateTime.Compare(x.StartDate, dtNow) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dtNow) > 0));
                IList<ProductTree> existedProductTrees = ptQuery.Where(y => y.EndDate == null || y.EndDate > dtNow).ToList();
                IList<Tuple<int, int>> existedProductTreesTuples = existedProductTrees.Select(y => new Tuple<int, int>(y.Id, y.ObjectId)).ToList();
                IList<int> existedProductTreesIds = existedProductTreesTuples.Select(y => y.Item2).ToList();

                //Запрос механик
                IList<Mechanic> mechQuery = context.Set<Mechanic>().AsNoTracking().Where(y => !y.Disabled).ToList();
                IList<Tuple<String, Guid>> mechanicTuples = mechQuery.Select(y => new Tuple<String, Guid>(y.Name, y.Id)).ToList();

                IList<MechanicType> mechTypeQuery = context.Set<MechanicType>().AsNoTracking().Where(y => !y.Disabled).ToList();
                IList<Tuple<String, Guid, double?>> mechanicTypeTuples = mechTypeQuery.Select(y => new Tuple<String, Guid, double?>(y.Name, y.Id, y.Discount)).ToList();


                //Присваивание ID
                Parallel.ForEach(sourceRecords, item => {
                    ImportNoNego typedItem = (ImportNoNego) item;

                    int clientObjId = typedItem.ClientObjectId;
                    if (existedClientTreesIds.Contains(clientObjId)) {
                        var finden = existedClientTreesTuples.FirstOrDefault(y => y.Item2 == clientObjId);
                        if (finden != null) {
                            typedItem.ClientTreeId = finden.Item1;
                        }
                    }

                    int productObjId = typedItem.ProductObjectId;
                    if (existedProductTreesIds.Contains(productObjId)) {
                        var finden = existedProductTreesTuples.FirstOrDefault(y => y.Item2 == productObjId);
                        if (finden != null) {
                            typedItem.ProductTreeId = finden.Item1;
                        }
                    }

                    Tuple<String, Guid> mech = mechanicTuples.FirstOrDefault(y => y.Item1 == typedItem.MechanicName);
                    if (mech != null) {
                        typedItem.MechanicId = mech.Item2;
                    }

                    Tuple<String, Guid, double?> mechType = mechanicTypeTuples.FirstOrDefault(y => y.Item1 == typedItem.MechanicTypeName);
                    if (mechType != null) {
                        typedItem.MechanicTypeId = mechType.Item2;
                    }

                    if (typedItem.ToDate == null) {
                        typedItem.ToDate = DateTimeOffset.MaxValue;
                    }

                });

                //Проверка по пересечению времени
                IList<Tuple<int, int, Guid?>> badTimesIds = new List<Tuple<int, int, Guid?>>();

                IList<Tuple<int, int, Guid?, DateTimeOffset?, DateTimeOffset?>> existedNoNegosTimes =
                    this.GetQuery(context).Where(x => !x.Disabled).Select(y => new Tuple<int, int, Guid?, DateTimeOffset?, DateTimeOffset?>(y.ClientTreeId, y.ProductTreeId, y.MechanicId, y.FromDate, y.ToDate)).ToList();

                IList<Tuple<int, int, Guid?, DateTimeOffset?, DateTimeOffset?>> importedNoNegosTimes =
                    sourceRecords.Select(y => new Tuple<int, int, Guid?, DateTimeOffset?, DateTimeOffset?>(((ImportNoNego) y).ClientTreeId, ((ImportNoNego) y).ProductTreeId, ((ImportNoNego) y).MechanicId, ((ImportNoNego) y).FromDate, ((ImportNoNego) y).ToDate)).ToList();


                Parallel.ForEach(sourceRecords, item => {
                    if (!DateCheck((ImportNoNego) item, existedNoNegosTimes, importedNoNegosTimes)) {
                        badTimesIds.Add(new Tuple<int, int, Guid?>(((ImportNoNego) item).ClientTreeId, ((ImportNoNego) item).ProductTreeId, ((ImportNoNego) item).MechanicId));
                    }
                });

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
                    } else if (!IsFilterSuitable(rec, existedClientTreesIds, existedProductTreesIds, mechanicTypeTuples, badTimesIds,mechTypeQuery, context, out validationErrors)) {
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

        //Кастомная проверка
        protected virtual bool IsFilterSuitable(IEntity<Guid> rec, IList<int> existedClientObjIds, IList<int> existedProductObjIds, IList<Tuple<String, Guid, double?>> mechanicTypeTuples, IList<Tuple<int, int, Guid?>> badTimesIds,IList<MechanicType> mechanicTypesList ,DatabaseContext context, out IList<string> errors) {
            errors = new List<string>();
            bool isError = false;

            ImportNoNego importObj = (ImportNoNego) rec;
            if (!String.IsNullOrEmpty(importObj.MechanicTypeName))
                SetMechanicTypeForObjectId(importObj,mechanicTypesList);

            //Проверка по существующим активным ClientTree для пользователя
            if (!existedClientObjIds.Contains(importObj.ClientObjectId)) {
                isError = true;
                errors.Add(importObj.ClientObjectId.ToString() + " not in user's active ClientTree list");
            }

            //Проверка по существующим активным ProductTree для пользователя
            if (!existedProductObjIds.Contains(importObj.ProductObjectId)) {
                isError = true;
                errors.Add(importObj.ProductObjectId.ToString() + " not in user's active ProductTree list");
            }

            //Проверка пересечения по времени на клиенте
            if (badTimesIds.Any(y => y.Item1 == importObj.ClientTreeId && y.Item2 == importObj.ProductTreeId && y.Item3 == importObj.MechanicId)) {
                isError = true;
                errors.Add(" there can not be two NoNego of client, product and Mechanic in some Time");
            }

            //Проверка FromDate, ToDate
            if (importObj.FromDate == null) {
                isError = true;
                errors.Add(" FromDate must be fullfilled");
            } else {

                if (importObj.FromDate > importObj.ToDate) {
                    isError = true;
                    errors.Add(" FromDate must be before ToDate");
                }

            }

            // Проверка Discount
            if (importObj.MechanicTypeId == null && importObj.MechanicDiscount == null) {
                isError = true;
                errors.Add(" Mechanic Type or Mechanic Discount must be fullfilled");
            } else {
                Tuple<String, Guid, double?> mechType = mechanicTypeTuples.FirstOrDefault(y => y.Item2 == importObj.MechanicTypeId);
                if (mechType !=null && importObj.MechanicDiscount != null && mechType.Item3 != importObj.MechanicDiscount) {
                    isError = true;
                    errors.Add(" Mechanic Discount is not corresponding with Mechanic Type");
                }

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
        /// Запись в базу аналогично изменению NoNego из интерфейса через контекст
        /// </summary>
        /// <param name="sourceRecords"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected int InsertDataToDatabase(IEnumerable<IEntity<Guid>> sourceRecords, DatabaseContext context) {
            IList<NoneNego> toCreate = new List<NoneNego>();
            var query = GetQuery(context).ToList();

            foreach (ImportNoNego newRecord in sourceRecords) {
                NoneNego oldRecord = query.FirstOrDefault(x => x.ClientTreeId == newRecord.ClientTreeId && x.ProductTreeId == newRecord.ProductTreeId && x.MechanicId == newRecord.MechanicId && !x.Disabled);

                NoneNego toSave = new NoneNego() {
                    MechanicId = newRecord.MechanicId,
                    MechanicTypeId = newRecord.MechanicTypeId,
                    ClientTreeId = newRecord.ClientTreeId,
                    ProductTreeId = newRecord.ProductTreeId,
                    Discount = newRecord.MechanicDiscount,
                    FromDate = newRecord.FromDate,
                    ToDate = newRecord.ToDate,
                    CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)
                };

                toCreate.Add(toSave);
            }

            foreach (IEnumerable<NoneNego> items in toCreate.Partition(100)) {
                context.Set<NoneNego>().AddRange(items);
            }
            context.SaveChanges();

            return sourceRecords.Count();
        }

        protected virtual IEnumerable<IEntity<Guid>> BeforeInsert(IEnumerable<IEntity<Guid>> records, DatabaseContext context) {
            return records;
        }

        private IEnumerable<NoneNego> GetQuery(DatabaseContext context) {
            IQueryable<NoneNego> query = context.Set<NoneNego>().AsNoTracking();
            return query.ToList();
        }


        // Логика проверки пересечения времени
        public bool DateCheck(ImportNoNego toCheck,
            IList<Tuple<int, int, Guid?, DateTimeOffset?, DateTimeOffset?>> existedNoNegosTimes,
            IList<Tuple<int, int, Guid?, DateTimeOffset?, DateTimeOffset?>> importedNoNegosTimes) {

            int clientTreeId = toCheck.ClientTreeId;
            int productTreeId = toCheck.ProductTreeId;
            Guid? mechanicId = toCheck.MechanicId;

            foreach (Tuple<int, int, Guid?, DateTimeOffset?, DateTimeOffset?> item in existedNoNegosTimes.Where(y => y.Item1 == clientTreeId && y.Item2 == productTreeId && y.Item3 == mechanicId)) {
                if ((item.Item4 <= toCheck.FromDate && item.Item5 >= toCheck.FromDate) ||
                    (item.Item4 <= toCheck.ToDate && item.Item5 >= toCheck.ToDate)) {
                    return false;
                }
            }

            var ctNoNego = importedNoNegosTimes.Where(y => y.Item1 == clientTreeId && y.Item2 == productTreeId && y.Item3 == mechanicId);
            if (ctNoNego.Count() > 1) {
                var thisNoNego = new Tuple<int, int, Guid?, DateTimeOffset?, DateTimeOffset?>(clientTreeId, productTreeId, mechanicId, toCheck.FromDate, toCheck.ToDate);
                if (ctNoNego.Where(y => y.Item1 == thisNoNego.Item1 && y.Item2 == thisNoNego.Item2 && y.Item3 == thisNoNego.Item3 && y.Item4 == thisNoNego.Item4).Count() > 1) {
                    return false;
                }
                foreach (Tuple<int, int, Guid?, DateTimeOffset?, DateTimeOffset?> item in ctNoNego.Where(y => !(y.Item1 == thisNoNego.Item1 && y.Item2 == thisNoNego.Item2 && y.Item3 == thisNoNego.Item3 && y.Item4 == thisNoNego.Item4))) {
                    if ((item.Item4 <= toCheck.FromDate && item.Item5 >= toCheck.FromDate) ||
                        (item.Item4 <= toCheck.ToDate && item.Item5 >= toCheck.ToDate)) {
                        return false;
                    }
                }
            }
            return true;
        }
        private void SetMechanicTypeForObjectId(ImportNoNego importNoNego, IList<MechanicType> mechanicTypes)
        {

            var mechanicType = mechanicTypes.Where(x => x.Name.Equals(importNoNego.MechanicTypeName));
            MechanicType mechanicTypeForClient = null;
            
            mechanicTypeForClient = mechanicType.Where(x => x.ClientTree != null && x.ClientTree.ObjectId.Equals(importNoNego.ClientObjectId)).FirstOrDefault();
            if (mechanicTypeForClient == null)
            {
                mechanicTypeForClient = mechanicType.FirstOrDefault();
            }

            importNoNego.MechanicTypeId = mechanicTypeForClient.Id;
            importNoNego.MechanicType = mechanicTypeForClient;
        }
    }
}
