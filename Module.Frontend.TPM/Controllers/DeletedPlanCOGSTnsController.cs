using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Module.Persist.TPM.Model.TPM;
using Persist.Model;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers
{
    public class DeletedPlanCOGSTnsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public DeletedPlanCOGSTnsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<PlanCOGSTn> GetConstraintedQuery()
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IQueryable<PlanCOGSTn> query = Context.Set<PlanCOGSTn>();

            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<PlanCOGSTn> GetDeletedPlanCOGSTns()
        {
            return GetConstraintedQuery().Where(e => e.Disabled);
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<PlanCOGSTn> GetDeletedPlanCOGSTn([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery()
                .Where(e => e.Id == key)
                .Where(e => e.Disabled));
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<PlanCOGSTn> GetFilteredData(ODataQueryOptions<PlanCOGSTn> options)
        {
            var query = GetConstraintedQuery().Where(e => e.Disabled);

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<PlanCOGSTn>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<PlanCOGSTn>;
        }
    }
}
