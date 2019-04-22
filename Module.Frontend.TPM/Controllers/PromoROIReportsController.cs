using AutoMapper;
using Core.Security;
using Core.Security.Models;
using Core.Settings;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions.Export;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Model.DTO;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;
using Module.Persist.TPM.Utils;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Module.Frontend.TPM.Controllers {

    public class PromoROIReportsController : EFContextController {
        private readonly IAuthorizationManager authorizationManager;

        public PromoROIReportsController(IAuthorizationManager authorizationManager) {
            this.authorizationManager = authorizationManager;
        }

        //[ClaimsAuthorize]
        //[EnableQuery(MaxNodeCount = int.MaxValue)]
        //public SingleResult<PlanPostPromoEffectReport> GetPlanPostPromoEffectReport([FromODataUri] System.Guid key) {
        //    return SingleResult.Create(GetConstraintedQuery());
        //}

        public IQueryable<PromoROIReport> GetConstraintedQuery() {
            List<PromoROIReport> result = new List<PromoROIReport>();
            List<Promo> promos = Context.Set<Promo>().Where(y => !y.Disabled).ToList();
            DateTime dt = DateTime.Now;
            foreach (Promo promo in promos) {
                result.Add(ReportCreate(promo));
            }
            return result.AsQueryable();
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<PromoROIReport> GetPromoROIReports() {
            return GetConstraintedQuery();
        }

        private IEnumerable<Column> GetExportSettings() {
            IEnumerable<Column> columns = new List<Column>() {
                new Column { Order = 1, Field = "Number", Header = "Promo ID", Quoting = false },
                new Column { Order = 2, Field = "Client1Level", Header = "NA/RKA", Quoting = false },
                new Column { Order = 3, Field = "Client2Level", Header = "Client Group", Quoting = false },
                new Column { Order = 4, Field = "Client3Level", Header = "Client", Quoting = false },
                new Column { Order = 5, Field = "Product1Level", Header = "Brand", Quoting = false },
                new Column { Order = 6, Field = "Product2Level", Header = "Technology", Quoting = false },
                new Column { Order = 7, Field = "Product3Level", Header = "Subrange", Quoting = false },
                new Column { Order = 8, Field = "MarsMechanicName", Header = "Mars mechanic", Quoting = false },
                new Column { Order = 9, Field = "MarsMechanicTypeName", Header = "Mars mechanic type", Quoting = false },
                new Column { Order = 10, Field = "MarsMechanicDiscount", Header = "Mars mechanic discount, %", Quoting = false },
                new Column { Order = 11, Field = "MechanicComment", Header = "Mechanic comment", Quoting = false },
                new Column { Order = 12, Field = "StartDate", Header = "Start date", Quoting = false, Format = "dd.MM.yyyy"  },
                new Column { Order = 13, Field = "EndDate", Header = "End date", Quoting = false, Format = "dd.MM.yyyy"  },
                new Column { Order = 14, Field = "PromoDuration", Header = "Promo duration", Quoting = false , Format = "0"},
                new Column { Order = 15, Field = "DispatchDuration", Header = "Dispatch Duration", Quoting = false, Format = "0" },
                new Column { Order = 16, Field = "EventName", Header = "Event", Quoting = false },
                new Column { Order = 17, Field = "PromoStatusName", Header = "Status", Quoting = false },
                new Column { Order = 18, Field = "PlanInstoreMechanicName", Header = "Plan Instore Mechanic Name", Quoting = false },
                new Column { Order = 19, Field = "PlanInstoreMechanicTypeName", Header = "Plan Instore Mechanic Type Name", Quoting = false },
                new Column { Order = 20, Field = "PlanInstoreMechanicDiscount", Header = "Plan Instore Mechanic Discount", Quoting = false,  Format = "0"  },
                new Column { Order = 21, Field = "PlanPromoBaselineLSV", Header = "Plan Promo Baseline LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 22, Field = "PlanPromoIncrementalLSV", Header = "Plan Promo Incremental LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 23, Field = "PlanPromoLSV", Header = "Plan Promo LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 24, Field = "PlanPromoUpliftPercent", Header = "Plan Promo Uplift %", Quoting = false,  Format = "0.00"  },
                new Column { Order = 25, Field = "PlanPromoTIShopper", Header = "Plan Promo TI Shopper", Quoting = false,  Format = "0.00"  },
                new Column { Order = 26, Field = "PlanPromoTIMarketing", Header = "Plan Promo TI Marketing", Quoting = false,  Format = "0.00"  },
                new Column { Order = 27, Field = "PlanPromoXSites", Header = "Plan Promo X-Sites", Quoting = false,  Format = "0.00"  },
                new Column { Order = 28, Field = "PlanPromoCatalogue", Header = "Plan Promo Catalogue", Quoting = false,  Format = "0.00"  },
                new Column { Order = 29, Field = "PlanPromoPOSMInClient", Header = "Plan Promo POSM In Client", Quoting = false,  Format = "0.00"  },
                new Column { Order = 30, Field = "PlanPromoBranding", Header = "Plan Promo Branding", Quoting = false,  Format = "0.00"  },
                new Column { Order = 31, Field = "PlanPromoBTL", Header = "Plan Promo BTL", Quoting = false,  Format = "0.00"  },
                new Column { Order = 32, Field = "PlanPromoCostProduction", Header = "Plan Promo Cost Production", Quoting = false,  Format = "0.00"  },
                new Column { Order = 33, Field = "PlanPromoCostProdXSites", Header = "Plan PromoCostProdXSites", Quoting = false,  Format = "0.00"  },
                new Column { Order = 34, Field = "PlanPromoCostProdCatalogue", Header = "Plan PromoCostProdCatalogue", Quoting = false,  Format = "0.00"  },
                new Column { Order = 35, Field = "PlanPromoCostProdPOSMInClient", Header = "Plan PromoCostProdPOSMInClient", Quoting = false,  Format = "0.00"  },
                new Column { Order = 36, Field = "PlanPromoCost", Header = "Plan Promo Cost", Quoting = false,  Format = "0.00"  },
                new Column { Order = 37, Field = "PlanPromoIncrementalBaseTI", Header = "Plan Promo Incremental BaseTI", Quoting = false,  Format = "0.00"  },
                new Column { Order = 38, Field = "PlanPromoIncrementalCOGS", Header = "Plan Promo Incremental COGS", Quoting = false,  Format = "0.00"  },
                new Column { Order = 39, Field = "PlanPromoTotalCost", Header = "Plan Promo Total Cost", Quoting = false,  Format = "0.00"  },
                new Column { Order = 40, Field = "PlanPostPromoEffectW1", Header = "Plan Post Promo Effect LSV W1", Quoting = false,  Format = "0.00"  },
                new Column { Order = 41, Field = "PlanPostPromoEffectW2", Header = "Plan Post Promo Effect LSV W2", Quoting = false,  Format = "0.00"  },
                new Column { Order = 42, Field = "PlanPostPromoEffect", Header = "Plan Post Promo Effect LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 43, Field = "PlanPromoNetIncrementalLSV", Header = "Plan Promo Net Incremental LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 44, Field = "PlanPromoNetLSV", Header = "PlanPromo Net LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 45, Field = "PlanPromoBaselineBaseTI", Header = "Plan Promo Baseline Base TI", Quoting = false,  Format = "0.00"  },
                new Column { Order = 46, Field = "PlanPromoBaseTI", Header = "Plan Promo Base TI", Quoting = false,  Format = "0.00"  },
                new Column { Order = 47, Field = "PlanPromoNetNSV", Header = "Plan Promo Net NSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 48, Field = "PlanPromoIncrementalNSV", Header = "Plan Promo Total Net NSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 49, Field = "PlanPromoNetIncrementalNSV", Header = "Plan Promo Incremental NSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 50, Field = "PlanPromoIncrementalMAC", Header = "Plan Promo Incremental MAC", Quoting = false,  Format = "0.00"  },
                new Column { Order = 51, Field = "PlanPromoNetIncrementalMAC", Header = "Plan Promo Net Incremental MAC", Quoting = false,  Format = "0.00"  },
                new Column { Order = 52, Field = "PlanPromoIncrementalEarnings", Header = "Plan Promo Incremental Earnings", Quoting = false,  Format = "0.00"  },
                new Column { Order = 53, Field = "PlanPromoNetIncrementalEarnings", Header = "Plan Promo Net Incremental Earnings", Quoting = false,  Format = "0.00"  },
                new Column { Order = 54, Field = "PlanPromoROIPercent", Header = "Plan Promo ROI, %", Quoting = false,  Format = "0"  },
                new Column { Order = 55, Field = "PlanPromoNetROIPercent", Header = "Plan Promo Net ROI, %", Quoting = false,  Format = "0"  },
                new Column { Order = 56, Field = "PlanPromoNetUpliftPercent", Header = "Plan Promo Net Uplift %", Quoting = false,  Format = "0"  },
                new Column { Order = 57, Field = "ActualInStoreMechanicName", Header = "Actual InStore Mechanic Name", Quoting = false },
                new Column { Order = 58, Field = "ActualInStoreMechanicTypeName", Header = "Actual InStore Mechanic Type Name", Quoting = false  },
                new Column { Order = 59, Field = "ActualInStoreMechanicDiscount", Header = "Actual InStore Mechanic Discount", Quoting = false,  Format = "0"  },
                new Column { Order = 60, Field = "ActualInStoreShelfPrice", Header = "Instore Shelf Price", Quoting = false,  Format = "0.00"  },
                new Column { Order = 61, Field = "InvoiceNumber", Header = "Invoice number", Quoting = false },
                new Column { Order = 62, Field = "ActualPromoBaselineLSV", Header = "Actual Promo Baseline LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 63, Field = "ActualPromoIncrementalLSV", Header = "Actual Promo Incremental LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 64, Field = "ActualPromoLSV", Header = "Actual Promo LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 65, Field = "ActualPromoUpliftPercent", Header = "Actual Promo Uplift %", Quoting = false,  Format = "0.00"  },
                new Column { Order = 66, Field = "ActualPromoTIShopper", Header = "Actual Promo TI Shopper", Quoting = false,  Format = "0.00"  },
                new Column { Order = 67, Field = "ActualPromoTIMarketing", Header = "Actual Promo TI Marketing", Quoting = false,  Format = "0.00"  },
                new Column { Order = 68, Field = "ActualPromoProdXSites", Header = "Actual Promo Prod XSites", Quoting = false,  Format = "0.00"  },
                new Column { Order = 69, Field = "ActualPromoProdCatalogue", Header = "Actual Promo Prod Catalogue", Quoting = false,  Format = "0.00"  },
                new Column { Order = 70, Field = "ActualPromoProdPOSMInClient", Header = "Actual Promo Prod POSMInClient", Quoting = false,  Format = "0.00"  },
                new Column { Order = 71, Field = "ActualPromoBranding", Header = "Actual Promo Branding", Quoting = false,  Format = "0.00"  },
                new Column { Order = 72, Field = "ActualPromoBTL", Header = "Actual Promo BTL", Quoting = false,  Format = "0.00"  },
                new Column { Order = 73, Field = "ActualPromoCostProduction", Header = "Actual Promo Cost Production", Quoting = false,  Format = "0.00"  },
                new Column { Order = 74, Field = "ActualPromoCostProdXSites", Header = "Actual Promo CostProdXSites", Quoting = false,  Format = "0.00"  },
                new Column { Order = 75, Field = "ActualPromoCostProdCatalogue", Header = "Actual Promo Cost ProdCatalogue", Quoting = false,  Format = "0.00"  },
                new Column { Order = 76, Field = "ActualPromoCostProdPOSMInClient", Header = "Actual Promo Cost ProdPOSMInClient", Quoting = false,  Format = "0.00"  },
                new Column { Order = 77, Field = "ActualPromoCost", Header = "Actual Promo Cost", Quoting = false,  Format = "0.00"  },
                new Column { Order = 78, Field = "ActualPromoIncrementalBaseTI", Header = "Actual Promo Incremental BaseTI", Quoting = false,  Format = "0.00"  },
                new Column { Order = 79, Field = "ActualPromoIncrementalCOGS", Header = "Actual Promo Incremental COGS", Quoting = false,  Format = "0.00"  },
                new Column { Order = 80, Field = "ActualPromoTotalCost", Header = "Actual Promo Total Cost", Quoting = false,  Format = "0.00"  },
                new Column { Order = 81, Field = "FactPostPromoEffectW1", Header = "Actual Post Promo Effect LSV W1", Quoting = false,  Format = "0.00"  },
                new Column { Order = 82, Field = "FactPostPromoEffectW2", Header = "Actual Post Promo Effect LSV W2", Quoting = false,  Format = "0.00"  },
                new Column { Order = 83, Field = "FactPostPromoEffect", Header = "Actual Post Promo Effect LSV", Quoting = false,  Format = "0"  },
                new Column { Order = 84, Field = "ActualPromoNetIncrementalLSV", Header = "Actual Promo Net Incremental LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 85, Field = "ActualPromoNetLSV", Header = "Actual Promo Net LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 86, Field = "ActualPromoIncrementalNSV", Header = "Actual Promo Incremental NSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 87, Field = "ActualPromoNetIncrementalNSV", Header = "Actual Promo Net Incremental NSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 88, Field = "ActualPromoBaselineBaseTI", Header = "Actual Promo Baseline Base TI", Quoting = false,  Format = "0.00"  },
                new Column { Order = 89, Field = "ActualPromoBaseTI", Header = "Actual Promo Base TI", Quoting = false,  Format = "0.00"  },
                new Column { Order = 90, Field = "ActualPromoNetNSV", Header = "Actual Promo Net NSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 91, Field = "ActualPromoIncrementalMAC", Header = "Actual Promo Incremental MAC", Quoting = false,  Format = "0.00"  },
                new Column { Order = 92, Field = "ActualPromoNetIncrementalMAC", Header = "Actual Promo Net Incremental MAC", Quoting = false,  Format = "0.00"  },
                new Column { Order = 93, Field = "ActualPromoIncrementalEarnings", Header = "Actual Promo Incremental Earnings", Quoting = false,  Format = "0.00"  },
                new Column { Order = 94, Field = "ActualPromoNetIncrementalEarnings", Header = "Actual Promo Net Incremental Earnings", Quoting = false,  Format = "0.00"  },
                new Column { Order = 95, Field = "ActualPromoROIPercent", Header = "Actual Promo ROI, %", Quoting = false,  Format = "0"  },
                new Column { Order = 96, Field = "ActualPromoNetROIPercent", Header = "Actual Promo Net ROI%", Quoting = false,  Format = "0"  },
                new Column { Order = 97, Field = "ActualPromoNetUpliftPercent", Header = "Actual Promo Net Uplift Percent", Quoting = false,  Format = "0"  }};                        
            return columns;           
        }
        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<PromoROIReport> options) {
            try {
                IQueryable results = options.ApplyTo(GetConstraintedQuery());
                IEnumerable<Column> columns = GetExportSettings();
                NonGuidIdExporter exporter = new NonGuidIdExporter(columns);
                UserInfo user = authorizationManager.GetCurrentUser();
                string username = user == null ? "" : user.Login;
                string filePath = exporter.GetExportFileName("PromoROIReport", username);
                exporter.Export(results, filePath);
                string filename = System.IO.Path.GetFileName(filePath);
                return Content<string>(HttpStatusCode.OK, filename);
            } catch (Exception e) {
                return Content<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private PromoROIReport ReportCreate(Promo promo) {
            PromoROIReport rep = new PromoROIReport();
            rep.Id = Guid.NewGuid();
            rep.Number = promo.Number;


            //SetTreeFields(promo, rep);

            rep.MarsMechanicName = promo.MarsMechanic == null ? null : promo.MarsMechanic.Name;
            rep.MarsMechanicTypeName = promo.MarsMechanicType == null ? null : promo.MarsMechanicType.Name;
            rep.MarsMechanicDiscount = promo.MarsMechanicType == null ? null : promo.MarsMechanicType.Discount;
            rep.MechanicComment = promo.MechanicComment;

            rep.StartDate = promo.StartDate;
            rep.EndDate = promo.EndDate;
            rep.PromoDuration = promo.PromoDuration;

            rep.DispatchDuration = promo.DispatchDuration;

            rep.EventName = promo.EventName;
            rep.PromoStatusName = promo.PromoStatus == null ? null : promo.PromoStatus.Name;

            rep.PlanInstoreMechanicName = promo.PlanInstoreMechanic == null ? null : promo.PlanInstoreMechanic.Name;
            rep.PlanInstoreMechanicTypeName = promo.PlanInstoreMechanicType == null ? null : promo.PlanInstoreMechanicType.Name;
            rep.PlanInstoreMechanicDiscount = promo.PlanInstoreMechanicType == null ? null : promo.PlanInstoreMechanicType.Discount;

            rep.PlanPromoBaselineLSV = promo.PlanPromoBaselineLSV;
            rep.PlanPromoIncrementalLSV = promo.PlanPromoIncrementalLSV;
            rep.PlanPromoLSV = promo.PlanPromoLSV;
            rep.PlanPromoUpliftPercent = promo.PlanPromoUpliftPercent;
            rep.PlanPromoTIShopper = promo.PlanPromoTIShopper;
            rep.PlanPromoTIMarketing = promo.PlanPromoTIMarketing;
            rep.PlanPromoXSites = promo.PlanPromoXSites;
            rep.PlanPromoCatalogue = promo.PlanPromoCatalogue;
            rep.PlanPromoPOSMInClient = promo.PlanPromoPOSMInClient;
            rep.PlanPromoBranding = promo.PlanPromoBranding;
            rep.PlanPromoBTL = promo.PlanPromoBTL;
            rep.PlanPromoCostProduction = promo.PlanPromoCostProduction;
            rep.PlanPromoCostProdXSites = promo.PlanPromoCostProdXSites;
            rep.PlanPromoCostProdCatalogue = promo.PlanPromoCostProdCatalogue;
            rep.PlanPromoCostProdPOSMInClient = promo.PlanPromoCostProdPOSMInClient;
            rep.PlanPromoCost = promo.PlanPromoCost;
            rep.PlanPromoIncrementalBaseTI = promo.PlanPromoIncrementalBaseTI;

            rep.PlanPromoIncrementalCOGS = promo.PlanPromoIncrementalCOGS;
            rep.PlanPromoTotalCost = promo.PlanPromoTotalCost;
            rep.PlanPostPromoEffectW1 = promo.PlanPostPromoEffectW1;
            rep.PlanPostPromoEffectW2 = promo.PlanPostPromoEffectW2;
            rep.PlanPostPromoEffect = promo.PlanPostPromoEffect;
            rep.PlanPromoNetIncrementalLSV = promo.PlanPromoNetIncrementalLSV;
            rep.PlanPromoNetLSV = promo.PlanPromoNetLSV;

            rep.PlanPromoBaselineBaseTI = promo.PlanPromoBaselineBaseTI;
            rep.PlanPromoBaseTI = promo.PlanPromoBaseTI;
            rep.PlanPromoNetNSV = promo.PlanPromoNetNSV;

            rep.PlanPromoIncrementalNSV = promo.PlanPromoIncrementalNSV;
            rep.PlanPromoNetIncrementalNSV = promo.PlanPromoNetIncrementalNSV;
            rep.PlanPromoIncrementalMAC = promo.PlanPromoIncrementalMAC;
            rep.PlanPromoNetIncrementalMAC = promo.PlanPromoNetIncrementalMAC;
            rep.PlanPromoIncrementalEarnings = promo.PlanPromoIncrementalEarnings;
            rep.PlanPromoNetIncrementalEarnings = promo.PlanPromoNetIncrementalEarnings;
            rep.PlanPromoROIPercent = promo.PlanPromoROIPercent;
            rep.PlanPromoNetROIPercent = promo.PlanPromoNetROIPercent;
            rep.PlanPromoNetUpliftPercent = promo.PlanPromoNetUpliftPercent;

            rep.ActualInStoreMechanicName = promo.ActualInStoreMechanic == null ? null : promo.ActualInStoreMechanic.Name;
            rep.ActualInStoreMechanicTypeName = promo.ActualInStoreMechanicType == null ? null : promo.ActualInStoreMechanicType.Name;
            rep.ActualInStoreMechanicDiscount = promo.ActualInStoreMechanicType == null ? null : promo.ActualInStoreMechanicType.Discount;
            rep.ActualInStoreShelfPrice = promo.ActualInStoreShelfPrice;
            rep.InvoiceNumber = promo.InvoiceNumber;
            rep.ActualPromoBaselineLSV = promo.ActualPromoBaselineLSV;
            rep.ActualPromoIncrementalLSV = promo.ActualPromoIncrementalLSV;
            rep.ActualPromoLSV = promo.ActualPromoLSV;
            rep.ActualPromoUpliftPercent = promo.ActualPromoUpliftPercent;
            rep.ActualPromoTIShopper = promo.ActualPromoTIShopper;
            rep.ActualPromoTIMarketing = promo.ActualPromoTIMarketing;

            rep.ActualPromoProdXSites = promo.ActualPromoCostProdXSites;
            rep.ActualPromoProdCatalogue = promo.ActualPromoCostProdCatalogue;
            rep.ActualPromoProdPOSMInClient = promo.ActualPromoCostProdPOSMInClient;

            rep.ActualPromoXSites = promo.ActualPromoXSites;
            rep.ActualPromoCatalogue = promo.ActualPromoCatalogue;
            rep.ActualPromoPOSMInClient = promo.ActualPromoPOSMInClient;

            rep.ActualPromoBranding = promo.ActualPromoBranding;
            rep.ActualPromoBTL = promo.ActualPromoBTL;
            rep.ActualPromoCostProduction = promo.ActualPromoCostProduction;

            rep.ActualPromoCostProdXSites = promo.ActualPromoCostProdXSites;
            rep.ActualPromoCostProdCatalogue = promo.ActualPromoCostProdCatalogue;
            rep.ActualPromoCostProdPOSMInClient = promo.ActualPromoCostProdPOSMInClient;

            rep.ActualPromoCost = promo.ActualPromoCost;
            rep.ActualPromoIncrementalBaseTI = promo.ActualPromoIncrementalBaseTI;
            rep.ActualPromoIncrementalCOGS = promo.ActualPromoIncrementalCOGS;
            rep.ActualPromoTotalCost = promo.ActualPromoTotalCost;

            rep.FactPostPromoEffectW1 = promo.FactPostPromoEffectW1;
            rep.FactPostPromoEffectW2 = promo.FactPostPromoEffectW2;
            rep.FactPostPromoEffect = promo.FactPostPromoEffect;
            rep.ActualPromoNetIncrementalLSV = promo.ActualPromoNetIncrementalLSV;

            rep.ActualPromoNetLSV = promo.ActualPromoNetLSV;
            rep.ActualPromoIncrementalNSV = promo.ActualPromoIncrementalNSV;
            rep.ActualPromoNetIncrementalNSV = promo.ActualPromoNetIncrementalNSV;

            rep.ActualPromoBaselineBaseTI = promo.ActualPromoBaselineBaseTI;
            rep.ActualPromoBaseTI = promo.ActualPromoBaseTI;
            rep.ActualPromoNetNSV = promo.ActualPromoNetNSV;

            rep.ActualPromoIncrementalMAC = promo.ActualPromoIncrementalMAC;
            rep.ActualPromoNetIncrementalMAC = promo.ActualPromoNetIncrementalMAC;
            rep.ActualPromoIncrementalEarnings = promo.ActualPromoIncrementalEarnings;
            rep.ActualPromoNetIncrementalEarnings = promo.ActualPromoNetIncrementalEarnings;
            rep.ActualPromoROIPercent = promo.ActualPromoROIPercent;
            rep.ActualPromoNetROIPercent = promo.ActualPromoNetROIPercent;
            rep.ActualPromoNetUpliftPercent = promo.ActualPromoNetUpliftPercent;
            rep.Product1Level = promo.Brand != null ? promo.Brand.Name : null;
            rep.Product2Level = promo.Technology != null ? promo.Technology.Name : null;

            rep.Client1Level = promo.Client1LevelName;
            rep.Client2Level = promo.Client2LevelName;
            rep.Client3Level = promo.ClientName;
            rep.Product3Level = promo.ProductSubrangesList;

            return rep;
        }

        //private void SetTreeFields(Promo promo, PromoROIReport rep) {

        //    DateTime dt = DateTime.Now;
        //    IQueryable<ProductTree> ptQuery = Context.Set<ProductTree>().Where(x => x.Type == "root"
        //     || (DateTime.Compare(x.StartDate, dt) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dt) > 0)));

        //    IQueryable<ClientTree> ctQuery = Context.Set<ClientTree>().Where(x => x.Type == "root"
        //       || (DateTime.Compare(x.StartDate, dt) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dt) > 0)));



        //    IEnumerable<int> promoProducts = Context.Set<PromoProductTree>().Where(y => y.PromoId == promo.Id && !y.Disabled).Select(z => z.ProductTreeObjectId);
        //    IQueryable<ProductTree> pts = ptQuery.Where(y => promoProducts.Contains(y.ObjectId) && y.EndDate < dt || y.EndDate == null);
        //    rep.Product3Level = String.Join(";", pts.Select(z => z.Name));

        //    ClientTree ct = ctQuery.FirstOrDefault(y => y.ObjectId == promo.ClientTreeId);
        //    if (ct != null) {
        //        rep.Client3Level = ct.Name;
        //    }
        //    while (ct != null && ct.depth != 0) {
        //        if (ct.depth == 1) {
        //            rep.Client1Level = ct.Name;
        //        } else if (ct.depth == 2) {
        //            rep.Client2Level = ct.Name;
        //        }
        //        ct = ctQuery.FirstOrDefault(y => y.ObjectId == ct.parentId);
        //    }

        //}

        private bool EntityExists(Guid key) {
            return Context.Set<Promo>().Count(e => e.Id == key) > 0;
        }

    }

}
