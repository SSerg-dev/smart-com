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
using Core.MarsCalendar;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Module.Frontend.TPM.Controllers {

    public class PlanPostPromoEffectReportsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public PlanPostPromoEffectReportsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        public IQueryable<PlanPostPromoEffectReportWeekView> GetConstraintedQuery()
        {
            var result = new ConcurrentBag<PlanPostPromoEffectReportWeekView>();
            var user = authorizationManager.GetCurrentUser();
            var role = authorizationManager.GetCurrentRoleName();
            var clientTrees = Context.Set<ClientTree>().AsNoTracking().ToList();
            
            var dateTimeNow = DateTime.Now;

            var simplePromoPromoProducts = Context.Database.SqlQuery<SimplePromoPromoProduct>
            (@"
                SELECT  promo.Id,
						promo.ClientTreeId, 
		                promo.ClientTreeKeyId, 
		                promo.Name,
		                promo.Number,
		                promo.StartDate,
		                promo.EndDate,
						promo.DispatchesStart,
						promo.DispatchesEnd,
		                promo.InOut,
		                promo.PlanPromoUpliftPercent,
		                promoStatus.Name AS PromoStatusName,
                        promo.BrandTechId,
		                promoProduct.ZREP, 
		                promoProduct.PlanProductIncrementalCaseQty, 
		                promoProduct.PlanProductBaselineCaseQty, 
		                promoProduct.PlanProductPostPromoEffectLSVW1, 
		                promoProduct.PlanProductPostPromoEffectLSVW2, 
		                promoProduct.PlanProductBaselineLSV 
		
                FROM Promo promo
	                 INNER JOIN PromoProduct promoProduct ON promoProduct.PromoId = promo.Id  
	                 INNER JOIN PromoStatus promoStatus ON promoStatus.Id = promo.PromoStatusId
	
                WHERE promoProduct.Disabled = 0 AND promo.Disabled = 0
            ").AsEnumerable();

			var baseLines = Context.Set<BaseLine>().Where(x => !x.Disabled).Select(x => new BaseLineSimpleModel
            {
                ProductZREP = x.Product.ZREP,
                QTY = x.QTY,
                BaselineLSV = x.BaselineLSV,
                StartDate = x.StartDate,
                DemandCode = x.DemandCode
            })
            .ToList();

			string allowedClientIds = String.Empty;
			var clientTreeBrandTeches = Context.Set<ClientTreeBrandTech>().Where(x => !x.Disabled).ToList();
			var constraints = user.Id.HasValue ? Context.Constraints
				.Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
				.ToList() : new List<Constraint>();
			if (constraints.Any())
			{
				var filters = FilterHelper.GetFiltersDictionary(constraints);
				var clientFilter = FilterHelper.GetFilter(filters, ModuleFilterName.Client);
				IEnumerable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
				hierarchy = hierarchy.Where(h => clientFilter.Contains(h.Id.ToString()) || clientFilter.Any(c => h.Hierarchy.Contains(c)));

				var allowedClientHierarchy = hierarchy.Select(h => h.Hierarchy.Split(',')).ToList();
				allowedClientHierarchy.AddRange(hierarchy.Select(h => new[] { h.Id.ToString() }));
				allowedClientIds = String.Join(",", allowedClientHierarchy.Select(x => String.Join(",", x)));
			}

			simplePromoPromoProducts.AsParallel().ForAll(simplePromoPromoProduct => {
				// Проверка ограничений по клиентам
				if (!String.IsNullOrEmpty(allowedClientIds) && simplePromoPromoProduct.ClientTreeId.HasValue)
				{
					bool hasAccess = allowedClientIds.Contains(simplePromoPromoProduct.ClientTreeId.Value.ToString());
					if (!hasAccess)
						return;
				}

				var promoStatus = simplePromoPromoProduct.PromoStatusName;
                var demandCode = String.Empty;

                var clientTree = clientTrees.FirstOrDefault(x => x.Id == simplePromoPromoProduct.ClientTreeKeyId && DateTime.Compare(x.StartDate, dateTimeNow) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dateTimeNow) > 0));
                if (clientTree == null)
                {
                    demandCode = null;
                }
                else
                {
                    demandCode = clientTree.DemandCode;
                }

                double? postPromoEffectW1 = null;
                double? postPromoEffectW2 = null;

                double? postPromoEffectW1Qty = null;
                double? postPromoEffectW2Qty = null;
                double? postPromoEffectW1QtyRounded = null;
                double? postPromoEffectW2QtyRounded = null;

                double? planProductBaselineCaseQtyW1 = null;
                double? planProductBaselineCaseQtyW2 = null;
                double? planProductBaselineCaseQtyW1Rounded = null;
                double? planProductBaselineCaseQtyW2Rounded = null;

                double? planProductPostPromoEffectLSVW1 = null;
                double? planProductPostPromoEffectLSVW2 = null;
                double? planProductPostPromoEffectLSVW1Rounded = null;
                double? planProductPostPromoEffectLSVW2Rounded = null;

                double? planProductBaselineLSVW1 = null;
                double? planProductBaselineLSVW2 = null;
                double? planProductBaselineLSVW1Rounded = null;
                double? planProductBaselineLSVW2Rounded = null;

                if (clientTree != null)
                {
                    postPromoEffectW1 = clientTree.PostPromoEffectW1.HasValue ? clientTree.PostPromoEffectW1.Value : 0;
                    postPromoEffectW2 = clientTree.PostPromoEffectW2.HasValue ? clientTree.PostPromoEffectW2.Value : 0;
                }

				var promoEffectBegin = ((DateTimeOffset)simplePromoPromoProduct.DispatchesEnd).Date;

                var marsWeekBeginDiff = DayOfWeek.Sunday - promoEffectBegin.DayOfWeek;
                if (marsWeekBeginDiff < 0)
                {
                    marsWeekBeginDiff += 7;
                }

                var weekStart = promoEffectBegin.AddDays(marsWeekBeginDiff);
                var week = TimeSpan.FromDays(7);

				if (String.IsNullOrEmpty(clientTree.DemandCode))
                {
                    clientTree = clientTrees.FirstOrDefault(y => y.ObjectId == clientTree.parentId && DateTime.Compare(y.StartDate, dateTimeNow) <= 0 && (!y.EndDate.HasValue || DateTime.Compare(y.EndDate.Value, dateTimeNow) > 0));
                    if (clientTree != null && !String.IsNullOrEmpty(clientTree.DemandCode))
                    {
                        demandCode = clientTree.DemandCode;
                    }
                }

                var baseLineW1 = weekStart;
                var baseLineW2 = baseLineW1.Add(week);

                BaseLineSimpleModel baseLinePostPromoEffectW1 = null;
                BaseLineSimpleModel baseLinePostPromoEffectW2 = null;

                var currentClient = new ClientTree { parentId = simplePromoPromoProduct.ClientTreeId.Value };
                while (currentClient != null && currentClient.Type != "root")
                {
                    currentClient = clientTrees.FirstOrDefault(x => !x.EndDate.HasValue && x.ObjectId == currentClient.parentId);
                    if (currentClient != null && !String.IsNullOrEmpty(currentClient.DemandCode))
                    {
                        baseLinePostPromoEffectW1 = baseLines.FirstOrDefault(x => x.DemandCode == currentClient.DemandCode && x.ProductZREP == simplePromoPromoProduct.ZREP && x.StartDate == baseLineW1);
                        baseLinePostPromoEffectW2 = baseLines.FirstOrDefault(x => x.DemandCode == currentClient.DemandCode && x.ProductZREP == simplePromoPromoProduct.ZREP && x.StartDate == baseLineW2);

                        if (baseLinePostPromoEffectW1 != null && baseLinePostPromoEffectW2 != null)
                        {
                            break;
                        }
                    }
                }

                if (baseLinePostPromoEffectW1 != null && baseLinePostPromoEffectW2 != null)
                {
                    var clientTreeBrandTech = clientTreeBrandTeches.FirstOrDefault(x => x.ClientTreeId == simplePromoPromoProduct.ClientTreeKeyId && x.BrandTechId == simplePromoPromoProduct.BrandTechId);

                    postPromoEffectW1Qty = (baseLinePostPromoEffectW1?.QTY ?? 0) * (clientTreeBrandTech?.Share ?? 1) / 100 * (postPromoEffectW1 ?? 0) / 100;
                    postPromoEffectW2Qty = (baseLinePostPromoEffectW2?.QTY ?? 0) * (clientTreeBrandTech?.Share ?? 1) / 100 * (postPromoEffectW2 ?? 0) / 100;

                    postPromoEffectW1QtyRounded = Math.Round(postPromoEffectW1Qty.Value, 2);
                    postPromoEffectW2QtyRounded = Math.Round(postPromoEffectW2Qty.Value, 2);

                    planProductBaselineCaseQtyW1 = (baseLinePostPromoEffectW1?.QTY ?? 0) * (clientTreeBrandTech?.Share ?? 1) / 100;
                    planProductBaselineCaseQtyW2 = (baseLinePostPromoEffectW2?.QTY ?? 0) * (clientTreeBrandTech?.Share ?? 1) / 100;

                    planProductBaselineCaseQtyW1Rounded = Math.Round(planProductBaselineCaseQtyW1.Value, 2);
                    planProductBaselineCaseQtyW2Rounded = Math.Round(planProductBaselineCaseQtyW2.Value, 2);

                    planProductBaselineLSVW1 = (baseLinePostPromoEffectW1?.BaselineLSV ?? 0) * (clientTreeBrandTech?.Share ?? 1) / 100;
                    planProductBaselineLSVW2 = (baseLinePostPromoEffectW2?.BaselineLSV ?? 0) * (clientTreeBrandTech?.Share ?? 1) / 100;

                    planProductBaselineLSVW1Rounded = Math.Round(planProductBaselineLSVW1.Value, 2);
                    planProductBaselineLSVW2Rounded = Math.Round(planProductBaselineLSVW2.Value, 2);

					var absPPEw1 = postPromoEffectW1.HasValue ? Math.Abs(postPromoEffectW1.Value) : 0;
					var absPPEw2 = postPromoEffectW2.HasValue ? Math.Abs(postPromoEffectW2.Value) : 0;
					planProductPostPromoEffectLSVW1 = (planProductBaselineLSVW1 ?? 0) * absPPEw1 / 100;
                    planProductPostPromoEffectLSVW2 = (planProductBaselineLSVW2 ?? 0) * absPPEw2 / 100;

                    planProductPostPromoEffectLSVW1Rounded = Math.Round(planProductPostPromoEffectLSVW1.Value, 2);
                    planProductPostPromoEffectLSVW2Rounded = Math.Round(planProductPostPromoEffectLSVW2.Value, 2);

                }

                result.Add(ReportCreateWeek(simplePromoPromoProduct, demandCode, promoStatus, weekStart, postPromoEffectW1QtyRounded, postPromoEffectW2QtyRounded, planProductBaselineCaseQtyW1Rounded, planProductBaselineCaseQtyW2Rounded, planProductPostPromoEffectLSVW1Rounded, planProductPostPromoEffectLSVW2Rounded, planProductBaselineLSVW1Rounded, planProductBaselineLSVW2Rounded));
            });

            return result.AsQueryable();
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<PlanPostPromoEffectReportWeekView> GetPlanPostPromoEffectReports() {
            return GetConstraintedQuery();
        }

        private IEnumerable<Column> GetExportSettings() {
			int order = 0;
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = order++, Field = "ZREP", Header = "ZREP", Quoting = false },
                new Column() { Order = order++, Field = "DemandCode", Header = "Demand Code", Quoting = false },
                new Column() { Order = order++, Field = "PromoNameId", Header = "Promo Name Id", Quoting = false },
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
            };
            return columns;
        }
        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<PlanPostPromoEffectReportWeekView> options) {
            try {
                
                IQueryable results = options.ApplyTo(GetConstraintedQuery());
               
                IEnumerable<Column> columns = GetExportSettings();
                NonGuidIdExporter exporter = new NonGuidIdExporter(columns);
                UserInfo user = authorizationManager.GetCurrentUser();
                string username = user == null ? "" : user.Login;
                string filePath = exporter.GetExportFileName("PlanPostPromoEffectReport", username);
                exporter.Export(MapToReport(results), filePath);
                string filename = System.IO.Path.GetFileName(filePath);
                return Content<string>(HttpStatusCode.OK, filename);
            } catch (Exception e) {
                return Content<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        private IQueryable MapToReport(IQueryable data)
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
                    InOut = plan.InOut
                });

                result.Add(new PlanPostPromoEffectReport()
                {
                    Id = plan.Id,
                    ZREP = plan.ZREP,
                    DemandCode = plan.DemandCode,
                    PromoNameId = plan.PromoNameId,
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
                    InOut = plan.InOut
                });
             
            }
            return result.AsQueryable();
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
            rep.PlanProductBaselineLSVW2= planProductBaselineLSVW2;
            rep.PlanUplift = simplePromoPromoProduct.PlanPromoUpliftPercent;

            return rep;
        }
        public string SetWeekByMarsDates(DateTime week)
		{
			string stringFormatYP2W = "{0}P{1:D2}W{2}";
			string marsDate = (new MarsDate((DateTimeOffset)week).ToString(stringFormatYP2W));
			return marsDate;
		}
		private bool EntityExists(Guid key) {
            return Context.Set<PromoProduct>().Count(e => e.Id == key) > 0;
        }
    }
}