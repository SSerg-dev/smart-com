﻿using Core.Dependency;
using Core.Security;
using Core.Security.Models;
using Core.Settings;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.FunctionalHelpers.RSPeriod;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.Interfaces;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
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
using System.Web.Http.Results;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;

namespace Module.Frontend.TPM.Controllers
{
    public class PromoProductCorrectionPriceIncreaseViewsController : EFContextController
    {
        private readonly UserInfo user;
        private readonly string role;
        private readonly Guid? roleId;

        public PromoProductCorrectionPriceIncreaseViewsController(IAuthorizationManager authorizationManager)
        {
            user = authorizationManager.GetCurrentUser();
            var roleInfo = authorizationManager.GetCurrentRole();
            role = roleInfo.SystemName;
            roleId = roleInfo.Id;

        }

        public PromoProductCorrectionPriceIncreaseViewsController(UserInfo User, string Role, Guid RoleId)
        {
            user = User;
            role = Role;
            roleId = RoleId;
        }

        public IQueryable<PromoProductCorrectionPriceIncreaseView> GetConstraintedQuery(DatabaseContext localContext = null)
        {
            PerformanceLogger logger = new PerformanceLogger();
            logger.Start();
            localContext = localContext ?? Context;
            IList<Constraint> constraints = user.Id.HasValue ? localContext.Constraints
                    .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                    .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<PromoProductCorrectionPriceIncreaseView> query = localContext.Set<PromoProductCorrectionPriceIncreaseView>().AsNoTracking();
            IQueryable<ClientTreeHierarchyView> hierarchy = localContext.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);
            logger.Stop();
            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<PromoProductCorrectionPriceIncreaseView> GetPromoProductCorrectionPriceIncreaseViews()
        {
            return GetConstraintedQuery();
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<PromoProductCorrectionPriceIncreaseView> GetFilteredData(ODataQueryOptions<PromoProductCorrectionPriceIncreaseView> options)
        {
            string bodyText = Helper.GetRequestBody(HttpContext.Current.Request);
            var query = GetConstraintedQuery();
            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };
            var optionsPost = new ODataQueryOptionsPost<PromoProductCorrectionPriceIncreaseView>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<PromoProductCorrectionPriceIncreaseView>;

        }

        //[ClaimsAuthorize]
        //public IHttpActionResult Put([FromODataUri] System.Guid key, Delta<PromoProductCorrectionPriceIncreaseView> patch)
        //{
        //    var model = Context.Set<PromoProductCorrectionPriceIncreaseView>().Find(key);
        //    if (model == null)
        //    {
        //        return NotFound();
        //    }

        //    patch.Put(model);

