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
    public class PromoProductCorrectionViewsController : EFContextController
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

        [ClaimsAuthorize]
        public IHttpActionResult Post(PromoProductCorrectionView model)
        {
            string bodyText = Helper.GetRequestBody(HttpContext.Current.Request);
            TPMmode tPMmode = JsonHelper.GetValueIfExists<TPMmode>(bodyText, "TPMmode");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PromoProductCorrectionView, PromoProductsCorrection>();
            });
            var mapperPromoProductCorrection = config.CreateMapper();
            var modelMapp = mapperPromoProductCorrection.Map<PromoProductsCorrection>(model);

            if (modelMapp.TempId == "")
            {
                modelMapp.TempId = null;
            }

            // если существует коррекция на данный PromoProduct, то не создаем новый объект
            var item = Context.Set<PromoProductsCorrection>()
                .Include(g => g.PromoProduct.Promo.IncrementalPromoes)
                .Include(g => g.PromoProduct.Promo.BTLPromoes)
                .Include(g => g.PromoProduct.Promo.PromoSupportPromoes)
                .Include(g => g.PromoProduct.Promo.PromoProductTrees)
                .FirstOrDefault(x => x.PromoProductId == modelMapp.PromoProductId && x.TempId == modelMapp.TempId && !x.Disabled);

            if (item != null)
            {
                if ((int)model.TPMmode != (int)tPMmode)
                {
                    List<PromoProductsCorrection> promoProductsCorrections = Context.Set<PromoProductsCorrection>()
                        .Include(g => g.PromoProduct.Promo.IncrementalPromoes)
                        .Include(g => g.PromoProduct.Promo.BTLPromoes)
                        .Include(g => g.PromoProduct.Promo.PromoSupportPromoes)
                        .Include(g => g.PromoProduct.Promo.PromoProductTrees)
                        .Where(x => x.PromoProduct.PromoId == item.PromoProduct.PromoId && !x.Disabled)
                        .ToList();
                    promoProductsCorrections = RSmodeHelper.EditToPromoProductsCorrectionRS(Context, promoProductsCorrections);
                    item = promoProductsCorrections.FirstOrDefault(g => g.PromoProduct.Promo.Number == item.PromoProduct.Promo.Number);
                }
                if (item.PromoProduct.Promo.NeedRecountUplift == false && String.IsNullOrEmpty(item.TempId))
                {
                    return InternalServerError(new Exception("Promo Locked Update"));
                }
                item.PlanProductUpliftPercentCorrected = modelMapp.PlanProductUpliftPercentCorrected;
                item.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                item.UserId = user.Id;
                item.UserName = user.Login;

                try
                {

                    var saveChangesResult = Context.SaveChanges();
                    if (saveChangesResult > 0)
                    {
                        CreateChangesIncident(Context.Set<ChangesIncident>(), item);
                        Context.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    return GetErorrRequest(e);
                }


                var configViewMapping = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<PromoProductsCorrection, PromoProductCorrectionView>()
                        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                        .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.PromoProduct.Promo.Number))
                        .ForMember(dest => dest.ClientHierarchy, opt => opt.MapFrom(src => src.PromoProduct.Promo.ClientHierarchy))
                        .ForMember(dest => dest.BrandTechName, opt => opt.MapFrom(src => src.PromoProduct.Promo.BrandTech.Name))
                        .ForMember(dest => dest.MarsMechanicName, opt => opt.MapFrom(src => src.PromoProduct.Promo.MarsMechanic.Name))
                        .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.PromoProduct.Promo.Event.Name))
                        .ForMember(dest => dest.PromoStatusSystemName, opt => opt.MapFrom(src => src.PromoProduct.Promo.PromoStatus.SystemName))
                        .ForMember(dest => dest.MarsStartDate, opt => opt.MapFrom(src => src.PromoProduct.Promo.MarsStartDate))
                        .ForMember(dest => dest.MarsEndDate, opt => opt.MapFrom(src => src.PromoProduct.Promo.MarsEndDate))
                        .ForMember(dest => dest.PlanProductBaselineLSV, opt => opt.MapFrom(src => src.PromoProduct.PlanProductBaselineLSV))
                        .ForMember(dest => dest.PlanProductIncrementalLSV, opt => opt.MapFrom(src => src.PromoProduct.PlanProductIncrementalLSV))
                        .ForMember(dest => dest.PlanProductLSV, opt => opt.MapFrom(src => src.PromoProduct.PlanProductLSV))
                        .ForMember(dest => dest.ZREP, opt => opt.MapFrom(src => src.PromoProduct.ZREP))
                        .ForMember(dest => dest.PlanProductUpliftPercentCorrected, opt => opt.MapFrom(src => src.PlanProductUpliftPercentCorrected))
                        .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreateDate))
                        .ForMember(dest => dest.ChangeDate, opt => opt.MapFrom(src => src.ChangeDate))
                        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                        .ForMember(dest => dest.TPMmode, opt => opt.MapFrom(src => src.TPMmode))
                        .ForMember(dest => dest.ClientTreeId, opt => opt.MapFrom(src => src.PromoProduct.Promo.ClientTreeId))
                        .ForMember(dest => dest.Disabled, opt => opt.MapFrom(src => src.Disabled))
                        .ForMember(dest => dest.PromoProductId, opt => opt.MapFrom(src => src.PromoProductId))
                        .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                        ;
                });
                var mapperPromoProductCorrectionView = configViewMapping.CreateMapper();
                var viewMapp = mapperPromoProductCorrectionView.Map<PromoProductCorrectionView>(item);

                return Updated(viewMapp);
            }
            else
            {
                var proxy = Context.Set<PromoProductsCorrection>().Create<PromoProductsCorrection>();
                var configuration = new MapperConfiguration(cfg =>
                    cfg.CreateMap<PromoProductCorrectionView, PromoProductsCorrection>().ReverseMap());
                var mapper = configuration.CreateMapper();
                var result = mapper.Map(model, proxy);
                var promoProduct = Context.Set<PromoProduct>()
                    .FirstOrDefault(x => x.Id == result.PromoProductId && !x.Disabled);

                if (promoProduct.Promo.NeedRecountUplift == false && String.IsNullOrEmpty(result.TempId))
                {
                    return InternalServerError(new Exception("Promo Locked Update"));
                }
                result.CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                result.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                result.UserId = user.Id;
                result.UserName = user.Login;

                Context.Set<PromoProductsCorrection>().Add(result);

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

                return Created(model);
            }
        }

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<PromoProductCorrectionView> patch)
        {
            try
            {
                var model = Context.Set<PromoProductsCorrection>()
                    .Include(g => g.PromoProduct.Promo.IncrementalPromoes)
                    .Include(g => g.PromoProduct.Promo.BTLPromoes)
                    .Include(g => g.PromoProduct.Promo.PromoSupportPromoes)
                    .Include(g => g.PromoProduct.Promo.PromoProductTrees)
                    .FirstOrDefault(x => x.Id == key);

                if (model == null)
                {
                    return NotFound();
                }
                var promoStatus = model.PromoProduct.Promo.PromoStatus.SystemName;

                patch.TryGetPropertyValue("TPMmode", out object mode);

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<PromoProductsCorrection, PromoProductCorrectionView>();
                });
                var mapperPromoProductCorrection = config.CreateMapper();
                var modelMapp = mapperPromoProductCorrection.Map<PromoProductCorrectionView>(model);

                patch.Patch(modelMapp);
                var config2 = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<PromoProductCorrectionView, PromoProductsCorrection>()
                    .ForMember(pTo => pTo.Id, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProduct, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductId, opt => opt.Ignore());
                });
                var mapperPromoProductCorrection2 = config2.CreateMapper();
                mapperPromoProductCorrection2.Map<PromoProductCorrectionView, PromoProductsCorrection>(modelMapp, model);

                if ((int)model.TPMmode != (int)mode)
                {
                    List<PromoProductsCorrection> promoProductsCorrections = Context.Set<PromoProductsCorrection>()
                        .Include(g => g.PromoProduct.Promo.IncrementalPromoes)
                        .Include(g => g.PromoProduct.Promo.BTLPromoes)
                        .Include(g => g.PromoProduct.Promo.PromoSupportPromoes)
                        .Include(g => g.PromoProduct.Promo.PromoProductTrees)
                        .Where(x => x.PromoProduct.PromoId == model.PromoProduct.PromoId && !x.Disabled)
                        .ToList();
                    promoProductsCorrections = RSmodeHelper.EditToPromoProductsCorrectionRS(Context, promoProductsCorrections);
                    model = promoProductsCorrections.FirstOrDefault(g => g.PromoProduct.Promo.Number == model.PromoProduct.Promo.Number);
                }


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
                if (model.PromoProduct.Promo.NeedRecountUplift == false)
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
                var model = Context.Set<PromoProductsCorrection>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }
                if (model.PromoProduct.Promo.NeedRecountUplift == false)
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

        private bool EntityExists(System.Guid key)
        {
            return Context.Set<PromoProductsCorrection>().Count(e => e.Id == key) > 0;
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

                var importDir = AppSettingsManager.GetSetting("IMPORT_DIRECTORY", "ImportFiles");
                var fileName = await FileUtility.UploadFile(Request, importDir);

                CreateImportTask(fileName, "FullXLSXUpdateImportPromoProductsCorrectionHandler");

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

        private void CreateImportTask(string fileName, string importHandler)
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
        public IHttpActionResult DownloadTemplateXLSX()
        {
            try
            {
                IEnumerable<Column> columns = GetImportSettings();
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

    }
}
