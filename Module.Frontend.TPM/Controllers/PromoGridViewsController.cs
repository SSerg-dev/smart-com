using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model;
using Persist.ScriptGenerator.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;

namespace Module.Frontend.TPM.Controllers
{
    public class PromoGridViewsController : EFContextController
    {
        private readonly UserInfo user;
        private readonly string role;
        private readonly Guid? roleId;

        public PromoGridViewsController(IAuthorizationManager authorizationManager)
        {
            user = authorizationManager.GetCurrentUser();
            var roleInfo = authorizationManager.GetCurrentRole();
            role = roleInfo.SystemName;
            roleId = roleInfo.Id;
        }

        public PromoGridViewsController(UserInfo User, string Role, Guid RoleId)
        {
            user = User;
            role = Role;
            roleId = RoleId;
        }

        public IQueryable<PromoGridView> GetConstraintedQuery(bool canChangeStateOnly = false, DatabaseContext localContext = null)
        {
            PerformanceLogger logger = new PerformanceLogger();
            logger.Start();
            if (localContext == null)
            {
                localContext = Context;
            }
            IList<Constraint> constraints = user.Id.HasValue ? localContext.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<PromoGridView> query = localContext.Set<PromoGridView>().AsNoTracking();
            IQueryable<ClientTreeHierarchyView> hierarchy = localContext.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters, FilterQueryModes.Active, canChangeStateOnly ? role : String.Empty);

            // Не администраторы не смотрят чужие черновики
            if (role != "Administrator" && role != "SupportAdministrator")
            {
                query = query.Where(e => e.PromoStatusSystemName != "Draft" || e.CreatorId == user.Id);
            }
            logger.Stop();
            return query;
        }

        protected IQueryable<Promo> GetFullConstraintedQuery()
        {
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<Promo> query = Context.Set<Promo>().AsNoTracking();
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

            // Не администраторы не смотрят чужие черновики
            if (role != "Administrator" && role != "SupportAdministrator")
            {
                query = query.Where(e => e.PromoStatus.SystemName != "Draft" || e.CreatorId == user.Id);
            }
            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<PromoGridView> GetPromoGridView([FromODataUri] Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<PromoGridView> GetPromoGridViews(bool canChangeStateOnly = false)
        {
            return GetConstraintedQuery(canChangeStateOnly);
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<PromoGridView> GetFilteredData(ODataQueryOptions<PromoGridView> options)
        {
            string bodyText = Helper.GetRequestBody(HttpContext.Current.Request);
            var query = GetConstraintedQuery(Helper.GetValueIfExists<bool>(bodyText, "canChangeStateOnly"));

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };
            var optionsPost = new ODataQueryOptionsPost<PromoGridView>(options.Context, Request, HttpContext.Current.Request);

            return optionsPost.ApplyTo(query, querySettings) as IQueryable<PromoGridView>;
        }

        [ClaimsAuthorize]
        public IHttpActionResult Delete([FromODataUri] Guid key)
        {
            try
            {
                var model = Context.Set<Promo>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                Promo promoCopy = AutomapperProfiles.PromoCopy(model);

                model.DeletedDate = DateTime.Now;
                model.Disabled = true;
                model.PromoStatusId = Context.Set<PromoStatus>().FirstOrDefault(e => e.SystemName == "Deleted").Id;

                string userRole = user.GetCurrentRole().SystemName;

                string message;

                PromoStateContext promoStateContext = new PromoStateContext(Context, promoCopy);
                bool status = promoStateContext.ChangeState(model, userRole, out message);

                if (!status)
                {
                    return InternalServerError(new Exception(message));
                }
                List<PromoProduct> promoProductToDeleteList = Context.Set<PromoProduct>().Where(x => x.PromoId == model.Id && !x.Disabled).ToList();
                foreach (PromoProduct promoProduct in promoProductToDeleteList)
                {
                    promoProduct.DeletedDate = System.DateTime.Now;
                    promoProduct.Disabled = true;
                }
                model.NeedRecountUplift = true;
                //необходимо удалить все коррекции
                var promoProductToDeleteListIds = promoProductToDeleteList.Select(x => x.Id).ToList();
                List<PromoProductsCorrection> promoProductCorrectionToDeleteList = Context.Set<PromoProductsCorrection>()
                    .Where(x => promoProductToDeleteListIds.Contains(x.PromoProductId) && x.Disabled != true).ToList();
                foreach (PromoProductsCorrection promoProductsCorrection in promoProductCorrectionToDeleteList)
                {
                    promoProductsCorrection.DeletedDate = DateTimeOffset.UtcNow;
                    promoProductsCorrection.Disabled = true;
                    promoProductsCorrection.UserId = (Guid)user.Id;
                    promoProductsCorrection.UserName = user.Login;
                }
                Context.SaveChanges();

                PromoCalculateHelper.RecalculateBudgets(model, user, Context);
                PromoCalculateHelper.RecalculateBTLBudgets(model, user, Context, safe: true);
                PromoHelper.WritePromoDemandChangeIncident(Context, model, true);

                //если промо инаут, необходимо убрать записи в IncrementalPromo при отмене промо
                if (model.InOut.HasValue && model.InOut.Value)
                {
                    PromoHelper.DisableIncrementalPromo(Context, model);
                }

                return StatusCode(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<PromoGridView> options)
        {
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            var url = HttpContext.Current.Request.Url.AbsoluteUri;
            var results = options.ApplyTo(GetConstraintedQuery()).Cast<PromoGridView>()
                                                .Where(x => !x.Disabled)
                                                .Select(p => p.Id)
                                                .ToList();
            IQueryable fullResults = GetFullConstraintedQuery()
                                                .Where(x => !x.Disabled)
                                                .Where(x => results.Contains(x.Id));

            using (DatabaseContext context = new DatabaseContext())
            {
                HandlerData data = new HandlerData();
                string handlerName = "ExportHandler";

                HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TModel", typeof(PromoGridView), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(PromoHelper), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(PromoHelper.GetViewExportSettings), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("SqlString", fullResults.ToTraceQuery(), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("URL", url, data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = $"Export {nameof(Promo)} dictionary",
                    Name = "Module.Host.TPM.Handlers." + handlerName,
                    ExecutionPeriod = null,
                    CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                    LastExecutionDate = null,
                    NextExecutionDate = null,
                    ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                    UserId = userId,
                    RoleId = roleId
                };
                handler.SetParameterData(data);
                context.LoopHandlers.Add(handler);
                context.SaveChanges();
            }

            return Content(HttpStatusCode.OK, "success");
        }
    }
}
