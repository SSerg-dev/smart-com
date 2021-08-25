using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist.Model;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;

namespace Module.Frontend.TPM.Controllers
{
    public class DeletedPromoProductsCorrectionsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public DeletedPromoProductsCorrectionsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<PromoProductsCorrection> GetConstraintedQuery()
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            IQueryable<PromoProductsCorrection> query = Context.Set<PromoProductsCorrection>();

            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<PromoProductsCorrection> GetDeletedPromoProductsCorrections()
        {
            return GetConstraintedQuery().Where(e => e.Disabled && e.TempId == null);
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public SingleResult<PromoProductsCorrection> GetDeletedPromoProductsCorrection([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery()
                .Where(e => e.Id == key)
                .Where(e => e.Disabled));
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<PromoProductsCorrection> GetFilteredData(ODataQueryOptions<PromoProductsCorrection> options)
        {
            var query = GetConstraintedQuery().Where(e => e.Disabled);

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<PromoProductsCorrection>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<PromoProductsCorrection>;
        }
    }
}

