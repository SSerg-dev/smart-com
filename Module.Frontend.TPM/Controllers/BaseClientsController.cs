using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Newtonsoft.Json;
using Persist.Model;
using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Контроллер для работы с базовыми клиентами из иерархии
    /// </summary>
    public class BaseClientsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public BaseClientsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<ClientTree> GetConstraintedQuery(bool onlyBaseClient)
        {

            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            DateTime dt = DateTime.Now;
            IQueryable<ClientTree> query;
            if (onlyBaseClient)
            {
                query = Context.Set<ClientTree>().Where(x => DateTime.Compare(x.StartDate, dt) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dt) > 0) && x.IsBaseClient == true);
            }
            else
            {
                query = Context.Set<ClientTree>().Where(x => DateTime.Compare(x.StartDate, dt) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dt) > 0));
            }
            
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);
            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<ClientTree> GetBaseClient([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery(true));
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<ClientTree> GetBaseClients()
        {
            return GetConstraintedQuery(true);
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<ClientTree> GetFilteredData(ODataQueryOptions<ClientTree> options)
        {
            var query = GetConstraintedQuery(true);

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<ClientTree>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<ClientTree>;
        }

        private bool EntityExists(int key)
        {
            return Context.Set<ClientTree>().Count(e => e.ObjectId == key) > 0;
        }
        [ClaimsAuthorize]
        
        public IHttpActionResult GetCalendarClients()
        {
            List<ClientTree> clientTrees = GetConstraintedQuery(false).ToList();
            List<ClientTree> clientGroup = clientTrees.Where(g => g.Type == "root").ToList();
            foreach (var item in clientGroup)
            {

            }
            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
        }
    }
}
