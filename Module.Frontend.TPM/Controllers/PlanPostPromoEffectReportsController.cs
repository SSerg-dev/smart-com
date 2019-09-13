﻿using AutoMapper;
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

namespace Module.Frontend.TPM.Controllers {

    public class PlanPostPromoEffectReportsController : EFContextController {
        private readonly IAuthorizationManager authorizationManager;

        public PlanPostPromoEffectReportsController(IAuthorizationManager authorizationManager) {
            this.authorizationManager = authorizationManager;
        }

        //[ClaimsAuthorize]
        //[EnableQuery(MaxNodeCount = int.MaxValue)]
        //public SingleResult<PlanPostPromoEffectReport> GetPlanPostPromoEffectReport([FromODataUri] System.Guid key) {
        //    return SingleResult.Create(GetConstraintedQuery());
        //}

        //public IQueryable<PlanPostPromoEffectReport> GetConstraintedQuery()
        //{
        //    List<PlanPostPromoEffectReport> result = new List<PlanPostPromoEffectReport>();
        //    List<PromoProduct> promoproducts = Context.Set<PromoProduct>().Where(y => !y.Disabled && y.PlanProductCaseQty > 0).ToList();
        //
        //    UserInfo user = authorizationManager.GetCurrentUser();
        //    string role = authorizationManager.GetCurrentRoleName();
        //    IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
        //        .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
        //        .ToList() : new List<Constraint>();
        //
        //    IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
        //    IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
        //
        //    promoproducts = ModuleApplyFilterHelper.ApplyFilter(promoproducts, hierarchy, filters);
        //
        //    DateTime dt = DateTime.Now;
        //    foreach (PromoProduct promoproduct in promoproducts)
        //    {
        //        Promo promo = Context.Set<Promo>().FirstOrDefault(x => x.Id == promoproduct.PromoId);
        //        String promoStatus = promo.PromoStatus.Name;
        //
        //        String demandCode = null;
        //
        //        ClientTree clientTree = Context.Set<ClientTree>().FirstOrDefault(x => x.ObjectId == promo.ClientTreeId && (DateTime.Compare(x.StartDate, dt) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dt) > 0)));
        //
        //        if (clientTree == null)
        //        {
        //            demandCode = null;
        //        }
        //        else if (String.IsNullOrEmpty(clientTree.DemandCode))
        //        {
        //            clientTree = Context.Set<ClientTree>().FirstOrDefault(y => y.ObjectId == clientTree.parentId && (DateTime.Compare(y.StartDate, dt) <= 0 && (!y.EndDate.HasValue || DateTime.Compare(y.EndDate.Value, dt) > 0)));
        //            if (clientTree != null && !String.IsNullOrEmpty(clientTree.DemandCode))
        //            {
        //                demandCode = clientTree.DemandCode;
        //            }
        //        }
        //        else
        //        {
        //            demandCode = clientTree.DemandCode;
        //        }
        //
        //        //Расчет прост промо эффекта
        //        double? postPromoEffectW1 = null;
        //        double? postPromoEffectW2 = null;
        //        double? postPromoEffectW1Qty = null;
        //        double? postPromoEffectW2Qty = null;
        //        double? planProductBaselineCaseQtyW1 = null;
        //        double? planProductBaselineCaseQtyW2 = null;
        //        double? planProductPostPromoEffectLSVW1 = null;
        //        double? planProductPostPromoEffectLSVW2 = null;
        //        double? planProductBaselineLSVW1 = null;
        //        double? planProductBaselineLSVW2 = null;
        //
        //        if (clientTree != null)
        //        {
        //            postPromoEffectW1 = clientTree.PostPromoEffectW1;
        //            postPromoEffectW2 = clientTree.PostPromoEffectW2;
        //        }
        //        if (postPromoEffectW1 != null)
        //        {
        //            postPromoEffectW1Qty = promoproduct.PlanProductIncrementalCaseQty * (postPromoEffectW1 / 100);
        //            planProductBaselineCaseQtyW1 = promoproduct.PlanProductBaselineCaseQty * (postPromoEffectW1 / 100);
        //            planProductPostPromoEffectLSVW1 = promoproduct.PlanProductPostPromoEffectLSV * (postPromoEffectW1 / 100);
        //            planProductBaselineLSVW1 = promoproduct.PlanProductBaselineLSV * (postPromoEffectW1 / 100);
        //        }
        //        if (postPromoEffectW2 != null)
        //        {
        //            postPromoEffectW2Qty = promoproduct.PlanProductIncrementalCaseQty * (postPromoEffectW2 / 100);
        //            planProductBaselineCaseQtyW2 = promoproduct.PlanProductBaselineCaseQty * (postPromoEffectW2 / 100);
        //            planProductPostPromoEffectLSVW2 = promoproduct.PlanProductPostPromoEffectLSV * (postPromoEffectW2 / 100);
        //            planProductBaselineLSVW2 = promoproduct.PlanProductBaselineLSV * (postPromoEffectW2 / 100);
        //        }
        //
        //        postPromoEffectW1Qty = postPromoEffectW1Qty != null ? Math.Round(postPromoEffectW1Qty.Value, 2) : 0;
        //        planProductBaselineCaseQtyW1 = planProductBaselineCaseQtyW1 != null ? Math.Round(planProductBaselineCaseQtyW1.Value, 2) : 0;
        //        planProductPostPromoEffectLSVW1 = planProductPostPromoEffectLSVW1 != null ? Math.Round(planProductPostPromoEffectLSVW1.Value, 2) : 0;
        //        planProductBaselineLSVW1 = planProductBaselineLSVW1 != null ? Math.Round(planProductBaselineLSVW1.Value, 2) : 0;
        //
        //        postPromoEffectW2Qty = postPromoEffectW2Qty != null ? Math.Round(postPromoEffectW2Qty.Value, 2) : 0;
        //        planProductBaselineCaseQtyW2 = planProductBaselineCaseQtyW2 != null ? Math.Round(planProductBaselineCaseQtyW2.Value, 2) : 0;
        //        planProductPostPromoEffectLSVW2 = planProductPostPromoEffectLSVW2 != null ? Math.Round(planProductPostPromoEffectLSVW2.Value, 2) : 0;
        //        planProductBaselineLSVW2 = planProductBaselineLSVW2 != null ? Math.Round(planProductBaselineLSVW2.Value, 2) : 0;
        //
        //        //Первый день после промо
        //        DateTime promoEffectBegin = ((DateTimeOffset)promo.EndDate).Date.AddDays(1);
        //
        //        int marsWeekBeginDiff = DayOfWeek.Sunday - promoEffectBegin.DayOfWeek;
        //        if (marsWeekBeginDiff < 0) { marsWeekBeginDiff += 7; }
        //
        //        DateTime weekStart = promoEffectBegin.AddDays(marsWeekBeginDiff);
        //        TimeSpan week = TimeSpan.FromDays(7);
        //
        //        // Первые 2 полные недели после промо
        //        result.Add(ReportCreate(promoproduct, promo, demandCode, promoStatus, weekStart, postPromoEffectW1Qty, planProductBaselineCaseQtyW1, planProductPostPromoEffectLSVW1, planProductBaselineLSVW1));
        //        weekStart += week;
        //        result.Add(ReportCreate(promoproduct, promo, demandCode, promoStatus, weekStart, postPromoEffectW2Qty, planProductBaselineCaseQtyW2, planProductPostPromoEffectLSVW2, planProductBaselineLSVW2));
        //    }
        //    return result.AsQueryable();
        //}
        public IQueryable<PlanPostPromoEffectReportWeekView> GetConstraintedQuery()
        {
            var result = new ConcurrentBag<PlanPostPromoEffectReportWeekView>();
            var clientTrees = Context.Set<ClientTree>().AsNoTracking().ToList();
            var user = authorizationManager.GetCurrentUser();
            var role = authorizationManager.GetCurrentRoleName();

            var constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();

            var filters = FilterHelper.GetFiltersDictionary(constraints);
            var hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            var promoProducts = Context.Set<PromoProduct>().AsNoTracking().Where(y => !y.Disabled && y.PlanProductCaseQty > 0).ToList();
            var dateTimeNow = DateTime.Now;


            var simplePromoes = new List<SimplePromo>();
            foreach (var promoProduct in promoProducts)
            {
                var simplePromo = Context.Database.SqlQuery<SimplePromo>($"SELECT TOP(1) promo.Id, promoStatus.Name AS PromoStatusName, promo.ClientTreeId, promo.EndDate, promo.Name, promo.Number, promo.DispatchesStart, promo.DispatchesEnd, promo.InOut, promo.PlanPromoUpliftPercent FROM Promo promo JOIN PromoStatus promoStatus ON promo.PromoStatusId = promoStatus.Id WHERE promo.Id = '{promoProduct.PromoId}'").FirstOrDefault();
                if (simplePromo != null)
                {
                    simplePromoes.Add(simplePromo);
                }
            }

            promoProducts = ModuleApplyFilterHelper.ApplyFilter(promoProducts, hierarchy, filters);
            promoProducts.AsParallel().ForAll(promoProduct =>
            {
                var promo = simplePromoes.FirstOrDefault(x => x.Id == promoProduct.PromoId);
                if (promo != null)
                {
                    var promoStatus = promo.PromoStatusName;
                    var demandCode = String.Empty;

                    var clientTree = clientTrees.FirstOrDefault(x => x.ObjectId == promo.ClientTreeId && DateTime.Compare(x.StartDate, dateTimeNow) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dateTimeNow) > 0));
                    if (clientTree == null)
                    {
                        demandCode = null;
                    }
                    else if (String.IsNullOrEmpty(clientTree.DemandCode))
                    {
                        clientTree = clientTrees.FirstOrDefault(y => y.ObjectId == clientTree.parentId && DateTime.Compare(y.StartDate, dateTimeNow) <= 0 && (!y.EndDate.HasValue || DateTime.Compare(y.EndDate.Value, dateTimeNow) > 0));
                        if (clientTree != null && !String.IsNullOrEmpty(clientTree.DemandCode))
                        {
                            demandCode = clientTree.DemandCode;
                        }
                    }
                    else
                    {
                        demandCode = clientTree.DemandCode;
                    }

                    //Расчет прост промо эффекта
                    double? postPromoEffectW1 = null;
                    double? postPromoEffectW2 = null;
                    double? postPromoEffectW1Qty = null;
                    double? postPromoEffectW2Qty = null;
                    double? planProductBaselineCaseQtyW1 = null;
                    double? planProductBaselineCaseQtyW2 = null;
                    double? planProductPostPromoEffectLSVW1 = null;
                    double? planProductPostPromoEffectLSVW2 = null;
                    double? planProductBaselineLSVW1 = null;
                    double? planProductBaselineLSVW2 = null;

                    if (clientTree != null)
                    {
                        postPromoEffectW1 = clientTree.PostPromoEffectW1;
                        postPromoEffectW2 = clientTree.PostPromoEffectW2;
                    }
                    if (postPromoEffectW1 != null)
                    {
                        postPromoEffectW1Qty = promoProduct.PlanProductIncrementalCaseQty * (postPromoEffectW1 / 100);
                        planProductBaselineCaseQtyW1 = promoProduct.PlanProductBaselineCaseQty * (postPromoEffectW1 / 100);
                        planProductPostPromoEffectLSVW1 = promoProduct.PlanProductPostPromoEffectLSV * (postPromoEffectW1 / 100);
                        planProductBaselineLSVW1 = promoProduct.PlanProductBaselineLSV * (postPromoEffectW1 / 100);
                    }
                    if (postPromoEffectW2 != null)
                    {
                        postPromoEffectW2Qty = promoProduct.PlanProductIncrementalCaseQty * (postPromoEffectW2 / 100);
                        planProductBaselineCaseQtyW2 = promoProduct.PlanProductBaselineCaseQty * (postPromoEffectW2 / 100);
                        planProductPostPromoEffectLSVW2 = promoProduct.PlanProductPostPromoEffectLSV * (postPromoEffectW2 / 100);
                        planProductBaselineLSVW2 = promoProduct.PlanProductBaselineLSV * (postPromoEffectW2 / 100);
                    }

                    postPromoEffectW1Qty = postPromoEffectW1Qty != null ? Math.Round(postPromoEffectW1Qty.Value, 2) : 0;
                    planProductBaselineCaseQtyW1 = planProductBaselineCaseQtyW1 != null ? Math.Round(planProductBaselineCaseQtyW1.Value, 2) : 0;
                    planProductPostPromoEffectLSVW1 = planProductPostPromoEffectLSVW1 != null ? Math.Round(planProductPostPromoEffectLSVW1.Value, 2) : 0;
                    planProductBaselineLSVW1 = planProductBaselineLSVW1 != null ? Math.Round(planProductBaselineLSVW1.Value, 2) : 0;

                    postPromoEffectW2Qty = postPromoEffectW2Qty != null ? Math.Round(postPromoEffectW2Qty.Value, 2) : 0;
                    planProductBaselineCaseQtyW2 = planProductBaselineCaseQtyW2 != null ? Math.Round(planProductBaselineCaseQtyW2.Value, 2) : 0;
                    planProductPostPromoEffectLSVW2 = planProductPostPromoEffectLSVW2 != null ? Math.Round(planProductPostPromoEffectLSVW2.Value, 2) : 0;
                    planProductBaselineLSVW2 = planProductBaselineLSVW2 != null ? Math.Round(planProductBaselineLSVW2.Value, 2) : 0;

                    //Первый день после промо
                    var promoEffectBegin = ((DateTimeOffset)promo.EndDate).Date.AddDays(1);

                    var marsWeekBeginDiff = DayOfWeek.Sunday - promoEffectBegin.DayOfWeek;
                    if (marsWeekBeginDiff < 0)
                    {
                        marsWeekBeginDiff += 7;
                    }

                    var weekStart = promoEffectBegin.AddDays(marsWeekBeginDiff);
                    var week = TimeSpan.FromDays(7);

                    // Первые 2 полные недели после промо
                    result.Add(ReportCreateWeek(promoProduct, promo, demandCode, promoStatus, weekStart, postPromoEffectW1Qty, postPromoEffectW2Qty, planProductBaselineCaseQtyW1, planProductBaselineCaseQtyW2, planProductPostPromoEffectLSVW1, planProductPostPromoEffectLSVW2, planProductBaselineLSVW1, planProductBaselineLSVW2));
                }
            });

