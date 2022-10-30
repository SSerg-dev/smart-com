using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
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
    public class DeletedPromoProductCorrectionPriceIncreasesController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public DeletedPromoProductCorrectionPriceIncreasesController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<PromoProductCorrectionPriceIncrease> GetConstraintedQuery()
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            IQueryable<PromoProductCorrectionPriceIncrease> query = Context.Set<PromoProductCorrectionPriceIncrease>();

            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<PromoProductCorrectionPriceIncrease> GetDeletedPromoProductCorrectionPriceIncreases()
        {
            return GetConstraintedQuery()
                .Where(e => e.Disabled && e.TempId == null);
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<PromoProductCorrectionPriceIncrease> GetFilteredData(ODataQueryOptions<PromoProductCorrectionPriceIncrease> options)
        {
            var query = GetConstraintedQuery().Where(e => e.Disabled);

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<PromoProductCorrectionPriceIncrease>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<PromoProductCorrectionPriceIncrease>;
        }
    }
}
