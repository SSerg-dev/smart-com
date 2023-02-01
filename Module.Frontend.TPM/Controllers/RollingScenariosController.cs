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
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;
using Module.Frontend.TPM.FunctionalHelpers.RSPeriod;
using System.Data.Entity.Infrastructure;
using Module.Frontend.TPM.Util;

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
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters, FilterQueryModes.Active); 


            logger.Stop();
            return query;
        }
        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<RollingScenario> GetRollingScenarios()
        {
            return GetConstraintedQuery();
        }

        /// <summary>
        /// Массовый апрув
        /// </summary>
        /// <returns></returns>
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
        public IHttpActionResult Decline(Guid rollingScenarioId)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);


            if (role.SystemName == "CMManager" || role.SystemName == "Administrator")
            {
                using (var transaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        
                        RSPeriodHelper.DeleteRSPeriod(rollingScenarioId, Context);
                        
                        transaction.Commit();
                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return InternalServerError(GetExceptionMessage.GetInnerException(e));
                    }

                }
            }
            else
            {
                return InternalServerError(new Exception("Only Key Account Manager send to approval!"));
            }
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
        public IHttpActionResult OnApproval(Guid rollingScenarioId)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            if (role.SystemName == "KeyAccountManager" || role.SystemName == "Administrator")
            {
                RSPeriodHelper.OnApprovalRSPeriod(rollingScenarioId, Context);
                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
            }
            else
            {
                return InternalServerError(new Exception("Only Key Account Manager send to approval!"));
            }
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult Approve(Guid rollingScenarioId)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            if (role.SystemName == "CMManager" || role.SystemName == "Administrator")
            {
                
                using (var transaction = Context.Database.BeginTransaction())
                {
                    try
                    {

                        RSPeriodHelper.ApproveRSPeriod(rollingScenarioId, Context);

                        transaction.Commit();
                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
                    }
                    catch (DbUpdateException e)
                    {
                        transaction.Rollback();
                        return InternalServerError(GetExceptionMessage.GetInnerException(e));
                    }

                }

            }
            else
            {
                return InternalServerError(new Exception("Only Key Account Manager send to approval!"));
            }
        }


        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult GetVisibleButton(Guid rollingScenarioId)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id ?? Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id ?? Guid.Empty);

            if (role.SystemName == "KeyAccountManager")
            {
                RollingScenario RS = Context.Set<RollingScenario>()
                    .AsNoTracking()
                    .FirstOrDefault(g => g.Id == rollingScenarioId);
                if (!RS.IsSendForApproval && !RS.Disabled)
                {
                    return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, OnApproval = false, Approve = true, Decline = true })); //статусы инвертированы для.setDisabled(false) 
                }
                else
                {
                    return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, OnApproval = true, Approve = true, Decline = true }));
                }
            }
            if (role.SystemName == "CMManager")
            {
                RollingScenario RS = Context.Set<RollingScenario>()
                    .Include(g => g.Promoes)
                    .AsNoTracking()
                    .FirstOrDefault(g => g.Id == rollingScenarioId);
                if (RS.IsSendForApproval && !RS.IsCMManagerApproved && !RS.Disabled) // TODO выяснить как определяется, нужно ли пересчитывать ночью промо
                {
                    return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, OnApproval = true, Approve = false, Decline = false }));
                }
                else
                {
                    return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, OnApproval = true, Approve = true, Decline = true }));
                }

            }
            if (role.SystemName == "Administrator")
            {
                RollingScenario RS = Context.Set<RollingScenario>()
                    .Include(g => g.Promoes)
                    .AsNoTracking()
                    .FirstOrDefault(g => g.Id == rollingScenarioId);
                if (!RS.IsSendForApproval && !RS.Disabled)
                {
                    return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, OnApproval = false, Approve = true, Decline = true }));
                }
                else if (RS.IsSendForApproval && !RS.IsCMManagerApproved && !RS.Disabled) // TODO выяснить как определяется, нужно ли пересчитывать ночью промо
                {
                    return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, OnApproval = true, Approve = false, Decline = false }));
                }
                else
                {
                    return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, OnApproval = true, Approve = true, Decline = true }));
                }
            }
            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false }));
        }
    }
}
