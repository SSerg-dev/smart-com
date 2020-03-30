using Core.Security;
using Core.Security.Models;

using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions.Export;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;

using Persist;
using Persist.Model;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;

using Thinktecture.IdentityModel.Authorization.WebApi;

using Utility;

namespace Module.Frontend.TPM.Controllers
{
    public class PromoROIReportsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public PromoROIReportsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<PromoROIReport> GetConstraintedQuery()
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
			IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);

            var query = GetPromoROIReportsStatic(Context);			
			IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
			query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

			return query;
        }

        public static IQueryable<PromoROIReport> GetPromoROIReportsStatic(DatabaseContext databaseContext)
        {
            var query = databaseContext.Set<Promo>().Where(x => !x.Disabled).Select(x => new PromoROIReport
            {
                Id = x.Id,
                Number = x.Number,
                Client1LevelName = x.ClientHierarchy,
                Client2LevelName = x.ClientHierarchy,
                ClientName = databaseContext.Set<ClientTree>().Where(c => c.ObjectId == x.ClientTreeId).Select(v => v.Name).FirstOrDefault(),
                ClientTreeId = x.ClientTreeId,
                BrandName = x.Brand.Name ?? (x.BrandTech != null ? (x.BrandTech.Brand != null ? x.BrandTech.Brand.Name : null) : null),
                TechnologyName = x.Technology.Name ?? (x.BrandTech != null ? (x.BrandTech.Technology != null ? x.BrandTech.Technology.Name : null) : null),
                ProductSubrangesList = x.ProductSubrangesList,
                MarsMechanicName = x.MarsMechanic.Name,
                MarsMechanicTypeName = x.MarsMechanicType.Name,
                MarsMechanicDiscount = x.MarsMechanicDiscount,
                MechanicComment = x.MechanicComment,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                PromoDuration = DbFunctions.DiffDays(x.StartDate, x.EndDate) + 1,
                EventName = x.EventName,
                PromoStatusName = x.PromoStatus.Name,
                InOut = x.InOut,
                IsGrowthAcceleration = x.IsGrowthAcceleration,
                PlanInstoreMechanicName = x.PlanInstoreMechanic.Name,
                PlanInstoreMechanicTypeName = x.PlanInstoreMechanicType.Name,
                PlanInstoreMechanicDiscount = x.PlanInstoreMechanicDiscount,
                PlanInStoreShelfPrice = x.PlanInStoreShelfPrice,
                PCPrice = databaseContext.Set<PromoProduct>().Where(y => y.PromoId == x.Id && y.PlanProductPCPrice > 0 && !y.Disabled)
                    .Average(z => z.PlanProductPCPrice),
                PlanPromoBaselineLSV = x.PlanPromoBaselineLSV,
                PlanPromoIncrementalLSV = x.PlanPromoIncrementalLSV,
                PlanPromoLSV = x.PlanPromoLSV,
                PlanPromoUpliftPercent = x.PlanPromoUpliftPercent,
                PlanPromoTIShopper = x.PlanPromoTIShopper,
                PlanPromoTIMarketing = x.PlanPromoTIMarketing,
                PlanPromoXSites = x.PlanPromoXSites,
                PlanPromoCatalogue = x.PlanPromoCatalogue,
                PlanPromoPOSMInClient = x.PlanPromoPOSMInClient,
                PlanPromoBranding = x.PlanPromoBranding,
                PlanPromoBTL = x.PlanPromoBTL,
                PlanPromoCostProduction = x.PlanPromoCostProduction,
                PlanPromoCostProdXSites = x.PlanPromoCostProdXSites,
                PlanPromoCostProdCatalogue = x.PlanPromoCostProdCatalogue,
                PlanPromoCostProdPOSMInClient = x.PlanPromoCostProdPOSMInClient,
                PlanPromoCost = x.PlanPromoCost,
                TIBasePercent = x.PlanPromoIncrementalLSV.HasValue && x.PlanPromoIncrementalLSV.Value != 0 ?
                    x.PlanPromoIncrementalBaseTI / x.PlanPromoIncrementalLSV * 100 : null,
                PlanPromoIncrementalBaseTI = x.PlanPromoIncrementalBaseTI,
                PlanPromoNetIncrementalBaseTI = x.PlanPromoNetIncrementalBaseTI,
                COGSPercent = x.PlanPromoIncrementalLSV.HasValue && x.PlanPromoIncrementalLSV.Value != 0 ?
                    x.PlanPromoIncrementalCOGS / x.PlanPromoIncrementalLSV * 100 : null,
                PlanPromoIncrementalCOGS = x.PlanPromoIncrementalCOGS,
                PlanPromoNetIncrementalCOGS = x.PlanPromoNetIncrementalCOGS,
                PlanPromoTotalCost = x.PlanPromoTotalCost,
                PlanPromoPostPromoEffectLSV = x.PlanPromoPostPromoEffectLSV,
                PlanPromoNetIncrementalLSV = x.PlanPromoNetIncrementalLSV,
                PlanPromoNetLSV = x.PlanPromoNetLSV,
                PlanPromoBaselineBaseTI = x.PlanPromoBaselineBaseTI,
                PlanPromoBaseTI = x.PlanPromoBaseTI,
                PlanPromoNetBaseTI = x.PlanPromoNetBaseTI,
                PlanPromoNSV = x.PlanPromoNSV,
                PlanPromoNetNSV = x.PlanPromoNetNSV,
                PlanPromoIncrementalNSV = x.PlanPromoIncrementalNSV,
                PlanPromoNetIncrementalNSV = x.PlanPromoNetIncrementalNSV,
                PlanPromoIncrementalMAC = x.PlanPromoIncrementalMAC,
                PlanPromoNetIncrementalMAC = x.PlanPromoNetIncrementalMAC,
                PlanPromoIncrementalEarnings = x.PlanPromoIncrementalEarnings,
                PlanPromoNetIncrementalEarnings = x.PlanPromoNetIncrementalEarnings,
                PlanPromoROIPercent = x.PlanPromoROIPercent,
                PlanPromoNetROIPercent = x.PlanPromoNetROIPercent,
                PlanPromoNetUpliftPercent = x.PlanPromoNetUpliftPercent,
                ActualInStoreMechanicName = x.ActualInStoreMechanic.Name,
                ActualInStoreMechanicTypeName = x.ActualInStoreMechanicType.Name,
                ActualInStoreDiscount = x.ActualInStoreDiscount,
                ActualInStoreShelfPrice = x.ActualInStoreShelfPrice,
                InvoiceNumber = x.InvoiceNumber,
                ActualPromoBaselineLSV = x.ActualPromoBaselineLSV,
                ActualPromoIncrementalLSV = x.ActualPromoIncrementalLSV,
                ActualPromoLSVByCompensation = x.ActualPromoLSVByCompensation,
                ActualPromoLSV = x.ActualPromoLSV,
                ActualPromoUpliftPercent = x.ActualPromoUpliftPercent,
                ActualPromoNetUpliftPercent = x.ActualPromoNetUpliftPercent,
                ActualPromoTIShopper = x.ActualPromoTIShopper,
                ActualPromoTIMarketing = x.ActualPromoTIMarketing,
                ActualPromoXSites = x.ActualPromoXSites,
                ActualPromoCatalogue = x.ActualPromoCatalogue,
                ActualPromoPOSMInClient = x.ActualPromoPOSMInClient,
                ActualPromoBranding = x.ActualPromoBranding,
                ActualPromoBTL = x.ActualPromoBTL,
                ActualPromoCostProduction = x.ActualPromoCostProduction,
                ActualPromoCostProdXSites = x.ActualPromoCostProdXSites,
                ActualPromoCostProdCatalogue = x.ActualPromoCostProdCatalogue,
                ActualPromoCostProdPOSMInClient = x.ActualPromoCostProdPOSMInClient,
                ActualPromoCost = x.ActualPromoCost,
                ActualPromoIncrementalBaseTI = x.ActualPromoIncrementalBaseTI,
                ActualPromoNetIncrementalBaseTI = x.ActualPromoNetIncrementalBaseTI,
                ActualPromoIncrementalCOGS = x.ActualPromoIncrementalCOGS,
                ActualPromoNetIncrementalCOGS = x.ActualPromoNetIncrementalCOGS,
                ActualPromoTotalCost = x.ActualPromoTotalCost,
                ActualPromoPostPromoEffectLSV = x.ActualPromoPostPromoEffectLSV,
                ActualPromoNetIncrementalLSV = x.ActualPromoNetIncrementalLSV,
                ActualPromoNetLSV = x.ActualPromoNetLSV,
                ActualPromoIncrementalNSV = x.ActualPromoIncrementalNSV,
                ActualPromoNetIncrementalNSV = x.ActualPromoNetIncrementalNSV,
                ActualPromoBaselineBaseTI = x.ActualPromoBaselineBaseTI,
                ActualPromoBaseTI = x.ActualPromoBaseTI,
                ActualPromoNetBaseTI = x.ActualPromoNetBaseTI,
                ActualPromoNSV = x.ActualPromoNSV,
                ActualPromoNetNSV = x.ActualPromoNetNSV,
                ActualPromoIncrementalMAC = x.ActualPromoIncrementalMAC,
                ActualPromoNetIncrementalMAC = x.ActualPromoNetIncrementalMAC,
                ActualPromoIncrementalEarnings = x.ActualPromoIncrementalEarnings,
                ActualPromoNetIncrementalEarnings = x.ActualPromoNetIncrementalEarnings,
                ActualPromoROIPercent = x.ActualPromoROIPercent,
                ActualPromoNetROIPercent = x.ActualPromoNetROIPercent,
                PromoTypesName = x.PromoTypes.Name
            }).ToList();
            foreach (var item in query)
            {
                item.Client1LevelName = item.Client1LevelName.Split('>')[0];
                item.Client2LevelName = item.Client2LevelName.Split('>')[1];
            }
            return query.AsQueryable();
        } 



        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public SingleResult<PromoROIReport> GetPromoROIReport([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetPromoROIReports());
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<PromoROIReport> GetPromoROIReports(ODataQueryOptions<PromoROIReport> queryOptions = null)
        {
            var query = GetConstraintedQuery();
            if (queryOptions != null && queryOptions.Filter != null)
            {
                query = RoundingHelper.ModifyQuery(query);
            }
            return query;
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<PromoROIReport> GetFilteredData(ODataQueryOptions<PromoROIReport> options)
        {
            var query = GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<PromoROIReport>(options.Context, Request, HttpContext.Current.Request);
            return RoundingHelper.ModifyQuery(optionsPost.ApplyTo(query, querySettings) as IQueryable<PromoROIReport>);
        }

        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<PromoROIReport> options)
        {
            try
            {
                IQueryable results = options.ApplyTo(GetConstraintedQuery());
                IEnumerable<Column> columns = GetPromoROIExportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                UserInfo user = authorizationManager.GetCurrentUser();
                string username = user == null ? "" : user.Login;
                string filePath = exporter.GetExportFileName("PromoROIReport", username);
                exporter.Export(results, filePath);
                string filename = System.IO.Path.GetFileName(filePath);
                return Content<string>(HttpStatusCode.OK, filename);
            }
            catch (Exception e)
            {
                return Content<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public static string ExportXLSXYearStatic(DatabaseContext databaseContext, User user, int year, Role defaultRole)
        {
            try
            {
                var userName = user?.Name ?? "NOT.USER";
                var results = GetPromoROIReportsStatic(databaseContext).Where(x => x.PromoStatusName != "Cancelled" &&  x.StartDate != null && x.StartDate.Value.Year == year);
                var hierarchy = databaseContext.Set<ClientTreeHierarchyView>().AsNoTracking();

                var defaultRoleSystemName = defaultRole?.SystemName;

                if (user != null)
                {
                    var constraints = databaseContext.Constraints.Where(x => x.UserRole.UserId.Equals(user.Id) && x.UserRole.Role.SystemName == defaultRoleSystemName).ToList();
                    IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
                    results = ModuleApplyFilterHelper.ApplyFilter(results, hierarchy, filters);
                }

                var columns = GetPromoROIExportSettingsStatic();
                var exporter = new XLSXExporter(columns);
                var currentDate = DateTimeOffset.Now;
                string filePath = exporter.GetExportFileName($"{nameof(PromoROIReport)}", userName);
                exporter.Export(results, filePath);
                string fileName = Path.GetFileName(filePath);

                return fileName;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private IEnumerable<Column> GetPromoROIExportSettings()
        {
            var columns = GetPromoROIExportSettingsStatic();
            return columns;
        }

        public static IEnumerable<Column> GetPromoROIExportSettingsStatic()
        {
            int orderNumber = 1;
            var columns = new List<Column>() 
            {
                new Column { Order = orderNumber++, Field = "Number", Header = "Promo ID", Quoting = false },
                new Column { Order = orderNumber++, Field = "Client1LevelName", Header = "NA/RKA", Quoting = false },
                new Column { Order = orderNumber++, Field = "Client2LevelName", Header = "Client Group", Quoting = false },
                new Column { Order = orderNumber++, Field = "ClientName", Header = "Client", Quoting = false },
                new Column { Order = orderNumber++, Field = "BrandName", Header = "Brand", Quoting = false },
                new Column { Order = orderNumber++, Field = "TechnologyName", Header = "Technology", Quoting = false },
                new Column { Order = orderNumber++, Field = "ProductSubrangesList", Header = "Subrange", Quoting = false },
                new Column { Order = orderNumber++, Field = "MarsMechanicName", Header = "Mars mechanic", Quoting = false },
                new Column { Order = orderNumber++, Field = "MarsMechanicTypeName", Header = "Mars mechanic type", Quoting = false },
                new Column { Order = orderNumber++, Field = "MarsMechanicDiscount", Header = "Mars mechanic discount, %", Quoting = false },
                new Column { Order = orderNumber++, Field = "MechanicComment", Header = "Mechanic comment", Quoting = false },
                new Column { Order = orderNumber++, Field = "StartDate", Header = "Start date", Quoting = false, Format = "dd.MM.yyyy"  },
                new Column { Order = orderNumber++, Field = "EndDate", Header = "End date", Quoting = false, Format = "dd.MM.yyyy"  },
                new Column { Order = orderNumber++, Field = "PromoDuration", Header = "Promo duration", Quoting = false , Format = "0"},
                new Column { Order = orderNumber++, Field = "EventName", Header = "Event", Quoting = false },
                new Column { Order = orderNumber++, Field = "PromoStatusName", Header = "Status", Quoting = false },
                new Column { Order = orderNumber++, Field = "InOut", Header = "In Out", Quoting = false },
                new Column { Order = orderNumber++, Field = "IsGrowthAcceleration", Header = "Growth acceleration", Quoting = false },
                new Column { Order = orderNumber++, Field = "PlanInstoreMechanicName", Header = "Plan Instore Mechanic Name", Quoting = false },
                new Column { Order = orderNumber++, Field = "PlanInstoreMechanicTypeName", Header = "Plan Instore Mechanic Type Name", Quoting = false },
                new Column { Order = orderNumber++, Field = "PlanInstoreMechanicDiscount", Header = "Plan Instore Mechanic Discount", Quoting = false,  Format = "0"  },
                new Column { Order = orderNumber++, Field = "PlanInStoreShelfPrice", Header = "Plan Instore Shelf Price", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PCPrice", Header = "PC Price", Quoting = false, Format = "0.00" },
                new Column { Order = orderNumber++, Field = "PlanPromoBaselineLSV", Header = "Plan Promo Baseline LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoIncrementalLSV", Header = "Plan Promo Incremental LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoLSV", Header = "Plan Promo LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoUpliftPercent", Header = "Plan Promo Uplift %", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoTIShopper", Header = "Plan Promo TI Shopper", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoTIMarketing", Header = "Plan Promo TI Marketing", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoXSites", Header = "Plan Promo X-Sites", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoCatalogue", Header = "Plan Promo Catalogue", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoPOSMInClient", Header = "Plan Promo POSM In Client", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoBranding", Header = "Plan Promo Branding", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoBTL", Header = "Plan Promo BTL", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoCostProduction", Header = "Plan Promo Cost Production", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoCostProdXSites", Header = "Plan PromoCostProdXSites", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoCostProdCatalogue", Header = "Plan PromoCostProdCatalogue", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoCostProdPOSMInClient", Header = "Plan PromoCostProdPOSMInClient", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoCost", Header = "Plan Promo Cost", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "TIBasePercent", Header = "TI Base", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoIncrementalBaseTI", Header = "Plan Promo Incremental Base TI", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoNetIncrementalBaseTI", Header = "Plan Promo Net Incremental Base TI", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "COGSPercent", Header = "COGS", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoIncrementalCOGS", Header = "Plan Promo Incremental COGS", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoNetIncrementalCOGS", Header = "Plan Promo Net Incremental COGS", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoTotalCost", Header = "Plan Promo Total Cost", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoPostPromoEffectLSV", Header = "Plan Post Promo Effect LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoNetIncrementalLSV", Header = "Plan Promo Net Incremental LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoNetLSV", Header = "PlanPromo Net LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoBaselineBaseTI", Header = "Plan Promo Baseline Base TI", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoBaseTI", Header = "Plan Promo Base TI", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoNetBaseTI", Header = "Plan Promo Net Base TI", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoNSV", Header = "Plan Promo NSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoNetNSV", Header = "Plan Promo Net NSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoIncrementalNSV", Header = "Plan Promo Incremental NSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoNetIncrementalNSV", Header = "Plan Promo Net Incremental NSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoIncrementalMAC", Header = "Plan Promo Incremental MAC", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoNetIncrementalMAC", Header = "Plan Promo Net Incremental MAC", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoIncrementalEarnings", Header = "Plan Promo Incremental Earnings", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoNetIncrementalEarnings", Header = "Plan Promo Net Incremental Earnings", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoROIPercent", Header = "Plan Promo ROI, %", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoNetROIPercent", Header = "Plan Promo Net ROI, %", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanPromoNetUpliftPercent", Header = "Plan Promo Net Uplift %", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualInStoreMechanicName", Header = "Actual InStore Mechanic Name", Quoting = false },
                new Column { Order = orderNumber++, Field = "ActualInStoreMechanicTypeName", Header = "Actual InStore Mechanic Type Name", Quoting = false  },
                new Column { Order = orderNumber++, Field = "ActualInStoreDiscount", Header = "Actual InStore Mechanic Discount", Quoting = false,  Format = "0"  },
                new Column { Order = orderNumber++, Field = "ActualInStoreShelfPrice", Header = "Actual Instore Shelf Price", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "InvoiceNumber", Header = "Invoice number", Quoting = false },
                new Column { Order = orderNumber++, Field = "ActualPromoBaselineLSV", Header = "Actual Promo Baseline LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoIncrementalLSV", Header = "Actual Promo Incremental LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoLSVByCompensation", Header = "Actual PromoLSV By Compensation", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoLSV", Header = "Actual Promo LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoUpliftPercent", Header = "Actual Promo Uplift %", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoNetUpliftPercent", Header = "Actual Promo Net Uplift Percent", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoTIShopper", Header = "Actual Promo TI Shopper", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoTIMarketing", Header = "Actual Promo TI Marketing", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoXSites", Header = "Actual Promo Prod XSites", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoCatalogue", Header = "Actual Promo Prod Catalogue", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoPOSMInClient", Header = "Actual Promo Prod POSMInClient", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoBranding", Header = "Actual Promo Branding", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoBTL", Header = "Actual Promo BTL", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoCostProduction", Header = "Actual Promo Cost Production", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoCostProdXSites", Header = "Actual Promo CostProdXSites", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoCostProdCatalogue", Header = "Actual Promo Cost ProdCatalogue", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoCostProdPOSMInClient", Header = "Actual Promo Cost ProdPOSMInClient", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoCost", Header = "Actual Promo Cost", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoIncrementalBaseTI", Header = "Actual Promo Incremental BaseTI", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoNetIncrementalBaseTI", Header = "Actual Promo Net Incremental BaseTI", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoIncrementalCOGS", Header = "Actual Promo Incremental COGS", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoNetIncrementalCOGS", Header = "Actual Promo Net Incremental COGS", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoTotalCost", Header = "Actual Promo Total Cost", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoPostPromoEffectLSV", Header = "Actual Post Promo Effect LSV", Quoting = false,  Format = "0"  },
                new Column { Order = orderNumber++, Field = "ActualPromoNetIncrementalLSV", Header = "Actual Promo Net Incremental LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoNetLSV", Header = "Actual Promo Net LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoIncrementalNSV", Header = "Actual Promo Incremental NSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoNetIncrementalNSV", Header = "Actual Promo Net Incremental NSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoBaselineBaseTI", Header = "Actual Promo Baseline Base TI", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoBaseTI", Header = "Actual Promo Base TI", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoNetBaseTI", Header = "Actual Promo Net Base TI", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoNSV", Header = "Actual Promo NSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoNetNSV", Header = "Actual Promo Net NSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoIncrementalMAC", Header = "Actual Promo Incremental MAC", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoNetIncrementalMAC", Header = "Actual Promo Net Incremental MAC", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoIncrementalEarnings", Header = "Actual Promo Incremental Earnings", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoNetIncrementalEarnings", Header = "Actual Promo Net Incremental Earnings", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoROIPercent", Header = "Actual Promo ROI, %", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualPromoNetROIPercent", Header = "Actual Promo Net ROI%", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PromoTypesName", Header = "Promo Type Name", Quoting = false }
            };
            return columns;
        }
    }
}
