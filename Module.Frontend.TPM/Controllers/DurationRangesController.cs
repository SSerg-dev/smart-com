using Core.Security;
using Frontend.Core.Controllers.Base;
using Module.Persist.TPM.Model.TPM;
using System.Linq;
using System.Web.Http.OData;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers
{
    public class DurationRangesController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public DurationRangesController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<DurationRange> GetDurationRanges()
        {
            return Context.Set<DurationRange>();
        }
    }
}