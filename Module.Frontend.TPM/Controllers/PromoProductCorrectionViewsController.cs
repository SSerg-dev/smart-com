using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Interfaces;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model;
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
    public class PromoProductCorrectionViewsController: EFContextController
    {
        private readonly UserInfo user;
        private readonly string role;
        private readonly Guid? roleId;

        public PromoProductCorrectionViewsController(IAuthorizationManager authorizationManager)
        {
            user = authorizationManager.GetCurrentUser();
            var roleInfo = authorizationManager.GetCurrentRole();
            role = roleInfo.SystemName;
            roleId = roleInfo.Id;

        }

        public PromoProductCorrectionViewsController(UserInfo User, string Role, Guid RoleId)
        {
            user = User;
            role = Role;
            roleId = RoleId;
        }

        public IQueryable<PromoProductCorrectionView> GetConstraintedQuery(TPMmode TPMmode, DatabaseContext localContext = null)
        {
            PerformanceLogger logger = new PerformanceLogger();
            logger.Start();
            localContext = localContext ?? Context;
            IList<Constraint> constraints = user.Id.HasValue ? localContext.Constraints
                    .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                    .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<PromoProductCorrectionView> query = localContext.Set<PromoProductCorrectionView>().AsNoTracking();
            IQueryable<ClientTreeHierarchyView> hierarchy = localContext.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, TPMmode, filters);
            logger.Stop();
            return query;
        }

        protected IQueryable<PromoProductsCorrection> GetFullConstraintedQuery(TPMmode tPMmode = TPMmode.Current)
        {
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<PromoProductsCorrection> query = Context.Set<PromoProductsCorrection>().AsNoTracking();
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, tPMmode, filters);
                        
            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<PromoProductCorrectionView> GetPromoProductCorrectionViews(TPMmode TPMmode = TPMmode.Current)
        {
            return GetConstraintedQuery(TPMmode);
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<PromoProductCorrectionView> GetFilteredData(ODataQueryOptions<PromoProductCorrectionView> options)
        {
            string bodyText = Helper.GetRequestBody(HttpContext.Current.Request);
            var query = GetConstraintedQuery(JsonHelper.GetValueIfExists<TPMmode>(bodyText, "TPMmode"));
            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };
            var optionsPost = new ODataQueryOptionsPost<PromoProductCorrectionView>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<PromoProductCorrectionView>;

        }

        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<PromoProductCorrectionView> options, [FromUri] TPMmode tPMmode)
        {
            string bodyText = Helper.GetRequestBody(HttpContext.Current.Request);
            //TPMmode tPMmode = JsonHelper.GetValueIfExists<TPMmode>(bodyText, "TPMmode");
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            var url = HttpContext.Current.Request.Url.AbsoluteUri;
            var results = options.ApplyTo(GetConstraintedQuery(tPMmode)).Cast<PromoProductCorrectionView>()
                                                .Where(x => !x.Disabled)
                                                .Select(p => p.Id);

            
            using (DatabaseContext context = new DatabaseContext())
            {
                HandlerData data = new HandlerData();
                string handlerName = "ExportHandler";

                HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TModel", typeof(PromoProductCorrectionView), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(PromoHelper), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(PromoHelper.GetPromoProductCorrectionExportSettings), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TPMmode", tPMmode, data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = $"Export {nameof(PromoProductsCorrection)} dictionary",
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

        [ClaimsAuthorize]
        public IHttpActionResult ExportCorrectionXLSX(ODataQueryOptions<PromoProductCorrectionView> options, [FromUri] TPMmode tPMmode)
        {
            List<string> stasuses = new List<string> { "DraftPublished", "OnApproval", "Approved", "Planned" };
            IQueryable<PromoProduct> results = Context.Set<PromoProduct>()
                .Where(g => !g.Disabled && stasuses.Contains(g.Promo.PromoStatus.SystemName) && !(bool)g.Promo.InOut && (bool)g.Promo.NeedRecountUplift)
                .OrderBy(g => g.Promo.Number).ThenBy(d => d.ZREP);
            //IQueryable results = options.ApplyTo(GetConstraintedQuery());
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            using (DatabaseContext context = new DatabaseContext())
            {
                HandlerData data = new HandlerData();
                string handlerName = "ExportHandler";

                HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TModel", typeof(PromoProduct), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(PromoHelper), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(PromoHelper.GetExportCorrectionSettings), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = $"Export {nameof(PromoProductsCorrection)} dictionary",
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
