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
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.Results;
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
        public IQueryable<PromoProductPriceIncreasesView> GetPromoProductPriceIncreaseViews([FromODataUri] Guid? promoId)
        {
            var query = GetConstraintedQuery(promoId);
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
        public IHttpActionResult Put(PromoProductCorrectionPriceIncreaseView model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Created(model);
            // если существует коррекция на данный PromoProduct, то не создаем новый объект
            //var item = Context.Set<PromoProductCorrectionPriceIncrease>()
            //    .FirstOrDefault(x => x.Id == model.Id && !x.Disabled);

            //if (item != null)
            //{
            //    if (item.PromoProduct.Promo.NeedRecountUplift == false && String.IsNullOrEmpty(item.TempId))
            //    {
            //        return InternalServerError(new Exception("Promo Locked Update"));
            //    }
            //    item.PlanProductUpliftPercentCorrected = model.PlanProductUpliftPercentCorrected;
            //    item.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
            //    item.UserId = user.Id;
            //    item.UserName = user.Login;

            //    try
            //    {
            //        var saveChangesResult = Context.SaveChanges();
            //        if (saveChangesResult > 0)
            //        {
            //            CreateChangesIncident(Context.Set<ChangesIncident>(), model);
            //            Context.SaveChanges();
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        return GetErorrRequest(e);
            //    }

            //    return Created(model);
            //}
            //else
            //{
            //    var proxy = Context.Set<PromoProductsCorrection>().Create<PromoProductsCorrection>();
            //    var configuration = new MapperConfiguration(cfg =>
            //        cfg.CreateMap<PromoProductsCorrection, PromoProductsCorrection>().ReverseMap());
            //    var mapper = configuration.CreateMapper();
            //    var result = mapper.Map(model, proxy);
            //    var promoProduct = Context.Set<PromoProduct>()
            //        .FirstOrDefault(x => x.Id == result.PromoProductId && !x.Disabled);

            //    if (promoProduct.Promo.NeedRecountUplift == false && String.IsNullOrEmpty(result.TempId))
            //    {
            //        return InternalServerError(new Exception("Promo Locked Update"));
            //    }
            //    result.CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
            //    result.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
            //    result.UserId = user.Id;
            //    result.UserName = user.Login;

            //    Context.Set<PromoProductsCorrection>().Add(result);

            //    try
            //    {
            //        var saveChangesResult = Context.SaveChanges();
            //        if (saveChangesResult > 0)
            //        {
            //            CreateChangesIncident(Context.Set<ChangesIncident>(), result);
            //            Context.SaveChanges();
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        return GetErorrRequest(e);
            //    }

            //    return Created(result);
            //}
        }
        [ClaimsAuthorize]
        public IHttpActionResult Post(PromoProductCorrectionPriceIncreaseView model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Created(model);
            // если существует коррекция на данный PromoProduct, то не создаем новый объект
            //var item = Context.Set<PromoProductCorrectionPriceIncrease>()
            //    .FirstOrDefault(x => x.Id == model.Id && !x.Disabled);

            //if (item != null)
            //{
            //    if (item.PromoProduct.Promo.NeedRecountUplift == false && String.IsNullOrEmpty(item.TempId))
            //    {
            //        return InternalServerError(new Exception("Promo Locked Update"));
            //    }
            //    item.PlanProductUpliftPercentCorrected = model.PlanProductUpliftPercentCorrected;
            //    item.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
            //    item.UserId = user.Id;
            //    item.UserName = user.Login;

            //    try
            //    {
            //        var saveChangesResult = Context.SaveChanges();
            //        if (saveChangesResult > 0)
            //        {
            //            CreateChangesIncident(Context.Set<ChangesIncident>(), model);
            //            Context.SaveChanges();
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        return GetErorrRequest(e);
            //    }

            //    return Created(model);
            //}
            //else
            //{
            //    var proxy = Context.Set<PromoProductsCorrection>().Create<PromoProductsCorrection>();
            //    var configuration = new MapperConfiguration(cfg =>
            //        cfg.CreateMap<PromoProductsCorrection, PromoProductsCorrection>().ReverseMap());
            //    var mapper = configuration.CreateMapper();
            //    var result = mapper.Map(model, proxy);
            //    var promoProduct = Context.Set<PromoProduct>()
            //        .FirstOrDefault(x => x.Id == result.PromoProductId && !x.Disabled);

            //    if (promoProduct.Promo.NeedRecountUplift == false && String.IsNullOrEmpty(result.TempId))
            //    {
            //        return InternalServerError(new Exception("Promo Locked Update"));
            //    }
            //    result.CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
            //    result.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
            //    result.UserId = user.Id;
            //    result.UserName = user.Login;

            //    Context.Set<PromoProductsCorrection>().Add(result);

            //    try
            //    {
            //        var saveChangesResult = Context.SaveChanges();
            //        if (saveChangesResult > 0)
            //        {
            //            CreateChangesIncident(Context.Set<ChangesIncident>(), result);
            //            Context.SaveChanges();
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        return GetErorrRequest(e);
            //    }

            //    return Created(result);
            //}
        }
        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<PromoProductCorrectionPriceIncreaseView> patch)
        {
            try
            {
                var model = Context.Set<PromoProductCorrectionPriceIncrease>()
                    .FirstOrDefault(x => x.Id == key);

                if (model == null)
                {
                    return NotFound();
                }
                //var promoStatus = model.PromoProduct.Promo.PromoStatus.SystemName;

                //patch.TryGetPropertyValue("TPMmode", out object mode);

                //if ((int)model.TPMmode != (int)mode)
                //{
                //    List<PromoProductsCorrection> promoProductsCorrections = Context.Set<PromoProductsCorrection>()
                //        .Include(g => g.PromoProduct.Promo.IncrementalPromoes)
                //        .Include(g => g.PromoProduct.Promo.BTLPromoes)
                //        .Include(g => g.PromoProduct.Promo.PromoSupportPromoes)
                //        .Include(g => g.PromoProduct.Promo.PromoProductTrees)
                //        .Where(x => x.PromoProduct.PromoId == model.PromoProduct.PromoId && !x.Disabled)
                //        .ToList();
                //    promoProductsCorrections = RSmodeHelper.EditToPromoProductsCorrectionRS(Context, promoProductsCorrections);
                //    model = promoProductsCorrections.FirstOrDefault(g => g.PromoProduct.Promo.Number == model.PromoProduct.Promo.Number);
                //}

                //patch.Patch(model);
                //model.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                //model.UserId = user.Id;
                //model.UserName = user.Login;

                //if (model.TempId == "")
                //{
                //    model.TempId = null;
                //}


                //ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                //string promoStatuses = settingsManager.GetSetting<string>("PROMO_PRODUCT_CORRECTION_PROMO_STATUS_LIST", "Draft,Deleted,Cancelled,Started,Finished,Closed");
                //string[] status = promoStatuses.Split(',');
                //if (status.Any(x => x == promoStatus) && !role.Equals("SupportAdministrator"))
                //    return InternalServerError(new Exception("Cannot be update correction where status promo = " + promoStatus));
                //if (model.PromoProduct.Promo.NeedRecountUplift == false)
                //{
                //    return InternalServerError(new Exception("Promo Locked Update"));
                //}

                //var saveChangesResult = Context.SaveChanges();
                //if (saveChangesResult > 0)
                //{
                //    CreateChangesIncident(Context.Set<ChangesIncident>(), model);
                //    Context.SaveChanges();
                //}

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

        private bool EntityExists(System.Guid key)
        {
            return Context.Set<PromoProductCorrectionPriceIncrease>().Count(e => e.Id == key) > 0;
        }

        private ExceptionResult GetErorrRequest(Exception e)
        {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
            {
                return InternalServerError(new Exception("This PromoProductCorrectionPriceIncrease has already existed"));
            }
            else
            {
                return InternalServerError(e);
            }
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
            string customFileName = promoNumber.HasValue && promoNumber.Value != 0 ? $"№{promoNumber}_PromoProductPriceIncrease" : string.Empty;
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
                string filePath = exporter.GetExportFileName("PromoProductPriceIncreasesUplift_PromoId_Template" + promoNumber, username);
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
        public async Task<HttpResponseMessage> FullImportXLSX([FromODataUri] Guid promoId)
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string importDir = Core.Settings.AppSettingsManager.GetSetting("IMPORT_DIRECTORY", "ImportFiles");
                string fileName = await FileUtility.UploadFile(Request, importDir);

                CreateImportTask(fileName, "FullXLSXUpdateImportPromoProductsPriceIncreaseUpliftHandler", promoId);

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

        private void CreateImportTask(string fileName, string importHandler, Guid promoId)
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
                HandlerDataHelper.SaveIncomingArgument("PromoId", promoId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportPromoProductsUplift), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportPromoProductsUplift).Name, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(PromoProductPriceIncreasesView), data, visible: false, throwIfNotExists: false);
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
                context.LoopHandlers.Add(handler);
                context.SaveChanges();
            }
        }
    }
}
