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
using System.Web;
using Utility;
using Utility.FileWorker;
using Utility.Import;
using Utility.Import.Cache;
using Utility.Import.ImportModelBuilder;
using Utility.Import.ModelBuilder;

namespace Module.Host.TPM.Actions {

    /// <summary>
    /// Переопределение Action из ядра приложения
    /// </summary>
    public class FullXLSXAssortmentMatrixImportAction : BaseAction {

        public FullXLSXAssortmentMatrixImportAction(FullImportSettings settings) {
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

                //Присваивание ID
                Parallel.ForEach(sourceRecords, item => {
                    ImportAssortmentMatrix typedItem = (ImportAssortmentMatrix) item;

                    int clientObjId = typedItem.ClientObjectId;
                    if (existedClientTreesIds.Contains(clientObjId)) {
                        var finden = existedClientTreesTuples.FirstOrDefault(y => y.Item2 == clientObjId);
                        if (finden != null) {
                            typedItem.ClientTreeId = finden.Item1;
                        }
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
                    } else if (!IsFilterSuitable(rec, existedClientTreesIds, context, out validationErrors)) {
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
        protected virtual bool IsFilterSuitable(IEntity<Guid> rec, IList<int> existedClientObjIds, DatabaseContext context, out IList<string> errors)
        {
            errors = new List<string>();
            bool isError = false;

            ImportAssortmentMatrix importObj = (ImportAssortmentMatrix)rec;
            //Проверка по существующим активным ClientTree для пользователя
            if (!existedClientObjIds.Contains(importObj.ClientObjectId))
            {
                isError = true;
                errors.Add(importObj.ClientObjectId.ToString() + " not in user's active ClientTree list");
            }

            //Проверка StartDate, EndDate
            if (importObj.StartDate == null || importObj.EndDate == null)
            {
                isError = true;
                errors.Add(" StartDate and EndDate must be fullfilled");
            }
            else
            {
                var currentDateWithoutTime = new DateTime(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTimeOffset.Now.Day);
                if (importObj.StartDate < currentDateWithoutTime)
                {
                    isError = true;
                    errors.Add("Start date must be after current date.");
                }

                if (importObj.EndDate < currentDateWithoutTime)
                {
                    isError = true;
                    errors.Add("End date must be after current date.");
                }

                if (importObj.StartDate >= importObj.EndDate)
                {
                    isError = true;
                    errors.Add(" StartDate must be before EndDate");
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
        /// Запись в базу аналогично изменению AssortmentMatrix из интерфейса через контекст
        /// </summary>
        /// <param name="sourceRecords"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected int InsertDataToDatabase(IEnumerable<IEntity<Guid>> sourceRecords, DatabaseContext context) {
            IList<AssortmentMatrix> toCreate = new List<AssortmentMatrix>();
			IList<ProductChangeIncident> incidents = new List<ProductChangeIncident>();
            var query = GetQuery(context).ToList();
			IList<Product> products = context.Set<Product>().Where(x => !x.Disabled).ToList();

			foreach (ImportAssortmentMatrix newRecord in sourceRecords) {
                AssortmentMatrix oldRecord = query.FirstOrDefault(x => x.ClientTreeId == newRecord.ClientTreeId && x.ProductId == newRecord.ProductId && !x.Disabled);
                AssortmentMatrix toSave = new AssortmentMatrix() {
                    ClientTreeId = newRecord.ClientTreeId,
                    ProductId = newRecord.ProductId,
                    StartDate = newRecord.StartDate,
                    EndDate = newRecord.EndDate,
                    CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)
                };
                toCreate.Add(toSave);

				Product product = products.Where(x => x.Id == toSave.ProductId).FirstOrDefault();
				ProductChangeIncident pci = new ProductChangeIncident
				{
					CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
					IsCreate = false,
					IsDelete = false,
					IsCreateInMatrix = true,
					IsDeleteInMatrix = false,
					IsChecked = false,
					Product = product,
					ProductId = product.Id
				};
				incidents.Add(pci);
			}

            foreach (IEnumerable<AssortmentMatrix> items in toCreate.Partition(100)) {
                context.Set<AssortmentMatrix>().AddRange(items);
            }
			foreach (IEnumerable<ProductChangeIncident> items in incidents.Partition(100))
			{
				context.Set<ProductChangeIncident>().AddRange(items);
			}
			context.SaveChanges();

            return sourceRecords.Count();
        }

        protected virtual IEnumerable<IEntity<Guid>> BeforeInsert(IEnumerable<IEntity<Guid>> records, DatabaseContext context) {
            return records;
        }

        private IEnumerable<AssortmentMatrix> GetQuery(DatabaseContext context) {
            IQueryable<AssortmentMatrix> query = context.Set<AssortmentMatrix>().AsNoTracking();
            return query.ToList();
        }
    }
}
