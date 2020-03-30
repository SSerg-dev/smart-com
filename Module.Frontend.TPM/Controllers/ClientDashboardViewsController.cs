﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Core.Data;
using Core.History;
using Core.Security;
using Core.Security.Models;
using Core.Settings;

using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions;
using Frontend.Core.Extensions.Export;

using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;

using Persist;
using Persist.Model;

using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;

namespace Module.Frontend.TPM.Controllers
{
    public class ClientDashboardViewsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public ClientDashboardViewsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<ClientDashboardView> GetConstraintedQuery()
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IQueryable<ClientDashboardView> query = Context.Set<ClientDashboardView>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints); 
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
           
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

            return query;
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<ClientDashboardView> GetClientDashboardView([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<ClientDashboardView> GetClientDashboardViews()
        {
            return GetConstraintedQuery();
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<ClientDashboardView> GetFilteredData(ODataQueryOptions<ClientDashboardView> options)
        {
            var query = GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<ClientDashboardView>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<ClientDashboardView>;
        }

        [ClaimsAuthorize]
        [AcceptVerbs("POST")]
        public IHttpActionResult Update(
            [FromODataUri] int ObjectId, string ClientHierarchy, string BrandTechName, Guid? BrandTechId, string Year,
            double? ShopperTiPlanPercent, double? MarketingTiPlanPercent, double? ProductionPlan, double? BrandingPlan,
            double? BTLPlan, double? ROIPlanPercent, double? IncrementalNSVPlan, double? PromoNSVPlan)
        {
            try
            {
                List<Tuple<IEntity<Guid>, IEntity<Guid>>> toHis = new List<Tuple<IEntity<Guid>, IEntity<Guid>>>();
                var model = Context.Set<ClientDashboard>().Where(x => x.ClientTreeId == ObjectId && x.BrandTechName == BrandTechName && x.Year == Year).FirstOrDefault();
                var hisModel = Context.Set<ClientDashboardView>().Where(x => x.ObjectId == ObjectId && x.BrandTechName == BrandTechName && x.Year.ToString() == Year).FirstOrDefault();
                OperationType operation;
                ClientDashboardView oldHisModel = null;
                if (hisModel != null)
                {
                    if (model == null)
                    {
                        model = new ClientDashboard()
                        {
                            Id = Guid.NewGuid(),
                            ClientTreeId = ObjectId,
                            ClientHierarchy = ClientHierarchy,
                            BrandTechName = BrandTechName,
                            BrandTechId = BrandTechId,
                            Year = Year,
                            ShopperTiPlanPercent = ShopperTiPlanPercent,
                            MarketingTiPlanPercent = MarketingTiPlanPercent,
                            ProductionPlan = ProductionPlan,
                            BrandingPlan = BrandingPlan,
                            BTLPlan = BTLPlan,
                            ROIPlanPercent = ROIPlanPercent,
                            IncrementalNSVPlan = IncrementalNSVPlan,
                            PromoNSVPlan = PromoNSVPlan
                        };
                        Context.Set<ClientDashboard>().Add(model);
                        operation = OperationType.Created;
                        toHis.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(null, model));
                        Context.HistoryWriter.Write(toHis, Context.AuthManager.GetCurrentUser(), Context.AuthManager.GetCurrentRole(), OperationType.Created);
                        string insertScript = String.Format("INSERT INTO ClientDashboard (ShopperTiPlanPercent, MarketingTiPlanPercent, ProductionPlan, BrandingPlan, BTLPlan, " +
                            "ROIPlanPercent ,IncrementalNSVPlan, PromoNSVPlan, ClientTreeId, BrandTechName, Year, [Id], ClientHierarchy, BrandTechId)" +
                            " VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, '{9}', '{10}', '{11}', '{12}', '{13}');",
                            model.ShopperTiPlanPercent, model.MarketingTiPlanPercent, model.ProductionPlan, model.BrandingPlan, model.BTLPlan,
                            model.ROIPlanPercent, model.IncrementalNSVPlan, model.PromoNSVPlan, model.ClientTreeId, model.BrandTechName,
                            model.Year, model.Id, model.ClientHierarchy, model.BrandTechId);
                        Context.Database.ExecuteSqlCommand(insertScript);
                    }
                    else
                    {
                        model.ShopperTiPlanPercent = ShopperTiPlanPercent;
                        model.MarketingTiPlanPercent = MarketingTiPlanPercent;
                        model.ProductionPlan = ProductionPlan;
                        model.BrandingPlan = BrandingPlan;
                        model.BTLPlan = BTLPlan;
                        model.ROIPlanPercent = ROIPlanPercent;
                        model.IncrementalNSVPlan = IncrementalNSVPlan;
                        model.PromoNSVPlan = PromoNSVPlan;

                        oldHisModel = hisModel.Clone();
                        operation = OperationType.Updated;
                        string updateScript = String.Format("UPDATE ClientDashboard SET ShopperTiPlanPercent = {0}, MarketingTiPlanPercent = {1}, ProductionPlan = {2}, BrandingPlan = {3}," +
                            "BTLPlan = {4}, ROIPlanPercent = {5}, IncrementalNSVPlan = {6}, PromoNSVPlan = {7}  WHERE Id = '{8}'",
                            model.ShopperTiPlanPercent, model.MarketingTiPlanPercent, model.ProductionPlan, model.BrandingPlan, model.BTLPlan,
                            model.ROIPlanPercent, model.IncrementalNSVPlan, model.PromoNSVPlan, model.Id);
                        Context.Database.ExecuteSqlCommand(updateScript);

                    }
                    hisModel.Id = model.Id;
                    hisModel.ShopperTiPlanPercent = model.ShopperTiPlanPercent;
                    hisModel.MarketingTiPlanPercent = model.MarketingTiPlanPercent;
                    hisModel.ProductionPlan = model.ProductionPlan;
                    hisModel.BrandingPlan = model.BrandingPlan;
                    hisModel.BTLPlan = model.BTLPlan;
                    hisModel.ROIPlanPercent = model.ROIPlanPercent;
                    hisModel.IncrementalNSVPlan = model.IncrementalNSVPlan;
                    hisModel.PromoNSVPlan = model.PromoNSVPlan;
                    toHis.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(oldHisModel, hisModel));
                    Context.HistoryWriter.Write(toHis, Context.AuthManager.GetCurrentUser(), Context.AuthManager.GetCurrentRole(), operation);

                    //Context.SaveChanges();
                    return Ok("Success");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e.InnerException);
            }
        }

        [ClaimsAuthorize]
        public IHttpActionResult DownloadTemplateXLSX()
        {
            try
            {
                IEnumerable<Column> columns = GetExportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
                string filename = string.Format("{0}Template.xlsx", nameof(ClientDashboard));
                if (!Directory.Exists(exportDir))
                {
                    Directory.CreateDirectory(exportDir);
                }
                string filePath = Path.Combine(exportDir, filename);
                exporter.Export(Enumerable.Empty<ClientDashboard>(), filePath);
                string file = Path.GetFileName(filePath);
                return Content(HttpStatusCode.OK, file);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }

        }

        [ClaimsAuthorize]
        public async Task<HttpResponseMessage> FullImportXLSX()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string importDir = Core.Settings.AppSettingsManager.GetSetting("IMPORT_DIRECTORY", "ImportFiles");
                string fileName = await FileUtility.UploadFile(Request, importDir);

                CreateImportTask(fileName, "FullXLSXUpdateImportClientDashboardHandler");

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

        private void CreateImportTask(string fileName, string importHandler)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            using (DatabaseContext context = new DatabaseContext())
            {
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
                HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportClientDashboard), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportClientDashboard).Name, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(ClientDashboard), data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Загрузка импорта из файла " + typeof(ImportClientDashboard).Name,
                    Name = "Module.Host.TPM.Handlers." + importHandler,
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
        }

        private IEnumerable<Column> GetExportSettings()
        {
            var order = 0;
            var columns = new List<Column>()
            {
                new Column() { Order = order++, Field = nameof(ClientDashboardView.ObjectId), Header = "Client ID", Quoting = false },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.ClientHierarchy), Header = "Client hierarchy", Quoting = false },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.BrandTechName), Header = "Brand Tech", Quoting = false },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.Year), Header = "Year", Quoting = false },

                new Column() { Order = order++, Field = nameof(ClientDashboardView.ShopperTiPlanPercent), Header = "Shopper TI Plan, %", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.ShopperTiPlan), Header = "Shopper TI Plan", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.ShopperTiYTD), Header = "Shopper TI YTD", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.ShopperTiYTDPercent), Header = "Shopper TI YTD, %", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.ShopperTiYEE), Header = "Shopper TI YEE", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.ShopperTiYEEPercent), Header = "Shopper TI YEE, %", Quoting = false, Format = "0.00" },

                new Column() { Order = order++, Field = nameof(ClientDashboardView.MarketingTiPlanPercent), Header = "Marketing TI Plan, %", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.MarketingTiPlan), Header = "Marketing TI Plan", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.MarketingTiYTD), Header = "Marketing TI YTD", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.MarketingTiYTDPercent), Header = "Marketing TI YTD, %", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.MarketingTiYEE), Header = "Marketing TI YEE", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.MarketingTiYEEPercent), Header = "Marketing TI YEE, %", Quoting = false, Format = "0.00" },

                new Column() { Order = order++, Field = nameof(ClientDashboardView.ProductionPlanPercent), Header = "Production Plan, %", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.ProductionPlan), Header = "Production Plan", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.ProductionYTD), Header = "Production YTD", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.ProductionYTDPercent), Header = "Production YTD, %", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.ProductionYEE), Header = "Production YEE", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.ProductionYEEPercent), Header = "Production YEE, %", Quoting = false, Format = "0.00" },

                new Column() { Order = order++, Field = nameof(ClientDashboardView.BrandingPlanPercent), Header = "Branding Plan, %", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.BrandingPlan), Header = "Branding Plan", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.BrandingYTD), Header = "Branding YTD", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.BrandingYTDPercent), Header = "Branding YTD, %", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.BrandingYEE), Header = "Branding YEE", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.BrandingYEEPercent), Header = "Branding YEE, %", Quoting = false, Format = "0.00" },

                new Column() { Order = order++, Field = nameof(ClientDashboardView.BTLPlanPercent), Header = "BTL Plan, %", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.BTLPlan), Header = "BTL Plan", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.BTLYTD), Header = "BTL YTD", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.BTLYTDPercent), Header = "BTL YTD, %", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.BTLYEE), Header = "BTL YEE", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.BTLYEEPercent), Header = "BTL YEE, %", Quoting = false, Format = "0.00" },

                new Column() { Order = order++, Field = nameof(ClientDashboardView.ROIPlanPercent), Header = "ROI Plan, %", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.ROIYTDPercent), Header = "ROI YTD, %", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.ROIYEEPercent), Header = "ROI YEE, %", Quoting = false, Format = "0.00" },

                new Column() { Order = order++, Field = nameof(ClientDashboardView.LSVPlan), Header = "LSV Plan", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.LSVYTD), Header = "LSV YTD", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.LSVYEE), Header = "LSV YEE", Quoting = false, Format = "0.00" },

                new Column() { Order = order++, Field = nameof(ClientDashboardView.IncrementalNSVPlan), Header = "Incremental NSV Plan", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.IncrementalNSVYTD), Header = "Incremental NSV YTD", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.IncrementalNSVYEE), Header = "Incremental NSV YEE", Quoting = false, Format = "0.00" },

                new Column() { Order = order++, Field = nameof(ClientDashboardView.PromoNSVPlan), Header = "Promo NSV Plan", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.PromoNSVYTD), Header = "Promo NSV YTD", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardView.PromoNSVYEE), Header = "Promo NSV YEE", Quoting = false, Format = "0.00" },
            };

            return columns;
        }

        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<ClientDashboardView> options)
        {
            try
            {
                IQueryable results = options.ApplyTo(GetConstraintedQuery());
                IEnumerable<Column> columns = GetExportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                UserInfo user = authorizationManager.GetCurrentUser();
                string username = user == null ? "" : user.Login;
                string filePath = exporter.GetExportFileName(nameof(ClientDashboardView), username);
                exporter.Export(results, filePath);
                string filename = System.IO.Path.GetFileName(filePath);
                return Content<string>(HttpStatusCode.OK, filename);
            }
            catch (Exception e)
            {
                return Content<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private bool EntityExists(System.Guid key)
        {
            return Context.Set<ClientDashboard>().Count(e => e.Id == key) > 0;
        }
    }
}
