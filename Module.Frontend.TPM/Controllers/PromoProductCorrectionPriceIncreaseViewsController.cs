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
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<PromoProductCorrectionPriceIncrease> patch)
        {
            try
            {
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
                var promoStatus = model.PromoProductPriceIncrease.PromoProduct.Promo.PromoStatus.SystemName;

                patch.Patch(model);
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
                HandlerDataHelper.SaveIncomingArgument("TModel", typeof(PromoProductCorrectionView), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(PromoHelper), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(PromoHelper.GetPromoProductCorrectionExportSettings), data, visible: false, throwIfNotExists: false);
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
        public IHttpActionResult ExportCorrectionXLSX(ODataQueryOptions<PromoProductCorrectionPriceIncreaseView> options)
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
            TPMmode tPMmode = model.TPMmode;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PromoProductCorrectionPriceIncreaseView, PromoProductsCorrection>();
            });

            var mapperPromoProductCorrection = config.CreateMapper();
            var modelMapp = mapperPromoProductCorrection.Map<PromoProductsCorrection>(model);

            if (modelMapp.TempId == "")
            {
                modelMapp.TempId = null;
            }
            var mode = Context.Set<PromoProduct>().FirstOrDefault(x => x.Id == modelMapp.PromoProductId).TPMmode;
            // если существует коррекция на данный PromoProduct, то не создаем новый объект
            var item = Context.Set<PromoProductsCorrection>()
                .Include(g => g.PromoProduct.Promo.IncrementalPromoes)
                .Include(g => g.PromoProduct.Promo.PromoSupportPromoes)
                .Include(g => g.PromoProduct.Promo.PromoProductTrees)
                .FirstOrDefault(x => x.PromoProductId == modelMapp.PromoProductId && x.TempId == modelMapp.TempId && !x.Disabled && x.TPMmode == tPMmode);

            if (item != null) //редактирование коррекции
            {
                if (tPMmode == TPMmode.RS && !CheckRSPeriodSuitable(item.PromoProduct.Promo, Context))
                {
                    return InternalServerError(new Exception("Promo is not in the RS period"));
                }
                if (item.PromoProduct.Promo.NeedRecountUplift == false && String.IsNullOrEmpty(item.TempId))
                {
                    return InternalServerError(new Exception("Promo Locked Update"));
                }
                // Редактирование Current promo в RS режиме => копируем в RS, работаем с копией
                if (tPMmode == TPMmode.RS && mode == TPMmode.Current)
                {
                    List<PromoProductsCorrection> promoProductsCorrections = Context.Set<PromoProductsCorrection>()
                        .Include(g => g.PromoProduct.Promo.IncrementalPromoes)
                        .Include(g => g.PromoProduct.Promo.PromoSupportPromoes)
                        .Include(g => g.PromoProduct.Promo.PromoProductTrees)
                        .Where(x => x.PromoProduct.PromoId == item.PromoProduct.PromoId && !x.Disabled)
                        .ToList();
                    promoProductsCorrections = RSmodeHelper.EditToPromoProductsCorrectionRS(Context, promoProductsCorrections);
                    item = promoProductsCorrections.FirstOrDefault(g => g.PromoProduct.ZREP == item.PromoProduct.ZREP);
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

                // редактирование в Current режиме пересоздаёт RS-копию при наличии
                if (tPMmode == TPMmode.Current)
                {
                    var promoRS = Context.Set<Promo>().FirstOrDefault(x => x.Number == item.PromoProduct.Promo.Number && x.TPMmode == TPMmode.RS);
                    if (promoRS != null)
                    {
                        Context.Set<Promo>().Remove(promoRS);
                        Context.SaveChanges();

                        var promoProductsCorrections = Context.Set<PromoProductsCorrection>()
                            .Include(g => g.PromoProduct.Promo.IncrementalPromoes)
                            .Include(g => g.PromoProduct.Promo.PromoSupportPromoes)
                            .Include(g => g.PromoProduct.Promo.PromoProductTrees)
                            .Where(x => x.PromoProduct.PromoId == item.PromoProduct.PromoId && !x.Disabled)
                            .ToList();
                        promoProductsCorrections = RSmodeHelper.EditToPromoProductsCorrectionRS(Context, promoProductsCorrections);
                        foreach (var ppc in promoProductsCorrections)
                        {
                            CreateChangesIncident(Context.Set<ChangesIncident>(), ppc);
                        }
                        Context.SaveChanges();
                    }
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
                var viewMapp = mapperPromoProductCorrectionView.Map<PromoProductCorrectionPriceIncreaseView>(item);

                return Updated(viewMapp);
            }
            else //добавление коррекции
            {
                var proxy = Context.Set<PromoProductsCorrection>().Create<PromoProductsCorrection>();
                var configuration = new MapperConfiguration(cfg =>
                    cfg.CreateMap<PromoProductCorrectionPriceIncreaseView, PromoProductsCorrection>().ReverseMap());
                var mapper = configuration.CreateMapper();
                var result = mapper.Map(model, proxy);

                if (tPMmode == TPMmode.Current)
                {
                    var promoProduct = Context.Set<PromoProduct>()
                    .FirstOrDefault(x => x.Id == result.PromoProductId && !x.Disabled && x.TPMmode == tPMmode);

                    if (promoProduct.Promo.NeedRecountUplift == false && String.IsNullOrEmpty(result.TempId))
                    {
                        return InternalServerError(new Exception("Promo Locked Update"));
                    }

                    result.CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                    result.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                    result.UserId = user.Id;
                    result.UserName = user.Login;

                    Context.Set<PromoProductsCorrection>().Add(result);

                    //если есть копия промо в RS, то копируем новую коррекцию
                    //надо уточнить что происходит при изменении промо в Current, если в RS оно удалено (скорее всего надо пересоздавать RS)
                    var promoRS = Context.Set<Promo>()
                        .Include(x => x.PromoProducts)
                        .FirstOrDefault(x => x.Number == promoProduct.Promo.Number && x.TPMmode == TPMmode.RS && !x.Disabled);
                    // to do remove?
                    if (promoRS != null)
                    {
                        //удаляем старое промо RS и копируем заново, записываем изменения в скопированную запись
                        Context.Set<Promo>().Remove(promoRS);
                        Context.SaveChanges();

                        var promoProductsCorrections = Context.Set<PromoProductsCorrection>()
                            .Include(g => g.PromoProduct.Promo.IncrementalPromoes)
                            .Include(g => g.PromoProduct.Promo.PromoSupportPromoes)
                            .Include(g => g.PromoProduct.Promo.PromoProductTrees)
                            .Where(x => x.PromoProduct.PromoId == promoProduct.PromoId && !x.Disabled)
                            .ToList();
                        promoProductsCorrections = RSmodeHelper.EditToPromoProductsCorrectionRS(Context, promoProductsCorrections);
                        var promoProductsCorrection = promoProductsCorrections.FirstOrDefault(g => g.PromoProduct.ZREP == promoProduct.ZREP);
                        promoProductsCorrection.PlanProductUpliftPercentCorrected = model.PlanProductUpliftPercentCorrected;
                        promoProductsCorrection.ChangeDate = DateTimeOffset.Now;
                        promoProductsCorrection.UserId = user.Id;
                        promoProductsCorrection.UserName = user.Login;
                    }
                }
                if (tPMmode == TPMmode.RS)
                {
                    var promoProduct = Context.Set<PromoProduct>()
                                        .Include(x => x.Promo)
                                        .FirstOrDefault(x => x.Id == result.PromoProductId && !x.Disabled);
                    if (!CheckRSPeriodSuitable(promoProduct.Promo, Context))
                    {
                        return InternalServerError(new Exception("Promo is not in the RS period"));
                    }
                    if (promoProduct.Promo.NeedRecountUplift == false && String.IsNullOrEmpty(result.TempId))
                    {
                        return InternalServerError(new Exception("Promo Locked Update"));
                    }
                    var promoProductRS = Context.Set<PromoProduct>()
                                        .FirstOrDefault(x => x.Promo.Number == promoProduct.Promo.Number && x.ZREP == promoProduct.ZREP && !x.Disabled && x.TPMmode == TPMmode.RS);
                    //to do передавать mode при запросе промо в searchfield
                    if (promoProductRS == null)
                    {
                        var currentPromo = Context.Set<Promo>()
                            .Include(g => g.BTLPromoes)
                            .Include(g => g.PromoSupportPromoes)
                            .Include(g => g.PromoProductTrees)
                            .Include(g => g.IncrementalPromoes)
                            .Include(x => x.PromoProducts.Select(y => y.PromoProductsCorrections))
                            .FirstOrDefault(p => p.Number == model.Number && p.TPMmode == TPMmode.Current);
                        var promoRS = RSmodeHelper.EditToPromoRS(Context, currentPromo);
                        promoProductRS = promoRS.PromoProducts
                            .FirstOrDefault(x => x.ZREP == model.ZREP);
                    }
                    result.PromoProduct = promoProductRS;
                    result.PromoProductId = promoProductRS.Id;
                    result.CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                    result.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                    result.UserId = user.Id;
                    result.UserName = user.Login;
                    Context.Set<PromoProductsCorrection>().Add(result);
                }
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
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<PromoProductCorrectionPriceIncreaseView> patch)
        {
            try
            {
                var model = Context.Set<PromoProductsCorrection>()
                    .Include(g => g.PromoProduct.Promo.IncrementalPromoes)
                    .Include(g => g.PromoProduct.Promo.PromoSupportPromoes)
                    .Include(g => g.PromoProduct.Promo.PromoProductTrees)
                    .FirstOrDefault(x => x.Id == key);

                if (model == null)
                {
                    return NotFound();
                }

                var tPMmode = model.TPMmode;
                var promoStatus = model.PromoProduct.Promo.PromoStatus.SystemName;
                ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                string promoStatuses = settingsManager.GetSetting<string>("PROMO_PRODUCT_CORRECTION_PROMO_STATUS_LIST", "Draft,Deleted,Cancelled,Started,Finished,Closed");
                string[] status = promoStatuses.Split(',');

                if (tPMmode == TPMmode.RS && !CheckRSPeriodSuitable(model.PromoProduct.Promo, Context))
                {
                    return InternalServerError(new Exception("Promo is not in the RS period"));
                }
                if (status.Any(x => x == promoStatus) && !role.Equals("SupportAdministrator"))
                {
                    return InternalServerError(new Exception("Cannot be update correction where status promo = " + promoStatus));
                }
                if (model.PromoProduct.Promo.NeedRecountUplift == false)
                {
                    return InternalServerError(new Exception("Promo Locked Update"));
                }


                patch.TryGetPropertyValue("TPMmode", out object mode);

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<PromoProductsCorrection, PromoProductCorrectionView>();
                });
                var mapperPromoProductCorrection = config.CreateMapper();
                var modelMapp = mapperPromoProductCorrection.Map<PromoProductCorrectionPriceIncreaseView>(model);

                patch.Patch(modelMapp);
                var config2 = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<PromoProductCorrectionPriceIncreaseView, PromoProductsCorrection>()
                    .ForMember(pTo => pTo.Id, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProduct, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductId, opt => opt.Ignore());
                });
                var mapperPromoProductCorrection2 = config2.CreateMapper();
                mapperPromoProductCorrection2.Map<PromoProductCorrectionPriceIncreaseView, PromoProductsCorrection>(modelMapp, model);

                if ((int)mode == (int)TPMmode.Current && tPMmode == TPMmode.Current)
                {
                    var promoRS = Context.Set<Promo>()
                        .FirstOrDefault(x => x.Number == model.PromoProduct.Promo.Number && x.TPMmode == TPMmode.RS);
                    if (promoRS != null)
                    {
                        Context.Set<Promo>().Remove(promoRS);
                        Context.SaveChanges();

                        var promoProductsCorrections = Context.Set<PromoProductsCorrection>()
                            .Include(g => g.PromoProduct.Promo.IncrementalPromoes)
                            .Include(g => g.PromoProduct.Promo.PromoSupportPromoes)
                            .Include(g => g.PromoProduct.Promo.PromoProductTrees)
                            .Where(x => x.PromoProduct.PromoId == model.PromoProduct.PromoId && !x.Disabled)
                            .ToList();
                        promoProductsCorrections = RSmodeHelper.EditToPromoProductsCorrectionRS(Context, promoProductsCorrections);
                        var promoProductsCorrection = promoProductsCorrections.FirstOrDefault(g => g.PromoProduct.ZREP == model.PromoProduct.ZREP);
                        promoProductsCorrection.PlanProductUpliftPercentCorrected = model.PlanProductUpliftPercentCorrected;
                        promoProductsCorrection.ChangeDate = DateTimeOffset.Now;
                        promoProductsCorrection.UserId = user.Id;
                        promoProductsCorrection.UserName = user.Login;
                    }
                }
                else if (model.TPMmode != tPMmode)
                {
                    List<PromoProductsCorrection> promoProductsCorrections = Context.Set<PromoProductsCorrection>()
                        .Include(g => g.PromoProduct.Promo.IncrementalPromoes)
                        .Include(g => g.PromoProduct.Promo.PromoSupportPromoes)
                        .Include(g => g.PromoProduct.Promo.PromoProductTrees)
                        .Where(x => x.PromoProduct.PromoId == model.PromoProduct.PromoId && !x.Disabled)
                        .ToList();
                    promoProductsCorrections = RSmodeHelper.EditToPromoProductsCorrectionRS(Context, promoProductsCorrections);
                    model = promoProductsCorrections.FirstOrDefault(g => g.PromoProduct.Promo.Number == model.PromoProduct.Promo.Number && g.PromoProduct.ZREP == model.PromoProduct.ZREP);
                }

                model.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                model.UserId = user.Id;
                model.UserName = user.Login;

                if (model.TempId == "")
                {
                    model.TempId = null;
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
