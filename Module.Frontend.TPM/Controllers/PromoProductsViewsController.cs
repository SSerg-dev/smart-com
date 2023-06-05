using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.Interfaces;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers
{
    public class PromoProductsViewsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public PromoProductsViewsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<PromoProductsView> GetConstraintedQuery(Guid? promoId)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();


            IQueryable<PromoProductsView> query = Context.Set<PromoProductsView>();
            if (promoId != null)
            {
                IQueryable<Guid> promoProducts = Context.Set<PromoProduct>().Where(x => x.PromoId == promoId).Select(y => y.Id);
                query = query.Where(e => promoProducts.Contains(e.Id));
            }

            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<PromoProductsView> GetPromoProductsView([FromODataUri] Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery(null));
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<PromoProductsView> GetPromoProductsViews([FromODataUri] Guid? promoId, string tempEditUpliftId)
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
        public IQueryable<PromoProductsView> GetFilteredData(ODataQueryOptions<PromoProductsView> options)
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
            var optionsPost = new ODataQueryOptionsPost<PromoProductsView>(options.Context, Request, HttpContext.Current.Request);

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

            return optionsPost.ApplyTo(query, querySettings) as IQueryable<PromoProductsView>;
        }

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> ExportXLSX(ODataQueryOptions<PromoProductsView> options, [FromODataUri] Guid? promoId = null)
        {
            IQueryable results = options.ApplyTo(GetConstraintedQuery(promoId));
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);
            int? promoNumber = Context.Set<Promo>().FirstOrDefault(p => p.Id == promoId)?.Number;
            string customFileName = promoNumber.HasValue && promoNumber.Value != 0 ? $"№{promoNumber}_PromoProduct" : string.Empty;
            Guid handlerId = Guid.NewGuid();

            HandlerData data = new HandlerData();
            string handlerName = "ExportHandler";

            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TModel", typeof(PromoProductsView), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(PromoProductsViewsController), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(PromoProductsViewsController.GetExportSettings), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);
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
            Context.LoopHandlers.Add(handler);
            await Context.SaveChangesAsync();

            return Content(HttpStatusCode.OK, handlerId);
        }

        [ClaimsAuthorize]
        public IHttpActionResult DownloadTemplateXLSX([FromODataUri] Guid? promoId = null)
        {

            try
            {
                IQueryable results = GetConstraintedQuery(promoId);
                IEnumerable<Column> columns = GetImportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                UserInfo user = authorizationManager.GetCurrentUser();
                string username = user == null ? "" : user.Login;
                string promoNumber = Context.Set<Promo>().FirstOrDefault(x => x.Id == promoId).Number.ToString();
                string filePath = exporter.GetExportFileName("PromoProductsUplift_PromoId_Template" + promoNumber, username);
                exporter.Export(results, filePath);
                string filename = System.IO.Path.GetFileName(filePath);
                return Content<string>(HttpStatusCode.OK, filename);
            }
            catch (Exception e)
            {
                return Content<string>(HttpStatusCode.InternalServerError, e.Message);
            }

        }

        [ClaimsAuthorize]
        public HttpResponseMessage FullImportXLSX()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [ClaimsAuthorize]
        public async Task<HttpResponseMessage> FullImportXLSX([FromODataUri] Guid promoId, string tempEditUpliftId, TPMmode tPMmode)
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string importDir = Core.Settings.AppSettingsManager.GetSetting("IMPORT_DIRECTORY", "ImportFiles");
                string fileName = await FileUtility.UploadFile(Request, importDir);

                await CreateImportTask(fileName, "FullXLSXUpdateImportPromoProductsUpliftHandler", promoId, tempEditUpliftId, tPMmode);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StringContent("success = true");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

                return result;
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
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

        private IEnumerable<Column> GetImportSettings()
        {
            int orderNumber = 1;
            IEnumerable<Column> columns = new List<Column>()
            {
                new Column { Order = orderNumber++, Field = "ZREP", Header = "ZREP", Quoting = false,  Format = "0.00"  },
                new Column { Order = orderNumber++, Field = "PlanProductUpliftPercent", Header = "Plan Product Uplift, %", Quoting = false,  Format = "0.00"},
            };
            return columns;
        }

        private async Task CreateImportTask(string fileName, string importHandler, Guid promoId, string tempEditUpliftId, TPMmode tPMmode)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            ImportResultFilesModel resiltfile = new ImportResultFilesModel();
            ImportResultModel resultmodel = new ImportResultModel();

            HandlerData data = new HandlerData();
            FileModel file = new FileModel()
            {
                LogicType = "Import",
                Name = System.IO.Path.GetFileName(fileName),
                DisplayName = System.IO.Path.GetFileName(fileName)
            };

            HandlerDataHelper.SaveIncomingArgument("File", file, data, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("PromoId", promoId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TPMmode", tPMmode, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TempId", tempEditUpliftId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportPromoProductsUplift), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportPromoProductsUplift).Name, data, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(PromoProductsView), data, visible: false, throwIfNotExists: false);
            //HandlerDataHelper.SaveIncomingArgument("UniqueFields", new List<String>() { "Name" }, data);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = "Загрузка импорта из файла " + typeof(ImportPromoProductsUplift).Name,
                Name = "Module.Host.TPM.Handlers." + importHandler,
                ExecutionPeriod = null,
                RunGroup = typeof(ImportPromoProductsUplift).Name,
                CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                LastExecutionDate = null,
                NextExecutionDate = null,
                ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                UserId = userId,
                RoleId = roleId
            };
            handler.SetParameterData(data);
            Context.LoopHandlers.Add(handler);
            await Context.SaveChangesAsync();
        }



        private bool EntityExists(Guid key)
        {
            return Context.Set<PromoProductsView>().Count(e => e.Id == key) > 0;
        }
    }
}
