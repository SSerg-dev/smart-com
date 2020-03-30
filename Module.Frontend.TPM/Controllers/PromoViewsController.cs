using AutoMapper;
using Core.Data;
using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions.Export;
using Module.Persist.TPM.Model.TPM;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;
using Module.Persist.TPM.Utils;
using Module.Persist.TPM.Model.DTO;
using Looper.Core;
using Looper.Parameters;
using Persist;
using System.Linq.Expressions;
using Module.Frontend.TPM.Util;
using System.Web;
using System.Web.Http.OData.Extensions;

namespace Module.Frontend.TPM.Controllers {
    public class PromoViewsController : EFContextController {
        private readonly IAuthorizationManager authorizationManager;

        public PromoViewsController(IAuthorizationManager authorizationManager) {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<PromoView> GetConstraintedQuery() {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<PromoView> query = Context.Set<PromoView>().AsNoTracking();
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

            // Не администраторы не смотрят чужие черновики
            if (role != "Administrator") {
                query = query.Where(e => e.PromoStatusSystemName != "Draft" || e.CreatorId == user.Id);
            }

            var statusesForExcluding = new List<string>() { "Deleted" };
            query = query.Where(x => !statusesForExcluding.Contains(x.PromoStatusSystemName));

            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<PromoView> GetPromoView([FromODataUri] Guid key) {
            return SingleResult.Create(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<PromoView> GetPromoViews() {
            return GetConstraintedQuery();
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<PromoView> GetFilteredData(ODataQueryOptions<PromoView> options)
        {
            var query = GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<PromoView>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<PromoView>;
        }

        private bool EntityExists(Guid key) {
            return Context.Set<PromoView>().Count(e => e.Id == key) > 0;
        }

        /// <summary>
        /// Экспорт календаря в эксель
        /// </summary>
        /// <param name="options"></param>
        /// <param name="data">clients - список id клиентов соответствующих фильтру на клиенте, year - год</param>
        /// <returns></returns>
        // [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult ExportSchedule(ODataQueryOptions<Promo> options, ODataActionParameters data)
        {
            try
            {
                // TODO: Передавать фильтр в параметры задачи
                //var tsts = options.RawValues.Filter;
                //var tst2s = JsonConvert.SerializeObject(options, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                UserInfo user = authorizationManager.GetCurrentUser();
                Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
                RoleInfo role = authorizationManager.GetCurrentRole();
                Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);
                IEnumerable<int> clients = (IEnumerable<int>)data["clients"];

                IQueryable<Promo> queryable = Enumerable.Empty<Promo>().AsQueryable();
                var rawFilter2s = options.RawValues;
                string rawFilters = LinqToQueryHelper.BuildQueryString(rawFilter2s);
                HandlerData handlerData = new HandlerData();
                HandlerDataHelper.SaveIncomingArgument("UserId", userId, handlerData, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, handlerData, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UserId", userId, handlerData, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("rawFilters", rawFilters, handlerData, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("clients", clients.ToList(), handlerData, visible: false, throwIfNotExists: false);

                //IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));
                //List<Promo> promoes = CastQueryToPromo(results);
                if (data.Count() > 1)
                {
                    HandlerDataHelper.SaveIncomingArgument("year", (int)data["year"], handlerData, visible: false, throwIfNotExists: false);
                }
                using (DatabaseContext context = new DatabaseContext())
                {
                    LoopHandler handler = new LoopHandler()
                    {
                        Id = Guid.NewGuid(),
                        ConfigurationName = "PROCESSING",
                        Description = "Scheduler Export",
                        Name = "Module.Host.TPM.Handlers.SchedulerExportHandler",
                        ExecutionPeriod = null,
                        CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                        LastExecutionDate = null,
                        NextExecutionDate = null,
                        ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                        UserId = userId,
                        RoleId = roleId
                    };
                    handler.SetParameterData(handlerData);
                    context.LoopHandlers.Add(handler);
                    context.SaveChanges();
                }
                return Content<string>(HttpStatusCode.OK, "Export task successfully created");
            }
            catch (Exception e)
            {
                return Content<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
