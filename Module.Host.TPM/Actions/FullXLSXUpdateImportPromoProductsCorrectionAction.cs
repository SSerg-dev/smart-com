using Core.Data;
using Core.Extensions;
using Core.Settings;
using Interfaces.Implementation.Action;
using Interfaces.Implementation.Import.FullImport;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Controllers;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using NLog;
using Persist;
using Persist.Model;
using Persist.ScriptGenerator;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Import;
using Utility.Import.Cache;
using Utility.Import.ImportModelBuilder;
using Utility.Import.ModelBuilder;
using Utility.LogWriter;

namespace Module.Host.TPM.Actions
{
    class FullXLSXUpdateImportPromoProductsCorrectionAction : BaseAction
    {
        private readonly Guid _userId;
        private readonly Guid _handlerId;

        public FullXLSXUpdateImportPromoProductsCorrectionAction(FullImportSettings settings, Guid userId, Guid handlerId)
        {
            this._userId = userId;
            this._handlerId = handlerId;

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

        protected string UniqueErrorMessage = "Такая запись уже существует в базе данных";
        protected string ErrorMessageScaffold = "При выполнении FullImportAction произошла ошибка: {0}";
        protected string FileParseErrorMessage = "Произошла ошибка при разборе файла импорта";

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

                // Сохранить выходные параметры
                Results["ImportSourceRecordCount"] = sourceRecords.Count();
                Results["ImportResultRecordCount"] = successCount;
                Results["ErrorCount"] = errorCount;
                Results["WarningCount"] = warningCount;
                Results["ImportResultFilesModel"] = resultFilesModel;

            } catch (Exception e) {
                HasErrors = true;
                string msg = String.Format(ErrorMessageScaffold, e.ToString());
                logger.Error(msg);
                string message;
                if (e.IsUniqueConstraintException()) {
                    message = UniqueErrorMessage;
                } else {
                    message = e.ToString();
                }
                Errors.Add(message);
                ResultStatus = ImportUtility.StatusName.ERROR;
            } finally
            {
                if (HasErrors)
                {
                    Fail();
                }
                else
                {
                    Success();
                }
                // информация о том, какой должен быть статус у задачи
                Results["ImportResultStatus"] = ResultStatus;
                logger.Debug("Finish");
                Complete();
            }
        }

