using Core.Data;
using Core.Extensions;
using Interfaces.Implementation.Action;
using Interfaces.Implementation.Import.FullImport;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Host.TPM.Util;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.Interfaces;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using NLog;
using Persist;
using Persist.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utility;
using Utility.FileWorker;
using Utility.Import;
using static Module.Frontend.TPM.Util.PromoHelper;

namespace Module.Host.TPM.Actions
{
    public class FullXLSXRpaTCLclosedImportAction : BaseAction
    {
        private readonly Guid UserId;
        private readonly Guid RoleId;
        private readonly Guid RPAId;
        private readonly Guid HandlerId;
        private readonly FileModel ImportFile;
        private readonly Type ImportType;
        private readonly Type ModelType;
        private readonly string Separator;
        private readonly string Quote;
        private readonly bool HasHeader;

        private ConcurrentBag<IEntity<Guid>> successList = new ConcurrentBag<IEntity<Guid>>();
        private ConcurrentBag<Tuple<IEntity<Guid>, string>> errorRecords = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();
        private ConcurrentBag<Tuple<IEntity<Guid>, string>> warningRecords = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();

        private bool AllowPartialApply { get; set; }
        private readonly Logger logger;
        private string ResultStatus { get; set; }
        private bool HasErrors { get; set; }

        private ScriptGenerator Generator { get; set; }

        public FullXLSXRpaTCLclosedImportAction(FullImportSettings settings, Guid rPAId, Guid handlerId)
        {
            UserId = settings.UserId;
            RoleId = settings.RoleId;
            ImportFile = settings.ImportFile;
            ImportType = settings.ImportType;
            ModelType = settings.ModelType;
            Separator = settings.Separator;
            Quote = settings.Quote;
            HasHeader = settings.HasHeader;
            RPAId = rPAId;
            HandlerId = handlerId;

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

                string rpaStatus = "In progress";
                using (DatabaseContext context = new DatabaseContext())
                {
                    RPA rpa = context.Set<RPA>().FirstOrDefault(x => x.Id == RPAId);
                    rpa.Status = rpaStatus;
                    context.SaveChanges();
                }

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
                var rpaStatus = "Error";
                using (var context = new DatabaseContext())
                {
                    var rpa = context.Set<RPA>().FirstOrDefault(x => x.Id == RPAId);
                    rpa.Status = rpaStatus;
                    context.SaveChanges();
                }
                HasErrors = true;
                string message = string.Format("FullImportAction failed: {0}", e.ToString());
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
            FileDispatcher fileDispatcher = new FileDispatcher();
            string importDir = Core.Settings.AppSettingsManager.GetSetting("RPA_DIRECTORY", "RPAFiles");

            string importFilePath = Path.Combine(importDir, ImportFile.Name);
            if (!fileDispatcher.IsExists(importDir, ImportFile.Name))
            {
                throw new Exception("Import File not found");
            }

            var builder = ImportModelFactory.GetCSVImportModelBuilder(ImportType);
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
                IList<string> warnings = new List<string>();
                IList<string> validationErrors = new List<string>();

                // Получить функцию Validate
                var validator = ImportModelFactory.GetImportValidator(ImportType);
                // Получить функцию SetProperty
                var builder = ImportModelFactory.GetModelBuilder(ImportType, ModelType);


                var cacheBuilder = ImportModelFactory.GetImportCacheBuilder(ImportType);
                var cache = cacheBuilder.Build(sourceRecords, context);

                //Ограничение пользователя  
                IList<Constraint> constraints = context.Constraints
                    .Where(x => x.UserRole.UserId.Equals(UserId) && x.UserRole.Role.Id.Equals(RoleId))
                    .ToList();
                IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
                //здесь должны быть все записи, а не только неудаленные!
                IQueryable<ClientTree> query = context.Set<ClientTree>().AsNoTracking();
                IQueryable<ClientTreeHierarchyView> hierarchy = context.Set<ClientTreeHierarchyView>().AsNoTracking();
                query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);
                List<ClientTree> existingClientTreeIds = query.ToList();
                string username = context.Users.FirstOrDefault(g => g.Id == UserId).Name;
                // проверка на дубликаты из файла
                CheckForDuplicatesImport(context, sourceRecords, out validationErrors);
                // создание пром
                List<Promo> promos = CreatePromoesFromTLCclosed(context, sourceRecords, username, existingClientTreeIds, out validationErrors);

