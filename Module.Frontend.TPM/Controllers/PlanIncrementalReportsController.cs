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

    public class PlanIncrementalReportsController : EFContextController {
        private readonly IAuthorizationManager authorizationManager;

        public PlanIncrementalReportsController(IAuthorizationManager authorizationManager) {
            this.authorizationManager = authorizationManager;
        }

        //[ClaimsAuthorize]
        //[EnableQuery(MaxNodeCount = int.MaxValue)]
        //public SingleResult<PlanIncrementalReport> GetPlanIncrementalReport([FromODataUri] System.Guid key) {
        //    return SingleResult.Create(GetConstraintedQuery());
        //}

        public IQueryable<PlanIncrementalReport> GetConstraintedQuery() {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();

            IQueryable<PlanIncrementalReport> query = Context.Set<PlanIncrementalReport>();
            return query;
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<PlanIncrementalReport> GetPlanIncrementalReports() {
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
                new Column() { Order = 9, Field = "PlanProductQty", Header = "Qty", Quoting = false, Format = "0.00"},
                new Column() { Order = 10, Field = "PlanUplift", Header = "Uplift Plan", Quoting = false, Format = "0.00" },
                new Column() { Order = 11, Field = "StartDate", Header = "Start date", Quoting = false, Format = "dd.MM.yyyy"  },
                new Column() { Order = 12, Field = "EndDate", Header = "End date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 13, Field = "Status", Header = "Status", Quoting = false },
            };
            return columns;
        }
        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<PlanIncrementalReport> options) {
            try {
                IQueryable results = options.ApplyTo(GetConstraintedQuery());
                IEnumerable<Column> columns = GetExportSettings();
                NonGuidIdExporter exporter = new NonGuidIdExporter(columns);
                UserInfo user = authorizationManager.GetCurrentUser();
                string username = user == null ? "" : user.Login;
                string filePath = exporter.GetExportFileName("PlanIncrementalReport", username);
                exporter.Export(results, filePath);
                string filename = System.IO.Path.GetFileName(filePath);
                return Content<string>(HttpStatusCode.OK, filename);
            } catch (Exception e) {
                return Content<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private bool EntityExists(Guid key) {
            return Context.Set<PromoProduct>().Count(e => e.Id == key) > 0;
        }
    }
}
