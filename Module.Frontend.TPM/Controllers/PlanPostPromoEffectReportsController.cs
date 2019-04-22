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

        public IQueryable<PlanPostPromoEffectReport> GetConstraintedQuery() {
            List<PlanPostPromoEffectReport> result = new List<PlanPostPromoEffectReport>();
            List<PromoProduct> promoproducts = Context.Set<PromoProduct>().Where(y => !y.Disabled && y.PlanProductQty > 0).ToList();
            DateTime dt = DateTime.Now;
            foreach (PromoProduct promoproduct in promoproducts) {
                Promo promo = Context.Set<Promo>().FirstOrDefault(x => x.Id == promoproduct.PromoId);
                String promoStatus = promo.PromoStatus.Name;

                String demandCode = null;

                ClientTree clientTree = Context.Set<ClientTree>().FirstOrDefault(x => x.ObjectId == promo.ClientTreeId && (DateTime.Compare(x.StartDate, dt) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dt) > 0)));

                if (clientTree == null) {
                    demandCode = null;
                } else if (String.IsNullOrEmpty(clientTree.DemandCode)) {
                    clientTree = Context.Set<ClientTree>().FirstOrDefault(y => y.ObjectId == clientTree.parentId && (DateTime.Compare(y.StartDate, dt) <= 0 && (!y.EndDate.HasValue || DateTime.Compare(y.EndDate.Value, dt) > 0)));
                    if (clientTree != null && !String.IsNullOrEmpty(clientTree.DemandCode)) {
                        demandCode = clientTree.DemandCode;
                    }
                } else {
                    demandCode = clientTree.DemandCode;
                }

                //Расчет прост промо эффекта
                double? postPromoEffectW1 = null;
                double? postPromoEffectW2 = null;
                double? postPromoEffectW1Qty = null;
                double? postPromoEffectW2Qty = null;
                if (clientTree != null) {
                    postPromoEffectW1 = clientTree.PostPromoEffectW1;
                    postPromoEffectW2 = clientTree.PostPromoEffectW2;
                }
                if (postPromoEffectW1 != null) {
                    postPromoEffectW1Qty = promoproduct.PlanProductIncrementalQty * (postPromoEffectW1 / 100);
                }
                if (postPromoEffectW2 != null) {
                    postPromoEffectW2Qty = promoproduct.PlanProductIncrementalQty * (postPromoEffectW2 / 100);
                }


                //Первый день после промо
                DateTime promoEffectBegin = ((DateTimeOffset) promo.EndDate).Date.AddDays(1);

                int marsWeekBeginDiff = DayOfWeek.Sunday - promoEffectBegin.DayOfWeek;
                if (marsWeekBeginDiff < 0) { marsWeekBeginDiff += 7; }

                DateTime weekStart = promoEffectBegin.AddDays(marsWeekBeginDiff); ;
                TimeSpan week = TimeSpan.FromDays(7);
                // Первые 2 полные недели после промо
                result.Add(ReportCreate(promoproduct, promo, demandCode, promoStatus, weekStart, postPromoEffectW1Qty != null ? Math.Round(postPromoEffectW1Qty.Value, 2) : 0));
                weekStart += week;
                result.Add(ReportCreate(promoproduct, promo, demandCode, promoStatus, weekStart, postPromoEffectW2Qty != null ? Math.Round(postPromoEffectW2Qty.Value, 2)  : 0));
            }
            return result.AsQueryable();
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<PlanPostPromoEffectReport> GetPlanPostPromoEffectReports() {
            return GetConstraintedQuery();
        }

        private IEnumerable<Column> GetExportSettings() {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 1, Field = "ZREP", Header = "ZREP", Quoting = false },
                new Column() { Order = 2, Field = "DemandCode", Header = "Demand Code", Quoting = false },
                new Column() { Order = 3, Field = "PromoName", Header = "Promo Name", Quoting = false },
                new Column() { Order = 4, Field = "PromoNameId", Header = "Promo Name Id", Quoting = false },
                new Column() { Order = 5, Field = "LocApollo", Header = "Loc", Quoting = false },
                new Column() { Order = 6, Field = "TypeApollo", Header = "Type", Quoting = false },
                new Column() { Order = 7, Field = "ModelApollo", Header = "Model", Quoting = false },
                new Column() { Order = 8, Field = "WeekStartDate", Header = "Week Start Date", Quoting = false, Format = "dd.MM.yyyy"  },
                new Column() { Order = 9, Field = "PlanPostPromoEffectQty", Header = "Qty", Quoting = false },
                new Column() { Order = 10, Field = "StartDate", Header = "Start date", Quoting = false, Format = "dd.MM.yyyy"  },
                new Column() { Order = 11, Field = "EndDate", Header = "End date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 12, Field = "Status", Header = "Status", Quoting = false },
            };
            return columns;
        }
        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<PlanPostPromoEffectReport> options) {
            try {
                IQueryable results = options.ApplyTo(GetConstraintedQuery());
                IEnumerable<Column> columns = GetExportSettings();
                NonGuidIdExporter exporter = new NonGuidIdExporter(columns);
                UserInfo user = authorizationManager.GetCurrentUser();
                string username = user == null ? "" : user.Login;
                string filePath = exporter.GetExportFileName("PlanPostPromoEffectReport", username);
                exporter.Export(results, filePath);
                string filename = System.IO.Path.GetFileName(filePath);
                return Content<string>(HttpStatusCode.OK, filename);
            } catch (Exception e) {
                return Content<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private PlanPostPromoEffectReport ReportCreate(PromoProduct promoproduct, Promo promo, String demandCode, String promoStatus, DateTime weekStart, double? qty) {
            PlanPostPromoEffectReport rep = new PlanPostPromoEffectReport();
            rep.ZREP = promoproduct.ZREP;
            rep.PlanPostPromoEffectQty = qty;
            rep.Status = promoStatus;
            rep.PromoNameId = promo.Name + "#" + promo.Number.ToString();
            rep.PromoName = promo.Name;
            rep.WeekStartDate = weekStart;
            rep.StartDate = promo.StartDate;
            rep.EndDate = promo.EndDate;
            rep.DemandCode = String.IsNullOrEmpty(demandCode) ? "Demand code was not found" : demandCode;

            rep.Id = Guid.NewGuid();
            rep.LocApollo = "RU_0125";
            rep.TypeApollo = "7";
            rep.ModelApollo = "SHIP_LEWAND_CS";

            return rep;
        }


        private bool EntityExists(Guid key) {
            return Context.Set<PromoProduct>().Count(e => e.Id == key) > 0;
        }

    }

}
