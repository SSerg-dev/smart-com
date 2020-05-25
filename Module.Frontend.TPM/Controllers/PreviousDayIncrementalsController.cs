using AutoMapper;
using Core.Security;
using Core.Security.Models;
using Core.Settings;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.Results;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;

namespace Module.Frontend.TPM.Controllers
{
    public class PreviousDayIncrementalsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public PreviousDayIncrementalsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }


        protected IQueryable<PreviousDayIncremental> GetConstraintedQuery()
        {

            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            int count = Context.Set<PreviousDayIncremental>().Count();
            IQueryable<PreviousDayIncremental> query = Context.Set<PreviousDayIncremental>().Take(count).AsQueryable();

            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

            return query;
        }
               
        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<PreviousDayIncremental> GetPreviousDayIncrementals()
        {
            return GetConstraintedQuery();
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<PreviousDayIncremental> GetFilteredData(ODataQueryOptions<PreviousDayIncremental> options)
        {
            var query = GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<PreviousDayIncremental>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<PreviousDayIncremental>;
        }

        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<PreviousDayIncremental> options)
        {
            try
            {
                IQueryable results = options.ApplyTo(GetConstraintedQuery());
                IEnumerable<Column> columns = GetPromoProductCorrectionExportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                UserInfo user = authorizationManager.GetCurrentUser();
                string username = user == null ? "" : user.Login;
                string filePath = exporter.GetExportFileName("PreviousDayIncremental", username);
                exporter.Export(results, filePath);
                string filename = System.IO.Path.GetFileName(filePath);
                return Content<string>(HttpStatusCode.OK, filename);
            }
            catch (Exception e)
            {
                return Content<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private IEnumerable<Column> GetPromoProductCorrectionExportSettings()
        {
            int orderNumber = 1;
            IEnumerable<Column> columns = new List<Column>
            {
                 new Column { Order = orderNumber++, Field = "Week", Header = "Week", Quoting = false },
                 new Column { Order = orderNumber++, Field = "DemandCode", Header = "Demand Code", Quoting = false },
                 new Column { Order = orderNumber++, Field = "IncrementalQty", Header = "Incremental Qty", Quoting = false },
                 new Column { Order = orderNumber++, Field = "Promo.Number", Header = "Promo ID", Quoting = false,  Format = "0" }, 
                 new Column { Order = orderNumber++, Field = "Product.ZREP", Header = "ZREP", Quoting = false,  Format = "0" }, 
                 new Column { Order = orderNumber++, Field = "LastChangeDate", Header = "Last Change Date", Quoting = false,Format = "dd.MM.yyyy"}

            };

            return columns;
        }

    }
}
