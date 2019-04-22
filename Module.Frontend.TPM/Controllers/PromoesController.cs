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
using Core.Dependency;
using System.Data.Entity;
using Module.Persist.TPM.PromoStateControl;
using System.Web.Http.Results;
using System.Collections.Concurrent;
using System.IO;
using Persist.ScriptGenerator.Filter;
using System.Net.Http.Headers;
using Module.Persist.TPM.CalculatePromoParametersModule;

namespace Module.Frontend.TPM.Controllers {
    public class PromoesController : EFContextController {
        
        private readonly IAuthorizationManager authorizationManager;

        public PromoesController(IAuthorizationManager authorizationManager) {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<Promo> GetConstraintedQuery(bool canChangeStateOnly = false) {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<Promo> query = Context.Set<Promo>().Where(e => !e.Disabled);
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters, FilterQueryModes.Active, canChangeStateOnly ? role : String.Empty);

            // Не администраторы не смотрят чужие черновики
            if (role != "Administrator") {
                query = query.Where(e => e.PromoStatus.SystemName != "Draft" || e.CreatorId == user.Id);
            }
            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public SingleResult<Promo> GetPromo([FromODataUri] Guid key) {
            return SingleResult.Create(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<Promo> GetPromoes(bool canChangeStateOnly = false) {
            return GetConstraintedQuery(canChangeStateOnly);
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<Promo> GetCanChangeStatePromoes(bool canChangeStateOnly = false)
        {
            return GetConstraintedQuery(canChangeStateOnly);
        }

        [ClaimsAuthorize]
        public IHttpActionResult Put([FromODataUri] Guid key, Delta<Promo> patch) {
            var model = Context.Set<Promo>().Find(key);
            if (model == null) {
                return NotFound();
            }
            patch.Put(model);
            try {
                //Установка полей по дереву ProductTree
                SetPromoByProductTree(model);
                //Установка дат в Mars формате
                SetPromoMarsDates(model);
                //Установка полей по дереву ClientTree
                SetPromoByClientTree(model);
                Context.SaveChanges();
            } catch (DbUpdateConcurrencyException) {
                if (!EntityExists(key)) {
                    return NotFound();
                } else {
                    throw;
                }
            }
            return Updated(model);
        }

        [ClaimsAuthorize]
        public IHttpActionResult Post(Promo model) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                if (model.EventId == null) {
                    Event promoEvent = Context.Set<Event>().FirstOrDefault(x => !x.Disabled && x.Name == "Standard promo");
                    if (promoEvent == null) {
                        return InternalServerError(new Exception("Event 'Standard promo' not found"));
                    }

                    model.EventId = promoEvent.Id;
                }

                UserInfo user = authorizationManager.GetCurrentUser();
                string userRole = user.GetCurrentRole().SystemName;

                string massage;

                PromoStateContext promoStateContext = new PromoStateContext(Context, null);
                bool status = promoStateContext.ChangeState(model, userRole, out massage);

                if (!status) {
                    return InternalServerError(new Exception(massage));
                }

                Promo proxy = Context.Set<Promo>().Create<Promo>();
                Promo result = (Promo) Mapper.Map(model, proxy, typeof(Promo), proxy.GetType(), opts => opts.CreateMissingTypeMaps = true);

                if (result.CreatorId == null) {
                    result.CreatorId = user.Id;
                }

                Context.Set<Promo>().Add(result);
                // Добавление продуктов
                AddProductTrees(model.ProductTreeObjectIds, result);
                Context.SaveChanges();
                //Установка полей по дереву ProductTree
                SetPromoByProductTree(result);
                //Установка дат в Mars формате
                SetPromoMarsDates(result);
                //Установка полей по дереву ClientTree
                SetPromoByClientTree(result);
                //Установка механик
                SetMechanic(result);
                SetMechanicIA(result);

                //Установка начального статуса
                PromoStatusChange psc = Context.Set<PromoStatusChange>().Create<PromoStatusChange>();
                psc.PromoId = result.Id;
                psc.StatusId = result.PromoStatusId;
                psc.UserId = (Guid) user.Id;
                psc.RoleId = (Guid) user.GetCurrentRole().Id;
                psc.Date = DateTimeOffset.UtcNow;
                Context.Set<PromoStatusChange>().Add(psc);

                //Установка времени последнгего присвоения статуса Approved
                if (result.PromoStatus != null && result.PromoStatus.SystemName == "Approved") {
                    result.LastApprovedDate = DateTimeOffset.UtcNow;
                }                

                WritePromoDemandChangeIncident(result);
                
                // для draft не проверяем и не считаем
                if (result.PromoStatus.SystemName.ToLower() != "draft")
                {
                    // если нет TI, COGS или продукты не подобраны по фильтрам, запретить сохранение (будет исключение)
                    CheckSupportInfo(result);
                    //создание отложенной задачи, выполняющей подбор аплифта и расчет параметров
                    CalculatePromo(result, false);
                }

                Context.SaveChanges();

                return Created(result);
            } catch (Exception e) {
                return InternalServerError(e);
            }

        }

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] Guid key, Delta<Promo> patch) {
            try
            {
                var model = Context.Set<Promo>().Find(key);

                if (model == null)
                {
                    return NotFound();
                }

                Promo promoCopy = new Promo(model);
                patch.Patch(model);

                // Добавление продуктов                
                AddProductTrees(model.ProductTreeObjectIds, model);
                Context.SaveChanges();

                UserInfo user = authorizationManager.GetCurrentUser();
                string userRole = user.GetCurrentRole().SystemName;

                string massage;

                PromoStateContext promoStateContext = new PromoStateContext(Context, promoCopy);
                bool status = promoStateContext.ChangeState(model, userRole, out massage);

                if (!status)
                {
                    return InternalServerError(new Exception(massage));
                }

                //Установка полей по дереву ProductTree
                SetPromoByProductTree(model);
                //Установка дат в Mars формате
                SetPromoMarsDates(model);
                //Установка полей по дереву ClientTree
                SetPromoByClientTree(model);
                //Установка механик
                SetMechanic(model);
                SetMechanicIA(model);

                var statusId = model.PromoStatusId;

                //Сохранение изменения статуса
                if (promoCopy.PromoStatusId != model.PromoStatusId)
                {
                    PromoStatusChange psc = Context.Set<PromoStatusChange>().Create<PromoStatusChange>();
                    psc.PromoId = model.Id;
                    psc.StatusId = model.PromoStatusId;
                    psc.UserId = (Guid)user.Id;
                    psc.RoleId = (Guid)user.GetCurrentRole().Id;
                    psc.Date = DateTimeOffset.UtcNow;
                    Context.Set<PromoStatusChange>().Add(psc);

                    //Установка времени последнгего присвоения статуса Approved
                    if (model.PromoStatus != null && model.PromoStatus.SystemName == "Approved")
                    {
                        model.LastApprovedDate = DateTimeOffset.UtcNow;
                    }
                }

                WritePromoDemandChangeIncident(model, patch, promoCopy);

                // для draft не проверяем и не считаем
                if (model.PromoStatus.SystemName.ToLower() != "draft")
                {
                    // если нет TI, COGS или продукты не подобраны по фильтрам, запретить сохранение (будет исключение)
                    CheckSupportInfo(model);

                    //создание отложенной задачи, выполняющей переподбор аплифта и перерассчет параметров                    
                    if (NeedRecalculatePromo(model, promoCopy))
                    {
                        // если меняем длительность промо, то пересчитываем Marketing TI
                        bool needCalculatePlanMarketingTI = promoCopy.StartDate != model.StartDate || promoCopy.EndDate != model.EndDate;
                        CalculatePromo(model, needCalculatePlanMarketingTI);
                    }
                }

                Context.SaveChanges();

                return Updated(model);
            } catch (DbUpdateConcurrencyException) {
                if (!EntityExists(key)) {
                    return NotFound();
                } else {
                    throw;
                }
            } catch (Exception ex) {
                return InternalServerError(ex);
            }
        }

