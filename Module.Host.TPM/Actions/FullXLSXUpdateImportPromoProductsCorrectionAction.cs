using Core.Data;
using Core.Extensions;
using Core.Settings;
using Interfaces.Implementation.Action;
using Interfaces.Implementation.Import.FullImport;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Controllers;
using Module.Frontend.TPM.FunctionalHelpers.RSmode;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.Interfaces;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using NLog;
using Persist;
using Persist.Model;
using Persist.ScriptGenerator;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Utility;
using Utility.FileWorker;
using Utility.Import;
using Utility.Import.Cache;
using Utility.Import.ImportModelBuilder;
using Utility.Import.ModelBuilder;
using Utility.LogWriter;
using Module.Host.TPM.Util;
using AutoMapper;
using Module.Persist.TPM.Model.SimpleModel;
using Module.Frontend.TPM.FunctionalHelpers.RSPeriod;

namespace Module.Host.TPM.Actions
{
    class FullXLSXUpdateImportPromoProductsCorrectionAction : BaseAction
    {
        private readonly Guid _userId;
        private readonly Guid _handlerId;
        LogWriter handlerLogger = null;
        private readonly TPMmode TPMmode;

        public FullXLSXUpdateImportPromoProductsCorrectionAction(FullImportSettings settings, Guid userId, Guid handlerId, TPMmode tPMmode)
        {
            this._userId = userId;
            this._handlerId = handlerId;
            handlerLogger = new LogWriter(handlerId.ToString());

            UserId = settings.UserId;
            RoleId = settings.RoleId;
            ImportFile = settings.ImportFile;
            ImportType = settings.ImportType;
            ModelType = settings.ModelType;
            Separator = settings.Separator;
            Quote = settings.Quote;
            HasHeader = settings.HasHeader;

            AllowPartialApply = false;

            TPMmode = tPMmode;


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
                string msg = String.Format(ErrorMessageScaffold, e.ToString());
                logger.Error(msg);
                string message;
                if (e.IsUniqueConstraintException())
                {
                    message = UniqueErrorMessage;
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
        protected virtual IList<IEntity<Guid>> ParseImportFile()
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
            IList<IEntity<Guid>> records = ImportUtilityTPM.ParseXLSXFile(importFilePath, null, builder, validator, Separator, Quote, HasHeader, out sourceRecordCount, out errors, out buildErrors, out validateErrors);
            logger.Trace("after parse file");

            // Обработать ошибки
            foreach (string err in errors)
            {
                Errors.Add(err);
            }
            if (errors.Any())
            {
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
        protected virtual ImportResultFilesModel ApplyImport(IList<IEntity<Guid>> sourceRecords, out int successCount, out int warningCount, out int errorCount)
        {

            // Логика переноса данных из временной таблицы в постоянную
            // Получить записи текущего импорта
            using (DatabaseContext context = new DatabaseContext())
            {

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
                    else if (TPMmode == TPMmode.RS)
                    {
                        StartEndModel startEndModel = RSPeriodHelper.GetRSPeriod(context);
                        if (promo.DispatchesStart < startEndModel.StartDate || startEndModel.EndDate < promo.EndDate)
                        {
                            HasErrors = true;
                            errorRecords.Add(new Tuple<IEntity<Guid>, string>(promo, $"Promo number:{promo.Number} is not in the RS period"));
                        }
                    }
                }

                if (!HasErrors)
                {
                    //AplyFilter for products AplyFilter for correction
                    var promoProducts = context.Set<PromoProduct>().Where(x => sourceRecordsPromoNumbers.Contains(x.Promo.Number)).ToList();
                    var filterPromoProducts = ApplyFilterForProduct(promoProducts.AsQueryable(), TPMmode).ToList();

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
                        else if (!IsFilterSuitable(item, convertedSourceRecords, out validationErrors, filterPromoProducts))
                        {
                            HasErrors = true;
                            errorRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", validationErrors)));
                        }
                        else
                        {
                            var importPromoProductCorrection = item;
                            var promoProduct = filterPromoProducts.
                                FirstOrDefault(x => x.Promo.Number == importPromoProductCorrection.PromoNumber && x.Product.ZREP == importPromoProductCorrection.ProductZREP && !x.Disabled);

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
                if (hasSuccessList)
                {
                    // Закончить импорт
                    var items = BeforeInsert(records, context).ToList();
                    resultRecordCount = InsertDataToDatabase(items, context, ref warningRecords);
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

        protected string GetImportStatus()
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

        protected virtual ScriptGenerator GetScriptGenerator()
        {
            if (_generator == null)
            {
                _generator = new ScriptGenerator(ModelType);
            }
            return _generator;
        }

        protected ScriptGenerator _generator { get; set; }

        protected virtual bool IsFilterSuitable(ImportPromoProductsCorrection item, IEnumerable<ImportPromoProductsCorrection> importedPromoProductCorrections, out IList<string> errors, List<PromoProduct> promoProducts)
        {
            errors = new List<string>();
            var promoProduct = promoProducts.Where(x => x.ZREP == item.ProductZREP && x.Promo.Number == item.PromoNumber).FirstOrDefault();

            var importedPromoProductCorrectionGroup = importedPromoProductCorrections.GroupBy(x => new { x.PromoNumber, x.ProductZREP }).FirstOrDefault(x => x.Key.PromoNumber == item.PromoNumber && x.Key.ProductZREP == item.ProductZREP);
            if (importedPromoProductCorrectionGroup.Count() > 1)
            {
                errors.Add($"Records must not be repeated (Promo number: {item.PromoNumber}, ZREP: {item.ProductZREP})");
                return false;
            }
            else if (item.PlanProductUpliftPercentCorrected <= 0)
            {
                errors.Add($"Uplift must be greater than 0 and must not be empty (Promo number: {item.PromoNumber}, ZREP: {item.ProductZREP})");
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

        protected virtual void Fail()
        {

        }

        protected virtual void Success()
        {

        }

        protected virtual void Complete()
        {

        }

        protected virtual IEnumerable<PromoProductsCorrection> BeforeInsert(IEnumerable<PromoProductsCorrection> records, DatabaseContext context)
        {
            return records;
        }

        protected int InsertDataToDatabase(IEnumerable<PromoProductsCorrection> importedPromoProductCorrections, DatabaseContext databaseContext, ref ConcurrentBag<Tuple<IEntity<Guid>, string>> warningRecords)
        {
            var currentUser = databaseContext.Set<User>().FirstOrDefault(x => x.Id == this._userId);
            var promoProductsCorrectionChangeIncidents = new List<PromoProductsCorrection>();

            var promoProductIds = importedPromoProductCorrections.Select(ppc => ppc.PromoProductId);
            var promoProducts = databaseContext.Set<PromoProduct>().Where(pp => promoProductIds.Contains(pp.Id)).ToList();
            var promoIds = promoProducts.Select(pp => pp.PromoId);
            var promoes = databaseContext.Set<Promo>().Where(p => promoIds.Contains(p.Id)).ToList();

            List<PromoProductsCorrection> promoProductsCorrections = new List<PromoProductsCorrection>();

            foreach (var importedPromoProductCorrection in importedPromoProductCorrections)
            {
                var promoProduct = databaseContext.Set<PromoProduct>()
                        .FirstOrDefault(x => x.Id == importedPromoProductCorrection.PromoProductId && !x.Disabled);
                var promo = promoes.FirstOrDefault(q => promoProduct != null && q.Id == promoProduct.PromoId);

                if (promo.InOut.HasValue && promo.InOut.Value)
                {
                    warningRecords.Add(new Tuple<IEntity<Guid>, string>(importedPromoProductCorrection, $"Promo Product Correction was not imported for In-Out promo №{promo.Number}"));
                    handlerLogger.Write(true, $"Promo Product Correction was not imported for In-Out promo №{promo.Number}", "Warning");
                    continue;
                }

                var currentPromoProductCorrection = databaseContext.Set<PromoProductsCorrection>()
                .Include(g => g.PromoProduct.Promo.IncrementalPromoes)
                .Include(g => g.PromoProduct.Promo.PromoSupportPromoes)
                .Include(g => g.PromoProduct.Promo.PromoProductTrees)
                .FirstOrDefault(x => x.PromoProductId == importedPromoProductCorrection.PromoProductId && x.TempId == importedPromoProductCorrection.TempId && !x.Disabled && x.TPMmode == TPMmode);

                if (currentPromoProductCorrection != null)
                {
                    if (TPMmode == TPMmode.RS && currentPromoProductCorrection.PromoProduct.Promo.TPMmode == TPMmode.Current)
                    {
                        promoProductsCorrections = databaseContext.Set<PromoProductsCorrection>()
                        .Include(g => g.PromoProduct.Promo.IncrementalPromoes)
                        .Include(g => g.PromoProduct.Promo.PromoSupportPromoes)
                        .Include(g => g.PromoProduct.Promo.PromoProductTrees)
                        .Where(x => x.PromoProduct.PromoId == currentPromoProductCorrection.PromoProduct.PromoId && !x.Disabled)
                        .ToList();
                        promoProductsCorrections = RSmodeHelper.EditToPromoProductsCorrectionRS(databaseContext, promoProductsCorrections);
                        currentPromoProductCorrection = promoProductsCorrections.FirstOrDefault(g => g.PromoProduct.ZREP == currentPromoProductCorrection.PromoProduct.ZREP);
                    }

                    if (currentPromoProductCorrection.PromoProduct.Promo.NeedRecountUplift == false && String.IsNullOrEmpty(currentPromoProductCorrection.TempId))
                    {
                        throw new ImportException("Promo Locked Update");
                    }

                    currentPromoProductCorrection.PlanProductUpliftPercentCorrected = importedPromoProductCorrection.PlanProductUpliftPercentCorrected;
                    currentPromoProductCorrection.ChangeDate = DateTimeOffset.Now;
                    currentPromoProductCorrection.UserId = this._userId;
                    currentPromoProductCorrection.UserName = currentUser?.Name ?? string.Empty;

                    if (TPMmode == TPMmode.Current)
                    {
                        var promoRS = databaseContext.Set<Promo>()
                        .Include(x => x.PromoProducts)
                        .FirstOrDefault(x => x.Number == promoProduct.Promo.Number && x.TPMmode == TPMmode.RS && !x.Disabled);
                        if (promoRS != null)
                        {
                            databaseContext.Set<Promo>().Remove(promoRS);
                            databaseContext.SaveChanges();

                            var currentPromoProductsCorrections = databaseContext.Set<PromoProductsCorrection>()
                                .Include(g => g.PromoProduct.Promo.IncrementalPromoes)
                                .Include(g => g.PromoProduct.Promo.PromoSupportPromoes)
                                .Include(g => g.PromoProduct.Promo.PromoProductTrees)
                                .Where(x => x.PromoProduct.PromoId == promoProduct.PromoId && !x.Disabled)
                                .ToList();
                            currentPromoProductsCorrections = RSmodeHelper.EditToPromoProductsCorrectionRS(databaseContext, currentPromoProductsCorrections);
                            var promoProductsCorrection = currentPromoProductsCorrections.FirstOrDefault(g => g.PromoProduct.ZREP == promoProduct.ZREP);
                            promoProductsCorrection.PlanProductUpliftPercentCorrected = importedPromoProductCorrection.PlanProductUpliftPercentCorrected;
                            promoProductsCorrection.ChangeDate = DateTimeOffset.Now;
                            promoProductsCorrection.UserId = this._userId;
                            promoProductsCorrection.UserName = currentUser?.Name ?? string.Empty;
                        }
                    }
                    promoProductsCorrectionChangeIncidents.Add(currentPromoProductCorrection);
                }
                else
                {
                    if (TPMmode == TPMmode.Current)
                    {
                        promoProduct = databaseContext.Set<PromoProduct>()
                            .FirstOrDefault(x => x.Id == importedPromoProductCorrection.PromoProductId && !x.Disabled && x.TPMmode == TPMmode);

                        if (promoProduct.Promo.NeedRecountUplift == false && String.IsNullOrEmpty(importedPromoProductCorrection.TempId))
                        {
                            throw new ImportException("Promo Locked Update");
                        }
                        importedPromoProductCorrection.CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                        importedPromoProductCorrection.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                        importedPromoProductCorrection.UserId = this._userId;
                        importedPromoProductCorrection.UserName = currentUser?.Name ?? string.Empty;

                        databaseContext.Set<PromoProductsCorrection>().Add(importedPromoProductCorrection);

                        var promoRS = databaseContext.Set<Promo>()
                        .Include(x => x.PromoProducts)
                        .FirstOrDefault(x => x.Number == promoProduct.Promo.Number && x.TPMmode == TPMmode.RS && !x.Disabled);

                        if (promoRS != null)
                        {
                            databaseContext.Set<Promo>().Remove(promoRS);
                            databaseContext.SaveChanges();

                            var currentPromoProductsCorrections = databaseContext.Set<PromoProductsCorrection>()
                                .Include(g => g.PromoProduct.Promo.IncrementalPromoes)
                                .Include(g => g.PromoProduct.Promo.PromoSupportPromoes)
                                .Include(g => g.PromoProduct.Promo.PromoProductTrees)
                                .Where(x => x.PromoProduct.PromoId == promoProduct.PromoId && !x.Disabled)
                                .ToList();
                            currentPromoProductsCorrections = RSmodeHelper.EditToPromoProductsCorrectionRS(databaseContext, currentPromoProductsCorrections);
                            var promoProductsCorrection = currentPromoProductsCorrections.FirstOrDefault(g => g.PromoProduct.ZREP == promoProduct.ZREP);
                            promoProductsCorrection.PlanProductUpliftPercentCorrected = importedPromoProductCorrection.PlanProductUpliftPercentCorrected;
                            promoProductsCorrection.ChangeDate = DateTimeOffset.Now;
                            promoProductsCorrection.UserId = this._userId;
                            promoProductsCorrection.UserName = currentUser?.Name ?? string.Empty;
                        }
                    }
                    if (TPMmode == TPMmode.RS)
                    {
                        var promoProductRS = databaseContext.Set<PromoProduct>()
                                        .FirstOrDefault(x => x.Promo.Number == promoProduct.Promo.Number && x.ZREP == promoProduct.ZREP && !x.Disabled && x.TPMmode == TPMmode.RS);
                        if (promoProductRS == null)
                        {
                            var currentPromo = databaseContext.Set<Promo>()
                                .Include(g => g.PromoSupportPromoes)
                                .Include(g => g.PromoProductTrees)
                                .Include(g => g.IncrementalPromoes)
                                .Include(x => x.PromoProducts.Select(y => y.PromoProductsCorrections))
                                .FirstOrDefault(p => p.Number == promo.Number && p.TPMmode == TPMmode.Current);
                            var promoRS = RSmodeHelper.EditToPromoRS(databaseContext, currentPromo);
                            promoProductRS = promoRS.PromoProducts
                                .FirstOrDefault(x => x.ZREP == promoProduct.ZREP);
                        }

                        importedPromoProductCorrection.TPMmode = TPMmode.RS;
                        importedPromoProductCorrection.PromoProduct = promoProductRS;
                        importedPromoProductCorrection.PromoProductId = promoProductRS.Id;
                        importedPromoProductCorrection.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                        importedPromoProductCorrection.UserId = this._userId;
                        importedPromoProductCorrection.UserName = currentUser?.Name ?? string.Empty;

                        databaseContext.Set<PromoProductsCorrection>().Add(importedPromoProductCorrection);
                        promoProductsCorrections.Add(importedPromoProductCorrection);
                        promoProductsCorrectionChangeIncidents.Add(importedPromoProductCorrection);                        
                    };
                }
            }

            // Необходимо выполнить перед созданием инцидентов.
            databaseContext.SaveChanges();

            foreach (var promoProductsCorrection in promoProductsCorrectionChangeIncidents)
            {
                var currentPromoProductsCorrection = databaseContext.Set<PromoProductsCorrection>().FirstOrDefault(x => x.PromoProductId == promoProductsCorrection.PromoProductId && !x.Disabled);
                if (currentPromoProductsCorrection != null)
                {
                    PromoProductsCorrectionsController.CreateChangesIncident(databaseContext.Set<ChangesIncident>(), currentPromoProductsCorrection);
                }
            }

            databaseContext.SaveChanges();
            return promoProductsCorrections.Count();
        }

        private IQueryable<PromoProductsCorrection> ApplyFilterForCorrection(IQueryable<PromoProductsCorrection> query, TPMmode mode)
        {
            query = query.Where(x => !x.Disabled || x.TPMmode == TPMmode.RS);
            switch (mode)
            {
                case TPMmode.Current:
                    query = query.Where(x => x.TPMmode == TPMmode.Current && !x.Disabled);
                    break;
                case TPMmode.RS:
                    query = query.GroupBy(x => new { x.PromoProduct.Promo.Number, x.PromoProductId }, (key, g) => g.OrderByDescending(e => e.TPMmode).FirstOrDefault());
                    query = query.Where(x => !x.Disabled);
                    break;
            }
            return query;
        }

        private IQueryable<PromoProduct> ApplyFilterForProduct(IQueryable<PromoProduct> query, TPMmode mode)
        {
            query = query.Where(x => !x.Disabled || x.TPMmode == TPMmode.RS);
            switch (mode)
            {
                case TPMmode.Current:
                    query = query.Where(x => x.TPMmode == TPMmode.Current && !x.Disabled);
                    break;
                case TPMmode.RS:
                    query = query.GroupBy(x => new { x.Promo.Number, x.Product.Id }, (key, g) => g.OrderByDescending(e => e.TPMmode).FirstOrDefault());
                    query = query.Where(x => !x.Disabled);
                    break;
            }
            return query;
        }
    }
}
