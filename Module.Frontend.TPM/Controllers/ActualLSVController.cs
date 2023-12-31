﻿using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
using Utility;

namespace Module.Frontend.TPM.Controllers
{
    public class ActualLSVsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;
        private readonly UserInfo user;
        private readonly RoleInfo roleInfo;
        private readonly string role;
        private readonly Guid? roleId;

        public ActualLSVsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
            user = authorizationManager.GetCurrentUser();
            roleInfo = authorizationManager.GetCurrentRole();
            role = roleInfo.SystemName;
            roleId = roleInfo.Id;
        }

        public ActualLSVsController(UserInfo User, string Role, Guid RoleId)
        {
            user = User;
            role = Role;
            roleId = RoleId;
        }

        public IQueryable<ActualLSV> GetConstraintedQuery(DatabaseContext context = null)
        {
            if (context == null)
            {
                context = Context;
            }
            IList<Constraint> constraints = user.Id.HasValue ? context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();

            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<ClientTreeHierarchyView> hierarchy = context.Set<ClientTreeHierarchyView>().AsNoTracking();
            IQueryable<Promo> promoes = context.Set<Promo>().Where(e => e.PromoStatus.SystemName.ToLower().IndexOf("finished") >= 0 && !e.Disabled);

            promoes = ModuleApplyFilterHelper.ApplyFilter(promoes, hierarchy, filters);
            IQueryable<ActualLSV> query = promoes
                .Select(n => new ActualLSV
                {
                    Id = n.Id,
                    Number = n.Number,
                    ClientHierarchy = n.ClientHierarchy,
                    Name = n.Name,
                    BrandTech = n.BrandTech.BrandsegTechsub,
                    Event = n.Event.Name,
                    Mechanic = n.Mechanic,
                    MechanicIA = n.MechanicIA,
                    StartDate = n.StartDate,
                    MarsStartDate = n.MarsStartDate,
                    EndDate = n.EndDate,
                    MarsEndDate = n.MarsEndDate,
                    DispatchesStart = n.DispatchesStart,
                    MarsDispatchesStart = n.MarsDispatchesStart,
                    DispatchesEnd = n.DispatchesEnd,
                    MarsDispatchesEnd = n.MarsDispatchesEnd,
                    Status = n.PromoStatus.Name,
                    ActualInStoreDiscount = n.ActualInStoreDiscount,
                    PlanPromoUpliftPercent = n.PlanPromoUpliftPercent,
                    ActualPromoUpliftPercent = n.ActualPromoUpliftPercent,
                    PlanPromoBaselineLSV = n.PlanPromoBaselineLSV,
                    ActualPromoBaselineLSV = n.ActualPromoBaselineLSV,
                    PlanPromoIncrementalLSV = n.PlanPromoIncrementalLSV,
                    ActualPromoIncrementalLSV = n.ActualPromoIncrementalLSV,
                    PlanPromoLSV = n.PlanPromoLSV,
                    ActualPromoLSVByCompensation = n.ActualPromoLSVByCompensation,
                    ActualPromoLSV = n.ActualPromoLSV,
                    ActualPromoLSVSI = n.ActualPromoLSVSI,
                    ActualPromoLSVSO = n.ActualPromoLSVSO,
                    PlanPromoPostPromoEffectLSVW1 = n.PlanPromoPostPromoEffectLSVW1,
                    ActualPromoPostPromoEffectLSVW1 = n.ActualPromoPostPromoEffectLSVW1,
                    PlanPromoPostPromoEffectLSVW2 = n.PlanPromoPostPromoEffectLSVW2,
                    ActualPromoPostPromoEffectLSVW2 = n.ActualPromoPostPromoEffectLSVW2,
                    PlanPromoPostPromoEffectLSV = n.PlanPromoPostPromoEffectLSV,
                    ActualPromoPostPromoEffectLSV = n.ActualPromoPostPromoEffectLSV,
                    ProductSubrangesList = n.ProductSubrangesList,
                    InOut = n.InOut,
                    IsOnInvoice = n.IsOnInvoice
                });

            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public SingleResult<ActualLSV> GetActualLSV([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetActualLSVs());
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<ActualLSV> GetActualLSVs(ODataQueryOptions<ActualLSV> queryOptions = null)
        {
            var query = GetConstraintedQuery();

            return query;
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<ActualLSV> GetFilteredData(ODataQueryOptions<ActualLSV> options)
        {
            var query = GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<ActualLSV>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<ActualLSV>;
        }

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] System.Guid key, Delta<ActualLSV> patch)
        {
            try
            {
                var model = Context.Set<Promo>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                Type promoType = model.GetType();
                IEnumerable<string> changedProperties = patch.GetChangedPropertyNames();
                foreach (string p in changedProperties)
                {
                    object value;

                    patch.TryGetPropertyValue(p, out value);
                    promoType.GetProperty(p).SetValue(model, value);
                }

                // ID обработчика, выполняющего расчеты
                Guid handlerId = Guid.NewGuid();

                // Если промо заблокировано, то нельзя ничего менять
                if (CalculationTaskManager.BlockPromo(model.Id, handlerId))
                    await CreateTaskCalculation(handlerId, model.Id);
                else
                    return InternalServerError(new Exception("Promo was blocked for calculation"));

                //// Если Demand ввел новое ActualPromoBaselineLSV.
                //if (changedProperties.Any(x => x == "ActualPromoBaselineLSV"))
                //{
                //    CalculateAllActualProductBaselineLSV(model);
                //}

                //// Если Demand ввел новое ActualPromoLSV.
                //if (changedProperties.Any(x => x == "ActualPromoLSV"))
                //{
                //    CalculateAllActualLSV(model);
                //}

                await Context.SaveChangesAsync();

                return Updated(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityExists(key))
                    return NotFound();
                else
                    throw;
            }
            catch (Exception e)
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }

        private bool EntityExists(System.Guid key)
        {
            return Context.Set<BrandTech>().Count(e => e.Id == key) > 0;
        }

        public static IEnumerable<Column> GetExportSettings()
        {
            int order = 0;
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = order++, Field = "Number", Header = "PromoID", Quoting = false },
                new Column() { Order = order++, Field = "ClientHierarchy", Header = "Client", Quoting = false },
                new Column() { Order = order++, Field = "Name", Header = "Promo name", Quoting = false },
                new Column() { Order = order++, Field = "InOut", Header = "In Out", Quoting = false },
                new Column() { Order = order++, Field = "BrandTech", Header = "Brandtech", Quoting = false },
                new Column() { Order = order++, Field = "ProductSubrangesList", Header = "Subrange", Quoting = false },
                new Column() { Order = order++, Field = "Event", Header = "Event", Quoting = false },
                new Column() { Order = order++, Field = "Mechanic", Header = "Mars mechanic", Quoting = false },
                new Column() { Order = order++, Field = "MechanicIA", Header = "IA mechanic", Quoting = false },
                new Column() { Order = order++, Field = "StartDate", Header = "Start date", Quoting = false },
                new Column() { Order = order++, Field = "MarsStartDate", Header = "Mars Start date", Quoting = false },
                new Column() { Order = order++, Field = "EndDate", Header = "End date", Quoting = false },
                new Column() { Order = order++, Field = "MarsEndDate", Header = "Mars End date", Quoting = false },
                new Column() { Order = order++, Field = "DispatchesStart", Header = "Dispatch start", Quoting = false },
                new Column() { Order = order++, Field = "MarsDispatchesStart", Header = "Mars Dispatch start", Quoting = false },
                new Column() { Order = order++, Field = "DispatchesEnd", Header = "Dispatch end", Quoting = false },
                new Column() { Order = order++, Field = "MarsDispatchesEnd", Header = "Mars Dispatch end", Quoting = false },
                new Column() { Order = order++, Field = "Status", Header = "Status", Quoting = false },
                new Column() { Order = order++, Field = "ActualInStoreDiscount", Header = "Actual InStore Discount", Quoting = false },
                new Column() { Order = order++, Field = "PlanPromoUpliftPercent", Header = "Plan Promo Uplift %", Quoting = false },
                new Column() { Order = order++, Field = "ActualPromoUpliftPercent", Header = "Actual Promo Uplift %", Quoting = false },
                new Column() { Order = order++, Field = "PlanPromoBaselineLSV", Header = "Plan Promo Baseline LSV", Quoting = false },
                new Column() { Order = order++, Field = "ActualPromoBaselineLSV", Header = "Actual Promo Baseline LSV", Quoting = false },
                new Column() { Order = order++, Field = "PlanPromoIncrementalLSV", Header = "Plan Promo Incremental LSV", Quoting = false },
                new Column() { Order = order++, Field = "ActualPromoIncrementalLSV", Header = "Actual Promo Incremental LSV", Quoting = false },
                new Column() { Order = order++, Field = "PlanPromoLSV", Header = "Plan Promo LSV", Quoting = false },
                new Column() { Order = order++, Field = "ActualPromoLSVByCompensation", Header = "Actual PromoLSV By Compensation", Quoting = false },
                new Column() { Order = order++, Field = "ActualPromoLSV", Header = "Actual Promo LSV", Quoting = false },
                new Column() { Order = order++, Field = "ActualPromoLSVSI", Header = "Actual Promo LSV SI", Quoting = false },
                new Column() { Order = order++, Field = "ActualPromoLSVSO", Header = "Actual Promo LSV SO", Quoting = false },
                new Column() { Order = order++, Field = "PlanPromoPostPromoEffectLSVW1", Header = "Plan Post Promo Effect LSV W1", Quoting = false },
                new Column() { Order = order++, Field = "ActualPromoPostPromoEffectLSVW1", Header = "Actual Post Promo Effect LSV W1", Quoting = false },
                new Column() { Order = order++, Field = "PlanPromoPostPromoEffectLSVW2", Header = "Plan Post Promo Effect LSV W2", Quoting = false },
                new Column() { Order = order++, Field = "ActualPromoPostPromoEffectLSVW2", Header = "Actual Post Promo Effect LSV W2", Quoting = false },
                new Column() { Order = order++, Field = "PlanPromoPostPromoEffectLSV", Header = "Plan Post Promo Effect LSV total", Quoting = false },
                new Column() { Order = order++, Field = "ActualPromoPostPromoEffectLSV", Header = "Actual Promo Effect LSV total", Quoting = false },
            };
            return columns;
        }
        [ClaimsAuthorize]
        public async Task<IHttpActionResult> ExportXLSX(ODataQueryOptions<ActualLSV> options)
        {
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            Guid roleId = roleInfo == null ? Guid.Empty : (roleInfo.Id.HasValue ? roleInfo.Id.Value : Guid.Empty);
            var url = HttpContext.Current.Request.Url.AbsoluteUri;

            HandlerData data = new HandlerData();
            string handlerName = "ExportHandler";

            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TModel", typeof(ActualLSV), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(ActualLSVsController), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(ActualLSVsController.GetExportSettings), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("URL", url, data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = $"Export {nameof(ActualLSV)} dictionary",
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

                await CreateImportTask(fileName, "XLSXImportActualLsvHandler");

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

        private async Task CreateImportTask(string fileName, string importHandler)
        {
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            Guid roleId = roleInfo == null ? Guid.Empty : (roleInfo.Id.HasValue ? roleInfo.Id.Value : Guid.Empty);


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
            HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportActualLsv), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportActualLsv).Name, data, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(ActualLSV), data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = "Загрузка импорта из файла " + typeof(ImportActualLsv).Name,
                Name = "Module.Host.TPM.Handlers." + importHandler,
                ExecutionPeriod = null,
                CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                RunGroup = typeof(ImportActualLsv).Name,
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

        /// <summary>
        /// Создать задачу на пересчет распределения BaseLine и ActualLSV, а также фактических параметров
        /// </summary>
        /// <param name="handlerId">ID обработчика</param>
        /// <param name="promoId">ID промо</param>
        private async Task CreateTaskCalculation(Guid handlerId, Guid promoId)
        {
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            Guid roleId = roleInfo == null ? Guid.Empty : (roleInfo.Id.HasValue ? roleInfo.Id.Value : Guid.Empty);

            HandlerData data = new HandlerData();
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("PromoIds", new Guid[] { promoId }, data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = handlerId,
                ConfigurationName = "PROCESSING",
                Description = "Calculate Actuals after change ActualLSV",
                Name = "Module.Host.TPM.Handlers.ActualLSVChangeHandler",
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
        }
    }
}
