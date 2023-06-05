﻿using Core.Security;
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

    public class DeletedTechHighLevelsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public DeletedTechHighLevelsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<TechHighLevel> GetConstraintedQuery()
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IQueryable<TechHighLevel> query = Context.Set<TechHighLevel>();

            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<TechHighLevel> GetDeletedTechHighLevels()
        {
            return GetConstraintedQuery().Where(e => e.Disabled);
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<TechHighLevel> GetDeletedTechHighLevel([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery()
                .Where(e => e.Id == key)
                .Where(e => e.Disabled));
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<TechHighLevel> GetFilteredData(ODataQueryOptions<TechHighLevel> options)
        {
            var query = GetConstraintedQuery().Where(e => e.Disabled);

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<TechHighLevel>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<TechHighLevel>;
        }
    }

}
