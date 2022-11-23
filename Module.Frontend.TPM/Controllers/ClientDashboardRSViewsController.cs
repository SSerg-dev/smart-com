﻿using Core.Data;
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
using Module.Persist.TPM.Model.SimpleModel;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Newtonsoft.Json;
using Persist;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;

namespace Module.Frontend.TPM.Controllers
{
    public class ClientDashboardRSViewsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public ClientDashboardRSViewsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }
        protected IQueryable<ClientDashboardRSView> GetConstraintedQuery()
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IQueryable<ClientDashboardRSView> query = Context.Set<ClientDashboardRSView>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();

            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

            return query;
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<ClientDashboardRSView> GetClientDashboardRSView([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<ClientDashboardRSView> GetClientDashboardRSViews()
        {
            return GetConstraintedQuery();
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<ClientDashboardRSView> GetClientDashboardRSViews(bool needFullYEEF)
        {
            return GetConstraintedQuery();
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult GetAllYEEF(int? clientTreeId, string year)
        {
            double YEE = 0;
            double YTD = 0;
            string GHierarchyCode = null;
            string DemandCode = "";
            int? clientTreeIdforWhile = clientTreeId;
            var clientTrees = Context.Set<ClientTree>().Where(x => x.EndDate == null);
            ClientTree clientTree = clientTrees.Where(x => x.ObjectId == clientTreeIdforWhile).FirstOrDefault();
            if (clientTree != null)
            {
                int? clientTreeKeyId = null;
                clientTreeKeyId = clientTree.Id;

                //Подбор GHierarchyCode
                do
                {
                    clientTree = clientTrees.Where(x => x.ObjectId == clientTreeIdforWhile).FirstOrDefault();
                    if (clientTree != null)
                    {
                        GHierarchyCode = clientTree.GHierarchyCode;
                        clientTreeIdforWhile = clientTree.parentId;
                    }
                } while (String.IsNullOrWhiteSpace(GHierarchyCode) && clientTreeIdforWhile != null && clientTreeIdforWhile != 5000000);

                clientTreeIdforWhile = clientTreeId;
                //Подбор DemandCode
                do
                {
                    clientTree = clientTrees.Where(x => x.ObjectId == clientTreeIdforWhile).FirstOrDefault();
                    if (clientTree != null)
                    {
                        clientTreeIdforWhile = clientTree.parentId;
                        DemandCode = clientTree.DemandCode;
                    }
                } while (String.IsNullOrWhiteSpace(DemandCode) && clientTreeIdforWhile != null && clientTreeIdforWhile != 5000000);

                if (!String.IsNullOrWhiteSpace(GHierarchyCode) && year != null && clientTreeKeyId != null)
                {
                    var shares = Context.Set<ClientTreeBrandTech>().Where(x => x.ClientTreeId == clientTreeKeyId && x.ParentClientTreeDemandCode == DemandCode && !x.Disabled);
                    var brandTechs = Context.Set<BrandTech>().Where(x => !x.Disabled);
                    ClientTreeBrandTech share;

                    GHierarchyCode = GHierarchyCode.TrimStart('0');

                    var YEEFscript = String.Format(
                        "SELECT * FROM [DefaultSchemaSetting].[YEAR_END_ESTIMATE_FDM] WHERE [G_HIERARCHY_ID] = '{0}' AND [YEAR] = '{1}'", GHierarchyCode, year);
                    var YEEFlist = Context.SqlQuery<YEAR_END_ESTIMATE_FDM>(YEEFscript).ToList();

                    foreach (var YEEF in YEEFlist)
                    {
                        var brandTech = brandTechs.Where(y => y.BrandsegTechsub_code == YEEF.BRAND_SEG_TECH_CODE && !y.Disabled).Select(y => y.Id).FirstOrDefault();
                        share = shares.Where(x => brandTech == x.BrandTechId).FirstOrDefault();
                        if (share != null)
                        {
                            YEEF.YTD_LSV = YEEF.YTD_LSV * share.Share / 100;
                            YEEF.YEE_LSV = YEEF.YEE_LSV * share.Share / 100;
                        }
                        else
                        {
                            YEEF.YTD_LSV = 0;
                            YEEF.YEE_LSV = 0;
                        }
                    }

                    YTD = YEEFlist.Sum(x => x.YTD_LSV);
                    YEE = YEEFlist.Sum(x => x.YEE_LSV);
                }
            }
            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { YTD, YEE }));
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<ClientDashboardRSView> GetFilteredData(ODataQueryOptions<ClientDashboardRSView> options)
        {
            var query = GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<ClientDashboardRSView>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<ClientDashboardRSView>;
        }

        [ClaimsAuthorize]
        [AcceptVerbs("POST")]
        public IHttpActionResult Update(
            [FromODataUri] int ObjectId, string ClientHierarchy, string BrandsegTechsubName, Guid? BrandTechId, string Year,
            double? ShopperTiPlanPercent, double? MarketingTiPlanPercent, double? ProductionPlan, double? BrandingPlan,
            double? BTLPlan, double? ROIPlanPercent, double? IncrementalNSVPlan, double? PromoNSVPlan, double? PlanLSV,
            double? PromoTiCostPlanPercent, double? NonPromoTiCostPlanPercent)
        {
            try
            {
                List<Tuple<IEntity<Guid>, IEntity<Guid>>> toHis = new List<Tuple<IEntity<Guid>, IEntity<Guid>>>();
                var model = Context.Set<ClientDashboard>().Where(x => x.ClientTreeId == ObjectId && x.BrandsegTechsubName == BrandsegTechsubName && x.Year == Year).FirstOrDefault();
                var oldHisModel = Context.Set<ClientDashboardRSView>().Where(x => x.ObjectId == ObjectId && x.BrandsegTechsubName == BrandsegTechsubName && x.Year.ToString() == Year).FirstOrDefault();
                OperationType operation;
                ClientDashboardRSView hisModel = null;
                if (oldHisModel != null)
                {
                    if (model == null)
                    {
                        model = new ClientDashboard()
                        {
                            Id = Guid.NewGuid(),
                            ClientTreeId = ObjectId,
                            ClientHierarchy = ClientHierarchy,
                            BrandsegTechsubName = BrandsegTechsubName,
                            BrandTechId = BrandTechId,
                            Year = Year,
                            ShopperTiPlanPercent = ShopperTiPlanPercent,
                            MarketingTiPlanPercent = MarketingTiPlanPercent,
                            ProductionPlan = ProductionPlan,
                            BrandingPlan = BrandingPlan,
                            BTLPlan = BTLPlan,
                            ROIPlanPercent = ROIPlanPercent,
                            IncrementalNSVPlan = IncrementalNSVPlan,
                            PromoNSVPlan = PromoNSVPlan,
                            PlanLSV = PlanLSV,
                            PromoTiCostPlanPercent = PromoTiCostPlanPercent,
                            NonPromoTiCostPlanPercent = NonPromoTiCostPlanPercent
                        };
                        Context.Set<ClientDashboard>().Add(model);
                        operation = OperationType.Created;
                        toHis.Add(new Tuple<IEntity<Guid>, IEntity<Guid>>(null, model));
                        Context.HistoryWriter.Write(toHis, Context.AuthManager.GetCurrentUser(), Context.AuthManager.GetCurrentRole(), OperationType.Created);
                        string insertScript = String.Format("INSERT INTO [DefaultSchemaSetting].ClientDashboard (ShopperTiPlanPercent, MarketingTiPlanPercent, ProductionPlan, BrandingPlan, BTLPlan, " +
                            "ROIPlanPercent ,IncrementalNSVPlan, PromoNSVPlan, ClientTreeId, BrandsegTechsubName, Year, [Id], ClientHierarchy, BrandTechId, PlanLSV, PromoTiCostPlanPercent, NonPromoTiCostPlanPercent)" +
                            " VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, '{9}', '{10}', '{11}', '{12}', '{13}', {14}, {15}, {16});",
                            model.ShopperTiPlanPercent, model.MarketingTiPlanPercent, model.ProductionPlan, model.BrandingPlan, model.BTLPlan,
                            model.ROIPlanPercent, model.IncrementalNSVPlan, model.PromoNSVPlan, model.ClientTreeId, model.BrandsegTechsubName,
                            model.Year, model.Id, model.ClientHierarchy, model.BrandTechId, model.PlanLSV, model.PromoTiCostPlanPercent, model.NonPromoTiCostPlanPercent);
                        Context.ExecuteSqlCommand(insertScript);
                        hisModel = Context.Set<ClientDashboardRSView>().Where(x => x.ObjectId == ObjectId && x.BrandsegTechsubName == BrandsegTechsubName && x.Year.ToString() == Year).FirstOrDefault();
                        oldHisModel = null;
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
                        model.PlanLSV = PlanLSV;
                        model.PromoTiCostPlanPercent = PromoTiCostPlanPercent;
                        model.NonPromoTiCostPlanPercent = NonPromoTiCostPlanPercent;

                        operation = OperationType.Updated;
                        string updateScript = String.Format("UPDATE [DefaultSchemaSetting].ClientDashboard SET ShopperTiPlanPercent = {0}, MarketingTiPlanPercent = {1}, ProductionPlan = {2}, BrandingPlan = {3}," +
                            "BTLPlan = {4}, ROIPlanPercent = {5}, IncrementalNSVPlan = {6}, PromoNSVPlan = {7}, PlanLSV = {8}, PromoTiCostPlanPercent = {9}, NonPromoTiCostPlanPercent = {10} WHERE Id = '{11}'",
                            model.ShopperTiPlanPercent, model.MarketingTiPlanPercent, model.ProductionPlan, model.BrandingPlan, model.BTLPlan,
                            model.ROIPlanPercent, model.IncrementalNSVPlan, model.PromoNSVPlan, model.PlanLSV, model.PromoTiCostPlanPercent, model.NonPromoTiCostPlanPercent, model.Id);
                        Context.ExecuteSqlCommand(updateScript);
                        hisModel = Context.Set<ClientDashboardRSView>().Where(x => x.ObjectId == ObjectId && x.BrandsegTechsubName == BrandsegTechsubName && x.Year.ToString() == Year).FirstOrDefault();
                    }
                    hisModel.Id = model.Id;
                    if (oldHisModel != null) oldHisModel.Id = model.Id;
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
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
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
                    RunGroup = typeof(ImportClientDashboard).Name,
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

        public static IEnumerable<Column> GetExportSettings()
        {
            var order = 0;
            var columns = new List<Column>()
            {
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.ObjectId), Header = "Client ID", Quoting = false },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.ClientHierarchy), Header = "Client hierarchy", Quoting = false },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.BrandsegTechsubName), Header = "Brand Seg Tech Sub", Quoting = false },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.Year), Header = "Year", Quoting = false },

                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.ShopperTiPlanPercent), Header = "Shopper TI Plan, %", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.ShopperTiPlan), Header = "Shopper TI Plan", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.ShopperTiYTD), Header = "Shopper TI YTD", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.ShopperTiYTDPercent), Header = "Shopper TI YTD, %", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.ShopperTiYEE), Header = "Shopper TI YEE", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.ShopperTiYEEPercent), Header = "Shopper TI YEE, %", Quoting = false, Format = "0.00" },

                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.MarketingTiPlanPercent), Header = "Marketing TI Plan, %", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.MarketingTiPlan), Header = "Marketing TI Plan", Quoting = false, Format = "0.00" },

                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.PromoTiCostPlanPercent), Header = "Promo Ti Cost Plan, %", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.PromoTiCostPlan), Header = "Promo Ti Cost Plan", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.PromoTiCostYTD), Header = "Promo Ti Cost YTD", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.PromoTiCostYTDPercent), Header = "Promo Ti Cost YTD, %", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.PromoTiCostYEE), Header = "Promo Ti Cost YEE", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.PromoTiCostYEEPercent), Header = "Promo Ti Cost YEE, %", Quoting = false, Format = "0.00" },

                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.NonPromoTiCostPlanPercent), Header = "Non Promo Ti Cost Plan, %", Quoting = false, Format = "0.00" },

                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.ProductionPlanPercent), Header = "Production Plan, %", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.ProductionPlan), Header = "Production Plan", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.ProductionYTD), Header = "Production YTD", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.ProductionYTDPercent), Header = "Production YTD, %", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.ProductionYEE), Header = "Production YEE", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.ProductionYEEPercent), Header = "Production YEE, %", Quoting = false, Format = "0.00" },

                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.BrandingPlanPercent), Header = "Branding Plan, %", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.BrandingPlan), Header = "Branding Plan", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.BrandingYTD), Header = "Branding YTD", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.BrandingYTDPercent), Header = "Branding YTD, %", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.BrandingYEE), Header = "Branding YEE", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.BrandingYEEPercent), Header = "Branding YEE, %", Quoting = false, Format = "0.00" },

                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.BTLPlanPercent), Header = "BTL Plan, %", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.BTLPlan), Header = "BTL Plan", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.BTLYTD), Header = "BTL YTD", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.BTLYTDPercent), Header = "BTL YTD, %", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.BTLYEE), Header = "BTL YEE", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.BTLYEEPercent), Header = "BTL YEE, %", Quoting = false, Format = "0.00" },

                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.ROIPlanPercent), Header = "ROI Plan, %", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.ROIYTDPercent), Header = "ROI YTD, %", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.ROIYEEPercent), Header = "ROI YEE, %", Quoting = false, Format = "0.00" },

                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.LSVPlan), Header = "LSV Plan", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.LSVYTD), Header = "LSV YTD", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.LSVYEE), Header = "LSV YEE", Quoting = false, Format = "0.00" },

                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.IncrementalNSVPlan), Header = "Incremental NSV Plan", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.IncrementalNSVYTD), Header = "Incremental NSV YTD", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.IncrementalNSVYEE), Header = "Incremental NSV YEE", Quoting = false, Format = "0.00" },

                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.PromoNSVPlan), Header = "Promo NSV Plan", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.PromoNSVYTD), Header = "Promo NSV YTD", Quoting = false, Format = "0.00" },
                new Column() { Order = order++, Field = nameof(ClientDashboardRSView.PromoNSVYEE), Header = "Promo NSV YEE", Quoting = false, Format = "0.00" },
            };

            return columns;
        }

        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<ClientDashboardRSView> options)
        {
            IQueryable results = options.ApplyTo(GetConstraintedQuery()).AsQueryable();

            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);
            using (DatabaseContext context = new DatabaseContext())
            {
                HandlerData data = new HandlerData();
                string handlerName = "ExportHandler";

                HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TModel", typeof(ClientDashboardRSView), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(ClientDashboardRSViewsController), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(ClientDashboardRSViewsController.GetExportSettings), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("SimpleModel", true, data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = $"Export {nameof(ClientDashboardRSView)} dictionary",
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

        private bool EntityExists(System.Guid key)
        {
            return Context.Set<ClientDashboard>().Count(e => e.Id == key) > 0;
        }
    }
}
