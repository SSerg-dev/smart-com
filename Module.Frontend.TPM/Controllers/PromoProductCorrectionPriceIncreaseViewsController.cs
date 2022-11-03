using AutoMapper;
using Core.Dependency;
using Core.Security;
using Core.Security.Models;
using Core.Settings;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.FunctionalHelpers.RSmode;
using Module.Frontend.TPM.FunctionalHelpers.RSPeriod;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.Interfaces;
using Module.Persist.TPM.Model.SimpleModel;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Newtonsoft.Json;
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

        [ClaimsAuthorize]
        public IHttpActionResult Put([FromODataUri] System.Guid key, Delta<PromoProductCorrectionPriceIncrease> patch)
        {
            var model = Context.Set<PromoProductCorrectionPriceIncrease>().Find(key);
            if (model == null)
            {
                return NotFound();
            }

            patch.Put(model);

            try
            {
                var saveChangesResult = Context.SaveChanges();
                if (saveChangesResult > 0)
                {
                    CreateChangesIncident(Context.Set<ChangesIncident>(), model);
                    Context.SaveChanges();
                }
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
        public IHttpActionResult Post(PromoProductCorrectionPriceIncrease model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (model.TempId == "")
            {
                model.TempId = null;
            }

            // если существует коррекция на данный PromoProduct, то не создаем новый объект
            var item = Context.Set<PromoProductCorrectionPriceIncrease>()
                .Include(g => g.PromoProductPriceIncrease)
                .FirstOrDefault(x => x.PromoProductPriceIncrease.PromoProductId == model.PromoProductPriceIncrease.PromoProductId && x.TempId == model.TempId && !x.Disabled);

            if (item != null)
            {
                if (item.PromoProductPriceIncrease.PromoProduct.Promo.NeedRecountUplift == false && String.IsNullOrEmpty(item.TempId))
                {
                    return InternalServerError(new Exception("Promo Locked Update"));
                }
                item.PlanProductUpliftPercentCorrected = model.PlanProductUpliftPercentCorrected;
                item.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                item.UserId = user.Id;
                item.UserName = user.Login;

                try
                {
                    var saveChangesResult = Context.SaveChanges();
                    if (saveChangesResult > 0)
                    {
                        CreateChangesIncident(Context.Set<ChangesIncident>(), model);
                        Context.SaveChanges();
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
                var proxy = Context.Set<PromoProductCorrectionPriceIncrease>().Create<PromoProductCorrectionPriceIncrease>();
                var configuration = new MapperConfiguration(cfg =>
                    cfg.CreateMap<PromoProductCorrectionPriceIncrease, PromoProductCorrectionPriceIncrease>().ReverseMap());
                var mapper = configuration.CreateMapper();
                var result = mapper.Map(model, proxy);
                var promoProduct = Context.Set<PromoProduct>()
                    .FirstOrDefault(x => x.Id == result.PromoProductPriceIncrease.PromoProductId && !x.Disabled);

                if (promoProduct.Promo.NeedRecountUplift == false && String.IsNullOrEmpty(result.TempId))
                {
                    return InternalServerError(new Exception("Promo Locked Update"));
                }
                result.CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                result.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                result.UserId = user.Id;
                result.UserName = user.Login;

                Context.Set<PromoProductCorrectionPriceIncrease>().Add(result);

                try
                {
                    var saveChangesResult = Context.SaveChanges();
                    if (saveChangesResult > 0)
                    {
                        CreateChangesIncident(Context.Set<ChangesIncident>(), result);
                        Context.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    return GetErorrRequest(e);
                }

                return Created(result);
            }
        }

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<PromoProductCorrectionPriceIncreaseView> patch)
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

                var saveChangesResult = Context.SaveChanges();
                if (saveChangesResult > 0)
                {
                    CreateChangesIncident(Context.Set<ChangesIncident>(), model);
                    Context.SaveChanges();
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


        [ClaimsAuthorize]
        public IHttpActionResult Delete([FromODataUri] System.Guid key)
        {
            try
            {
                var model = Context.Set<PromoProductCorrectionPriceIncrease>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }
                if (model.PromoProductPriceIncrease.PromoProduct.Promo.NeedRecountUplift == false)
                {
                    return InternalServerError(new Exception("Promo Locked Update"));
                }
                model.DeletedDate = System.DateTime.Now;
                model.Disabled = true;

                var saveChangesResult = Context.SaveChanges();
                if (saveChangesResult > 0)
                {
                    CreateChangesIncident(Context.Set<ChangesIncident>(), model);
                    Context.SaveChanges();
                }

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return InternalServerError(e.InnerException);
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
        public IHttpActionResult ExportXLSX(ODataQueryOptions<PromoProductCorrectionPriceIncreaseView> options)
        {
            string bodyText = Helper.GetRequestBody(HttpContext.Current.Request);
            //TPMmode tPMmode = JsonHelper.GetValueIfExists<TPMmode>(bodyText, "TPMmode");
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            var url = HttpContext.Current.Request.Url.AbsoluteUri;
            var results = options.ApplyTo(GetConstraintedQuery()).Cast<PromoProductCorrectionPriceIncreaseView>()
                                                .Where(x => !x.Disabled)
                                                .Select(p => p.Id);


            using (DatabaseContext context = new DatabaseContext())
            {
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
                context.LoopHandlers.Add(handler);
                context.SaveChanges();
            }

            return Content(HttpStatusCode.OK, "success");
        }

        [ClaimsAuthorize]
        public IHttpActionResult ExportCorrectionXLSX(ODataQueryOptions<PromoProductCorrectionPriceIncreaseView> options)
        {
            List<string> stasuses = new List<string> { "DraftPublished", "OnApproval", "Approved", "Planned" };
            IQueryable<PromoProductPriceIncrease> results = Context.Set<PromoProductPriceIncrease>()
                .Include(g=>g.PromoPriceIncrease.Promo)
                .Where(g => !g.Disabled && stasuses.Contains(g.PromoPriceIncrease.Promo.PromoStatus.SystemName) && !(bool)g.PromoPriceIncrease.Promo.InOut && (bool)g.PromoPriceIncrease.Promo.NeedRecountUplift)
                .OrderBy(g => g.PromoPriceIncrease.Promo.Number).ThenBy(d => d.ZREP);
            //IQueryable results = options.ApplyTo(GetConstraintedQuery());
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            using (DatabaseContext context = new DatabaseContext())
            {
                HandlerData data = new HandlerData();
                string handlerName = "ExportHandler";

                HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TModel", typeof(PromoProductPriceIncrease), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(PromoHelper), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(PromoHelper.GetExportCorrectionSettings), data, visible: false, throwIfNotExists: false);
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
                context.LoopHandlers.Add(handler);
                context.SaveChanges();
            }

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
                return InternalServerError(e);
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

                CreateImportTask(fileName, "FullXLSXUpdateImportPromoProductsCorrectionHandler", tPMmode);

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

        private void CreateImportTask(string fileName, string importHandler, TPMmode tPMmode)
        {
            var userId = user == null ? Guid.Empty : (user.Id ?? Guid.Empty);

            using (var databaseContext = new DatabaseContext())
            {
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

                var loopHandler = new LoopHandler()
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

                loopHandler.SetParameterData(handlerData);
                databaseContext.LoopHandlers.Add(loopHandler);
                databaseContext.SaveChanges();
            }
        }

        [ClaimsAuthorize]
        public IHttpActionResult DownloadTemplateXLSX([FromUri] TPMmode tPMmode)
        {
            try
            {
                IEnumerable<Column> columns = GetImportSettings();
                //if (tPMmode == TPMmode.Current)
                //{
                //    columns = GetImportSettings();
                //}
                //if (tPMmode == TPMmode.RS)
                //{
                //    columns = GetImportSettingsRS();
                //}
                XLSXExporter exporter = new XLSXExporter(columns);
                string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
                string filename = string.Format("{0}Template.xlsx", "PromoProductsUplift");
                if (!Directory.Exists(exportDir))
                {
                    Directory.CreateDirectory(exportDir);
                }
                string filePath = Path.Combine(exportDir, filename);
                exporter.Export(Enumerable.Empty<PromoProductsCorrection>(), filePath);
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
