﻿using Core.Security;
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
using Module.Persist.TPM.Enum;
using Looper.Core;
using Looper.Parameters;
using Persist;
using Persist.Model.Settings;
using Module.Frontend.TPM.FunctionalHelpers.Scenario;

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

                        ScenarioHelper.DeleteScenarioPeriod(rollingScenarioId, Context);

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
        public async Task<IHttpActionResult> Calculate(int rsId)
        {
            try
            {
                Setting settingTime = Context.Set<Setting>().FirstOrDefault(g => g.Name == "ML_TIME_BLOCK");
                List<DateTimeOffset> times = settingTime.Value.Split(';').Select(g => ChangeTimeZoneUtil.ResetTimeZone(DateTimeOffset.Parse(g))).ToList();
                DateTimeOffset TimeNow = TimeHelper.Now();
                if (TimeNow < times[0] || times[1] < TimeNow)
                {
                    return InternalServerError(new Exception(string.Format("Scenario calculation can be started from {0} to {1}", times[0], times[1])));
                }
                UserInfo user = authorizationManager.GetCurrentUser();
                Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
                RoleInfo role = authorizationManager.GetCurrentRole();
                Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

                if (role.SystemName == "CMManager" || role.SystemName == "Administrator" || role.SystemName == "FunctionalExpert" ||
                    role.SystemName == "KeyAccountManager" || role.SystemName == "CustomerMarketing" || role.SystemName == "SupportAdministrator")
                {
                    HandlerData handlerData = new HandlerData();
                    HandlerDataHelper.SaveIncomingArgument("UserId", userId, handlerData, visible: false, throwIfNotExists: false);
                    HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, handlerData, visible: false, throwIfNotExists: false);
                    HandlerDataHelper.SaveIncomingArgument("UserId", userId, handlerData, visible: false, throwIfNotExists: false);
                    HandlerDataHelper.SaveIncomingArgument("RsId", rsId, handlerData, visible: false, throwIfNotExists: false);


                    LoopHandler handler = new LoopHandler()
                    {
                        Id = Guid.NewGuid(),
                        ConfigurationName = "PROCESSING",
                        Description = "Preparing scenario for calculation",
                        Name = "Module.Host.TPM.Handlers.ProcessMLCalendarHandler",
                        ExecutionPeriod = null,
                        CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                        LastExecutionDate = null,
                        NextExecutionDate = null,
                        ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                        UserId = userId,
                        RoleId = roleId
                    };
                    handler.SetParameterData(handlerData);
                    Context.LoopHandlers.Add(handler);
                    await Context.SaveChangesAsync();
                    return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
                }
                else
                {
                    return InternalServerError(new Exception("Only Key Account Manager send to approval!"));
                }
            }
            catch (Exception e)
            {
                return Content<string>(HttpStatusCode.InternalServerError, e.Message);
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
                ScenarioHelper.OnApprovalScenarioPeriod(rollingScenarioId, Context);
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

                        ScenarioHelper.ApproveScenarioPeriod(rollingScenarioId, Context);

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
                return InternalServerError(new Exception("Only Customer Marketing Manager approve!"));
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

            RollingScenario RS = Context.Set<RollingScenario>()
                .AsNoTracking()
                .FirstOrDefault(g => g.Id == rollingScenarioId);

            bool isApprovalDisabled = RS.RSstatus != RSstateNames.DRAFT;
            bool isShowLogDisabled = RS.HandlerId == null;

            if (role.SystemName == "KeyAccountManager")
            {
                if (RS.RSstatus == RSstateNames.WAITING || RS.RSstatus == RSstateNames.CALCULATING)
                {
                    if (RS.RSstatus == RSstateNames.WAITING && string.IsNullOrEmpty(RS.TaskStatus))
                    {
                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, OnApproval = isApprovalDisabled, Approve = true, Decline = true, Calculate = false, ShowLog = isShowLogDisabled }));
                    }
                    if (RS.RSstatus == RSstateNames.CALCULATING)
                    {
                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, OnApproval = isApprovalDisabled, Approve = true, Decline = true, Calculate = true, ShowLog = isShowLogDisabled }));
                    }
                }
                else
                {
                    if (!RS.IsSendForApproval && !RS.Disabled && (RS.TaskStatus == TaskStatusNames.COMPLETE || string.IsNullOrEmpty(RS.TaskStatus)))
                    {
                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, OnApproval = isApprovalDisabled, Approve = true, Decline = true, Calculate = true, ShowLog = isShowLogDisabled })); //статусы инвертированы для.setDisabled(false) 
                    }
                    else
                    {
                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, OnApproval = isApprovalDisabled, Approve = true, Decline = true, Calculate = true, ShowLog = isShowLogDisabled }));
                    }
                }

            }
            if (role.SystemName == "CMManager")
            {
                if (RS.RSstatus == RSstateNames.WAITING || RS.RSstatus == RSstateNames.CALCULATING)
                {
                    if (RS.RSstatus == RSstateNames.WAITING && string.IsNullOrEmpty(RS.TaskStatus))
                    {
                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, OnApproval = isApprovalDisabled, Approve = true, Decline = true, Calculate = false, ShowLog = isShowLogDisabled }));
                    }
                    if (RS.RSstatus == RSstateNames.CALCULATING)
                    {
                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, OnApproval = isApprovalDisabled, Approve = true, Decline = true, Calculate = true, ShowLog = isShowLogDisabled }));
                    }
                }
                else
                {
                    if (RS.IsSendForApproval && !RS.IsCMManagerApproved && !RS.Disabled && (RS.TaskStatus == TaskStatusNames.COMPLETE || string.IsNullOrEmpty(RS.TaskStatus))) // TODO выяснить как определяется, нужно ли пересчитывать ночью промо
                    {
                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, OnApproval = isApprovalDisabled, Approve = false, Decline = false, Calculate = true, ShowLog = isShowLogDisabled }));
                    }
                    else
                    {
                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, OnApproval = isApprovalDisabled, Approve = true, Decline = true, Calculate = true, ShowLog = isShowLogDisabled }));
                    }
                }
            }
            if (role.SystemName == "Administrator")
            {
                if (RS.RSstatus == RSstateNames.WAITING || RS.RSstatus == RSstateNames.CALCULATING)
                {
                    if (RS.RSstatus == RSstateNames.WAITING && string.IsNullOrEmpty(RS.TaskStatus))
                    {
                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, OnApproval = isApprovalDisabled, Approve = true, Decline = true, Calculate = false, ShowLog = isShowLogDisabled }));
                    }
                    if (RS.RSstatus == RSstateNames.CALCULATING)
                    {
                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, OnApproval = isApprovalDisabled, Approve = true, Decline = true, Calculate = true, ShowLog = isShowLogDisabled }));
                    }
                }
                else
                {
                    if (!RS.IsSendForApproval && !RS.Disabled && (RS.TaskStatus == TaskStatusNames.COMPLETE || string.IsNullOrEmpty(RS.TaskStatus)))
                    {
                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, OnApproval = isApprovalDisabled, Approve = true, Decline = true, Calculate = true, ShowLog = isShowLogDisabled }));
                    }
                    else if (RS.IsSendForApproval && !RS.IsCMManagerApproved && !RS.Disabled) // TODO выяснить как определяется, нужно ли пересчитывать ночью промо
                    {
                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, OnApproval = isApprovalDisabled, Approve = false, Decline = false, Calculate = true, ShowLog = isShowLogDisabled }));
                    }
                    else
                    {
                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, OnApproval = isApprovalDisabled, Approve = true, Decline = true, Calculate = true, ShowLog = isShowLogDisabled }));
                    }
                }

            }
            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false }));
        }
        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult UploadScenario(Guid savedScenarioId)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    ClientTree clientTree = ScenarioHelper.UploadSavedScenario(savedScenarioId, Context);
                    transaction.Commit();
                    return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, message = $"Scenario for client {clientTree.FullPathName} upload" }));
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return InternalServerError(GetExceptionMessage.GetInnerException(e));
                }
            }
        }
        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult GetStatusScenario(Guid savedScenarioId)
        {
            try
            {
                ScenarioHelper.StatusScenarioResult statusScenarioResult = ScenarioHelper.GetStatusScenario(savedScenarioId, Context);
                if (statusScenarioResult != null)
                {
                    return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, message = $"The client {statusScenarioResult.ClientName} has an active RA scenario id № {statusScenarioResult.RSId}. Change the active scenario to Cancelled?" }));
                }
                else
                {
                    return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false}));
                }
                
            }
            catch (Exception e)
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }
        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult GetStatusScenario(int objectId)
        {
            try
            {
                if (ScenarioHelper.CheckCogs(objectId, Context))
                {
                    return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, gogs = true, message = $"Error. There are no COGSs for (Brandtech) by (Client)." }));
                }
                ScenarioHelper.StatusScenarioResult statusScenarioResult = ScenarioHelper.GetStatusScenario(objectId, Context);
                if (statusScenarioResult != null)
                {
                    return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, message = $"The client {statusScenarioResult.ClientName} has an active RA scenario id № {statusScenarioResult.RSId}. Change the active scenario to Cancelled?" }));
                }
                else
                {
                    return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false }));
                }

            }
            catch (Exception e)
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }
    }
}
