using AutoMapper;
using Core.Data;
using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Model;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using Newtonsoft.Json;
using Persist;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Core.MarsCalendar;
using Utility;
using Module.Persist.TPM.Utils;
using Module.Persist.TPM.Model.DTO;
using Core.Settings;
using Module.Persist.TPM.PromoStateControl;
using System.Web.Http.Results;
using System.IO;
using Persist.ScriptGenerator.Filter;
using System.Net.Http.Headers;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Frontend.TPM.Util;
using System.Data.SqlClient;

namespace Module.Frontend.TPM.Controllers
{
    public class ActualLSVsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public ActualLSVsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<ActualLSV> GetConstraintedQuery()
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();

            IQueryable<ActualLSV> query = Context.Set<Promo>().Where(e => e.PromoStatus.SystemName.ToLower().IndexOf("finished") >= 0 && !e.Disabled)
                .Select(n => new ActualLSV
                {
                    Id = n.Id,
                    Number = n.Number,
                    ClientHierarchy = n.ClientHierarchy,
                    Name = n.Name,
                    BrandTech = n.BrandTech.Name,
                    Event = n.EventName,
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
                    PlanPromoPostPromoEffectLSVW1 = n.PlanPromoPostPromoEffectLSVW1,
                    ActualPromoPostPromoEffectLSVW1 = n.ActualPromoPostPromoEffectLSVW1,
                    PlanPromoPostPromoEffectLSVW2 = n.PlanPromoPostPromoEffectLSVW2,
                    ActualPromoPostPromoEffectLSVW2 = n.ActualPromoPostPromoEffectLSVW2,
                    PlanPromoPostPromoEffectLSV = n.PlanPromoPostPromoEffectLSV,
                    ActualPromoPostPromoEffectLSV = n.ActualPromoPostPromoEffectLSV
                });

            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public SingleResult<ActualLSV> GetActualLSV([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<ActualLSV> GetActualLSVs()
        {
            return GetConstraintedQuery();
        }

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<ActualLSV> patch)
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
                    CreateTaskCalculation(handlerId, model.Id);
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

                Context.SaveChanges();

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
                return InternalServerError(e);
            }
        }

        private bool EntityExists(System.Guid key)
        {
            return Context.Set<BrandTech>().Count(e => e.Id == key) > 0;
        }

        private IEnumerable<Column> GetExportSettings()
        {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 0, Field = "Number", Header = "PromoID", Quoting = false },
                new Column() { Order = 1, Field = "ClientHierarchy", Header = "Client", Quoting = false },
                new Column() { Order = 2, Field = "Name", Header = "Promo name", Quoting = false },
                new Column() { Order = 3, Field = "BrandTech", Header = "Brandtech", Quoting = false },
                new Column() { Order = 4, Field = "Event", Header = "Event", Quoting = false },
                new Column() { Order = 5, Field = "Mechanic", Header = "Mars mechanic", Quoting = false },
                new Column() { Order = 6, Field = "MechanicIA", Header = "IA mechanic", Quoting = false },
                new Column() { Order = 7, Field = "StartDate", Header = "Start date", Quoting = false },
                new Column() { Order = 8, Field = "MarsStartDate", Header = "Mars Start date", Quoting = false },
                new Column() { Order = 9, Field = "EndDate", Header = "End date", Quoting = false },
                new Column() { Order = 10, Field = "MarsEndDate", Header = "Mars End date", Quoting = false },
                new Column() { Order = 11, Field = "DispatchesStart", Header = "Dispatch start", Quoting = false },
                new Column() { Order = 12, Field = "MarsDispatchesStart", Header = "Mars Dispatch start", Quoting = false },
                new Column() { Order = 13, Field = "DispatchesEnd", Header = "Dispatch end", Quoting = false },
                new Column() { Order = 14, Field = "MarsDispatchesEnd", Header = "Mars Dispatch end", Quoting = false },
                new Column() { Order = 15, Field = "Status", Header = "Status", Quoting = false },
                new Column() { Order = 16, Field = "ActualInStoreDiscount", Header = "Actual InStore Discount", Quoting = false },
                new Column() { Order = 17, Field = "PlanPromoUpliftPercent", Header = "Plan Promo Uplift %", Quoting = false },
                new Column() { Order = 18, Field = "ActualPromoUpliftPercent", Header = "Actual Promo Uplift %", Quoting = false },
                new Column() { Order = 19, Field = "PlanPromoBaselineLSV", Header = "Plan Promo Baseline LSV", Quoting = false },
                new Column() { Order = 20, Field = "ActualPromoBaselineLSV", Header = "Actual Promo Baseline LSV", Quoting = false },
                new Column() { Order = 21, Field = "PlanPromoIncrementalLSV", Header = "Plan Promo Incremental LSV", Quoting = false },
                new Column() { Order = 22, Field = "ActualPromoIncrementalLSV", Header = "Actual Promo Incremental LSV", Quoting = false },
                new Column() { Order = 23, Field = "PlanPromoLSV", Header = "Plan Promo LSV", Quoting = false },
                new Column() { Order = 24, Field = "ActualPromoLSVByCompensation", Header = "Actual PromoLSV By Compensation", Quoting = false },
                new Column() { Order = 25, Field = "ActualPromoLSV", Header = "Actual Promo LSV", Quoting = false },
                new Column() { Order = 26, Field = "PlanPromoPostPromoEffectLSVW1", Header = "Plan Post Promo Effect W1, %", Quoting = false },
                new Column() { Order = 27, Field = "ActualPromoPostPromoEffectLSVW1", Header = "Actual Post Promo Effect W1", Quoting = false },
                new Column() { Order = 28, Field = "PlanPromoPostPromoEffectLSVW2", Header = "Plan Post Promo Effect W2, %", Quoting = false },
                new Column() { Order = 29, Field = "ActualPromoPostPromoEffectLSVW2", Header = "Actual Post Promo Effect W2, %", Quoting = false },
                new Column() { Order = 30, Field = "PlanPromoPostPromoEffectLSV", Header = "Plan Post Promo Effect LSV total", Quoting = false },
                new Column() { Order = 31, Field = "ActualPromoPostPromoEffectLSV", Header = "Actual Promo Effect LSV total", Quoting = false },
            };
            return columns;
        }
        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<ActualLSV> options)
        {
            try
            {
                IQueryable results = options.ApplyTo(GetConstraintedQuery());
                IEnumerable<Column> columns = GetExportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                UserInfo user = authorizationManager.GetCurrentUser();
                string username = user == null ? "" : user.Login;
                string filePath = exporter.GetExportFileName("ActualLSV", username);
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

                CreateImportTask(fileName, "XLSXImportActualLsvHandler");

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
                    CreateDate = DateTimeOffset.Now,
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

        /// <summary>
        /// Создать задачу на пересчет распределения BaseLine и ActualLSV, а также фактических параметров
        /// </summary>
        /// <param name="handlerId">ID обработчика</param>
        /// <param name="promoId">ID промо</param>
        private void CreateTaskCalculation(Guid handlerId, Guid promoId)
        {        
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

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
                CreateDate = DateTimeOffset.Now,
                LastExecutionDate = null,
                NextExecutionDate = null,
                ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                UserId = userId,
                RoleId = roleId
            };

            handler.SetParameterData(data);
            Context.LoopHandlers.Add(handler);
            Context.SaveChanges();
        }
    }    
}
