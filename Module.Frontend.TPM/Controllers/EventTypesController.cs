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
    public class EventTypesController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public EventTypesController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }
        protected IQueryable<EventType> GetConstraintedQuery()
        {

            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IQueryable<EventType> query = Context.Set<EventType>().Where(e => !e.Disabled);

            return query;
        }
        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<EventType> GetEventTypes()
        {
            var result = GetConstraintedQuery();
            return result;
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<Event> GetFilteredData(ODataQueryOptions<Event> options)
        {
            var query = GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };
            var optionsPost = new ODataQueryOptionsPost<Event>(options.Context, Request, HttpContext.Current.Request);

            return optionsPost.ApplyTo(query, querySettings) as IQueryable<Event>;
        }
    }
}
