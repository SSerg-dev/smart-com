﻿using AutoMapper;
using Core.Security;
using Core.Security.Models;
using Core.Settings;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions;
using Frontend.Core.Extensions.Export;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Newtonsoft.Json;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    
        public class DeletedNonPromoSupportsController : EFContextController
        {
            private readonly IAuthorizationManager authorizationManager;

            public DeletedNonPromoSupportsController(IAuthorizationManager authorizationManager)
            {
                this.authorizationManager = authorizationManager;
            }

            protected IQueryable<NonPromoSupport> GetConstraintedQuery()
            {
                UserInfo user = authorizationManager.GetCurrentUser();
                string role = authorizationManager.GetCurrentRoleName();
                IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                    .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                    .ToList() : new List<Constraint>();

                IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
                IQueryable<NonPromoSupport> query = Context.Set<NonPromoSupport>();
                IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();

                query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

                return query;
            }

            [ClaimsAuthorize]
            [EnableQuery(MaxNodeCount = int.MaxValue)]
            public IQueryable<NonPromoSupport> GetDeletedNonPromoSupports()
            {
                return GetConstraintedQuery().Where(e => e.Disabled);
            }
             [ClaimsAuthorize]
             [EnableQuery(MaxNodeCount = int.MaxValue)]
             public SingleResult<NonPromoSupport> GetDeletedNonPromoSupport ([FromODataUri] System.Guid key)
             {
                 return SingleResult.Create(GetConstraintedQuery()
                     .Where(e => e.Id == key)
                     .Where(e => e.Disabled));
             }


    }
    
}