        //    try
        //    {
        //        var saveChangesResult = Context.SaveChanges();
        //        if (saveChangesResult > 0)
        //        {
        //            CreateChangesIncident(Context.Set<ChangesIncident>(), model);
        //            Context.SaveChanges();
        //        }
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!EntityExists(key))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return Updated(model);
        //}

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> Post(PromoProductCorrectionPriceIncreaseView model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //if (model.TempId == "")
            //{
            //    model.TempId = null;
            //}
            if (!CheckPriceListA(model))
            {
                return InternalServerError(new Exception($"Promo {model.Number} not found products with FuturePriceMarker"));
            }

            // если существует коррекция на данный PromoProduct, то не создаем новый объект
            var item = Context.Set<PromoProductCorrectionPriceIncrease>()
                .Include(g => g.PromoProductPriceIncrease.PromoPriceIncrease.Promo)
                .FirstOrDefault(x => x.PromoProductPriceIncrease.ZREP == model.ZREP && x.PromoProductPriceIncrease.PromoPriceIncrease.Promo.Number == model.Number);

            if (item != null)
            {
                if (item.PromoProductPriceIncrease.PromoProduct.Promo.NeedRecountUpliftPI == true)
                {
                    return InternalServerError(new Exception("Promo Locked Update"));
                }
                item.PlanProductUpliftPercentCorrected = model.PlanProductUpliftPercentCorrected;
                item.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                item.UserId = user.Id;
                item.UserName = user.Login;
                if (item.Disabled)
                {
                    item.Disabled = false;
                    item.DeletedDate = null;
                }
                try
                {
                    var saveChangesResult = await Context.SaveChangesAsync();
                    if (saveChangesResult > 0)
                    {
                        CreateChangesIncident(Context.Set<ChangesIncident>(), item);
                        await Context.SaveChangesAsync();
                    }
                }
                catch (Exception e)
                {
                    return GetErorrRequest(e);
                }

                return Created(model);
            }
            else
            {
                var promo = Context.Set<Promo>()
                    .Include(g => g.PromoPriceIncrease.PromoProductPriceIncreases)
                    .Where(g => g.Number == model.Number && g.PromoPriceIncrease.PromoProductPriceIncreases.Any(h => h.ZREP == model.ZREP))
                    .FirstOrDefault();
                PromoProductCorrectionPriceIncrease correctionPriceIncrease = new PromoProductCorrectionPriceIncrease();
                if (promo.NeedRecountUpliftPI == true)
                {
                    return InternalServerError(new Exception("Promo Locked Update"));
                }
                if (promo.PromoPriceIncrease == null)
                {
                    return InternalServerError(new Exception($"Promo {promo.Number} not have PriceIncrease - recalculate"));
                }
                else
                {
                    var pppiId = promo.PromoPriceIncrease.PromoProductPriceIncreases.FirstOrDefault(g => g.ZREP == model.ZREP).Id;
                    PromoProductCorrectionPriceIncrease ppcpi = Context.Set<PromoProductCorrectionPriceIncrease>()
                        .FirstOrDefault(x => x.PromoProductPriceIncreaseId == pppiId);
                    if (ppcpi != null)
                    {
                        if (ppcpi.Disabled == true)
                        {
                            ppcpi.Disabled = false;
                            ppcpi.DeletedDate = null;
                        }
                        ppcpi.PlanProductUpliftPercentCorrected = model.PlanProductUpliftPercentCorrected;
                        ppcpi.CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                        ppcpi.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                        ppcpi.UserId = user.Id;
                        ppcpi.UserName = user.Login;
                    }
                    else
                    {
                        ppcpi = new PromoProductCorrectionPriceIncrease
                        {
                            PlanProductUpliftPercentCorrected = model.PlanProductUpliftPercentCorrected,
                            CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                            ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                            UserId = user.Id,
                            UserName = user.Login,
                            PromoProductPriceIncreaseId = pppiId
                        };
                        Context.Set<PromoProductCorrectionPriceIncrease>().Add(ppcpi);
                    }
                    correctionPriceIncrease = ppcpi;
                }

                try
                {
                    var saveChangesResult = await Context.SaveChangesAsync();
                    if (saveChangesResult > 0)
                    {
                        CreateChangesIncident(Context.Set<ChangesIncident>(), correctionPriceIncrease);
                        await Context.SaveChangesAsync();
                    }
                }
                catch (Exception e)
                {
                    return GetErorrRequest(e);
                }

                return Created(model);
            }
        }

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] System.Guid key, Delta<PromoProductCorrectionPriceIncreaseView> patch)
        {
            try
            {
                var view = Context.Set<PromoProductCorrectionPriceIncreaseView>()
                    .AsNoTracking()
                    .FirstOrDefault(x => x.Id == key);

                if (view == null)
                {
                    return NotFound();
                }
                patch.Patch(view);
                if (!CheckPriceListA(view))
                {
                    return InternalServerError(new Exception($"Promo {view.Number} not found products with FuturePriceMarker"));
                }
                var model = Context.Set<PromoProductCorrectionPriceIncrease>()
                    .Include(g => g.PromoProductPriceIncrease.PromoProduct.Promo.IncrementalPromoes)
                    .Include(g => g.PromoProductPriceIncrease.PromoProduct.Promo.BTLPromoes)
                    .Include(g => g.PromoProductPriceIncrease.PromoProduct.Promo.PromoSupportPromoes)
                    .Include(g => g.PromoProductPriceIncrease.PromoProduct.Promo.PromoProductTrees)
                    .FirstOrDefault(x => x.Id == key);

                if (model == null)
                {
                    return NotFound();
                }

                model.PlanProductUpliftPercentCorrected = view.PlanProductUpliftPercentCorrected;

                var promoStatus = model.PromoProductPriceIncrease.PromoProduct.Promo.PromoStatus.SystemName;

                model.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                model.UserId = user.Id;
                model.UserName = user.Login;

                if (model.TempId == "")
                {
                    model.TempId = null;
                }


                ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                string promoStatuses = settingsManager.GetSetting<string>("PROMO_PRODUCT_CORRECTION_PROMO_STATUS_LIST", "Draft,Deleted,Cancelled,Started,Finished,Closed");
                string[] status = promoStatuses.Split(',');
                if (status.Any(x => x == promoStatus) && !role.Equals("SupportAdministrator"))
                    return InternalServerError(new Exception("Cannot be update correction where status promo = " + promoStatus));
                if (model.PromoProductPriceIncrease.PromoProduct.Promo.NeedRecountUplift == false)
                {
                    return InternalServerError(new Exception("Promo Locked Update"));
                }

                var saveChangesResult = await Context.SaveChangesAsync();
                if (saveChangesResult > 0)
                {
                    CreateChangesIncident(Context.Set<ChangesIncident>(), model);
                    await Context.SaveChangesAsync();
                }

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

        private bool CheckPriceListA(PromoProductCorrectionPriceIncreaseView view)
        {
            Promo promo = Context.Set<Promo>()
                .Include(g => g.PromoPriceIncrease.PromoProductPriceIncreases)
                .FirstOrDefault(g => g.Number == view.Number);

            List<PromoProduct> promoProducts = Context.Set<PromoProduct>()
                .Include(f => f.PromoProductsCorrections)
                .Where(x => !x.Disabled && x.PromoId == promo.Id)
                .ToList();
            List<PriceList> allPriceLists = Context.Set<PriceList>().Where(x => !x.Disabled && x.StartDate <= promo.DispatchesStart
                                                                    && x.EndDate >= promo.DispatchesStart
                                                                    && x.ClientTreeId == promo.ClientTreeKeyId).ToList();
            List<PriceList> priceListsForPromoAndPromoProductsFPM = allPriceLists.Where(x => promoProducts.Any(y => y.ProductId == x.ProductId && x.FuturePriceMarker == true)).ToList();

            bool IsOneProductWithFuturePriceMarker = false;
            foreach (PromoProduct promoProduct in promoProducts)
            {
                var priceListFPM = priceListsForPromoAndPromoProductsFPM.Where(x => x.ProductId == promoProduct.ProductId)
                                                                  .OrderByDescending(x => x.StartDate).FirstOrDefault();
                if (priceListFPM != null)
                {
                    IsOneProductWithFuturePriceMarker = true;
                }
            }
            return IsOneProductWithFuturePriceMarker;
        }

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> Delete([FromODataUri] System.Guid key)
        {
            try
            {
                var model = Context.Set<PromoProductCorrectionPriceIncrease>()
                    .Include(g => g.PromoProductPriceIncrease.PromoPriceIncrease.Promo)
                    .FirstOrDefault(g => g.Id == key);
                if (model == null)
                {
                    return NotFound();
                }
                if (model.PromoProductPriceIncrease.PromoPriceIncrease.Promo.NeedRecountUpliftPI == true)
                {
                    return InternalServerError(new Exception("Promo Locked Update"));
                }
                model.DeletedDate = System.DateTime.Now;
                model.Disabled = true;

                var saveChangesResult = await Context.SaveChangesAsync();
                if (saveChangesResult > 0)
                {
                    CreateChangesIncident(Context.Set<ChangesIncident>(), model);
                    await Context.SaveChangesAsync();
                }

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }

        private bool EntityExists(Guid key)
        {
            return Context.Set<PromoProductCorrectionPriceIncrease>().Count(e => e.Id == key) > 0;
        }

        public static void CreateChangesIncident(DbSet<ChangesIncident> changesIncidents, PromoProductCorrectionPriceIncrease promoProductCorrection)
        {
            changesIncidents.Add(new ChangesIncident
            {
                Id = Guid.NewGuid(),
                DirectoryName = nameof(PromoProductCorrectionPriceIncrease),
                ItemId = promoProductCorrection.Id.ToString(),
                CreateDate = DateTimeOffset.Now,
                ProcessDate = null,
                DeletedDate = null,
                Disabled = false
            });
        }

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> ExportXLSX(ODataQueryOptions<PromoProductCorrectionPriceIncreaseView> options)
        {
            string bodyText = Helper.GetRequestBody(HttpContext.Current.Request);
            //TPMmode tPMmode = JsonHelper.GetValueIfExists<TPMmode>(bodyText, "TPMmode");
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            var url = HttpContext.Current.Request.Url.AbsoluteUri;
            var results = options.ApplyTo(GetConstraintedQuery()).Cast<PromoProductCorrectionPriceIncreaseView>()
                                                .Where(x => !x.Disabled)
                                                .Select(p => p.Id);

            HandlerData data = new HandlerData();
            string handlerName = "ExportHandler";

            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TModel", typeof(PromoProductCorrectionPriceIncreaseView), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(PromoHelper), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(PromoHelper.GetPromoProductCorrectionExportSettings), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = $"Export {nameof(PromoProductCorrectionPriceIncreaseView)} dictionary",
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

            return Content(HttpStatusCode.OK, "success");
        }

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> ExportCorrectionXLSX(ODataQueryOptions<PromoProductCorrectionPriceIncreaseView> options)
        {
            List<string> stasuses = new List<string> { "DraftPublished", "OnApproval", "Approved", "Planned" };
            IQueryable<PromoProductPriceIncrease> results = Context.Set<PromoProductPriceIncrease>()
                .Include(g => g.PromoPriceIncrease.Promo)
                .Where(g => !g.Disabled && stasuses.Contains(g.PromoPriceIncrease.Promo.PromoStatus.SystemName) && !(bool)g.PromoPriceIncrease.Promo.InOut && (bool)g.PromoPriceIncrease.Promo.NeedRecountUplift)
                .OrderBy(g => g.PromoPriceIncrease.Promo.Number).ThenBy(d => d.ZREP);
            //IQueryable results = options.ApplyTo(GetConstraintedQuery());
            var ddd = results.ToList();
            Guid userId = user == null ? Guid.Empty : (user.Id ?? Guid.Empty);

            HandlerData data = new HandlerData();
            string handlerName = "ExportHandler";

            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TModel", typeof(PromoProductPriceIncrease), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(PromoHelper), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(PromoHelper.GetExportCorrectionPISettings), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = $"Export {nameof(PromoProductCorrectionPriceIncrease)} dictionary",
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

            return Content(HttpStatusCode.OK, "success");
        }

        private ExceptionResult GetErorrRequest(Exception e)
        {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
            {
                return InternalServerError(new Exception("This PromoProductCorrection has already existed"));
            }
            else
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }


        public static void CreateChangesIncident(DbSet<ChangesIncident> changesIncidents, PromoProductsCorrection promoProductCorrection)
        {
            changesIncidents.Add(new ChangesIncident
            {
                Id = Guid.NewGuid(),
                DirectoryName = nameof(PromoProductsCorrection),
                ItemId = promoProductCorrection.Id.ToString(),
                CreateDate = DateTimeOffset.Now,
                ProcessDate = null,
                DeletedDate = null,
                Disabled = false
            });
        }

        [ClaimsAuthorize]
        public async Task<HttpResponseMessage> FullImportXLSX([FromUri] TPMmode tPMmode)
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                var importDir = AppSettingsManager.GetSetting("IMPORT_DIRECTORY", "ImportFiles");
                var fileName = await FileUtility.UploadFile(Request, importDir);

                await CreateImportTask(fileName, "FullXLSXUpdateImportPromoProductCorrectionPriceIncreaseHandler", tPMmode);

                var result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StringContent("success = true");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

                return result;
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private async Task CreateImportTask(string fileName, string importHandler, TPMmode tPMmode)
        {
            var userId = user == null ? Guid.Empty : (user.Id ?? Guid.Empty);

            var resiltfile = new ImportResultFilesModel();
            var resultmodel = new ImportResultModel();

            var handlerData = new HandlerData();
            var fileModel = new FileModel()
            {
                LogicType = "Import",
                Name = Path.GetFileName(fileName),
                DisplayName = Path.GetFileName(fileName)
            };

            HandlerDataHelper.SaveIncomingArgument("TPMmode", tPMmode, handlerData, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("File", fileModel, handlerData, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, handlerData, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, handlerData, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportPromoProductsCorrection), handlerData, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportPromoProductsCorrection).Name, handlerData, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(PromoProductsCorrection), handlerData, visible: false, throwIfNotExists: false);
            //HandlerDataHelper.SaveIncomingArgument("UniqueFields", new List<String>() {"Name"}, handlerData);

            var handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = "Загрузка импорта из файла " + typeof(PromoProductsCorrection).Name,
                Name = "Module.Host.TPM.Handlers." + importHandler,
                ExecutionPeriod = null,
                RunGroup = typeof(PromoProductsCorrection).Name,
                CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                LastExecutionDate = null,
                NextExecutionDate = null,
                ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                UserId = userId,
                RoleId = roleId
            };

            handler.SetParameterData(handlerData);
            Context.LoopHandlers.Add(handler);
            await Context.SaveChangesAsync();
        }

        [ClaimsAuthorize]
        public IHttpActionResult DownloadTemplateXLSX([FromUri] TPMmode tPMmode)
        {
            try
            {
                IEnumerable<Column> columns = GetImportSettings();
                if (tPMmode == TPMmode.Current)
                {
                    columns = GetImportSettings();
                }
                //if (tPMmode == TPMmode.RS)
                //{
                //    columns = GetImportSettingsRS();
                //}
                XLSXExporter exporter = new XLSXExporter(columns);
                string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
                string filename = string.Format("{0}Template.xlsx", "PromoProductCorrectionPricesUplift");
                if (!Directory.Exists(exportDir))
                {
                    Directory.CreateDirectory(exportDir);
                }
                string filePath = Path.Combine(exportDir, filename);
                exporter.Export(Enumerable.Empty<PromoProductCorrectionPriceIncrease>(), filePath);
                string file = Path.GetFileName(filePath);
                return Content(HttpStatusCode.OK, file);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }

        }

        private IEnumerable<Column> GetImportSettings()
        {
            int orderNumber = 1;
            IEnumerable<Column> columns = new List<Column>()
            {
                new Column { Order = orderNumber++, Field = "PromoNumber", Header = "Promo ID", Quoting = false,  Format = "0"  },
                new Column { Order = orderNumber++, Field = "ProductZREP", Header = "ZREP", Quoting = false,  Format = "0"  },
                new Column { Order = orderNumber++, Field = "PlanProductUpliftPercent", Header = "Plan Product Uplift, %", Quoting = false,  Format = "0.00"},

            };
            return columns;
        }

        private IEnumerable<Column> GetImportSettingsRS()
        {
            int orderNumber = 1;
            IEnumerable<Column> columns = new List<Column>()
            {
                new Column { Order = orderNumber++, Field = "TPMmode", Header = "Indicator", Quoting = false },
                new Column { Order = orderNumber++, Field = "PromoNumber", Header = "Promo ID", Quoting = false,  Format = "0"  },
                new Column { Order = orderNumber++, Field = "ProductZREP", Header = "ZREP", Quoting = false,  Format = "0"  },
                new Column { Order = orderNumber++, Field = "PlanProductUpliftPercent", Header = "Plan Product Uplift, %", Quoting = false,  Format = "0.00"},

            };
            return columns;
        }

        private bool CheckRSPeriodSuitable(Promo promo, DatabaseContext context)
        {
            var startEndModel = RSPeriodHelper.GetRSPeriod(context);
            return promo.DispatchesStart >= startEndModel.StartDate && startEndModel.EndDate >= promo.EndDate;
        }
    }
}