        [ClaimsAuthorize]
        public IHttpActionResult Delete([FromODataUri] Guid key) {
            try {
                var model = Context.Set<Promo>().Find(key);
                if (model == null) {
                    return NotFound();
                }

                Promo promoCopy = new Promo(model);

                model.DeletedDate = DateTime.Now;
                model.Disabled = true;
                model.PromoStatusId = Context.Set<PromoStatus>().FirstOrDefault(e => e.SystemName == "Deleted").Id;

                UserInfo user = authorizationManager.GetCurrentUser();
                string userRole = user.GetCurrentRole().SystemName;

                string massage;

                PromoStateContext promoStateContext = new PromoStateContext(Context, promoCopy);
                bool status = promoStateContext.ChangeState(model, userRole, out massage);

                if (!status) {
                    return InternalServerError(new Exception(massage));
                }

                Context.SaveChanges();
                WritePromoDemandChangeIncident(model, true);
                return StatusCode(HttpStatusCode.NoContent);
            } catch (Exception e) {
                return InternalServerError(e);
            }
        }

        /// <summary>
        /// Смена статуса промо
        /// </summary>
        /// <param name="id">Id Промо</param>
        /// <param name="promoNewStatusId">Id статуса</param>
        /// <returns></returns>
        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult ChangeStatus(Guid id, Guid promoNewStatusId) {
            // При запросе минуя Odata транзакция не ведется
            using (var transaction = Context.Database.BeginTransaction()) {
                try {
                    Delta<Promo> patch = new Delta<Promo>((new Promo()).GetType(), new string[] { "PromoStatusId" });
                    patch.TrySetPropertyValue("PromoStatusId", promoNewStatusId);

                    // если возвращается Update, то всё прошло без ошибок
                    var result = Patch(id, patch);
                    if (result is System.Web.Http.OData.Results.UpdatedODataResult<Promo>) {
                        transaction.Commit();
                        return Json(new { success = true });
                    } else {
                        ExceptionResult exc = result as ExceptionResult;
                        if (exc != null)
                            throw exc.Exception;
                        else
                            throw new Exception("Unknown Error");
                    }

                } catch (Exception e) {
                    transaction.Rollback();
                    return InternalServerError(e);
                }
            }
        }

