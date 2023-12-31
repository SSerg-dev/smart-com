﻿using AutoMapper;
using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Newtonsoft.Json;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.Results;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers
{
    public class BTLsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public BTLsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }


        protected IQueryable<BTL> GetConstraintedQuery()
        {

            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IQueryable<BTL> query = Context.Set<BTL>().Where(e => !e.Disabled);

            return query;
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<BTL> GetBTL([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<BTL> GetBTLs()
        {
            return GetConstraintedQuery();
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<BTL> GetFilteredData(ODataQueryOptions<BTL> options)
        {
            var query = GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<BTL>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<BTL>;
        }

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> Put([FromODataUri] System.Guid key, Delta<BTL> patch)
        {
            var model = Context.Set<BTL>().Find(key);
            if (model == null)
            {
                return NotFound();
            }
            patch.Put(model);
            try
            {
                var resultSaveChanges = await Context.SaveChangesAsync();
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
        public async Task<IHttpActionResult> Post(BTL model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var proxy = Context.Set<BTL>().Create<BTL>();
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<BTL, BTL>().ReverseMap());
            var mapper = configuration.CreateMapper();
            var result = mapper.Map(model, proxy);
            Context.Set<BTL>().Add(result);

            try
            {
                var resultSaveChanges = await Context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return GetErorrRequest(e);
            }

            return Created(result);
        }

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] System.Guid key, Delta<BTL> patch)
        {

            try
            {
                var model = Context.Set<BTL>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                patch.Patch(model);
                // делаем UTC +3
                model.StartDate = ChangeTimeZoneUtil.ResetTimeZone(model.StartDate);
                model.EndDate = ChangeTimeZoneUtil.ResetTimeZone(model.EndDate);

                var resultSaveChanges = await Context.SaveChangesAsync();
                CalculateBTLBudgetsCreateTask(key.ToString());
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
        public async Task<IHttpActionResult> Delete([FromODataUri] System.Guid key)
        {
            try
            {
                List<Guid> promoIdsToRecalculate = new List<Guid>();

                IQueryable<BTLPromo> pbQuery = Context.Set<BTLPromo>().Where(x => x.BTLId == key && !x.Disabled);
                foreach (var pb in pbQuery)
                {
                    pb.DeletedDate = System.DateTime.Now;
                    pb.Disabled = true;

                    promoIdsToRecalculate.Add(pb.PromoId);
                }

                var model = Context.Set<BTL>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                model.DeletedDate = System.DateTime.Now;
                model.Disabled = true;

                IQueryable<Promo> promos = Context.Set<Promo>().Where(x => promoIdsToRecalculate.Any(y => x.Id == y));
                if (promos.Any(x => x.PromoStatus.Name == "Closed"))
                    return InternalServerError(new Exception("Cannot be deleted due to closed promo"));

                CalculateBTLBudgetsCreateTask(model.Id.ToString(), promoIdsToRecalculate);
                var resultSaveChanges = await Context.SaveChangesAsync();
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }

        private bool EntityExists(System.Guid key)
        {
            return Context.Set<BTL>().Count(e => e.Id == key) > 0;
        }

        public static IEnumerable<Column> GetExportSettings()
        {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 0, Field = "Number", Header = "Number", Quoting = false },
                new Column() { Order = 1, Field = "PlanBTLTotal", Header = "Plan BTL Total", Quoting = false },
                new Column() { Order = 2, Field = "ActualBTLTotal", Header = "Actual BTL Total", Quoting = false },
                new Column() { Order = 3, Field = "StartDate", Header = "Start Date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 4, Field = "EndDate", Header = "End Date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 5, Field = "InvoiceNumber", Header = "Invoice Number", Quoting = false },
                new Column() { Order = 6, Field = "Event.Name", Header = "Event Name", Quoting = false }
            };
            return columns;
        }
        [ClaimsAuthorize]
        public async Task<IHttpActionResult> ExportXLSX(ODataQueryOptions<BTL> options)
        {
            IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            HandlerData data = new HandlerData();
            string handlerName = "ExportHandler";

            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TModel", typeof(BTL), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(BTLsController), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(BTLsController.GetExportSettings), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = $"Export {nameof(BTL)} dictionary",
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

        /// <summary>
        /// Создание отложенной задачи, выполняющей перерасчет бюджетов
        /// </summary>
        /// <param name="promoSupportPromoIds">список ID подстатей</param>
        /// <param name="calculatePlanCostTE">Необходимо ли пересчитывать значения плановые Cost TE</param>
        /// <param name="calculateFactCostTE">Необходимо ли пересчитывать значения фактические Cost TE</param>
        /// <param name="calculatePlanCostProd">Необходимо ли пересчитывать значения плановые Cost Production</param>
        /// <param name="calculateFactCostProd">Необходимо ли пересчитывать значения фактические Cost Production</param>
        private void CalculateBTLBudgetsCreateTask(string btlId, List<Guid> unlinkedPromoIds = null)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            HandlerData data = new HandlerData();
            HandlerDataHelper.SaveIncomingArgument("BTLId", btlId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            if (unlinkedPromoIds != null)
            {
                HandlerDataHelper.SaveIncomingArgument("UnlinkedPromoIds", unlinkedPromoIds, data, visible: false, throwIfNotExists: false);
            }

            bool success = CalculationTaskManager.CreateCalculationTask(CalculationTaskManager.CalculationAction.BTL, data, Context);

            if (!success)
                throw new Exception("Promo was blocked for calculation");
        }

        private ExceptionResult GetErorrRequest(Exception e)
        {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
            {
                return InternalServerError(new Exception("This BTL has already existed"));
            }
            else
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }
        [HttpPost]
        [ClaimsAuthorize]
        public async Task<IHttpActionResult> GetEventBTL()
        {
            string resultData = Request.Content.ReadAsStringAsync().Result;
            EventBTLModel eventBTL = new EventBTLModel();
            Event standartPromo = await Context.Set<Event>().AsNoTracking().Where(g => g.Name == "Standard promo" && !g.Disabled).FirstOrDefaultAsync();
            if (resultData != null)
            {
                eventBTL = JsonConvert.DeserializeObject<EventBTLModel>(resultData);
                eventBTL.DurationDateStart = eventBTL.DurationDateStart.LocalDateTime;
                eventBTL.DurationDateEnd = eventBTL.DurationDateEnd.LocalDateTime;
                List<Guid> productGuids = eventBTL.InOutProductIds.Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => Guid.Parse(s)).ToList();
                List<string> products = await Context.Set<Product>().AsNoTracking().Where(g => productGuids.Contains(g.Id) && !g.Disabled).Select(f => f.MarketSegment).ToListAsync();
                List<string> distinctList = products.Distinct().ToList();
                string marketSegment;
                if (distinctList.Count == 1)
                {
                    marketSegment = distinctList[0];
                }
                else
                {
                    int countRange = 0;
                    int position = 0;
                    for (int i = 0; i < distinctList.Count; i++)
                    {
                        string item = distinctList[i];
                        int countItem = products.Where(g => g == item).ToList().Count;
                        if (countItem > countRange)
                        {
                            countRange = countItem;
                            position = i;
                        }
                    }
                    marketSegment = distinctList[position];
                }
                List<Event> events = await Context.Set<Event>()
                    .AsNoTracking()
                    .Include(g => g.BTLs)
                    .Where(g => !g.Disabled && g.EventType.National && (g.MarketSegment == marketSegment || string.IsNullOrEmpty(g.MarketSegment)) && !g.EventType.Disabled)
                    .ToListAsync();
                //events = events.Where(g => g.BTLs.Any(f => (eventBTL.DurationDateStart <= f.StartDate && eventBTL.DurationDateEnd > f.StartDate) || (eventBTL.DurationDateStart >= f.StartDate && eventBTL.DurationDateStart > f.EndDate))).ToList();
                //List<Event> filterEvents = new List<Event>();
                foreach (Event item in events)
                {
                    item.BTLs = item.BTLs.Where(f => (eventBTL.DurationDateStart <= f.EndDate && eventBTL.DurationDateEnd >= f.StartDate) && !f.Disabled).ToList();
                    if (item.BTLs.Count > 0)
                    {
                        var copyBtls = item.BTLs.ToList();
                        foreach (BTL btl in item.BTLs)
                        {
                            double totalDays = (eventBTL.DurationDateEnd - eventBTL.DurationDateStart).TotalDays;
                            double totalDaysBTL = ((DateTimeOffset)btl.EndDate - (DateTimeOffset)btl.StartDate).TotalDays;
                            DateTimeOffset startDateDiff;
                            DateTimeOffset endDateDiff;
                            if (eventBTL.DurationDateStart >= (DateTimeOffset)btl.StartDate)
                            {
                                startDateDiff = eventBTL.DurationDateStart;
                                //countDays += (eventBTL.DurationDateStart - (DateTimeOffset)btl.StartDate).TotalDays;
                            }
                            else
                            {
                                startDateDiff = (DateTimeOffset)btl.StartDate;
                            }
                            if (eventBTL.DurationDateEnd <= (DateTimeOffset)btl.EndDate)
                            {
                                endDateDiff = eventBTL.DurationDateEnd;
                                //countDays += ((DateTimeOffset)btl.EndDate - eventBTL.DurationDateEnd).TotalDays;
                            }
                            else
                            {
                                endDateDiff = (DateTimeOffset)btl.EndDate;
                            }
                            double diffPeriod = (endDateDiff - startDateDiff).TotalDays;
                            if (totalDays / 2 > diffPeriod)
                            {
                                copyBtls.Remove(btl);
                            }
                        }
                        item.BTLs = copyBtls;
                    }
                }
                events = events.Where(g => g.BTLs.Count != 0).ToList();
                Event returnEvent = new Event();
                if (events.Count != 0)
                {
                    returnEvent = events.FirstOrDefault();
                }
                else
                {
                    returnEvent = standartPromo;
                }
                returnEvent.BTLs = null;
                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(returnEvent, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));
            }
            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(standartPromo, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }
    }
    public class EventBTLModel
    {
        public DateTimeOffset DurationDateStart { get; set; }
        public DateTimeOffset DurationDateEnd { get; set; }
        public string InOutProductIds { get; set; }
    }
}
