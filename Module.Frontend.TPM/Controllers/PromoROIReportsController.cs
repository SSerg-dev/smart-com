using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model;
using System;
using System.Collections.Generic;
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

            IQueryable<PromoROIReport> query = Context.Set<PromoROIReport>();
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public SingleResult<PromoROIReport> GetPromoROIReport([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetPromoROIReports2());
        }

        public IQueryable<PromoROIReport> GetPromoROIReports2(ODataQueryOptions<PromoROIReport> queryOptions = null)
        {
            IQueryable<PromoROIReport> query = GetConstraintedQuery();
            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<PromoROIReport> GetPromoROIReports(ODataQueryOptions<PromoROIReport> queryOptions = null)
        {
            IQueryable<PromoROIReport> query = GetConstraintedQuery();
            foreach (PromoROIReport promoROIReport in query)
            {
                promoROIReport.ActualPromoNetROIPercent = (promoROIReport.ActualPromoNetIncrementalEarnings / promoROIReport.ActualPromoCost + 1) * 100;
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
            IQueryable results = options.ApplyTo(GetConstraintedQuery());
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);
            using (DatabaseContext context = new DatabaseContext())
            {
                HandlerData data = new HandlerData();
                string handlerName = "ExportHandler";

                HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TModel", typeof(PromoROIReport), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(PromoROIReportsController), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(PromoROIReportsController.GetPromoROIExportSettingsStatic), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = $"Export {nameof(PromoROIReport)} dictionary",
                    Name = "Module.Host.TPM.Handlers." + handlerName,
                    ExecutionPeriod = null,
                    CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                    LastExecutionDate = null,
                    NextExecutionDate = null,
                    ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                    UserId = userId,
                    RoleId = roleId
                };
                handler.SetParameterData(data);
                context.LoopHandlers.Add(handler);
                context.SaveChanges();
            }

            return Content(HttpStatusCode.OK, "success");
        }

        public static string ExportXLSXYearStatic(DatabaseContext databaseContext, User user, int year, Role defaultRole, bool yearInName = false)
        {
            try
            {
                var userName = user?.Name ?? "NOT.USER";
                var results = databaseContext.Set<PromoROIReport>().Where(x => x.PromoStatusName != "Cancelled" && x.StartDate != null && x.StartDate.Value.Year == year);
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
                if (yearInName)
                {
                    filePath = filePath.Insert(filePath.LastIndexOf("."), "_" + year);
                }

                exporter.Export(results, filePath);
                string fileName = Path.GetFileName(filePath);

                return fileName;
            }
            catch (Exception e)
            {
                return null;
            }
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
                new Column { Order = orderNumber++, Field = "SubName", Header = "Sub", Quoting = false },
                new Column { Order = orderNumber++, Field = "ProductSubrangesList", Header = "Subrange", Quoting = false },
                new Column { Order = orderNumber++, Field = "MarsMechanicName", Header = "Mars mechanic", Quoting = false },
                new Column { Order = orderNumber++, Field = "MarsMechanicTypeName", Header = "Mars mechanic type", Quoting = false },
                new Column { Order = orderNumber++, Field = "MarsMechanicDiscount", Header = "Mars mechanic discount, %", Quoting = false },
                new Column { Order = orderNumber++, Field = "MechanicComment", Header = "Mechanic comment", Quoting = false },
                new Column { Order = orderNumber++, Field = "StartDate", Header = "Start date", Quoting = false, Format = "dd.MM.yyyy"  },
                new Column { Order = orderNumber++, Field = "EndDate", Header = "End date", Quoting = false, Format = "dd.MM.yyyy"  },
                new Column { Order = orderNumber++, Field = "BudgetYear", Header = "Budget year", Quoting = false, Format = "0"  },
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
                new Column { Order = orderNumber++, Field = "PlanAddTIShopperApproved", Header = "Plan Add TI Shopper Approved", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanAddTIShopperCalculated", Header = "Plan Add TI Shopper Calculated", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanAddTIMarketingApproved", Header = "Plan Add TI Marketing Approved", Quoting = false,  Format = "0.00"  },
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
                new Column { Order = orderNumber++, Field = "ActualAddTIShopper", Header = "Actual Add TI Shopper", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "ActualAddTIMarketing", Header = "Actual Add TI Marketing", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PromoTypesName", Header = "Promo Type Name", Quoting = false },
                new Column { Order = orderNumber++, Field = "SumInvoice", Header = "Sum In Invoice", Quoting = false,  Format = "0.00"  },
            };
            return columns;
        }
    }
}
