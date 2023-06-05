using Core.MarsCalendar;
using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;

namespace Module.Frontend.TPM.Controllers
{

    public class PlanPostPromoEffectReportsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public PlanPostPromoEffectReportsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        public IQueryable<PlanPostPromoEffectReportWeekView> GetConstraintedQuery()
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();

            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();

            IQueryable<PlanPostPromoEffectReportWeekView> query = Context.Set<PlanPostPromoEffectReportWeekView>();

            query = ModuleApplyFilterHelper.ApplyFilter(query, Context, hierarchy, filters);


            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<PlanPostPromoEffectReportWeekView> GetPlanPostPromoEffectReports()
        {
            return GetConstraintedQuery();
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<PlanPostPromoEffectReportWeekView> GetFilteredData(ODataQueryOptions<PlanPostPromoEffectReportWeekView> options)
        {
            var query = GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<PlanPostPromoEffectReportWeekView>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<PlanPostPromoEffectReportWeekView>;
        }

        public static IEnumerable<Column> GetExportSettings()
        {
            int order = 0;
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = order++, Field = "ZREP", Header = "ZREP", Quoting = false },
                new Column() { Order = order++, Field = "DemandCode", Header = "Demand Code", Quoting = false },
                new Column() { Order = order++, Field = "PromoNameId", Header = "Promo Name Id", Quoting = false },
                new Column() { Order = order++, Field = "PromoNumber", Header = "Promo Number", Quoting = false },
                new Column() { Order = order++, Field = "LocApollo", Header = "Loc", Quoting = false },
                new Column() { Order = order++, Field = "TypeApollo", Header = "Type", Quoting = false },
                new Column() { Order = order++, Field = "ModelApollo", Header = "Model", Quoting = false },
                new Column() { Order = order++, Field = "WeekStartDate", Header = "Week Start Date", Quoting = false, Format = "dd.MM.yyyy"  },
                new Column() { Order = order++, Field = "PlanPostPromoEffectQty", Header = "Qty", Quoting = false },
                new Column() { Order = order++, Field = "PlanUplift", Header = "Uplift Plan", Quoting = false },
                new Column() { Order = order++, Field = "StartDate", Header = "Start Date", Quoting = false, Format = "dd.MM.yyyy"  },
                new Column() { Order = order++, Field = "EndDate", Header = "End Date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = order++, Field = "Status", Header = "Status", Quoting = false },
                new Column() { Order = order++, Field = "Week", Header = "Week", Quoting = false },
                new Column() { Order = order++, Field = "PlanProductBaselineCaseQty", Header = "Plan Product Baseline Case Qty", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = "PlanProductPostPromoEffectLSV", Header = "Plan Product Post Promo Effect LSV", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = "PlanProductBaselineLSV", Header = "Plan Product Baseline LSV", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = "InOut", Header = "InOut", Quoting = false },
                new Column() { Order = order++, Field = "IsOnInvoice", Header = "On-Invoice", Quoting = false },
            };
            return columns;
        }
        [ClaimsAuthorize]
        public async Task<IHttpActionResult> ExportXLSX(ODataQueryOptions<PlanPostPromoEffectReportWeekView> options)
        {
            IQueryable results = options.ApplyTo(GetConstraintedQuery());
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            HandlerData data = new HandlerData();
            string handlerName = "ExportHandler";

            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TModel", typeof(PlanPostPromoEffectReportWeekView), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(PlanPostPromoEffectReportsController), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(PlanPostPromoEffectReportsController.GetExportSettings), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = $"Export {nameof(PlanPostPromoEffectReportWeekView)} dictionary",
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
            Context.LoopHandlers.Add(handler);
            await Context.SaveChangesAsync();

            return Content(HttpStatusCode.OK, "success");
        }
        public static IQueryable<T> MapToReport<T>(IQueryable data) where T : class
        {
            List<PlanPostPromoEffectReport> result = new List<PlanPostPromoEffectReport>();

            foreach (PlanPostPromoEffectReportWeekView plan in data)
            {
                TimeSpan week = TimeSpan.FromDays(7);
                result.Add(new PlanPostPromoEffectReport()
                {
                    Id = plan.Id,
                    ZREP = plan.ZREP,
                    DemandCode = plan.DemandCode,
                    PromoNameId = plan.PromoNameId,
                    PromoNumber = plan.PromoNumber,
                    LocApollo = plan.LocApollo,
                    TypeApollo = plan.TypeApollo,
                    ModelApollo = plan.ModelApollo,
                    WeekStartDate = plan.WeekStartDate,
                    PlanPostPromoEffectQty = plan.PlanPostPromoEffectQtyW1,
                    PlanUplift = Math.Round((plan.PlanUplift ?? 0), 2),
                    StartDate = plan.WeekStartDate,
                    EndDate = plan.WeekStartDate + week,
                    Week = SetWeekByMarsDates(plan.WeekStartDate.Value.Date),
                    Status = plan.Status,
                    PlanProductBaselineCaseQty = plan.PlanProductBaselineCaseQtyW1,
                    PlanProductPostPromoEffectLSV = plan.PlanProductPostPromoEffectLSVW1,
                    PlanProductBaselineLSV = plan.PlanProductBaselineLSVW1,
                    InOut = plan.InOut,
                    IsOnInvoice = plan.IsOnInvoice
                });

                result.Add(new PlanPostPromoEffectReport()
                {
                    Id = plan.Id,
                    ZREP = plan.ZREP,
                    DemandCode = plan.DemandCode,
                    PromoNameId = plan.PromoNameId,
                    PromoNumber = plan.PromoNumber,
                    LocApollo = plan.LocApollo,
                    TypeApollo = plan.TypeApollo,
                    ModelApollo = plan.ModelApollo,
                    WeekStartDate = plan.WeekStartDate + week,
                    PlanPostPromoEffectQty = plan.PlanPostPromoEffectQtyW2,
                    PlanUplift = Math.Round((plan.PlanUplift ?? 0), 2),
                    StartDate = plan.WeekStartDate + week,
                    EndDate = plan.WeekStartDate + week + week,
                    Week = SetWeekByMarsDates(plan.WeekStartDate.Value.Date + week),
                    Status = plan.Status,
                    PlanProductBaselineCaseQty = plan.PlanProductBaselineCaseQtyW2,
                    PlanProductPostPromoEffectLSV = plan.PlanProductPostPromoEffectLSVW2,
                    PlanProductBaselineLSV = plan.PlanProductBaselineLSVW2,
                    InOut = plan.InOut,
                    IsOnInvoice = plan.IsOnInvoice
                });
            }
            return result.AsQueryable().Cast<T>();
        }

