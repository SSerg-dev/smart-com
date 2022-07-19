using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Newtonsoft.Json;
using Persist.Model;
using Persist.ScriptGenerator.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;

namespace Module.Frontend.TPM.Controllers
{
    public class RollingScenariosController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;
        private readonly UserInfo user;
        private readonly string role;
        private readonly Guid? roleId;

        public RollingScenariosController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
            user = authorizationManager.GetCurrentUser();
            var roleInfo = authorizationManager.GetCurrentRole();
            role = roleInfo.SystemName;
            roleId = roleInfo.Id;
        }
        public IQueryable<RollingScenario> GetConstraintedQuery()
        {
            PerformanceLogger logger = new PerformanceLogger();
            logger.Start();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<RollingScenario> query = Context.Set<RollingScenario>().AsNoTracking();
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters, FilterQueryModes.Active); // TODO workflow

            // Не администраторы не смотрят чужие черновики
            if (role != "Administrator" && role != "SupportAdministrator")
            {
                query = query.Where(e => e.PromoStatus.SystemName != "Draft" || e.CreatorId == user.Id);
            }
            logger.Stop();
            return query;
        }
        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<RollingScenario> GetRollingScenarios()
        {
            return GetConstraintedQuery();
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult MassApprove()
        {
            var promoNumbers = Request.Content.ReadAsStringAsync().Result;
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);
            
            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult Cancel()
        {
            var promoNumbers = Request.Content.ReadAsStringAsync().Result;
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult GetCanceled()
        {
            var promoNumbers = Request.Content.ReadAsStringAsync().Result;
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult OnApproval()
        {
            var promoNumbers = Request.Content.ReadAsStringAsync().Result;
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult Approve()
        {
            var promoNumbers = Request.Content.ReadAsStringAsync().Result;
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult Decline()
        {
            var promoNumbers = Request.Content.ReadAsStringAsync().Result;
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
        }
        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult GetVisibleButton()
        {
            var promoNumbers = Request.Content.ReadAsStringAsync().Result;
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
        }
    }
}
