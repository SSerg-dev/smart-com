using AutoMapper;
using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions.Export;
using Module.Persist.TPM.Model.TPM;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Thinktecture.IdentityModel.Authorization.WebApi;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Net.Http;
using Frontend.Core.Extensions;
using Persist;
using Looper.Parameters;
using Looper.Core;
using Module.Persist.TPM.Model.Import;
using System.Web.Http.Results;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Core.Settings;
using Module.Persist.TPM.Utils;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Module.Frontend.TPM.Util;

namespace Module.Frontend.TPM.Controllers
{
    public class PromoProductsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public PromoProductsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<PromoProduct> GetConstraintedQuery(ODataQueryOptions<PromoProduct> options, bool updateActualsMode = false, Guid? promoIdInUpdateActualsMode = null, bool isActualsExport = false)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();

            IQueryable<PromoProduct> query = null;
            if (updateActualsMode && promoIdInUpdateActualsMode != null && promoIdInUpdateActualsMode != Guid.Empty)
            {
                if (isActualsExport)
                {
                    var result = Context.Set<PromoProduct>().Where(e => e.PromoId == promoIdInUpdateActualsMode && !e.Disabled);
                    return result;
                }
                else
                {
                    var sumGroup = Context.Set<PromoProduct>().Where(e => e.PromoId == promoIdInUpdateActualsMode && !e.Disabled)
                                                              .GroupBy(x => x.EAN_PC)
                                                              .Select(s => new
                                                              {
                                                                  sumActualProductPCQty = s.Sum(x => x.ActualProductPCQty),
                                                                  sumActualProductPCLSV = s.Sum(x => x.ActualProductPCLSV),
                                                                  promoProduct = s.Select(x => x)
                                                              })
                                                              .ToList();

                    List<PromoProduct> promoProductList = new List<PromoProduct>();
                    foreach (var item in sumGroup)
                    {
                        PromoProduct pp = item.promoProduct.ToList()[0];
                        pp.ActualProductPCQty = item.sumActualProductPCQty;
                        pp.ActualProductPCLSV = item.sumActualProductPCLSV;

                        promoProductList.Add(pp);
                    }

                    //List<PromoProduct> promoProducts = Context.Set<PromoProduct>().Where(e => e.PromoId == promoIdInUpdateActualsMode && !e.Disabled).DistinctBy(x => x.EAN_PC).ToList();
                    return promoProductList.AsQueryable();
                }
            }
            else
            {
                query = Context.Set<PromoProduct>().Where(e => !e.Disabled).FixOdataExpand(options);
                return query;
            }
        }

        //[ClaimsAuthorize]
        //[EnableQuery(MaxNodeCount = int.MaxValue)]
        //public SingleResult<PromoProduct> GetPromoProduct([FromODataUri] System.Guid key)
        //{
        //    return SingleResult.Create(GetConstraintedQuery());
        //}

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<PromoProduct> GetPromoProducts(ODataQueryOptions<PromoProduct> options)
        {
            bool updateActualsMode = Convert.ToBoolean(HttpContext.Current.Request.QueryString["updateActualsMode"]);
            Guid.TryParse(HttpContext.Current.Request.QueryString["clientTreeId"], out Guid promoIdInUpdateActualsMode);
            return GetConstraintedQuery(options, updateActualsMode, promoIdInUpdateActualsMode);
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<PromoProduct> GetFilteredData(ODataQueryOptions<PromoProduct> options)
        {
            string bodyText = Helper.GetRequestBody(HttpContext.Current.Request);
            var updateActualsMode = Helper.GetValueIfExists<bool>(bodyText, "updateActualsMode");
            var promoIdInUpdateActualsMode = Helper.GetValueIfExists<Guid?>(bodyText, "promoIdInUpdateActualsMode");
            return GetConstraintedQuery(options, updateActualsMode, promoIdInUpdateActualsMode);
            //var query = GetConstraintedQuery(options, updateActualsMode, promoIdInUpdateActualsMode);

            //var querySettings = new ODataQuerySettings
            //{
            //    EnsureStableOrdering = false,
            //    HandleNullPropagation = HandleNullPropagationOption.False
            //};
            //var optionsPost = new ODataQueryOptionsPost<PromoProduct>(options.Context, Request, HttpContext.Current.Request);

            //return optionsPost.ApplyTo(query, querySettings) as IQueryable<PromoProduct>;
        }

        [ClaimsAuthorize]
        public IHttpActionResult Put([FromODataUri] System.Guid key, Delta<PromoProduct> patch)
        {
            var model = Context.Set<PromoProduct>().Find(key);
            if (model == null)
            {
                return NotFound();
            }
            
            patch.Put(model);

            try
            {
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(model);
        }

        [ClaimsAuthorize]
        public IHttpActionResult Post(PromoProduct model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var proxy = Context.Set<PromoProduct>().Create<PromoProduct>();
            var result = (PromoProduct)Mapper.Map(model, proxy, typeof(PromoProduct), proxy.GetType(), opts => opts.CreateMissingTypeMaps = true);
            Context.Set<PromoProduct>().Add(result);

            try
            {
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                return GetErorrRequest(e);
            }

            return Created(model);
        }

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<PromoProduct> patch)
        {            
            try
            {
                var model = Context.Set<PromoProduct>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                patch.Patch(model);
                var newActualProductPCQtyValue = model.ActualProductPCQty;

                Promo promo = Context.Set<Promo>().Where(x => x.Id == model.PromoId && !x.Disabled).FirstOrDefault();
                if (promo != null)
                {
                    // сброс старых значений ActualProductPCQty
                    var productsToReset = Context.Set<PromoProduct>().Where(x => x.EAN_PC == model.EAN_PC && x.PromoId == model.PromoId && !x.Disabled);
                    foreach(var item in productsToReset)
                    {
                        item.ActualProductPCQty = null;
                    }

                    if (!promo.InOut.HasValue || !promo.InOut.Value)
                    {
                        //выбор продуктов с ненулевым BaseLine
                        var productsWithRealBaseline = Context.Set<PromoProduct>().Where(x => x.EAN_PC == model.EAN_PC && x.PromoId == model.PromoId
                                                       && x.PlanProductBaselineLSV != null && x.PlanProductBaselineLSV != 0 && !x.Disabled);

                        if (productsWithRealBaseline != null && productsWithRealBaseline.Count() > 0)
                        {
                            //распределение импортируемого количества пропорционально PlanProductBaselineLSV(или ActualProductBaselineLSV) не важно, т.к. пропорция будет одна и та же
                            var sumBaseline = productsWithRealBaseline.Sum(x => x.PlanProductBaselineLSV);
                            int? sumActualProductPCQty = 0;
                            foreach (var p in productsWithRealBaseline)
                            {
                                p.ActualProductUOM = "PC";
                                p.ActualProductPCQty = (int?)(newActualProductPCQtyValue / sumBaseline * p.PlanProductBaselineLSV);
                                sumActualProductPCQty += p.ActualProductPCQty;
                            }

                            var differenceActualProductPCQty = newActualProductPCQtyValue - sumActualProductPCQty;
                            if (differenceActualProductPCQty.HasValue && differenceActualProductPCQty != 0)
                            {
                                productsWithRealBaseline.FirstOrDefault().ActualProductPCQty += differenceActualProductPCQty;
                            }
                        }
                        else
                        {
                            //если не найдено продуктов с ненулевым basline, просто записываем импортируемое количество в первый попавшийся продукт, чтобы сохранилось
                            PromoProduct oldRecord = Context.Set<PromoProduct>().FirstOrDefault(x => x.EAN_PC == model.EAN_PC && x.PromoId == model.PromoId && !x.Disabled);
                            if (oldRecord != null)
                            {
                                oldRecord.ActualProductUOM = "PC";
                                oldRecord.ActualProductPCQty = newActualProductPCQtyValue;
                            }
                        }
                    }
                    else
                    {
                        //в случае inout промо выбираем продукты с ненулевой ценой PlanProductPCPrice, которая подбирается из справочника IncrementalPromo
                        var productsWithRealPCPrice = Context.Set<PromoProduct>().Where(x => x.EAN_PC == model.EAN_PC && x.PromoId == model.PromoId && x.PlanProductPCPrice != null
                                                                                        && x.PlanProductPCPrice != 0 && !x.Disabled).ToList();

                        if (productsWithRealPCPrice != null && productsWithRealPCPrice.Count() > 0)
                        {
                            //распределение импортируемого количества пропорционально PlanProductIncrementalLSV не важно, т.к. пропорция будет одна и та же
                            var sumBaseline = productsWithRealPCPrice.Sum(x => x.PlanProductIncrementalLSV);
                            int? sumActualProductPCQty = 0;
                            foreach (var p in productsWithRealPCPrice)
                            {
                                p.ActualProductUOM = "PC";
                                p.ActualProductPCQty = (int?)(newActualProductPCQtyValue / sumBaseline * p.PlanProductIncrementalLSV);
                                sumActualProductPCQty += p.ActualProductPCQty;
                            }

                            var differenceActualProductPCQty = newActualProductPCQtyValue - sumActualProductPCQty;
                            if (differenceActualProductPCQty.HasValue && differenceActualProductPCQty != 0)
                            {
                                productsWithRealPCPrice.FirstOrDefault().ActualProductPCQty += differenceActualProductPCQty;
                            }
                        }
                        else
                        {
                            //если не найдено продуктов с ненулевой PC Price, просто записываем импортируемое количество в первый попавшийся продукт, чтобы сохранилось
                            PromoProduct oldRecord = Context.Set<PromoProduct>().FirstOrDefault(x => x.EAN_PC == model.EAN_PC && x.PromoId == model.PromoId && !x.Disabled);
                            if (oldRecord != null)
                            {
                                oldRecord.ActualProductUOM = "PC";
                                oldRecord.ActualProductPCQty = newActualProductPCQtyValue;
                            }
                        }
                    }
                }
              
                Context.SaveChanges();

                // перерасчет фактических параметров
                CreateTaskCalculateActual(model.PromoId);

                return Updated(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception e)
            {
                return GetErorrRequest(e);
            }            
        }

        [ClaimsAuthorize]
        public IHttpActionResult Delete([FromODataUri] System.Guid key)
        {
            try
            {
                var model = Context.Set<PromoProduct>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                model.DeletedDate = System.DateTime.Now;
                model.Disabled = true;
                Context.SaveChanges();

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return InternalServerError(e.InnerException);
            }
        }
        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult GetPromoProductByPromoAndProduct([FromODataUri] Guid promoId, [FromODataUri] Guid productId)
        {
            try
            {
                PromoProduct promoProduct = Context.Set<PromoProduct>().FirstOrDefault(x => x.PromoId == promoId && x.ProductId == productId);
                 return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, models = promoProduct }));

            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false }));
            }
        }
        private bool EntityExists(System.Guid key)
        {
            return Context.Set<PromoProduct>().Count(e => e.Id == key) > 0;
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

        private IEnumerable<Column> GetImportTemplateSettingsTLC()
        {
            IEnumerable<Column> columns = new List<Column>()
            {
                new Column() { Order = 0, Field = "EAN_PC", Header = "EAN PC", Quoting = false },
                new Column() { Order = 1, Field = "ActualProductPCQty", Header = "Actual Product PC Qty", Quoting = false },
                new Column() { Order = 2, Field = "ActualProductSellInPrice", Header = "Price", Quoting = false },
            };

            return columns;
        }

        private IEnumerable<Column> GetImportTemplateSettings() {
            IEnumerable<Column> columns = new List<Column>()
            {
                new Column() { Order = 0, Field = "EAN_PC", Header = "EAN PC", Quoting = false },
                new Column() { Order = 1, Field = "ActualProductPCQty", Header = "Actual Product PC Qty", Quoting = false },
            };

            return columns;
        }

        private IEnumerable<Column> GetImportTemplatePluSettings()
        {
            IEnumerable<Column> columns = new List<Column>()
            {
                new Column() { Order = 0, Field = "Plu.PluCode", Header = "PLU", Quoting = false },
                new Column() { Order = 1, Field = "ActualProductPCQty", Header = "Actual Product PC Qty", Quoting = false },
            };

            return columns;
        }

        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<PromoProduct> options, string additionalColumn = null, Guid? promoId = null, bool updateActualsMode = false)
        {
            // Во вкладке Promo -> Activity можно смотреть детализацию раличных параметров
            // Это один грид с разными столбцами, additionalColumn - набор столбцов
            IQueryable results = GetConstraintedQuery(options, updateActualsMode, promoId, true).Where(x => !x.Disabled && (!promoId.HasValue || x.PromoId == promoId.Value));
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
                HandlerDataHelper.SaveIncomingArgument("TModel", typeof(PromoProduct), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(PromoProductsController), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(PromoProductsController.GetExportSettings), data, visible: false, throwIfNotExists: false);
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

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> FullImportXLSX(Guid promoId)
        {
            try
            {
                bool promoAvaible = CalculationTaskManager.BlockPromo(promoId, Guid.Empty);

                if (promoAvaible)
                {
                    if (!Request.Content.IsMimeMultipartContent())
                    {
                        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                    }

                    string importDir = Core.Settings.AppSettingsManager.GetSetting("IMPORT_DIRECTORY", "ImportFiles");
                    string fileName = await FileUtility.UploadFile(Request, importDir);

                    CreateImportTask(fileName, promoId);
                    return Json(new { success = true });
                }
                else
                {
                    return GetErorrRequest(new Exception("Promo was blocked for calculation"));
                }

            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> FullImportPluXLSX(Guid promoId)
		{
            try
            {
                bool promoAvaible = CalculationTaskManager.BlockPromo(promoId, Guid.Empty);

                if (promoAvaible)
                {
                    if (!Request.Content.IsMimeMultipartContent())
                    {
                        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                    }

                    string importDir = Core.Settings.AppSettingsManager.GetSetting("IMPORT_DIRECTORY", "ImportFiles");
                    string fileName = await FileUtility.UploadFile(Request, importDir);

                    CreateImportTask(fileName, promoId, true);
                    return Json(new { success = true });
                }
                else
                {
                    return GetErorrRequest(new Exception("Promo was blocked for calculation"));
                }

            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        private void CreateImportTask(string fileName, Guid promoId, bool isPlu = false)
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

                Promo promo = Context.Set<Promo>().FirstOrDefault(x => x.Id == promoId);
                string handlerName;
                Type importModel;
                if (promo.LoadFromTLC)
                {
                    handlerName = "FullXLSXImportPromoProductFromTLCHandler";
                    importModel = typeof(ImportPromoProductFromTLC);
                }
                else
                {
                    if(isPlu)
					{
                        handlerName = "FullXLSXImportPromoProductPluHandler";
                        importModel = typeof(ImportPromoProductPlu);
                    }
                    else
					{
                        handlerName = "FullXLSXImportPromoProductHandler";
                        importModel = typeof(ImportPromoProduct);
                    }
                }
                HandlerDataHelper.SaveIncomingArgument("File", file, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportType", importModel, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", importModel.Name, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(PromoProduct), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("PromoId", promoId, data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Import Actuals For Promo " + promo.Number?.ToString(),
                    Name = "Module.Host.TPM.Handlers." + handlerName,
                    ExecutionPeriod = null,
                    RunGroup = $"ImportActualsPromo{promo.Number?.ToString()}",
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

        [ClaimsAuthorize]
        public IHttpActionResult DownloadTemplateXLSX()
        {
            try
            {
                IEnumerable<Column> columns = GetImportTemplateSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
                string filename = string.Format("{0}Template.xlsx", "PromoProduct");
                if (!Directory.Exists(exportDir))
                {
                    Directory.CreateDirectory(exportDir);
                }
                string filePath = Path.Combine(exportDir, filename);
                exporter.Export(Enumerable.Empty<PromoProduct>(), filePath);
                string file = Path.GetFileName(filePath);
                return Content(HttpStatusCode.OK, file);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }

        }

        [ClaimsAuthorize]
        public IHttpActionResult DownloadTemplatePluXLSX()
        {
            try
            {
                IEnumerable<Column> columns = GetImportTemplatePluSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
                string filename = string.Format("{0}Template.xlsx", "PromoProductPlu");
                if (!Directory.Exists(exportDir))
                {
                    Directory.CreateDirectory(exportDir);
                }
                string filePath = Path.Combine(exportDir, filename);
                exporter.Export(Enumerable.Empty<PromoProduct>(), filePath);
                string file = Path.GetFileName(filePath);
                return Content(HttpStatusCode.OK, file);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }

        }

        [ClaimsAuthorize]
        public IHttpActionResult DownloadTemplateXLSXTLC()
        {
            try
            {
                IEnumerable<Column> columns = GetImportTemplateSettingsTLC();
                XLSXExporter exporter = new XLSXExporter(columns);
                string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
                string filename = string.Format("{0}TemplateTLC.xlsx", "PromoProduct");
                if (!Directory.Exists(exportDir))
                {
                    Directory.CreateDirectory(exportDir);
                }
                string filePath = Path.Combine(exportDir, filename);
                exporter.Export(Enumerable.Empty<PromoProduct>(), filePath);
                string file = Path.GetFileName(filePath);
                return Content(HttpStatusCode.OK, file);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        
        /// <summary>
        /// Создание отложенной задачи, выполняющей расчет фактических параметров продуктов и промо
        /// </summary>
        /// <param name="promoId">ID промо</param>
        private void CreateTaskCalculateActual(Guid promoId)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            HandlerData data = new HandlerData();
            HandlerDataHelper.SaveIncomingArgument("PromoId", promoId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("needRedistributeLSV", true, data, visible: false, throwIfNotExists: false);

            bool success = CalculationTaskManager.CreateCalculationTask(CalculationTaskManager.CalculationAction.Actual, data, Context, promoId);

            if (!success)
                throw new Exception("Promo was blocked for calculation");
        }

        private ExceptionResult GetErorrRequest(Exception e)
        {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
            {
                return InternalServerError(new Exception("This PromoProduct has already existed"));
            }
            else
            {
                return InternalServerError(e);
            }
        }


        [ClaimsAuthorize]
        public IHttpActionResult SupportAdminExportXLSX(ODataQueryOptions<PromoProduct> options, Guid? promoId = null)
        {
            var products = GetConstraintedQuery(options, false, promoId).Where(x => !x.Disabled && (!promoId.HasValue || x.PromoId == promoId.Value));
            var corrections = Context.Set<PromoProductsCorrection>().Where(x => products.Select(y => y.Id).Contains(x.PromoProductId) && x.TempId == null && x.Disabled != true);
            foreach (var singleCorrection in corrections)
            {
                products.Where(x => x.Id == singleCorrection.PromoProductId).FirstOrDefault().PlanProductUpliftPercent = singleCorrection.PlanProductUpliftPercentCorrected;
            };
            IQueryable results = options.ApplyTo(products);
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
                HandlerDataHelper.SaveIncomingArgument("TModel", typeof(PromoProduct), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(PromoProductsController), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(PromoProductsController.GetSupportAdminExportSettings), data, visible: false, throwIfNotExists: false);
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
                context.LoopHandlers.Add(handler);
                context.SaveChanges();
            }

            return Content(HttpStatusCode.OK, handlerId);
        }

        public static IEnumerable<Column> GetSupportAdminExportSettings()
        {
            int orderNum = 0;
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = orderNum++, Field = "ZREP", Header = "ZREP", Quoting = false },
                new Column() { Order = orderNum++, Field = "EAN_Case", Header = "EAN_Case", Quoting = false },
                new Column() { Order = orderNum++, Field = "EAN_PC", Header = "EAN_PC", Quoting = false },
                new Column() { Order = orderNum++, Field = "PlanProductCaseQty", Header = "PlanProductCaseQty", Quoting = false },
                new Column() { Order = orderNum++, Field = "PlanProductPCQty", Header = "PlanProductPCQty", Quoting = false },
                new Column() { Order = orderNum++, Field = "PlanProductCaseLSV", Header = "PlanProductCaseLSV", Quoting = false },
                new Column() { Order = orderNum++, Field = "PlanProductPCLSV", Header = "PlanProductPCLSV", Quoting = false },
                new Column() { Order = orderNum++, Field = "PlanProductBaselineLSV", Header = "PlanProductBaselineLSV", Quoting = false },
                new Column() { Order = orderNum++, Field = "ActualProductBaselineLSV", Header = "ActualProductBaselineLSV", Quoting = false },
                new Column() { Order = orderNum++, Field = "PlanProductIncrementalLSV", Header = "PlanProductIncrementalLSV", Quoting = false },
                new Column() { Order = orderNum++, Field = "PlanProductLSV", Header = "PlanProductLSV", Quoting = false },
                new Column() { Order = orderNum++, Field = "PlanProductBaselineCaseQty", Header = "PlanProductBaselineCaseQty", Quoting = false },
                new Column() { Order = orderNum++, Field = "ProductBaselinePrice", Header = "ProductBaselinePrice", Quoting = false },
                new Column() { Order = orderNum++, Field = "PlanProductPCPrice", Header = "PlanProductPCPrice", Quoting = false },
                new Column() { Order = orderNum++, Field = "ActualProductPCQty", Header = "ActualProductPCQty", Quoting = false },
                new Column() { Order = orderNum++, Field = "ActualProductCaseQty", Header = "ActualProductCaseQty", Quoting = false },
                new Column() { Order = orderNum++, Field = "ActualProductUOM", Header = "ActualProductUOM", Quoting = false },
                new Column() { Order = orderNum++, Field = "ActualProductSellInPrice", Header = "ActualProductSellInPrice", Quoting = false },
                new Column() { Order = orderNum++, Field = "ActualProductPCLSV", Header = "ActualProductPCLSV", Quoting = false },
                new Column() { Order = orderNum++, Field = "ActualProductUpliftPercent", Header = "ActualProductUpliftPercent", Quoting = false },
                new Column() { Order = orderNum++, Field = "ActualProductIncrementalPCQty", Header = "ActualProductIncrementalPCQty", Quoting = false },
                new Column() { Order = orderNum++, Field = "ActualProductIncrementalPCLSV", Header = "ActualProductIncrementalPCLSV", Quoting = false },
                new Column() { Order = orderNum++, Field = "ActualProductIncrementalLSV", Header = "ActualProductIncrementalLSV", Quoting = false },
                new Column() { Order = orderNum++, Field = "PlanProductPostPromoEffectLSVW1", Header = "PlanProductPostPromoEffectLSVW1", Quoting = false },
                new Column() { Order = orderNum++, Field = "PlanProductPostPromoEffectLSVW2", Header = "PlanProductPostPromoEffectLSVW2", Quoting = false },
                new Column() { Order = orderNum++, Field = "PlanProductPostPromoEffectLSV", Header = "PlanProductPostPromoEffectLSV", Quoting = false },
                new Column() { Order = orderNum++, Field = "ActualProductPostPromoEffectLSV", Header = "ActualProductPostPromoEffectLSV", Quoting = false },
                new Column() { Order = orderNum++, Field = "PlanProductIncrementalCaseQty", Header = "PlanProductIncrementalCaseQty", Quoting = false },
                new Column() { Order = orderNum++, Field = "PlanProductUpliftPercent", Header = "PlanProductUpliftPercent", Quoting = false },
                new Column() { Order = orderNum++, Field = "ActualProductLSV", Header = "ActualProductLSV", Quoting = false },
                new Column() { Order = orderNum++, Field = "ActualProductPostPromoEffectQtyW1", Header = "ActualProductPostPromoEffectQtyW1", Quoting = false },
                new Column() { Order = orderNum++, Field = "ActualProductPostPromoEffectQtyW2", Header = "ActualProductPostPromoEffectQtyW2", Quoting = false },
                new Column() { Order = orderNum++, Field = "ActualProductPostPromoEffectQty", Header = "ActualProductPostPromoEffectQty", Quoting = false },
                new Column() { Order = orderNum++, Field = "PlanProductPostPromoEffectQtyW1", Header = "PlanProductPostPromoEffectQtyW1", Quoting = false },
                new Column() { Order = orderNum++, Field = "PlanProductPostPromoEffectQtyW2", Header = "PlanProductPostPromoEffectQtyW2", Quoting = false },
                new Column() { Order = orderNum++, Field = "PlanProductPostPromoEffectQty", Header = "PlanProductPostPromoEffectQty", Quoting = false },
                new Column() { Order = orderNum++, Field = "ProductEN", Header = "ProductEN", Quoting = false },
                new Column() { Order = orderNum++, Field = "ActualProductLSVByCompensation", Header = "ActualProductLSVByCompensation", Quoting = false },
            };
            return columns;
        }
    }

}
