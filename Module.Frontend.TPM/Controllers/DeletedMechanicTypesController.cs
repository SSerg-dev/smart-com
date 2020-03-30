﻿using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.TPM;
using Persist.Model;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers {

    public class DeletedMechanicTypesController : EFContextController {
        private readonly IAuthorizationManager authorizationManager;

        public DeletedMechanicTypesController(IAuthorizationManager authorizationManager) {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<MechanicType> GetConstraintedQuery() {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IQueryable<MechanicType> query = Context.Set<MechanicType>();

            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<MechanicType> GetDeletedMechanicTypes() {
            return GetConstraintedQuery().Where(e => e.Disabled);
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<MechanicType> GetDeletedMechanicType([FromODataUri] System.Guid key) {
            return SingleResult.Create(GetConstraintedQuery()
                .Where(e => e.Id == key)
                .Where(e => e.Disabled));
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<MechanicType> GetFilteredData(ODataQueryOptions<MechanicType> options)
        {
            var query = GetConstraintedQuery().Where(e => e.Disabled);

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<MechanicType>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<MechanicType>;
        }
    }

}
