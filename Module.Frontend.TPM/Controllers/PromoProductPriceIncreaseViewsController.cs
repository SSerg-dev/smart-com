using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.DTO;
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

namespace Module.Frontend.TPM.Controllers
{
    public class PromoProductPriceIncreaseViewsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public PromoProductPriceIncreaseViewsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }
        protected IQueryable<PromoProductPriceIncreasesView> GetConstraintedQuery(Guid? promoId)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();


            IQueryable<PromoProductPriceIncreasesView> query = Context.Set<PromoProductPriceIncreasesView>();
            if (promoId != null)
            {
                IQueryable<Guid> promoProducts = Context.Set<PromoProductPriceIncrease>().Where(x => x.PromoPriceIncreaseId == promoId).Select(y => y.Id);
                query = query.Where(e => promoProducts.Contains(e.Id));
            }

            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<PromoProductPriceIncreasesView> GetPromoProductPriceIncreaseViews([FromODataUri] Guid? promoId, string tempEditUpliftId)
        {
            var query = GetConstraintedQuery(promoId);
            if (tempEditUpliftId != null)
            {
                var tempQuery = Context.Set<PromoProductsCorrection>().Where(x => x.TempId == tempEditUpliftId && x.Disabled != true);
                var ZrepList = tempQuery.Select(x => x.PromoProduct.ZREP);
                foreach (var promoProduct in query)
                {
                    if (ZrepList.Contains(promoProduct.ZREP))
                    {
                        promoProduct.IsCorrection = true;
                        promoProduct.PlanProductUpliftPercent = tempQuery.First(x => x.PromoProduct.ZREP == promoProduct.ZREP).PlanProductUpliftPercentCorrected;
                    }
                }
            };
            return query;
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<PromoProductPriceIncreasesView> GetFilteredData(ODataQueryOptions<PromoProductPriceIncreasesView> options)
        {
            string bodyText = Helper.GetRequestBody(HttpContext.Current.Request);
            var promoId = Helper.GetValueIfExists<Guid?>(bodyText, "promoId");
            var tempEditUpliftId = Helper.GetValueIfExists<string>(bodyText, "tempEditUpliftId");
            var query = GetConstraintedQuery(promoId);

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };
            var optionsPost = new ODataQueryOptionsPost<PromoProductPriceIncreasesView>(options.Context, Request, HttpContext.Current.Request);

            if (tempEditUpliftId != null)
            {
                var tempQuery = Context.Set<PromoProductsCorrection>().Where(x => x.TempId == tempEditUpliftId && x.Disabled != true);
                var ZrepList = tempQuery.Select(x => x.PromoProduct.ZREP);
                foreach (var promoProduct in query)
                {
                    if (ZrepList.Contains(promoProduct.ZREP))
                    {
                        promoProduct.IsCorrection = true;
                        promoProduct.PlanProductUpliftPercent = tempQuery.First(x => x.PromoProduct.ZREP == promoProduct.ZREP).PlanProductUpliftPercentCorrected;
                    }
                }
            };

            return optionsPost.ApplyTo(query, querySettings) as IQueryable<PromoProductPriceIncreasesView>;
        }

        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<PromoProductPriceIncreasesView> options, [FromODataUri] Guid? promoId = null)
        {
            IQueryable results = options.ApplyTo(GetConstraintedQuery(promoId));
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);
            int? promoNumber = Context.Set<Promo>().FirstOrDefault(p => p.Id == promoId)?.Number;
            string customFileName = promoNumber.HasValue && promoNumber.Value != 0 ? $"№{promoNumber}_PromoProduct" : string.Empty;
            Guid handlerId = Guid.NewGuid();
            using (DatabaseContext context = new DatabaseContext())
            {
                HandlerData data = new HandlerData();
                string handlerName = "ExportHandler";

                HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TModel", typeof(PromoProductPriceIncreasesView), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(PromoProductPriceIncreaseViewsController), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(PromoProductPriceIncreaseViewsController.GetExportSettings), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("CustomFileName", customFileName, data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = handlerId,
                    ConfigurationName = "PROCESSING",
                    Description = string.IsNullOrEmpty(customFileName) ? $"Export {nameof(PromoProductPriceIncrease)} dictionary" : $"Export {customFileName.Replace('_', ' ')} dictionary",
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

            return Content(HttpStatusCode.OK, handlerId);
        }
        public static IEnumerable<Column> GetExportSettings()
        {
            int orderNumber = 1;
            IEnumerable<Column> columns = new List<Column>() {
                new Column { Order = orderNumber++, Field = "ZREP", Header = "ZREP", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanProductUpliftPercent", Header = "Plan Product Uplift, %", Quoting = false,  Format = "0.00"},
                new Column { Order = orderNumber++, Field = "ProductEN", Header = "ProductEN", Quoting = false },
                new Column { Order = orderNumber++, Field = "PlanProductBaselineLSV", Header = "Plan Product Baseline, LSV", Quoting = false,  Format = "0.00"},
                new Column { Order = orderNumber++, Field = "PlanProductIncrementalLSV", Header = "Plan Product Incremental, LSV", Quoting = false,  Format = "0.00"},
                new Column { Order = orderNumber++, Field = "PlanProductLSV", Header = "Plan Product LSV, LSV", Quoting = false,  Format = "0.00"},
                new Column { Order = orderNumber++, Field = "PlanProductBaselineCaseQty", Header = "Plan Product Baseline Case, Qty", Quoting = false,  Format = "0.00"},
                new Column { Order = orderNumber++, Field = "PlanProductIncrementalCaseQty", Header = "Plan Product Incremental Case, Qty", Quoting = false,  Format = "0.00"},
                new Column { Order = orderNumber++, Field = "PlanProductCaseQty", Header = "Plan Product Case, Qty", Quoting = false,  Format = "0.00"},
                new Column { Order = orderNumber++, Field = "IsCorrection", Header = "Is Correction", Quoting = false,},
                new Column { Order = orderNumber++, Field = "AverageMarker", Header = "Average Marker", Quoting = false, }
            };
            return columns;
        }
    }
}