        /// <summary>
        /// Выполнить разбор файла импорта
        /// </summary>
        /// <returns></returns>
        protected virtual IList<IEntity<Guid>> ParseImportFile() {
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
                throw new ImportException(FileParseErrorMessage);
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
        protected virtual ImportResultFilesModel ApplyImport(IList<IEntity<Guid>> sourceRecords, out int successCount, out int warningCount, out int errorCount) {

            // Логика переноса данных из временной таблицы в постоянную
            // Получить записи текущего импорта
            using (DatabaseContext context = new DatabaseContext()) {
                
                var records = new List<PromoProductsCorrection>();
                ConcurrentBag<IEntity<Guid>> successList = new ConcurrentBag<IEntity<Guid>>();
                ConcurrentBag<Tuple<IEntity<Guid>, string>> errorRecords = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();
                ConcurrentBag<Tuple<IEntity<Guid>, string>> warningRecords = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();

                // Получить функцию Validate
                IImportValidator validator = ImportModelFactory.GetImportValidator(ImportType);
                // Получить функцию SetProperty
                IModelBuilder builder = ImportModelFactory.GetModelBuilder(ImportType, ModelType);

                IImportCacheBuilder cacheBuilder = ImportModelFactory.GetImportCacheBuilder(ImportType);
                IImportCache cache = cacheBuilder.Build(sourceRecords, context);
                var convertedSourceRecords = sourceRecords.Cast<ImportPromoProductsCorrection>().ToList();
                List<int?> sourceRecordsPromoNumbers = convertedSourceRecords.Select(y => y.PromoNumber).Cast<int?>().ToList();
                List<Promo> promoes = context.Set<Promo>().Where(x => sourceRecordsPromoNumbers.Contains(x.Number) && x.Disabled != true).ToList();
                string[] goodStatuses = new string[] { "OnApproval", "Planned", "Approved", "DraftPublished" };
                var errors = new List<string>();
                foreach (var promo in promoes)
                {
                    if (!goodStatuses.Contains(promo.PromoStatus.SystemName) || promo.NeedRecountUplift == false)
                    {
                        HasErrors = true;
                        errorRecords.Add(new Tuple<IEntity<Guid>, string>(promo, $"Can't create corrections for Promo {promo.Number}"));
                    }
                }

                if (!HasErrors)
                {
                    var promoProducts = context.Set<PromoProduct>().Where(x => sourceRecordsPromoNumbers.Contains(x.Promo.Number) && x.Disabled != true).ToList();
                    var promoProductCorrections = context.Set<PromoProductsCorrection>().ToList();

                    foreach (var item in convertedSourceRecords)
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
                        else if (!IsFilterSuitable(item, convertedSourceRecords, out validationErrors, promoProducts))
                        {
                            HasErrors = true;
                            errorRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", validationErrors)));
                        }
                        else
                        {
                            var importPromoProductCorrection = item;
                            var promoProduct = context.Set<PromoProduct>().FirstOrDefault(x => x.Promo.Number == importPromoProductCorrection.PromoNumber && x.Product.ZREP == importPromoProductCorrection.ProductZREP);

                            records.Add(new PromoProductsCorrection
                            {
                                PromoProductId = promoProduct?.Id ?? new Guid(),
                                PromoProduct = promoProduct,
                                PlanProductUpliftPercentCorrected = importPromoProductCorrection.PlanProductUpliftPercentCorrected
                            });

                            successList.Add(item);
                            if (warnings.Any())
                            {
                                warningRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", warnings)));
                            }
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
                if (hasSuccessList) {
                    // Закончить импорт
                    var items = BeforeInsert(records, context).ToList();
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

         protected string GetImportStatus() {
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

        protected virtual ScriptGenerator GetScriptGenerator() {
            if (_generator == null) {
                _generator = new ScriptGenerator(ModelType);
            }
            return _generator;
        }

        protected ScriptGenerator _generator { get; set; }

        protected virtual bool IsFilterSuitable(ImportPromoProductsCorrection item, IEnumerable<ImportPromoProductsCorrection> importedPromoProductCorrections,  out IList<string> errors, List<PromoProduct> promoProducts)
        {
            errors = new List<string>();
            var promoProduct = promoProducts.Where(x => x.ZREP == item.ProductZREP && x.Promo.Number == item.PromoNumber).FirstOrDefault();

            var importedPromoProductCorrectionGroup = importedPromoProductCorrections.GroupBy(x => new { x.PromoNumber, x.ProductZREP }).FirstOrDefault(x => x.Key.PromoNumber == item.PromoNumber && x.Key.ProductZREP == item.ProductZREP);
            if (importedPromoProductCorrectionGroup.Count() > 1)
            {
                errors.Add($"Records must not be repeated (Promo number: {item.PromoNumber}, ZREP: {item.ProductZREP})");
                return false;
            }
            else if (item.PlanProductUpliftPercentCorrected < 0)
            {
                errors.Add($"Uplift must be greater than 0 (Promo number: {item.PromoNumber}, ZREP: {item.ProductZREP})");
                return false;
            }
            else if (promoProduct == null)
            {
                errors.Add($"No product with ZREP: {item.ProductZREP} found in Promo: {item.PromoNumber}");
                return false;
            }
            else
            {
                return true;
            }
        }

        //protected void UpdateRecordError(BaseImportEntity item, bool hasError, IList<string> errors) {
        //    item.HasErrors = hasError;
        //    item.ErrorMessage = hasError ? String.Join(Environment.NewLine, errors) : null;
        //}

        protected virtual void Fail() {

        }

        protected virtual void Success() {

        }

        protected virtual void Complete() {

        }

        protected virtual IEnumerable<PromoProductsCorrection> BeforeInsert(IEnumerable<PromoProductsCorrection> records, DatabaseContext context) {
            return records;
        }

        protected int InsertDataToDatabase(IEnumerable<PromoProductsCorrection> importedPromoProductCorrections, DatabaseContext databaseContext)
        {
            var currentUser = databaseContext.Set<User>().FirstOrDefault(x => x.Id == this._userId);
            var promoProductsCorrectionChangeIncidents = new List<PromoProductsCorrection>();

            var promoProductCorrections = databaseContext.Set<PromoProductsCorrection>();
            foreach (var importedPromoProductCorrection in importedPromoProductCorrections)
            {
                var currentPromoProductCorrections = promoProductCorrections.Where(x => x.PromoProductId == importedPromoProductCorrection.PromoProductId && !x.Disabled);
                if (currentPromoProductCorrections.Count() > 0)
                {
                    foreach (var currentPromoProductCorrection in currentPromoProductCorrections)
                    {
                        currentPromoProductCorrection.PlanProductUpliftPercentCorrected = importedPromoProductCorrection.PlanProductUpliftPercentCorrected;
                        currentPromoProductCorrection.ChangeDate = DateTimeOffset.Now;
                        currentPromoProductCorrection.UserId = this._userId;
                        currentPromoProductCorrection.UserName = currentUser?.Name ?? string.Empty;

                        promoProductsCorrectionChangeIncidents.Add(currentPromoProductCorrection);
                    }
                }
                else
                {
                    var newPromoProductCorrection = new PromoProductsCorrection
                    {
                        Disabled = false,
                        DeletedDate = null,
                        PromoProductId = importedPromoProductCorrection.PromoProductId,
                        PlanProductUpliftPercentCorrected = importedPromoProductCorrection.PlanProductUpliftPercentCorrected,
                        UserId = this._userId,
                        UserName = currentUser?.Name ?? string.Empty,
                        CreateDate = DateTimeOffset.Now,
                        ChangeDate = DateTimeOffset.Now
                    };
                    promoProductCorrections.Add(newPromoProductCorrection);
                    promoProductsCorrectionChangeIncidents.Add(newPromoProductCorrection);
                }
            }

            // Необходимо выполнить перед созданием инцидентов.
            databaseContext.SaveChanges();

            foreach (var promoProductsCorrection in promoProductsCorrectionChangeIncidents)
            {
                var currentPromoProductsCorrection = promoProductCorrections.FirstOrDefault(x => x.PromoProductId == promoProductsCorrection.PromoProductId && !x.Disabled);
                if (currentPromoProductsCorrection != null)
                {
                    PromoProductsCorrectionsController.CreateChangesIncident(databaseContext.Set<ChangesIncident>(), currentPromoProductsCorrection);
                }
            }

            databaseContext.SaveChanges();
            return promoProductCorrections.Count();
        }
    }
}