            return result.AsQueryable();
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<PlanPostPromoEffectReportWeekView> GetPlanPostPromoEffectReports() {
            return GetConstraintedQuery();
        }

        private IEnumerable<Column> GetExportSettings() {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 1, Field = "ZREP", Header = "ZREP", Quoting = false },
                new Column() { Order = 2, Field = "DemandCode", Header = "Demand Code", Quoting = false },
                new Column() { Order = 3, Field = "PromoNameId", Header = "Promo Name Id", Quoting = false },
                new Column() { Order = 4, Field = "LocApollo", Header = "Loc", Quoting = false },
                new Column() { Order = 5, Field = "TypeApollo", Header = "Type", Quoting = false },
                new Column() { Order = 6, Field = "ModelApollo", Header = "Model", Quoting = false },
                new Column() { Order = 7, Field = "WeekStartDate", Header = "Week Start Date", Quoting = false, Format = "dd.MM.yyyy"  },
                new Column() { Order = 8, Field = "PlanPostPromoEffectQty", Header = "Qty", Quoting = false },
				new Column() { Order = 9, Field = "PlanUplift", Header = "Uplift Plan", Quoting = false },
                new Column() { Order = 10, Field = "DispatchesStart", Header = "Dispatch Start", Quoting = false, Format = "dd.MM.yyyy"  },
                new Column() { Order = 11, Field = "DispatchesEnd", Header = "Dispatch End", Quoting = false, Format = "dd.MM.yyyy" },
				new Column() { Order = 12, Field = "Week", Header = "Week", Quoting = false },
				new Column() { Order = 13, Field = "Status", Header = "Status", Quoting = false },
				new Column() { Order = 14, Field = "PlanProductBaselineCaseQty", Header = "Plan Product Baseline Case Qty", Quoting = false, Format = "0.00" },
				new Column() { Order = 15, Field = "PlanProductPostPromoEffectLSV", Header = "Plan Product Post Promo Effect LSV", Quoting = false, Format = "0.00" },
				new Column() { Order = 16, Field = "PlanProductBaselineLSV", Header = "Plan Product Baseline LSV", Quoting = false, Format = "0.00" },
				new Column() { Order = 17, Field = "InOut", Header = "InOut", Quoting = false },
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
                    PlanUplift = plan.PlanUplift,
                    DispatchesStart = plan.DispatchesStart,
                    DispatchesEnd = plan.DispatchesEnd,
                    Week = plan.Week,
                    Status = plan.Status,
                    PlanProductBaselineCaseQty = plan.PlanProductBaselineCaseQtyW1,
                    PlanProductPostPromoEffectLSV = plan.PlanProductPostPromoEffectLSVW1,
                    PlanProductBaselineLSV = plan.PlanProductBaselineLSVW1,
                    InOut = plan.InOut
                });
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
                    WeekStartDate = plan.WeekStartDate + week,
                    PlanPostPromoEffectQty = plan.PlanPostPromoEffectQtyW2,
                    PlanUplift = plan.PlanUplift,
                    DispatchesStart = plan.DispatchesStart,
                    DispatchesEnd = plan.DispatchesEnd,
                    Week = plan.Week,
                    Status = plan.Status,
                    PlanProductBaselineCaseQty = plan.PlanProductBaselineCaseQtyW2,
                    PlanProductPostPromoEffectLSV = plan.PlanProductPostPromoEffectLSVW2,
                    PlanProductBaselineLSV = plan.PlanProductBaselineLSVW2,
                    InOut = plan.InOut
                });
             
            }
            return result.AsQueryable();
        }
        
        private PlanPostPromoEffectReport ReportCreate(PromoProduct promoproduct, Promo promo, String demandCode, String promoStatus, DateTime weekStart, double? qty, double? planProductBaselineCaseQty, double? planProductPostPromoEffectLSV, double? planProductBaselineLSV) {
			PlanPostPromoEffectReport rep = new PlanPostPromoEffectReport();
            rep.ZREP = promoproduct.ZREP + "_0125";
            rep.PlanPostPromoEffectQty = qty;
            rep.Status = promoStatus;
            rep.PromoNameId = promo.Name + "#" + promo.Number.ToString();
            rep.WeekStartDate = weekStart;
            rep.DispatchesStart = promo.DispatchesStart;
            rep.DispatchesEnd = promo.DispatchesEnd;
            rep.DemandCode = String.IsNullOrEmpty(demandCode) ? "Demand code was not found" : demandCode;
            rep.InOut = promo.InOut;
            rep.Id = Guid.NewGuid();
            rep.LocApollo = "RU_0125";
            rep.TypeApollo = "7";
            rep.ModelApollo = "SHIP_LEWAND_CS";
			rep.Week = SetWeekByMarsDates(weekStart);
			rep.PlanProductBaselineCaseQty = planProductBaselineCaseQty;
			rep.PlanProductPostPromoEffectLSV = planProductPostPromoEffectLSV;
			rep.PlanProductBaselineLSV = planProductBaselineLSV;
			rep.PlanUplift = promo.PlanPromoUpliftPercent;

			return rep;
        }
        private PlanPostPromoEffectReportWeekView ReportCreateWeek(PromoProduct promoproduct, SimplePromo promo, String demandCode, String promoStatus, DateTime weekStart, double? qtyW1, double? qtyW2, double? planProductBaselineCaseQtyW1, double? planProductBaselineCaseQtyW2, double? planProductPostPromoEffectLSVW1, double? planProductPostPromoEffectLSVW2, double? planProductBaselineLSVW1, double? planProductBaselineLSVW2)
        {
            PlanPostPromoEffectReportWeekView rep = new PlanPostPromoEffectReportWeekView();
            rep.ZREP = promoproduct.ZREP + "_0125";
            rep.Status = promoStatus;
            rep.PromoNameId = promo.Name + "#" + promo.Number.ToString();
            rep.WeekStartDate = weekStart;
            rep.DispatchesStart = promo.DispatchesStart;
            rep.DispatchesEnd = promo.DispatchesEnd;
            rep.DemandCode = String.IsNullOrEmpty(demandCode) ? "Demand code was not found" : demandCode;
            rep.InOut = promo.InOut;
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
            rep.PlanUplift = promo.PlanPromoUpliftPercent;

            return rep;
        }
        //Преобразование дат в формат MarsDate 
        public string SetWeekByMarsDates(DateTime weekStartDate)
		{
			string stringFormatYP2W = "{0}P{1:D2}W{2}";
			string marsDate = (new MarsDate((DateTimeOffset)weekStartDate).ToString(stringFormatYP2W));
			return marsDate;
		}

		private bool EntityExists(Guid key) {
            return Context.Set<PromoProduct>().Count(e => e.Id == key) > 0;
        }

    }

    public class SimplePromo
    {
        public Guid Id { get; set; }
        public string PromoStatusName { get; set; }
        public int? ClientTreeId { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string Name { get; set; }

        public int? Number { get; set; }
        public DateTimeOffset? DispatchesStart { get; set; }
        public DateTimeOffset? DispatchesEnd { get; set; }
        public bool? InOut { get; set; }
        public double? PlanPromoUpliftPercent { get; set; }
    }
}