        private PlanPostPromoEffectReportWeekView ReportCreateWeek(SimplePromoPromoProduct simplePromoPromoProduct, String demandCode, String promoStatus, DateTime weekStart, double? qtyW1, double? qtyW2, double? planProductBaselineCaseQtyW1, double? planProductBaselineCaseQtyW2, double? planProductPostPromoEffectLSVW1, double? planProductPostPromoEffectLSVW2, double? planProductBaselineLSVW1, double? planProductBaselineLSVW2)
        {
            TimeSpan week = TimeSpan.FromDays(7);

            PlanPostPromoEffectReportWeekView rep = new PlanPostPromoEffectReportWeekView();
            rep.ZREP = simplePromoPromoProduct.ZREP + "_0125";
            rep.Status = promoStatus;
            rep.PromoNameId = simplePromoPromoProduct.Name + "#" + simplePromoPromoProduct.Number.ToString();
            rep.WeekStartDate = weekStart;
            rep.StartDate = rep.WeekStartDate;
            rep.EndDate = rep.WeekStartDate + week + week;
            rep.DemandCode = String.IsNullOrEmpty(demandCode) ? "Demand code was not found" : demandCode;
            rep.InOut = simplePromoPromoProduct.InOut;
            rep.Id = Guid.NewGuid();
            rep.LocApollo = "RU_0125";
            rep.TypeApollo = "7";
            rep.ModelApollo = "SHIP_LEWAND_CS";
            rep.Week = SetWeekByMarsDates(weekStart);
            rep.PlanPostPromoEffectQtyW1 = qtyW1;
            rep.PlanPostPromoEffectQtyW2 = qtyW2;
            rep.PlanProductBaselineCaseQtyW1 = planProductBaselineCaseQtyW1;
            rep.PlanProductBaselineCaseQtyW2 = planProductBaselineCaseQtyW2;
            rep.PlanProductPostPromoEffectLSVW1 = planProductPostPromoEffectLSVW1;
            rep.PlanProductPostPromoEffectLSVW2 = planProductPostPromoEffectLSVW2;
            rep.PlanProductBaselineLSVW1 = planProductBaselineLSVW1;
            rep.PlanProductBaselineLSVW2 = planProductBaselineLSVW2;
            rep.PlanUplift = simplePromoPromoProduct.PlanPromoUpliftPercent;
            rep.IsOnInvoice = simplePromoPromoProduct.IsOnInvoice;

            return rep;
        }
        public static string SetWeekByMarsDates(DateTime week)
        {
            string stringFormatYP2W = "{0}P{1:D2}W{2}";
            string marsDate = (new MarsDate((DateTimeOffset)week).ToString(stringFormatYP2W));
            return marsDate;
        }
        private bool EntityExists(Guid key)
        {
            return Context.Set<PromoProduct>().Count(e => e.Id == key) > 0;
        }
    }
}