        /// <summary>
        /// Отклонение промо
        /// </summary>
        /// <param name="rejectPromoId">Id Промо</param>
        /// <param name="rejectReasonId">Id Причины</param>
        /// <param name="rejectComment">Комментарий</param>
        /// <returns></returns>
        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult DeclinePromo([FromODataUri] Guid rejectPromoId, [FromODataUri] Guid rejectReasonId, [FromODataUri] string rejectComment) {
            // При запросе минуя Odata транзакция не ведется
            using (var transaction = Context.Database.BeginTransaction()) {
                try {
                    Promo promo = Context.Set<Promo>().Find(rejectPromoId);
                    RejectReason rejectreason = Context.Set<RejectReason>().Find(rejectReasonId);

                    if (promo == null || rejectreason == null) {
                        return NotFound();
                    }

                    PromoStatus draftPublishedStatus = Context.Set<PromoStatus>().First(n => n.SystemName == "DraftPublished");
                    Delta<Promo> patch = new Delta<Promo>(promo.GetType(), new string[] { "PromoStatusId", "RejectReasonId" });
                    patch.TrySetPropertyValue("PromoStatusId", draftPublishedStatus.Id);
                    patch.TrySetPropertyValue("RejectReasonId", rejectReasonId);

                    // если возвращается Update, то всё прошло без ошибок
                    var result = Patch(promo.Id, patch);
                    if (result is System.Web.Http.OData.Results.UpdatedODataResult<Promo>) {
                        UserInfo user = authorizationManager.GetCurrentUser();
                        // Ищем записанное изменение статуса и добавляем комментарий
                        PromoStatusChange psc = Context.Set<PromoStatusChange>().Where(n => n.PromoId == promo.Id && n.UserId == user.Id)
                            .OrderByDescending(n => n.Date).First();

                        psc.RejectReasonId = rejectreason.Id;
                        psc.Comment = rejectreason.SystemName == "Other" ? rejectComment : null;

                        Context.SaveChanges();
                        transaction.Commit();

                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, data = promo }));
                    } else {
                        ExceptionResult exc = result as ExceptionResult;
                        if (exc != null)
                            throw exc.Exception;
                        else
                            throw new Exception("Unknown Error");
                    }
                } catch (Exception e) {
                    transaction.Rollback();
                    return InternalServerError(e);
                }
            }
        }

        private FilterContainer GetFilter(Delta<Promo> patch, string fieldName) {
            object fieldValue;
            if (patch.TryGetPropertyValue(fieldName, out fieldValue) && fieldValue != null) {
                FilterContainer result = JsonConvert.DeserializeObject<FilterContainer>((string) fieldValue);
                return result;
            } else {
                return null;
            }
        }

        private FilterContainer GetFilter(string fieldValue) {
            if (fieldValue != null) {
                FilterContainer result = JsonConvert.DeserializeObject<FilterContainer>(fieldValue);
                return result;
            } else {
                return null;
            }
        }

        private bool EntityExists(Guid key) {
            return Context.Set<Promo>().Count(e => e.Id == key) > 0;
        }

        private IEnumerable<Column> GetExportSettings() {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 0, Field = "Number", Header = "Promo ID", Quoting = false },
                new Column() { Order = 0, Field = "Name", Header = "Promo name", Quoting = false },
                new Column() { Order = 0, Field = "PromoStatus.Name", Header = "Status", Quoting = false },
                new Column() { Order = 0, Field = "MarsMechanic.Name", Header = "Mars mechanic", Quoting = false },
                new Column() { Order = 0, Field = "MarsMechanicType.Name", Header = "Mars mechanic type", Quoting = false },
                new Column() { Order = 0, Field = "MarsMechanicDiscount", Header = "Mars mechanic discount, %", Quoting = false },
                new Column() { Order = 0, Field = "InstoreMechanic.Name", Header = "IA mechanic", Quoting = false },
                new Column() { Order = 0, Field = "InstoreMechanicType.Name", Header = "IA mechanic type", Quoting = false },
                new Column() { Order = 0, Field = "InstoreMechanicDiscount", Header = "IA mechanic discount, %", Quoting = false },
                new Column() { Order = 0, Field = "Brand.Name", Header = "Brand", Quoting = false },
                new Column() { Order = 0, Field = "BrandTech.Name", Header = "Brandtech", Quoting = false },
                new Column() { Order = 0, Field = "StartDate", Header = "Start date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "EndDate", Header = "End date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "DispatchesStart", Header = "Dispatch start", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "DispatchesEnd", Header = "Dispatch end", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 0, Field = "EventName", Header = "Event", Quoting = false },
                new Column() { Order = 0, Field = "Priority", Header = "Priority", Quoting = false },
                new Column() { Order = 0, Field = "ClientHierarchy", Header = "Client hierarchy", Quoting = false },
                new Column() { Order = 0, Field = "ProductHierarchy", Header = "Product hierarchy", Quoting = false },
                new Column() { Order = 0, Field = "ShopperTi", Header = "Shopper TI, RUR", Quoting = false },
                new Column() { Order = 0, Field = "MarketingTi", Header = "Marketing TI, RUR", Quoting = false },
                new Column() { Order = 0, Field = "Branding", Header = "Branding, RUR", Quoting = false },
                new Column() { Order = 0, Field = "BTL", Header = "BTL, RUR", Quoting = false },
                new Column() { Order = 0, Field = "CostProduction", Header = "Cost production, RUR", Quoting = false },
                new Column() { Order = 0, Field = "TotalCost", Header = "Total cost, RUR", Quoting = false },
                new Column() { Order = 0, Field = "PlanUplift", Header = "Uplift, %", Quoting = false },
                new Column() { Order = 0, Field = "PlanIncrementalLsv", Header = "Incremental LSV, RUR", Quoting = false },
                new Column() { Order = 0, Field = "PlanTotalPromoLsv", Header = "Total promo LSV, RUR", Quoting = false },
                new Column() { Order = 0, Field = "PlanPostPromoEffect", Header = "Post promo effect, RUR", Quoting = false },
                new Column() { Order = 0, Field = "PlanRoi", Header = "ROI, %", Quoting = false },
                new Column() { Order = 0, Field = "PlanIncrementalNsv", Header = "Incremental NSV, RUR", Quoting = false },
                new Column() { Order = 0, Field = "PlanTotalPromoNsv", Header = "Total promo NSV, RUR", Quoting = false },
                new Column() { Order = 0, Field = "PlanIncrementalMac", Header = "Incremental Mac, RUR", Quoting = false },
                new Column() { Order = 0, Field = "FactShopperTi", Header = "Actual Shopper TI, RUR", Quoting = false },
                new Column() { Order = 0, Field = "FactMarketingTi", Header = "Actual Marketing TI, RUR", Quoting = false },
                new Column() { Order = 0, Field = "FactBranding", Header = "Actual Branding, RUR", Quoting = false },
                new Column() { Order = 0, Field = "FactBTL", Header = "Actual BTL, RUR", Quoting = false },
                new Column() { Order = 0, Field = "FactCostProduction", Header = "Actual Cost production, RUR", Quoting = false },
                new Column() { Order = 0, Field = "FactTotalCost", Header = "Actual Total cost, RUR", Quoting = false },
                new Column() { Order = 0, Field = "FactUplift", Header = "Fact Uplift, %", Quoting = false },
                new Column() { Order = 0, Field = "FactIncrementalLsv", Header = "Fact Incremental LSV, RUR", Quoting = false },
                new Column() { Order = 0, Field = "FactTotalPromoLsv", Header = "Fact Total promo LSV, RUR", Quoting = false },
                new Column() { Order = 0, Field = "FactPostPromoEffect", Header = "Fact Post promo effect, RUR", Quoting = false },
                new Column() { Order = 0, Field = "FactRoi", Header = "Fact ROI, %", Quoting = false },
                new Column() { Order = 0, Field = "FactIncrementalNsv", Header = "Fact Incremental NSV, RUR", Quoting = false },
                new Column() { Order = 0, Field = "FactTotalPromoNsv", Header = "Fact Total promo NSV, RUR", Quoting = false },
                new Column() { Order = 0, Field = "FactIncrementalMac", Header = "Fact Incremental Mac, RUR", Quoting = false },
                new Column() { Order = 0, Field = "Color.DisplayName", Header = "Color name", Quoting = false }
            };
            return columns;
        }

        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<Promo> options) {
            try {
                IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));
                IEnumerable<Column> columns = GetExportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                UserInfo user = authorizationManager.GetCurrentUser();
                string username = user == null ? "" : user.Login;
                string filePath = exporter.GetExportFileName("Promo", username);
                exporter.Export(results, filePath);
                string filename = System.IO.Path.GetFileName(filePath);
                return Content<string>(HttpStatusCode.OK, filename);
            } catch (Exception e) {
                return Content<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Экспорт календаря в эксель
        /// </summary>
        /// <param name="options"></param>
        /// <param name="data">clients - список id клиентов соответствующих фильтру на клиенте, year - год</param>
        /// <returns></returns>
        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult ExportSchedule(ODataQueryOptions<Promo> options, ODataActionParameters data) {
            try {
                // TODO: Передавать фильтр в параметры задачи
                //var tsts = options.RawValues.Filter;
                //var tsts = JsonConvert.SerializeObject(options, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                UserInfo user = authorizationManager.GetCurrentUser();
                Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
                RoleInfo role = authorizationManager.GetCurrentRole();
                Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

                IEnumerable<int> clients = (IEnumerable<int>) data["clients"];

                HandlerData handlerData = new HandlerData();
                HandlerDataHelper.SaveIncomingArgument("UserId", userId, handlerData, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, handlerData, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UserId", userId, handlerData, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("clients", clients.ToList(), handlerData, visible: false, throwIfNotExists: false);

                //IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));
                //List<Promo> promoes = CastQueryToPromo(results);
                if (data.Count() > 1) {
                    HandlerDataHelper.SaveIncomingArgument("year", (int) data["year"], handlerData, visible: false, throwIfNotExists: false);
                }
                using (DatabaseContext context = new DatabaseContext()) {
                    LoopHandler handler = new LoopHandler() {
                        Id = Guid.NewGuid(),
                        ConfigurationName = "PROCESSING",
                        Description = "Scheduler Export",
                        Name = "Module.Host.TPM.Handlers.SchedulerExportHandler",
                        ExecutionPeriod = null,
                        CreateDate = DateTimeOffset.Now,
                        LastExecutionDate = null,
                        NextExecutionDate = null,
                        ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                        UserId = userId,
                        RoleId = roleId
                    };
                    handler.SetParameterData(handlerData);
                    context.LoopHandlers.Add(handler);
                    context.SaveChanges();
                }
                return Content<string>(HttpStatusCode.OK, "Export task successfully created");
            } catch (Exception e) {
                return Content<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private string GetUserName(string userName) {
            string[] userParts = userName.Split(new char[] { '/', '\\' });
            return userParts[userParts.Length - 1];
        }

        /// <summary>
        /// Преобразование записей в модель Promo
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        private List<Promo> CastQueryToPromo(IQueryable records) {
            List<Promo> castedPromoes = new List<Promo>();
            Promo proxy = Context.Set<Promo>().Create<Promo>();
            foreach (var item in records) {
                if (item is IEntity<Guid>) {
                    Promo result = (Promo) Mapper.Map(item, proxy, typeof(Promo), proxy.GetType(), opts => opts.CreateMissingTypeMaps = true);
                    castedPromoes.Add(result);
                } else if (item is ISelectExpandWrapper) {
                    var property = item.GetType().GetProperty("Instance");
                    var instance = property.GetValue(item);
                    Promo val = null;
                    if (instance is Promo) {
                        val = (Promo) instance;
                        castedPromoes.Add(val);
                    }
                }
            }
            return castedPromoes;
        }

        [ClaimsAuthorize]
        public async Task<HttpResponseMessage> FullImportXLSX() {
            try {
                if (!Request.Content.IsMimeMultipartContent()) {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string importDir = Core.Settings.AppSettingsManager.GetSetting("IMPORT_DIRECTORY", "ImportFiles");
                string fileName = await FileUtility.UploadFile(Request, importDir);

                CreateImportTask(fileName, "FullXLSXUpdateImportHandler");

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StringContent("success = true");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

                return result;
            } catch (Exception e) {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Создание отложенной задачи, выполняющей подбор аплифта и расчет параметров промо и продуктов
        /// </summary>
        /// <param name="promo"></param>
        private void CalculatePromo(Promo promo, bool needCalculatePlanMarketingTI) {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            HandlerData data = new HandlerData();
            HandlerDataHelper.SaveIncomingArgument("PromoId", promo.Id, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("NeedCalculatePlanMarketingTI", needCalculatePlanMarketingTI, data, visible: false, throwIfNotExists: false);

            bool success = CalculationTaskManager.CreateCalculationTask(CalculationTaskManager.CalculationAction.Uplift, data, Context, promo.Id);

            if (!success)
                throw new Exception("Операция не возможна, данное промо участвует в расчетах и было заблокировано");
        }

        /// <summary>
        /// Чтение лога задачи 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ClaimsAuthorize]
        public IHttpActionResult ReadPromoCalculatingLog(String promoId)
        {
            Guid promoGuid = Guid.Parse(promoId);
            String respond = null;
            int codeTo = 0;
            String opDataTo = null;
            try
            {
                Guid? handlerGuid = null;
                //Guid? handlerGuid = GetCalculatedPromo(promoGuid);
                BlockedPromo bp = Context.Set<BlockedPromo>().FirstOrDefault(n => n.PromoId == promoGuid && !n.Disabled);

                if (bp != null)
                {
                    //Чтение файла лога
                    handlerGuid = bp.HandlerId;

                    string logDir = AppSettingsManager.GetSetting("HANDLER_LOG_DIRECTORY", "HandlerLogs");
                    string logFileName = String.Format("{0}.txt", handlerGuid);
                    string filePath = System.IO.Path.Combine(logDir, logFileName);
                    if (File.Exists(filePath))
                    {
                        respond = File.ReadAllText(filePath);
                    }
                    else
                    {
                        respond = "";
                    }
                }
                else
                    codeTo = 1;

                //Чтение файла лога
                //if (handlerGuid.HasValue)
                //{
                    
                //}

                // Получение параметров хендлера
                /*using (DatabaseContext context = new DatabaseContext())
                {
                    LoopHandler task = context.Set<LoopHandler>().Where(x => x.Id == handlerGuid).FirstOrDefault();
                    if (task == null)
                    {                        
                        codeTo = 1;
                    }
                    else
                    {
                        //if (b)
                        //if (task.Status == Looper.Consts.StatusName.COMPLETE)
                        //{                            
                        //    codeTo = 1;
                        //}
                        //else if (task.Status == Looper.Consts.StatusName.ERROR)
                        //{                            
                        //    codeTo = 1;
                        //}
                    }
                }*/
            }
            catch (Exception e)
            {
                respond = e.Message;
                codeTo = 1;
            }

            return Json(new
            {
                success = true,
                data = respond,
                code = codeTo,
                opData = opDataTo
            });
        }

        private void CreateImportTask(string fileName, string importHandler) {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            using (DatabaseContext context = new DatabaseContext()) {
                ImportResultFilesModel resiltfile = new ImportResultFilesModel();
                ImportResultModel resultmodel = new ImportResultModel();

                HandlerData data = new HandlerData();
                FileModel file = new FileModel() {
                    LogicType = "Import",
                    Name = System.IO.Path.GetFileName(fileName),
                    DisplayName = System.IO.Path.GetFileName(fileName)
                };

                HandlerDataHelper.SaveIncomingArgument("File", file, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportPromo), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportPromo).Name, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(Promo), data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler() {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Загрузка импорта из файла " + typeof(ImportPromo).Name,
                    //Name = "ProcessingHost.Handlers.Import." + importHandler,
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


        //Простановка дат в формате Mars
        private void SetPromoMarsDates(Promo promo) {
            string stringFormatYP2WD = "{0}P{1:D2}W{2}D{3}";

            if (promo.StartDate != null) {
                promo.MarsStartDate = (new MarsDate((DateTimeOffset) promo.StartDate)).ToString(stringFormatYP2WD);
            }
            if (promo.EndDate != null) {
                promo.MarsEndDate = (new MarsDate((DateTimeOffset) promo.EndDate)).ToString(stringFormatYP2WD);
            }
            if (promo.DispatchesStart != null) {
                promo.MarsDispatchesStart = (new MarsDate((DateTimeOffset) promo.DispatchesStart)).ToString(stringFormatYP2WD);
            }
            if (promo.DispatchesEnd != null) {
                promo.MarsDispatchesEnd = (new MarsDate((DateTimeOffset) promo.DispatchesEnd)).ToString(stringFormatYP2WD);
            }
        }

        /// <summary>
        /// Установка в промо цвета, бренда и BrandTech на основании дерева продуктов
        /// </summary>
        /// <param name="promo"></param>
        private void SetPromoByProductTree(Promo promo) {
            IEnumerable<PromoProductTree> promoProducts = Context.Set<PromoProductTree>().Where(y => y.PromoId == promo.Id && !y.Disabled);
            PromoProductTree product = promoProducts.FirstOrDefault();
            DateTime dt = DateTime.Now;
            if (product != null) {
                //Заполнение Subranges
                IQueryable<ProductTree> ptQuery = Context.Set<ProductTree>().Where(x => x.Type == "root"
                    || (DateTime.Compare(x.StartDate, dt) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dt) > 0)));
                IEnumerable<int> promoProductsPTOIds = promoProducts.Select(z => z.ProductTreeObjectId);
                IQueryable<ProductTree> pts = ptQuery.Where(y => promoProductsPTOIds.Contains(y.ObjectId));
                promo.ProductSubrangesList = String.Join(";", pts.Select(z => z.Name));

                int objectId = product.ProductTreeObjectId;
                ProductTree pt = Context.Set<ProductTree>().FirstOrDefault(x => (x.StartDate < dt && (x.EndDate > dt || !x.EndDate.HasValue)) && x.ObjectId == objectId);
                if (pt != null) {
                    Guid? BrandId = null;
                    Guid? TechId = null;
                    Brand brandTo = null;
                    bool end = false;
                    do {
                        if (pt.Type == "Brand") {
                            brandTo = Context.Set<Brand>().FirstOrDefault(x => x.Name == pt.Name);
                            if (brandTo != null) {
                                BrandId = brandTo.Id;
                                promo.BrandId = brandTo.Id;
                            }
                        }
                        if (pt.Type == "Technology") {
                            var tech = Context.Set<Technology>().FirstOrDefault(x => x.Name == pt.Name);
                            if (tech != null) {
                                TechId = tech.Id;
                            }
                        }
                        if (pt.parentId != 1000000) {
                            pt = Context.Set<ProductTree>().FirstOrDefault(x => (x.StartDate < dt && (x.EndDate > dt || !x.EndDate.HasValue)) && x.ObjectId == pt.parentId);
                        } else {
                            end = true;
                        }
                    } while (!end && pt != null);

                    if (brandTo == null) {
                        promo.BrandId = null;
                    }

                    BrandTech bt = Context.Set<BrandTech>().FirstOrDefault(x => !x.Disabled && x.TechnologyId == TechId && x.BrandId == BrandId);
                    if (bt != null) {
                        promo.BrandTechId = bt.Id;
                        var colors = Context.Set<Color>().Where(x => !x.Disabled && x.BrandTechId == bt.Id).ToList();
                        if (colors.Count() == 1) {
                            promo.ColorId = colors.First().Id;
                        } else {
                            promo.ColorId = null;
                        }
                    } else {
                        promo.ColorId = null;
                        promo.BrandTechId = null;
                    }
                } else {
                    promo.ColorId = null;
                }
            }
        }

        /// <summary>
        /// Установка значения единого поля для Mechanic
        /// </summary>
        /// <param name="promo"></param>
        private void SetMechanic(Promo promo) {
            // нет механики - нет остального
            if (promo.MarsMechanicId != null) {
                var mechanic = Context.Set<Mechanic>().Find(promo.MarsMechanicId);
                string result = mechanic.Name;

                if (promo.MarsMechanicTypeId != null) {
                    var mechanicType = Context.Set<MechanicType>().Find(promo.MarsMechanicTypeId);
                    result += " " + mechanicType.Name;
                }

                if (promo.MarsMechanicDiscount != null)
                    result += " " + promo.MarsMechanicDiscount + "%";

                promo.Mechanic = result;
            }
        }

        /// <summary>
        /// Установка значения единого поля для Mechanic IA
        /// </summary>
        /// <param name="promo"></param>
        private void SetMechanicIA(Promo promo) {
            string result = null;

            // нет механики - нет остального
            if (promo.PlanInstoreMechanicId != null) {
                var mechanic = Context.Set<Mechanic>().Find(promo.PlanInstoreMechanicId);
                result = mechanic.Name;

                if (promo.PlanInstoreMechanicTypeId != null) {
                    var mechanicType = Context.Set<MechanicType>().Find(promo.PlanInstoreMechanicTypeId);
                    result += " " + mechanicType.Name;
                }

                if (promo.MarsMechanicDiscount != null)
                    result += " " + promo.PlanInstoreMechanicDiscount + "%";
            }

            promo.MechanicIA = result;
        }


        /// <summary>
        /// Установка промо по дереву клиентов
        /// </summary>
        /// <param name="promo"></param>
        private void SetPromoByClientTree(Promo promo) {
            int? ClientTreeId = promo.ClientTreeId;
            String resultMultiBaseStr = "";
            if (promo.ClientTreeId != null) {

                IQueryable<ClientTree> ctQuery = Context.Set<ClientTree>().Where(x => x.Type == "root"
               || (DateTime.Compare(x.StartDate, DateTime.Now) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, DateTime.Now) > 0)));
                ClientTree ct = ctQuery.FirstOrDefault(y => y.ObjectId == promo.ClientTreeId); ;
                if (ct != null) {
                    promo.ClientName = ct.Name;
                }
                while (ct != null && ct.depth != 0) {
                    if (ct.depth == 1) {
                        promo.Client1LevelName = ct.Name;
                    } else if (ct.depth == 2) {
                        promo.Client2LevelName = ct.Name;
                    }
                    ct = ctQuery.FirstOrDefault(y => y.ObjectId == ct.parentId);
                }

                int? upBaseClientId = RecursiveUpBaseClientsFind(ClientTreeId);
                if (upBaseClientId.HasValue) {
                    resultMultiBaseStr = upBaseClientId.ToString();
                } else {
                    resultMultiBaseStr =
                        String.Join("|", RecursiveDownBaseClientsFind(promo.ClientTreeId));
                }

            }
            promo.BaseClientTreeIds = resultMultiBaseStr;
        }

        /// <summary>
        /// Поиск базовых клиентов в дереве в корень
        /// </summary>
        /// <param name="clientTreeId"></param>
        /// <returns></returns>
        private int? RecursiveUpBaseClientsFind(int? clientTreeId) {
            if (!clientTreeId.HasValue) {
                return null;
            } else {
                ClientTree ctn = Context.Set<ClientTree>().FirstOrDefault(x => x.ObjectId == clientTreeId &&
                 (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, DateTime.Now) > 0));
                if (ctn == null) {
                    return null;
                } else if (ctn.IsBaseClient) {
                    return ctn.ObjectId;
                } else if (ctn.ObjectId == ctn.parentId) {
                    return null;
                } else {
                    return RecursiveUpBaseClientsFind(ctn.parentId);
                }

            }
        }


        /// <summary>
        /// Поиск базовых клиентов в дереве назад
        /// </summary>
        /// <param name="clientTreeId"></param>
        /// <returns></returns>
        private List<int> RecursiveDownBaseClientsFind(int? clientTreeId) {
            if (!clientTreeId.HasValue) {
                return new List<int>();
            } else {
                ClientTree ct = Context.Set<ClientTree>().FirstOrDefault(x => x.ObjectId == clientTreeId &&
                 (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, DateTime.Now) > 0));
                if (ct.IsBaseClient) {
                    return new List<int>() { ct.ObjectId };
                }

                List<ClientTree> ctChilds = Context.Set<ClientTree>().Where(
                 x =>
                 //DateTime.Compare(x.StartDate, ct.StartDate) <= 0 && 
                 x.parentId == ct.ObjectId &&
                 (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, DateTime.Now) > 0)).ToList();

                List<int> res = new List<int>();
                res.AddRange(ctChilds.Where(y => y.IsBaseClient).Select(z => z.ObjectId).ToList());

                foreach (var item in ctChilds.Where(y => !y.IsBaseClient)) {
                    res.AddRange(RecursiveDownBaseClientsFind(item.ObjectId));
                }
                return res;
            }
        }



        /// <summary>
        /// Создание отложенной задачи для расчета планового аплифта
        /// </summary>
        /// <param name="promo"></param>
        private void UpdateUplift(Promo promo) {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            using (DatabaseContext context = new DatabaseContext()) {
                HandlerData data = new HandlerData();
                HandlerDataHelper.SaveIncomingArgument("PromoId", promo.Id, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler() {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Update uplift value",
                    Name = "Module.Host.TPM.Handlers.UpdateUpliftHandler",
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
        /// Создание записи о создании/удалении нового промо
        /// </summary>
        /// <param name="newRecord"></param>
        /// <param name="isDelete"></param>
        private void WritePromoDemandChangeIncident(Promo record, bool isDelete = false) {
            PromoDemandChangeIncident change = new PromoDemandChangeIncident(record, isDelete) { };
            Context.Set<PromoDemandChangeIncident>().Add(change);
            Context.SaveChanges();
        }

        /// <summary>
        /// Создание записи об изменении/удалении промо
        /// </summary>
        /// <param name="newRecord"></param>
        /// <param name="patch"></param>
        /// <param name="oldRecord"></param>
        private void WritePromoDemandChangeIncident(Promo newRecord, Delta<Promo> patch, Promo oldRecord) {
            bool needCreateIncident = CheckCreateIncidentCondition(oldRecord, newRecord, patch);
            if (needCreateIncident) {
                // Если промо удалено, нет необходимости писать старые значения
                //bool isDelete = newRecord.PromoStatus != null && newRecord.PromoStatus.SystemName == "Deleted";
                //PromoDemandChangeIncident change = isDelete? new PromoDemandChangeIncident(oldRecord, isDelete) {} : new PromoDemandChangeIncident(oldRecord, newRecord) {};
                PromoDemandChangeIncident change = new PromoDemandChangeIncident(oldRecord, newRecord) { };
                Context.Set<PromoDemandChangeIncident>().Add(change);
                Context.SaveChanges();
            }
        }

        /// <summary>
        /// Проверка на то, что о изменениях применённых к промо необходимо создавать сообщение
        /// </summary>
        /// <param name="oldRecord"></param>
        /// <param name="newRecord"></param>
        /// <param name="patch"></param>
        /// <returns></returns>
        private bool CheckCreateIncidentCondition(Promo oldRecord, Promo newRecord, Delta<Promo> patch) {
            bool result = false;
            // TODO: Изменения продуктов не учитывается из-за удаления ProductTreeId из Promo
            // Получение настроек
            ISettingsManager settingsManager = (ISettingsManager) IoC.Kernel.GetService(typeof(ISettingsManager));

            string promoPropertiesSetting = settingsManager.GetSetting<string>("PROMO_CHANGE_PROPERTIES",
                "ClientTreeId, ProductHierarchy, StartDate, EndDate, DispatchesStart, DispatchesEnd, PlanUplift, PlanIncrementalLsv, MarsMechanicDiscount, InstoreMechanicDiscount, Mechanic, MarsMechanic, InstoreMechanic");
            int daysToCheckSetting = settingsManager.GetSetting<int>("PROMO_CHANGE_PERIOD_DAYS", 84);
            int marsDiscountSetting = settingsManager.GetSetting<int>("PROMO_CHANGE_MARS_DISCOUNT", 3);
            int instoreDiscountSetting = settingsManager.GetSetting<int>("PROMO_CHANGE_INSTORE_DISCOUNT", 5);
            int durationSetting = settingsManager.GetSetting<int>("PROMO_CHANGE_DURATION_DAYS", 3);
            int dispatchSetting = settingsManager.GetSetting<int>("PROMO_CHANGE_DISPATCH_DAYS", 5);

            string[] propertiesToCheck = promoPropertiesSetting.Split(',').Select(x => x.Trim(' ')).ToArray();

            DateTimeOffset today = DateTimeOffset.Now;
            bool isOldStartInCheckPeriod = oldRecord.StartDate.HasValue && (oldRecord.StartDate.Value - today).Days <= daysToCheckSetting;
            bool isNewStartInCheckPeriod = newRecord.StartDate.HasValue && (newRecord.StartDate.Value - today).Days <= daysToCheckSetting;
            IEnumerable<string> changedProperties = patch.GetChangedPropertyNames();
            changedProperties = changedProperties.Where(p => propertiesToCheck.Contains(p));
            bool relevantByTime = isOldStartInCheckPeriod || isNewStartInCheckPeriod;
            // Если дата начала промо соответствует настройке и изменилось какое-либо поле из указанных в настройках создаётся запись об изменении
            if (relevantByTime && changedProperties.Any()) {
                bool productChange = oldRecord.ProductHierarchy != newRecord.ProductHierarchy;
                bool clientChange = oldRecord.ClientTreeId != newRecord.ClientTreeId;
                bool marsMechanicChange = oldRecord.MarsMechanic != newRecord.MarsMechanic;
                bool instoreMechanicChange = oldRecord.PlanInstoreMechanic != newRecord.PlanInstoreMechanic;
                bool marsDiscountChange = oldRecord.MarsMechanicDiscount.HasValue ? newRecord.MarsMechanicDiscount.HasValue ? Math.Abs(oldRecord.MarsMechanicDiscount.Value - newRecord.MarsMechanicDiscount.Value) > marsDiscountSetting : true : false;
                bool instoreDiscountChange = oldRecord.PlanInstoreMechanicDiscount.HasValue ? newRecord.PlanInstoreMechanicDiscount.HasValue ? Math.Abs(oldRecord.PlanInstoreMechanicDiscount.Value - newRecord.PlanInstoreMechanicDiscount.Value) > instoreDiscountSetting : true : false;
                TimeSpan? oldDuration = oldRecord.EndDate - oldRecord.StartDate;
                TimeSpan? newDuration = newRecord.EndDate - newRecord.StartDate;
                bool durationChange = oldDuration.HasValue && newDuration.HasValue && Math.Abs(oldDuration.Value.Days - newDuration.Value.Days) > durationSetting;
                TimeSpan? dispatchStartDif = oldRecord.DispatchesStart - newRecord.DispatchesStart;
                TimeSpan? dispatchEndDif = oldRecord.DispatchesEnd - newRecord.DispatchesEnd;
                bool dispatchChange = dispatchStartDif.HasValue && dispatchEndDif.HasValue && Math.Abs(dispatchStartDif.Value.Days + dispatchEndDif.Value.Days) > dispatchSetting;
                bool isDelete = newRecord.PromoStatus != null && newRecord.PromoStatus.SystemName == "Deleted";

                List<bool> conditionnCheckResults = new List<bool>() { productChange, clientChange, marsMechanicChange, instoreMechanicChange, marsDiscountChange, instoreDiscountChange, durationChange, dispatchChange, isDelete };
                result = conditionnCheckResults.Any(x => x == true);
            }
            return result;
        }

        /// <summary>
        /// Добавить продукты из иерархии к промо
        /// </summary>
        /// <param name="objectIds">Список ObjectId продуктов в иерархии</param>
        /// <param name="promo">Промо к которому прикрепляются продукты</param>
        private void AddProductTrees(string objectIds, Promo promo) {
            // Если Null, значит продукты не менялись
            if (objectIds != null) {
                List<int> productTreeObjectIds = new List<int>();

                if (objectIds.Length > 0) {
                    productTreeObjectIds = objectIds.Split(';').Select(n => Int32.Parse(n)).ToList();
                }

                // находим прежние записи, если они остались то ислючаем их из нового списка
                // иначе удаляем
                var oldRecords = Context.Set<PromoProductTree>().Where(n => n.PromoId == promo.Id && !n.Disabled);
                foreach (var rec in oldRecords) {
                    int index = productTreeObjectIds.IndexOf(rec.ProductTreeObjectId);

                    if (index >= 0) {
                        productTreeObjectIds.RemoveAt(index);
                    } else {
                        rec.DeletedDate = System.DateTime.Now;
                        rec.Disabled = true;
                    }
                }

                // Добавляем новые продукты в промо
                foreach (int objectId in productTreeObjectIds) {
                    PromoProductTree promoProductTree = new PromoProductTree() {
                        Id = Guid.NewGuid(),
                        ProductTreeObjectId = objectId,
                        PromoId = promo.Id
                    };

                    Context.Set<PromoProductTree>().Add(promoProductTree);
                }
            }
        }

        /// <summary>
        /// Проверка TI, COGS и наличия продуктов, попадающих под фильтрация
        /// </summary>
        /// <param name="promo">Проверяемое промо</param>
        /// <exception cref="Exception">Исключение генерируется при отсутсвии одного из проверяемых параметров</exception>
        private void CheckSupportInfo(Promo promo)
        {
            List<string> messagesError = new List<string>();
            string message = null;

            // проверка на наличие TI
            PlanPromoParametersCalculation.GetTIBasePercent(promo, Context, out message);
            if (message != null)
            {
                messagesError.Add(message);
                message = null;
            }

            // проверка на наличие COGS
            PlanPromoParametersCalculation.GetCOGSPercent(promo, Context, out message);
            if (message != null)
            {
                messagesError.Add(message);
                message = null;
            }

            // проверка на наличие продуктов, попадающих под фильтр
            PlanProductParametersCalculation.GetProductFiltered(promo.Id, Context, out message);
            if (message != null)
            {
                messagesError.Add(message);
            }

            // если что-то не найдено, то генерируем ошибку
            if (messagesError.Count > 0)
            {
                string messageError = "";
                for (int i = 0; i < messagesError.Count; i++)
                {
                    string endString = i == messagesError.Count - 1 ? "" : " ";
                    messageError += messagesError[i] + endString;
                }

                throw new Exception(messageError);
            }
        }


        private IEnumerable<Column> GetPromoROIExportSettings() {
            IEnumerable<Column> columns = new List<Column>() {
                new Column { Order = 1, Field = "Number", Header = "Promo ID", Quoting = false },
                new Column { Order = 2, Field = "Client1LevelName", Header = "NA/RKA", Quoting = false },
                new Column { Order = 3, Field = "Client2LevelName", Header = "Client Group", Quoting = false },
                new Column { Order = 4, Field = "ClientName", Header = "Client", Quoting = false },
                new Column { Order = 5, Field = "BrandName", Header = "Brand", Quoting = false },
                new Column { Order = 6, Field = "TechnologyName", Header = "Technology", Quoting = false },
                new Column { Order = 7, Field = "ProductSubrangesList", Header = "Subrange", Quoting = false },
                new Column { Order = 8, Field = "MarsMechanic.Name", Header = "Mars mechanic", Quoting = false },
                new Column { Order = 9, Field = "MarsMechanicType.Name", Header = "Mars mechanic type", Quoting = false },
                new Column { Order = 10, Field = "MarsMechanicDiscount", Header = "Mars mechanic discount, %", Quoting = false },
                new Column { Order = 11, Field = "MechanicComment", Header = "Mechanic comment", Quoting = false },
                new Column { Order = 12, Field = "StartDate", Header = "Start date", Quoting = false, Format = "dd.MM.yyyy"  },
                new Column { Order = 13, Field = "EndDate", Header = "End date", Quoting = false, Format = "dd.MM.yyyy"  },
                new Column { Order = 14, Field = "PromoDuration", Header = "Promo duration", Quoting = false , Format = "0"},
                new Column { Order = 15, Field = "DispatchDuration", Header = "Dispatch Duration", Quoting = false, Format = "0" },
                new Column { Order = 16, Field = "EventName", Header = "Event", Quoting = false },
                new Column { Order = 17, Field = "PromoStatusName", Header = "Status", Quoting = false },
                new Column { Order = 18, Field = "PlanInstoreMechanic.Name", Header = "Plan Instore Mechanic Name", Quoting = false },
                new Column { Order = 19, Field = "PlanInstoreMechanicType.Name", Header = "Plan Instore Mechanic Type Name", Quoting = false },
                new Column { Order = 20, Field = "PlanInstoreMechanicDiscount", Header = "Plan Instore Mechanic Discount", Quoting = false,  Format = "0"  },
                new Column { Order = 21, Field = "PlanPromoBaselineLSV", Header = "Plan Promo Baseline LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 22, Field = "PlanPromoIncrementalLSV", Header = "Plan Promo Incremental LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 23, Field = "PlanPromoLSV", Header = "Plan Promo LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 24, Field = "PlanPromoUpliftPercent", Header = "Plan Promo Uplift %", Quoting = false,  Format = "0.00"  },
                new Column { Order = 25, Field = "PlanPromoTIShopper", Header = "Plan Promo TI Shopper", Quoting = false,  Format = "0.00"  },
                new Column { Order = 26, Field = "PlanPromoTIMarketing", Header = "Plan Promo TI Marketing", Quoting = false,  Format = "0.00"  },
                new Column { Order = 27, Field = "PlanPromoXSites", Header = "Plan Promo X-Sites", Quoting = false,  Format = "0.00"  },
                new Column { Order = 28, Field = "PlanPromoCatalogue", Header = "Plan Promo Catalogue", Quoting = false,  Format = "0.00"  },
                new Column { Order = 29, Field = "PlanPromoPOSMInClient", Header = "Plan Promo POSM In Client", Quoting = false,  Format = "0.00"  },
                new Column { Order = 30, Field = "PlanPromoBranding", Header = "Plan Promo Branding", Quoting = false,  Format = "0.00"  },
                new Column { Order = 31, Field = "PlanPromoBTL", Header = "Plan Promo BTL", Quoting = false,  Format = "0.00"  },
                new Column { Order = 32, Field = "PlanPromoCostProduction", Header = "Plan Promo Cost Production", Quoting = false,  Format = "0.00"  },
                new Column { Order = 33, Field = "PlanPromoCostProdXSites", Header = "Plan PromoCostProdXSites", Quoting = false,  Format = "0.00"  },
                new Column { Order = 34, Field = "PlanPromoCostProdCatalogue", Header = "Plan PromoCostProdCatalogue", Quoting = false,  Format = "0.00"  },
                new Column { Order = 35, Field = "PlanPromoCostProdPOSMInClient", Header = "Plan PromoCostProdPOSMInClient", Quoting = false,  Format = "0.00"  },
                new Column { Order = 36, Field = "PlanPromoCost", Header = "Plan Promo Cost", Quoting = false,  Format = "0.00"  },
                new Column { Order = 37, Field = "PlanPromoIncrementalBaseTI", Header = "Plan Promo Incremental BaseTI", Quoting = false,  Format = "0.00"  },
                new Column { Order = 38, Field = "PlanPromoIncrementalCOGS", Header = "Plan Promo Incremental COGS", Quoting = false,  Format = "0.00"  },
                new Column { Order = 39, Field = "PlanPromoTotalCost", Header = "Plan Promo Total Cost", Quoting = false,  Format = "0.00"  },
                new Column { Order = 40, Field = "PlanPostPromoEffectW1", Header = "Plan Post Promo Effect LSV W1", Quoting = false,  Format = "0.00"  },
                new Column { Order = 41, Field = "PlanPostPromoEffectW2", Header = "Plan Post Promo Effect LSV W2", Quoting = false,  Format = "0.00"  },
                new Column { Order = 42, Field = "PlanPostPromoEffect", Header = "Plan Post Promo Effect LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 43, Field = "PlanPromoNetIncrementalLSV", Header = "Plan Promo Net Incremental LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 44, Field = "PlanPromoNetLSV", Header = "PlanPromo Net LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 45, Field = "PlanPromoBaselineBaseTI", Header = "Plan Promo Baseline Base TI", Quoting = false,  Format = "0.00"  },
                new Column { Order = 46, Field = "PlanPromoBaseTI", Header = "Plan Promo Base TI", Quoting = false,  Format = "0.00"  },
                new Column { Order = 47, Field = "PlanPromoNetNSV", Header = "Plan Promo Net NSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 48, Field = "PlanPromoIncrementalNSV", Header = "Plan Promo Total Net NSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 49, Field = "PlanPromoNetIncrementalNSV", Header = "Plan Promo Incremental NSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 50, Field = "PlanPromoIncrementalMAC", Header = "Plan Promo Incremental MAC", Quoting = false,  Format = "0.00"  },
                new Column { Order = 51, Field = "PlanPromoNetIncrementalMAC", Header = "Plan Promo Net Incremental MAC", Quoting = false,  Format = "0.00"  },
                new Column { Order = 52, Field = "PlanPromoIncrementalEarnings", Header = "Plan Promo Incremental Earnings", Quoting = false,  Format = "0.00"  },
                new Column { Order = 53, Field = "PlanPromoNetIncrementalEarnings", Header = "Plan Promo Net Incremental Earnings", Quoting = false,  Format = "0.00"  },
                new Column { Order = 54, Field = "PlanPromoROIPercent", Header = "Plan Promo ROI, %", Quoting = false,  Format = "0"  },
                new Column { Order = 55, Field = "PlanPromoNetROIPercent", Header = "Plan Promo Net ROI, %", Quoting = false,  Format = "0"  },
                new Column { Order = 56, Field = "PlanPromoNetUpliftPercent", Header = "Plan Promo Net Uplift %", Quoting = false,  Format = "0"  },
                new Column { Order = 57, Field = "ActualInStoreMechanic.Name", Header = "Actual InStore Mechanic Name", Quoting = false },
                new Column { Order = 58, Field = "ActualInStoreMechanicType.Name", Header = "Actual InStore Mechanic Type Name", Quoting = false  },
                new Column { Order = 59, Field = "ActualInStoreMechanicDiscount", Header = "Actual InStore Mechanic Discount", Quoting = false,  Format = "0"  },
                new Column { Order = 60, Field = "ActualInStoreShelfPrice", Header = "Instore Shelf Price", Quoting = false,  Format = "0.00"  },
                new Column { Order = 61, Field = "InvoiceNumber", Header = "Invoice number", Quoting = false },
                new Column { Order = 62, Field = "ActualPromoBaselineLSV", Header = "Actual Promo Baseline LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 63, Field = "ActualPromoIncrementalLSV", Header = "Actual Promo Incremental LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 64, Field = "ActualPromoLSV", Header = "Actual Promo LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 65, Field = "ActualPromoUpliftPercent", Header = "Actual Promo Uplift %", Quoting = false,  Format = "0.00"  },
                new Column { Order = 66, Field = "ActualPromoTIShopper", Header = "Actual Promo TI Shopper", Quoting = false,  Format = "0.00"  },
                new Column { Order = 67, Field = "ActualPromoTIMarketing", Header = "Actual Promo TI Marketing", Quoting = false,  Format = "0.00"  },
                new Column { Order = 68, Field = "ActualPromoProdXSites", Header = "Actual Promo Prod XSites", Quoting = false,  Format = "0.00"  },
                new Column { Order = 69, Field = "ActualPromoProdCatalogue", Header = "Actual Promo Prod Catalogue", Quoting = false,  Format = "0.00"  },
                new Column { Order = 70, Field = "ActualPromoProdPOSMInClient", Header = "Actual Promo Prod POSMInClient", Quoting = false,  Format = "0.00"  },
                new Column { Order = 71, Field = "ActualPromoBranding", Header = "Actual Promo Branding", Quoting = false,  Format = "0.00"  },
                new Column { Order = 72, Field = "ActualPromoBTL", Header = "Actual Promo BTL", Quoting = false,  Format = "0.00"  },
                new Column { Order = 73, Field = "ActualPromoCostProduction", Header = "Actual Promo Cost Production", Quoting = false,  Format = "0.00"  },
                new Column { Order = 74, Field = "ActualPromoCostProdXSites", Header = "Actual Promo CostProdXSites", Quoting = false,  Format = "0.00"  },
                new Column { Order = 75, Field = "ActualPromoCostProdCatalogue", Header = "Actual Promo Cost ProdCatalogue", Quoting = false,  Format = "0.00"  },
                new Column { Order = 76, Field = "ActualPromoCostProdPOSMInClient", Header = "Actual Promo Cost ProdPOSMInClient", Quoting = false,  Format = "0.00"  },
                new Column { Order = 77, Field = "ActualPromoCost", Header = "Actual Promo Cost", Quoting = false,  Format = "0.00"  },
                new Column { Order = 78, Field = "ActualPromoIncrementalBaseTI", Header = "Actual Promo Incremental BaseTI", Quoting = false,  Format = "0.00"  },
                new Column { Order = 79, Field = "ActualPromoIncrementalCOGS", Header = "Actual Promo Incremental COGS", Quoting = false,  Format = "0.00"  },
                new Column { Order = 80, Field = "ActualPromoTotalCost", Header = "Actual Promo Total Cost", Quoting = false,  Format = "0.00"  },
                new Column { Order = 81, Field = "FactPostPromoEffectW1", Header = "Actual Post Promo Effect LSV W1", Quoting = false,  Format = "0.00"  },
                new Column { Order = 82, Field = "FactPostPromoEffectW2", Header = "Actual Post Promo Effect LSV W2", Quoting = false,  Format = "0.00"  },
                new Column { Order = 83, Field = "FactPostPromoEffect", Header = "Actual Post Promo Effect LSV", Quoting = false,  Format = "0"  },
                new Column { Order = 84, Field = "ActualPromoNetIncrementalLSV", Header = "Actual Promo Net Incremental LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 85, Field = "ActualPromoNetLSV", Header = "Actual Promo Net LSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 86, Field = "ActualPromoIncrementalNSV", Header = "Actual Promo Incremental NSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 87, Field = "ActualPromoNetIncrementalNSV", Header = "Actual Promo Net Incremental NSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 88, Field = "ActualPromoBaselineBaseTI", Header = "Actual Promo Baseline Base TI", Quoting = false,  Format = "0.00"  },
                new Column { Order = 89, Field = "ActualPromoBaseTI", Header = "Actual Promo Base TI", Quoting = false,  Format = "0.00"  },
                new Column { Order = 90, Field = "ActualPromoNetNSV", Header = "Actual Promo Net NSV", Quoting = false,  Format = "0.00"  },
                new Column { Order = 91, Field = "ActualPromoIncrementalMAC", Header = "Actual Promo Incremental MAC", Quoting = false,  Format = "0.00"  },
                new Column { Order = 92, Field = "ActualPromoNetIncrementalMAC", Header = "Actual Promo Net Incremental MAC", Quoting = false,  Format = "0.00"  },
                new Column { Order = 93, Field = "ActualPromoIncrementalEarnings", Header = "Actual Promo Incremental Earnings", Quoting = false,  Format = "0.00"  },
                new Column { Order = 94, Field = "ActualPromoNetIncrementalEarnings", Header = "Actual Promo Net Incremental Earnings", Quoting = false,  Format = "0.00"  },
                new Column { Order = 95, Field = "ActualPromoROIPercent", Header = "Actual Promo ROI, %", Quoting = false,  Format = "0"  },
                new Column { Order = 96, Field = "ActualPromoNetROIPercent", Header = "Actual Promo Net ROI%", Quoting = false,  Format = "0"  },
                new Column { Order = 97, Field = "ActualPromoNetUpliftPercent", Header = "Actual Promo Net Uplift Percent", Quoting = false,  Format = "0"  }};
            return columns;
        }
        [ClaimsAuthorize]
        public IHttpActionResult ExportPromoROIReportXLSX(ODataQueryOptions<Promo> options) {
            try {
                IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));
                IEnumerable<Column> columns = GetPromoROIExportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                UserInfo user = authorizationManager.GetCurrentUser();
                string username = user == null ? "" : user.Login;
                string filePath = exporter.GetExportFileName("PromoROIReport", username);
                exporter.Export(results, filePath);
                string filename = System.IO.Path.GetFileName(filePath);
                return Content<string>(HttpStatusCode.OK, filename);
            } catch (Exception e) {
                return Content<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private bool NeedRecalculatePromo(Promo newPromo, Promo oldPromo)
        {
            bool needReacalculate = false;

            if (oldPromo.ClientTreeId != newPromo.ClientTreeId
                    || oldPromo.ProductHierarchy != newPromo.ProductHierarchy
                    || oldPromo.MarsMechanicId != newPromo.MarsMechanicId
                    || oldPromo.MarsMechanicTypeId != newPromo.MarsMechanicTypeId
                    || oldPromo.MarsMechanicDiscount != newPromo.MarsMechanicDiscount
                    || oldPromo.PlanInstoreMechanicId != newPromo.PlanInstoreMechanicId
                    || oldPromo.PlanInstoreMechanicTypeId != newPromo.PlanInstoreMechanicTypeId
                    || oldPromo.PlanInstoreMechanicDiscount != newPromo.PlanInstoreMechanicDiscount
                    || oldPromo.StartDate != newPromo.StartDate
                    || oldPromo.EndDate != newPromo.EndDate
                    || oldPromo.DispatchesStart != newPromo.DispatchesStart
                    || oldPromo.DispatchesEnd != newPromo.DispatchesEnd
                    || oldPromo.ActualPromoBTL != newPromo.ActualPromoBTL
                    || oldPromo.PlanPromoBTL != newPromo.PlanPromoBTL
                    || oldPromo.ActualPromoBranding != newPromo.ActualPromoBranding
                    || oldPromo.PlanPromoBranding != newPromo.PlanPromoBranding
                    || oldPromo.PlanPromoUpliftPercent != newPromo.PlanPromoUpliftPercent
                    || oldPromo.NeedRecountUplift != newPromo.NeedRecountUplift
                    || oldPromo.PromoStatus.Name.ToLower() == "draft")
            {
                needReacalculate = true;
            }

            return needReacalculate;
        }
    }
}
