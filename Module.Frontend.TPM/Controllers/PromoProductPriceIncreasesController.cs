using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers
{
    public class PromoProductPriceIncreasesController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public PromoProductPriceIncreasesController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }
        protected IQueryable<PromoProductPriceIncrease> GetConstraintedQuery(ODataQueryOptions<PromoProductPriceIncrease> options)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();

            IQueryable<PromoProductPriceIncrease> query = null;
            query = Context.Set<PromoProductPriceIncrease>().Where(e => !e.Disabled).FixOdataExpand(options);
            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<PromoProductPriceIncrease> GetPromoProductPriceIncreases(ODataQueryOptions<PromoProductPriceIncrease> options)
        {
            return GetConstraintedQuery(options);
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<PromoProductPriceIncrease> GetFilteredData(ODataQueryOptions<PromoProductPriceIncrease> options)
        {
            return GetConstraintedQuery(options);
        }

        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<PromoProductPriceIncrease> options, string additionalColumn = null, Guid? promoId = null, bool updateActualsMode = false)
        {
            // Во вкладке Promo -> Activity можно смотреть детализацию раличных параметров
            // Это один грид с разными столбцами, additionalColumn - набор столбцов
            IQueryable results = GetConstraintedQuery(options).Where(x => !x.Disabled && (!promoId.HasValue || x.PromoPriceIncreaseId == promoId.Value));
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
                HandlerDataHelper.SaveIncomingArgument("TModel", typeof(PromoProductPriceIncrease), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(PromoProductPriceIncreasesController), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(PromoProductPriceIncreasesController.GetExportSettings), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnMethodParams", additionalColumn, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("IsActuals", updateActualsMode, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("CustomFileName", customFileName, data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = handlerId,
                    ConfigurationName = "PROCESSING",
                    Description = string.IsNullOrEmpty(customFileName) ? $"Export {nameof(PromoProduct)} dictionary" : $"Export {customFileName.Replace('_', ' ')} dictionary",
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
        public static IEnumerable<Column> GetExportSettings(string additionalColumn = null)
        {
            IEnumerable<Column> columns = new List<Column>();

            // если импорт идет из детализации, приходит список столбцов и выбираем нужные
            if (additionalColumn != null && additionalColumn.Length > 0)
            {
                Dictionary<string, Column> columnMap = new Dictionary<string, Column>()
                {
                    { "zrep", new Column() { Order = 0, Field = "ZREP", Header = "ZREP", Quoting = false }},
                    { "producten", new Column() { Order = 1, Field = "ProductEN", Header = "Product EN", Quoting = false }},
                    { "planproductbaselinelsv", new Column() { Order = 2, Field = "PlanProductBaselineLSV", Header = "Plan Product Baseline LSV", Quoting = false }},
                    { "actualproductbaselinelsv", new Column() { Order = 2, Field = "ActualProductBaselineLSV", Header = "Actual Product Baseline LSV", Quoting = false }},
                    { "planproductincrementallsv", new Column() { Order = 2, Field = "PlanProductIncrementalLSV", Header = "Plan Product Incremental LSV", Quoting = false }},
                    { "actualproductincrementallsv", new Column() { Order = 2, Field = "ActualProductIncrementalLSV", Header = "Actual Product Incremental LSV", Quoting = false }},
                    { "planproductlsv", new Column() { Order = 2, Field = "PlanProductLSV", Header = "Plan Product LSV", Quoting = false }},
                    { "planproductpostpromoeffectlsv", new Column() { Order = 2, Field = "PlanProductPostPromoEffectLSV", Header = "Plan Product Post Promo Effect LSV", Quoting = false }},
                    { "actualproductlsv", new Column() { Order = 2, Field = "ActualProductLSV", Header = "Actual Product LSV", Quoting = false }},
                    { "actualproductpostpromoeffectlsv", new Column() { Order = 2, Field = "ActualProductPostPromoEffectLSV", Header = "Actual Product Post Promo Effect LSV", Quoting = false }},
                    { "actualproductlsvbycompensation", new Column() { Order = 2, Field = "ActualProductLSVByCompensation", Header = "Actual Product LSV By Compensation", Quoting = false }},
                    { "actualproductupliftpercent", new Column() { Order = 2, Field = "ActualProductUpliftPercent", Header = "Actual Product Uplift %", Quoting = false }},
                    { "suminvoiceproduct", new Column() { Order = 2, Field = "SumInvoiceProduct", Header = "Invoice Total Product", Quoting = false }},
                };

                additionalColumn = additionalColumn.ToLower();
                string[] columnsName = additionalColumn.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                foreach (string columnName in columnsName)
                {
                    if (columnMap.ContainsKey(columnName))
                    {
                        columns = new List<Column>()
                        {
                            new Column() { Order = 0, Field = "ZREP", Header = "ZREP", Quoting = false },
                            new Column() { Order = 1, Field = "ProductEN", Header = "Product EN", Quoting = false },
                            columnMap[columnName]
                        };
                    }
                }
            }
            else
            {
                columns = new List<Column>()
                {
                    new Column() { Order = 0, Field = "EAN_PC", Header = "EAN PC", Quoting = false },
                    new Column() { Order = 1, Field = "PluCode", Header = "PLU", Quoting = false },
                    new Column() { Order = 2, Field = "ActualProductPCQty", Header = "Actual Product PC Qty", Quoting = false },
                    new Column() { Order = 3, Field = "ActualProductPCLSV", Header = "Actual Product PC LSV", Quoting = false },
                };
            }

            return columns;
        }
    }
}