                // Проверка на дубликаты из базы
                promos = CheckForDuplicates(context, promos, sourceRecords, out validationErrors);

                logger.Trace("Persist models built");

                int resultRecordCount = 0;

                ResultStatus = GetImportStatus();
                string rpaStatus = ResultStatus;
                RPA rpa = context.Set<RPA>().FirstOrDefault(x => x.Id == RPAId);
                rpa.Status = rpaStatus;
                var importModel = ImportUtility.BuildActiveImport(UserId, RoleId, ImportType);
                importModel.Status = ResultStatus;
                context.Imports.Add(importModel);

                bool hasSuccessList = AllowPartialApply || !HasErrors;
                if (hasSuccessList)
                {
                    // Закончить импорт
                    resultRecordCount = InsertDataToDatabase(promos, context);
                    //CalculateBudgetsCreateTask(context, promoesForBudgetCalculation);
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

        private void CheckForDuplicatesImport(DatabaseContext context, IList<IEntity<Guid>> sourceRecords, out IList<string> errors)
        {
            errors = new List<string>();
            List<ImportRpaTLCclosed> sourceTemplateRecords = sourceRecords
                .Select(sr => (sr as ImportRpaTLCclosed)).ToList();
            var promos1 = sourceTemplateRecords.Select(g => new { g.Client, g.BrandTech, g.PromoStartDate, g.PromoEndDate,g.DispatchStartDate,g.DispatchEndDate, g.Discount, g.PromoType, g.Subrange }).ToList();
            var promos2 = promos1.Distinct().ToList();
            if (promos2.Count != sourceRecords.Count)
            {
                var duplicate = promos1.GroupBy(s => s)
                    .SelectMany(grp => grp.Skip(1)).FirstOrDefault();
                errors.Add("Duplicate present in client:" + duplicate.Client + ", startdate:" + duplicate.PromoStartDate + " enddate:" + duplicate.PromoEndDate + " promotype:" + duplicate.PromoType);
                HasErrors = true;
                errorRecords.Add(new Tuple<IEntity<Guid>, string>(sourceTemplateRecords.FirstOrDefault(g => g.Client == duplicate.Client && g.PromoStartDate == duplicate.PromoStartDate && g.PromoEndDate == duplicate.PromoEndDate && g.Subrange == duplicate.Subrange && g.DispatchStartDate==duplicate.DispatchStartDate && g.DispatchEndDate==duplicate.DispatchEndDate ), string.Join(", ", errors)));
            }

        }

        private List<Promo> CheckForDuplicates(DatabaseContext context, List<Promo> promos, IList<IEntity<Guid>> sourceRecords, out IList<string> errors)
        {
            errors = new List<string>();
            List<ImportRpaTLCclosed> sourceTemplateRecords = sourceRecords
                .Select(sr => (sr as ImportRpaTLCclosed)).ToList();
            List<Guid> promoStatuses = context.Set<PromoStatus>().Where(g => !g.Disabled && (g.SystemName == "Closed" || g.SystemName == "Finished")).Select(g => g.Id).ToList();
            var promosPresent = context.Set<Promo>()
                .Where(g => promoStatuses.Contains((Guid)g.PromoStatusId) && !g.Disabled && g.TPMmode == TPMmode.Current)
                .Select(g => new { g.Number, g.StartDate, g.EndDate, g.ClientTreeId, g.BrandTechId, g.MarsMechanicDiscount, g.PromoTypesId, g.ProductSubrangesList, g.DispatchesStart, g.DispatchesEnd })
                .ToList();
            List<BrandTech> brandTeches = context.Set<BrandTech>().Where(g => !g.Disabled).ToList();
            var promoImports = promos.Select(g => new { g.Number, g.StartDate, g.EndDate,g.DispatchesStart,g.DispatchesEnd, g.ClientTreeId, g.BrandTechId, g.MarsMechanicDiscount, g.PromoTypesId, g.ProductSubrangesList }).ToList();
            foreach (var promoImport in promoImports)
            {
                var present = promosPresent
                    .FirstOrDefault(g => g.StartDate == promoImport.StartDate && g.EndDate == promoImport.EndDate && g.ClientTreeId == promoImport.ClientTreeId && g.BrandTechId == promoImport.BrandTechId && g.MarsMechanicDiscount == promoImport.MarsMechanicDiscount && g.PromoTypesId == promoImport.PromoTypesId && g.ProductSubrangesList == promoImport.ProductSubrangesList && g.DispatchesStart == promoImport.DispatchesStart && g.DispatchesEnd==promoImport.DispatchesEnd);
                if (present != null)
                {
                    var dublicate = sourceTemplateRecords.FirstOrDefault(g => g.PromoStartDate == promoImport.StartDate && g.PromoEndDate == promoImport.EndDate && g.ClientHierarchyCode == promoImport.ClientTreeId && g.BrandTech == brandTeches.FirstOrDefault(j => j.Id == promoImport.BrandTechId).Name && g.Discount == promoImport.MarsMechanicDiscount && g.Subrange == promoImport.ProductSubrangesList);
                    errors.Add("Duplicate present in client:" + dublicate.Client + ", startdate:" + dublicate.PromoStartDate + " enddate:" + dublicate.PromoEndDate);
                    HasErrors = true;
                    errorRecords.Add(new Tuple<IEntity<Guid>, string>(sourceTemplateRecords.FirstOrDefault(g => g.Client == dublicate.Client && g.PromoStartDate == dublicate.PromoStartDate && g.PromoEndDate == dublicate.PromoEndDate), string.Join(", ", errors)));
                }
            }


            return promos;
        }

        private int InsertDataToDatabase(List<Promo> promos, DatabaseContext context)
        {
            context.Set<Promo>().AddRange(promos);
            context.SaveChanges();
            // записать в TLCImport
            if (promos.Count > 0)
            {
                DateTimeOffset date = TimeHelper.Now();
                List<TLCImport> tLCImports = promos.Select(g => new TLCImport { PromoId = g.Id, LoadDate = date, HandlerId = HandlerId }).ToList();
                context.Set<TLCImport>().AddRange(tLCImports);
                List<PromoStatusChange> promoStatusChanges = promos.Select(g => new PromoStatusChange { PromoId = g.Id, StatusId = g.PromoStatusId, Date = TimeHelper.Now(), UserId = UserId, RoleId = RoleId }).ToList();
                context.Set<PromoStatusChange>().AddRange(promoStatusChanges);
                context.SaveChanges();
                return promos.Count;
            }
            return 0;

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

        private List<Promo> CreatePromoesFromTLCclosed(DatabaseContext context, IList<IEntity<Guid>> sourceRecords, string username, List<ClientTree> existingClientTreeIds, out IList<string> errors)
        {
            errors = new List<string>();
            List<Promo> promos = new List<Promo>();
            List<ImportRpaTLCclosed> sourceTemplateRecords = sourceRecords
                            .Select(sr => (sr as ImportRpaTLCclosed)).ToList();
            List<ClientTree> clientTrees = context.Set<ClientTree>().Where(g => g.EndDate == null).ToList();
            List<PromoTypes> promoTypes = context.Set<PromoTypes>().Where(g => !g.Disabled && (g.SystemName == "Regular" || g.SystemName == "InOut")).ToList();
            List<Mechanic> mechanics = context.Set<Mechanic>().Where(g => !g.Disabled).ToList();
            List<MechanicType> mechanicTypes = context.Set<MechanicType>().Where(g => !g.Disabled).ToList();
            List<BrandTech> brandTeches = context.Set<BrandTech>().Where(g => !g.Disabled).ToList();
            PromoStatus promoStatus = context.Set<PromoStatus>().FirstOrDefault(g => g.SystemName == "Closed");
            foreach (ImportRpaTLCclosed import in sourceTemplateRecords)
            {
                ClientTree clientTree = clientTrees.Where(x => x.EndDate == null && x.ObjectId == import.ClientHierarchyCode).FirstOrDefault();
                if (clientTree == null)
                {
                    errors.Add("Client not found: " + clientTree.FullPathName);
                    HasErrors = true;
                    errorRecords.Add(new Tuple<IEntity<Guid>, string>(import, String.Join(", ", errors)));
                    break;
                }
                if (!existingClientTreeIds.Any(x => x.ObjectId == clientTree.ObjectId))
                {
                    errors.Add("No access to the client " + clientTree.FullPathName);
                    HasErrors = true;
                    errorRecords.Add(new Tuple<IEntity<Guid>, string>(import, string.Join(", ", errors)));
                }
                var btId = brandTeches.FirstOrDefault(g => g.BrandsegTechsub.Equals(import.BrandTech, StringComparison.OrdinalIgnoreCase))?.Id;
                if (btId == null)
                {
                    errors.Add("BrandTech not found: " + import.BrandTech);
                    HasErrors = true;
                    errorRecords.Add(new Tuple<IEntity<Guid>, string>(import, String.Join(", ", errors)));
                    break;
                }
                Promo promo = new Promo
                {
                    CreatorLogin = username,
                    LoadFromTLC = true,
                    PromoTypesId = promoTypes.FirstOrDefault(g => g.Name == import.PromoType).Id,
                    PromoStatusId = promoStatus.Id,
                    ClientHierarchy = clientTree.FullPathName,
                    ClientTreeId = clientTree.ObjectId,
                    ClientTreeKeyId = clientTree.Id,
                    BrandTechId = btId,
                    ProductSubrangesList = import.Subrange,
                    StartDate = import.PromoStartDate,
                    EndDate = import.PromoEndDate,
                    DispatchesStart= import.DispatchStartDate,
                    DispatchesEnd=import.DispatchEndDate,
                    BudgetYear = import.BudgetYear,
                    PromoDuration = import.PromoDuration,
                    PlanPromoBaselineLSV = import.PlanPromoBaselineLSV,
                    PlanPromoIncrementalLSV = import.PlanPromoIncrementalLSV,
                    PlanPromoLSV = import.PlanPromoLSV,
                    PlanPromoUpliftPercent = import.PlanPromoUpliftPercent,
                    PlanPromoTIShopper = import.PlanPromoTIShopper,
                    PlanPromoTIMarketing = import.PlanPromoTIMarketing,
                    PlanPromoXSites = import.PlanPromoXSites,
                    PlanPromoCatalogue = import.PlanPromoCatalogue,
                    PlanPromoPOSMInClient = import.PlanPromoPOSMInClient,
                    PlanPromoBranding = import.PlanPromoBranding,
                    PlanPromoBTL = import.PlanPromoBTL,
                    PlanPromoCostProduction = import.PlanPromoCostProduction,
                    PlanPromoCostProdXSites = import.PlanPromoCostProdXSites,
                    PlanPromoCostProdCatalogue = import.PlanPromoCostProdCatalogue,
                    PlanPromoCostProdPOSMInClient = import.PlanPromoCostProdPOSMInClient,
                    PlanPromoCost = import.PlanPromoCost,
                    PlanPromoIncrementalBaseTI = import.PlanPromoIncrementalBaseTI,
                    PlanPromoNetIncrementalBaseTI = import.PlanPromoNetIncrementalBaseTI,
                    PlanPromoIncrementalCOGS = import.PlanPromoIncrementalCOGSLSV,
                    PlanPromoNetIncrementalCOGS = import.PlanPromoNetIncrementalCOGSLSV,
                    PlanPromoIncrementalCOGSTn = import.PlanPromoIncrementalCOGStn,
                    PlanPromoNetIncrementalCOGSTn = import.PlanPromoNetIncrementalCOGStn,
                    PlanPromoIncrementalEarningsLSV = import.PlanPromoIncrementalEarningsLSV,
                    PlanPromoNetIncrementalEarningsLSV = import.PlanPromoNetIncrementalEarningsLSV,
                    ActualPromoIncrementalEarningsLSV = import.ActualPromoIncrementalEarningsLSV,
                    ActualPromoNetIncrementalEarningsLSV = import.ActualPromoNetIncrementalEarningsLSV,
                    PlanPromoROIPercentLSV = import.PlanPromoROILSV,
                    PlanPromoNetROIPercentLSV = import.PlanPromoNetROILSV,
                    ActualPromoROIPercentLSV = import.ActualPromoROILSV,
                    ActualPromoNetROIPercentLSV = import.ActualPromoNetROILSV,
                    PlanPromoTotalCost = import.PlanPromoTotalCost,
                    PlanPromoPostPromoEffectLSV = import.PlanPostPromoEffectLSV,
                    PlanPromoNetIncrementalLSV = import.PlanPromoNetIncrementalLSV,
                    PlanPromoNetLSV = import.PlanPromoNetLSV,
                    PlanPromoBaselineBaseTI = import.PlanPromoBaselineBaseTI,
                    PlanPromoBaseTI = import.PlanPromoBaseTI,
                    PlanPromoNetBaseTI = import.PlanPromoNetBaseTI,
                    PlanPromoNSV = import.PlanPromoNSV,
                    PlanPromoNetNSV = import.PlanPromoNetNSV,
                    PlanPromoIncrementalNSV = import.PlanPromoIncrementalNSV,
                    PlanPromoNetIncrementalNSV = import.PlanPromoNetIncrementalNSV,
                    PlanPromoIncrementalMAC = import.PlanPromoIncrementalMAC,
                    PlanPromoIncrementalMACLSV = import.PlanPromoIncrementalMACLSV,
                    PlanPromoNetIncrementalMAC = import.PlanPromoNetIncrementalMAC,
                    PlanPromoNetIncrementalMACLSV = import.PlanPromoNetIncrementalMACLSV,
                    PlanPromoIncrementalEarnings = import.PlanPromoIncrementalEarnings,
                    PlanPromoNetIncrementalEarnings = import.PlanPromoNetIncrementalEarnings,
                    PlanPromoROIPercent = import.PlanPromoROI,
                    PlanPromoNetROIPercent = import.PlanPromoNetROI,
                    PlanPromoNetUpliftPercent = import.PlanPromoNetUplift,
                    PlanAddTIShopperApproved = import.PlanAddTIShopperApproved,
                    PlanAddTIShopperCalculated = import.PlanAddTIShopperCalculated,
                    PlanAddTIMarketingApproved = import.PlanAddTIMarketingApproved,
                    ActualInStoreShelfPrice = import.ActualInstoreShelfPrice,
                    InvoiceNumber = import.Invoicenumber,
                    ActualPromoBaselineLSV = import.ActualPromoBaselineLSV,
                    ActualPromoIncrementalLSV = import.ActualPromoIncrementalLSV,
                    ActualPromoLSVByCompensation = import.ActualPromoLSVByCompensation,
                    ActualPromoLSV = import.ActualPromoLSV,
                    ActualPromoUpliftPercent = import.ActualPromoUplift,
                    ActualPromoNetUpliftPercent = import.ActualPromoNetUpliftPercent,
                    ActualPromoTIShopper = import.ActualPromoTIShopper,
                    ActualPromoTIMarketing = import.ActualPromoTIMarketing,
                    ActualPromoXSites = import.ActualPromoProdXSites,
                    ActualPromoCatalogue = import.ActualPromoProdCatalogue,
                    ActualPromoPOSMInClient = import.ActualPromoProdPOSMInClient,
                    ActualPromoBranding = import.ActualPromoBranding,
                    ActualPromoBTL = import.ActualPromoBTL,
                    ActualPromoCostProduction = import.ActualPromoCostProduction,
                    ActualPromoCostProdXSites = import.ActualPromoCostProdXSites,
                    ActualPromoCostProdCatalogue = import.ActualPromoCostProdCatalogue,
                    ActualPromoCostProdPOSMInClient = import.ActualPromoCostProdPOSMInClient,
                    ActualPromoCost = import.ActualPromoCost,
                    ActualPromoIncrementalBaseTI = import.ActualPromoIncrementalBaseTI,
                    ActualPromoNetIncrementalBaseTI = import.ActualPromoNetIncrementalBaseTI,
                    ActualPromoIncrementalCOGS = import.ActualPromoIncrementalCOGSLSV,
                    ActualPromoNetIncrementalCOGS = import.ActualPromoNetIncrementalCOGSLSV,
                    ActualPromoIncrementalCOGSTn = import.ActualPromoIncrementalCOGStn,
                    ActualPromoNetIncrementalCOGSTn = import.ActualPromoNetIncrementalCOGStn,
                    ActualPromoTotalCost = import.ActualPromoTotalCost,
                    ActualPromoPostPromoEffectLSV = import.ActualPostPromoEffectLSV,
                    ActualPromoNetIncrementalLSV = import.ActualPromoNetIncrementalLSV,
                    ActualPromoNetLSV = import.ActualPromoNetLSV,
                    ActualPromoIncrementalNSV = import.ActualPromoIncrementalNSV,
                    ActualPromoNetIncrementalNSV = import.ActualPromoNetIncrementalNSV,
                    ActualPromoBaselineBaseTI = import.ActualPromoBaselineBaseTI,
                    ActualPromoBaseTI = import.ActualPromoBaseTI,
                    ActualPromoNetBaseTI = import.ActualPromoNetBaseTI,
                    ActualPromoNSV = import.ActualPromoNSV,
                    ActualPromoNetNSV = import.ActualPromoNetNSV,
                    ActualPromoIncrementalMAC = import.ActualPromoIncrementalMAC,
                    ActualPromoIncrementalMACLSV = import.ActualPromoIncrementalMACLSV,
                    ActualPromoNetIncrementalMAC = import.ActualPromoNetIncrementalMAC,
                    ActualPromoNetIncrementalMACLSV = import.ActualPromoNetIncrementalMACLSV,
                    ActualPromoIncrementalEarnings = import.ActualPromoIncrementalEarnings,
                    ActualPromoNetIncrementalEarnings = import.ActualPromoNetIncrementalEarnings,
                    ActualPromoROIPercent = import.ActualPromoROI,
                    ActualPromoNetROIPercent = import.ActualPromoNetROI,
                    ActualAddTIShopper = import.ActualAddTIShopper,
                    ActualAddTIMarketing = import.ActualAddTIMarketing,
                    SumInvoice = import.SumInInvoice,
                    InOut = import.PromoType == "InOut",
                };
                promo = SetDispatchDates(clientTree, import.PromoStartDate, import.PromoEndDate, promo);
                if (!CheckBudgetYear((DateTimeOffset)promo.DispatchesStart, (int)promo.BudgetYear))
                {
                    errors.Add("Wrong BudgetYear " + import.BudgetYear);
                    HasErrors = true;
                    errorRecords.Add(new Tuple<IEntity<Guid>, string>(import, string.Join(", ", errors)));
                }
                if (TimeHelper.TodayEndDay() < promo.EndDate)
                {
                    errors.Add("End date later than the current day " + import.PromoEndDate);
                    HasErrors = true;
                    errorRecords.Add(new Tuple<IEntity<Guid>, string>(import, string.Join(", ", errors)));
                }
                if (promo.DispatchesStart > promo.DispatchesEnd)
                {
                    errors.Add("Dispatches End Date before Dispatches start Date " + import.DispatchStartDate);
                    HasErrors = true;
                    errorRecords.Add(new Tuple<IEntity<Guid>, string>(import, string.Join(", ", errors)));
                    break;
                }
                Mechanic mechanic = mechanics.FirstOrDefault(g => g.SystemName == import.Mechanic && g.PromoTypesId == promo.PromoTypesId);
                if (mechanic == null)
                {
                    errors.Add("Mechanic not available: " + import.Mechanic);
                    HasErrors = true;
                    errorRecords.Add(new Tuple<IEntity<Guid>, string>(import, String.Join(", ", errors)));
                    break;
                }
                promo.MarsMechanicId = mechanic.Id;
                promo.MarsMechanicDiscount = import.Discount;
                MechanicType mechanicType = mechanicTypes.FirstOrDefault(g => g.Name == import.MechanicType);
                promo.MarsMechanicTypeId = mechanicType.Id;
                promo.MechanicComment = import.MechanicComment;
                if (!string.IsNullOrEmpty(import.ActualInStoreMechanicName))
                {
                    promo.ActualInStoreMechanicId = mechanics.FirstOrDefault(g => g.SystemName == import.ActualInStoreMechanicName && g.PromoTypesId == promo.PromoTypesId).Id;
                    promo.ActualInStoreMechanicTypeId = mechanicTypes.FirstOrDefault(g => g.Name == import.ActualInStoreMechanicTypeName).Id;
                    promo.ActualInStoreDiscount = import.ActualInStoreMechanicDiscount;
                }

                promos.Add(promo);
            }

            return promos;
        }
        private bool CheckBudgetYear(DateTimeOffset dispatchStartDate, int budgetYear)
        {
            List<int> buggetYears = TimeHelper.GetBudgetYears(dispatchStartDate);

            if (buggetYears.Contains(budgetYear))
            {
                return true;
            }
            return false;
        }
    }
}
