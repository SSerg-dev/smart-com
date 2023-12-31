﻿using AutoMapper;
using Core.Dependency;
using Core.Security;
using Core.Security.Models;
using Core.Settings;
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
using Utility;

namespace Module.Frontend.TPM.Controllers
{
    public class BTLPromoesController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public BTLPromoesController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<BTLPromo> GetConstraintedQuery()
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();

            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();

            IQueryable<BTLPromo> query = Context.Set<BTLPromo>();
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public SingleResult<BTLPromo> GetBTLPromo([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<BTLPromo> GetBTLPromoes()
        {
            return GetConstraintedQuery();
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<BTLPromo> GetFilteredData(ODataQueryOptions<BTLPromo> options)
        {
            var bodyText = HttpContext.Current.Request.GetRequestBody();
            var query = GetConstraintedQuery();
            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<BTLPromo>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<BTLPromo>;
        }

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> Put([FromODataUri] System.Guid key, Delta<BTLPromo> patch)
        {
            var model = Context.Set<BTLPromo>().Find(key);
            if (model == null)
            {
                return NotFound();
            }

            patch.Put(model);

            try
            {
                CalculateBTLBudgetsCreateTask(key.ToString());
                await Context.SaveChangesAsync();
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
        public async Task<IHttpActionResult> Post(BTLPromo model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var proxy = Context.Set<BTLPromo>().Create<BTLPromo>();
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<BTLPromo, BTLPromo>().ReverseMap());
            var mapper = configuration.CreateMapper();
            var result = mapper.Map(model, proxy);
            Context.Set<BTLPromo>().Add(result);

            try
            {
                await Context.SaveChangesAsync();
                CalculateBTLBudgetsCreateTask(model.BTLId.ToString());
            }
            catch (Exception e)
            {
                return GetErorrRequest(e);
            }

            return Created(model);
        }

        [ClaimsAuthorize]
        [HttpPost]
        public async Task<IHttpActionResult> BTLPromoPost(Guid btlId)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                    int diffBetweenPromoInDays = settingsManager.GetSetting<int>("DIFF_BETWEEN_PROMO_IN_DAYS", 7 * 12);

                    List<string> promoIdsList = new List<string>();
                    string promoIds = Request.Content.ReadAsStringAsync().Result;
                    if (promoIds != null)
                    {
                        promoIdsList = JsonConvert.DeserializeObject<List<string>>(promoIds);
                    }

                    List<Guid> guidPromoIds = new List<Guid>();
                    foreach (var id in promoIdsList)
                    {
                        Guid promoId = Guid.Parse(id);
                        guidPromoIds.Add(promoId);
                    }

                    List<BTLPromo> bTLPromos = Context.Set<BTLPromo>()
                        .Include(x => x.Promo.PromoProducts.Select(y => y.PromoProductsCorrections))
                        .Include(x => x.Promo.IncrementalPromoes)
                        .Include(x => x.Promo.PromoProductTrees)
                        .Include(x => x.Promo.PromoSupportPromoes)
                        .Where(n => n.BTLId == btlId && !n.Disabled && !n.Promo.Disabled)
                        .ToList();

                    foreach (var promoId in guidPromoIds)
                    {
                        Promo promo = Context.Set<Promo>().Find(promoId);
                        if (promo.PromoStatus.SystemName != "Closed")
                        {
                            // Разница между промо в подстатье должно быть меньше 2 периодов (8 недель) уже 3
                            // если end date добавляемого промо лежит в 8 неделях от самого раннего start date, то всё ок, если более, то добавить промо нельзя.
                            List<DateTimeOffset> endDateList = new List<DateTimeOffset>(bTLPromos.Select(x => x.Promo.EndDate.Value)) { promo.EndDate.Value };
                            List<DateTimeOffset> startDateList = new List<DateTimeOffset>(bTLPromos.Select(x => x.Promo.StartDate.Value)) { promo.StartDate.Value };
                            DateTimeOffset maxEnd = endDateList.Max();
                            DateTimeOffset minStart = startDateList.Min();

                            if (minStart != null && maxEnd != null && endDateList.Count() > 1)
                            {
                                bool bigDifference = Math.Abs(maxEnd.Subtract(minStart).TotalDays) > diffBetweenPromoInDays;

                                if (bigDifference)
                                    throw new Exception("The difference between the dates of the promo should be less than 3 periods");
                            }

                            BTLPromo bp = new BTLPromo
                            {
                                BTLId = btlId,
                                PromoId = promoId,
                                ClientTreeId = promo.ClientTreeKeyId.Value,
                            };
                            Context.Set<BTLPromo>().Add(bp); //bTLPromos.Add(bp); так нельзя потому что делается прокси System.Data.Entity.DynamicProxies при lazyload

                        }
                    }

                    // TODO: Исправить кнопку selectAll в js конроллере
                    //if (linkedPromoes.Any())
                    //    throw new Exception(String.Format("Promoes with numbers {0} are already attached to another BTL.", string.Join(",", linkedPromoes)));

                    await Context.SaveChangesAsync();

                    CalculateBTLBudgetsCreateTask(btlId.ToString());

                    transaction.Commit();
                    return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    // обработка при создании дублирующей записи
                    SqlException exc = e.GetBaseException() as SqlException;
                    if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
                    {
                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, message = "Promo has already associated." }));
                    }
                    else
                    {
                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, message = e.Message }));
                    }
                }
            }
        }

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] System.Guid key, Delta<BTLPromo> patch)
        {
            try
            {
                var model = Context.Set<BTLPromo>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                patch.Patch(model);
                CalculateBTLBudgetsCreateTask(key.ToString());
                await Context.SaveChangesAsync();

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
        [HttpPost]
        public IHttpActionResult GetPromoesWithBTL(string eventId)
        {
            try
            {
                string[] excludedStatuses = { "Deleted", "Cancelled", "Closed", "Draft" };
                IQueryable<int?> promoesWithBTL = Enumerable.Empty<int?>().AsQueryable();

                Guid eventIdGuid;
                if (Guid.TryParse(eventId, out eventIdGuid))
                {
                    promoesWithBTL = Context.Set<BTLPromo>()
                    .Where(x => !x.Disabled && x.DeletedDate == null && !excludedStatuses.Contains(x.Promo.PromoStatus.SystemName) && x.Promo.Event.Id == eventIdGuid)
                    .Select(x => x.Promo.Number).Distinct();
                }
                else
                {
                    promoesWithBTL = Context.Set<BTLPromo>()
                    .Where(x => !x.Disabled && x.DeletedDate == null && !excludedStatuses.Contains(x.Promo.PromoStatus.SystemName))
                    .Select(x => x.Promo.Number).Distinct();
                }

                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, data = promoesWithBTL }, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
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
                var model = Context.Set<BTLPromo>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                model.DeletedDate = System.DateTime.Now;
                model.Disabled = true;

                CalculateBTLBudgetsCreateTask(model.BTLId.ToString(), new List<Guid>() { model.PromoId });
                await Context.SaveChangesAsync();
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return GetErorrRequest(e);
            }
        }

        public static IEnumerable<Column> GetExportSettingsBTLPromo()
        {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 0, Field = "Promo.Number", Header = "Promo ID", Quoting = false },
            };
            return columns;
        }

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> ExportXLSX(ODataQueryOptions<PromoSupportPromo> options, string section = "")
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
            HandlerDataHelper.SaveIncomingArgument("TModel", typeof(BTLPromo), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(BTLPromoesController), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(BTLPromoesController.GetExportSettingsBTLPromo), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = $"Export {nameof(BTLPromo)} dictionary",
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

        private bool EntityExists(System.Guid key)
        {
            return Context.Set<BTLPromo>().Count(e => e.Id == key) > 0;
        }

        private ExceptionResult GetErorrRequest(Exception e)
        {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
            {
                return InternalServerError(new Exception("This BTLPromo has already existed"));
            }
            else
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }

        /// <summary>
        /// Создание отложенной задачи, выполняющей перерасчет бюджетов
        /// </summary>
        /// <param name="promoSupportPromoIds">список ID подстатей</param>
        /// <param name="calculatePlanCostTE">Необходимо ли пересчитывать значения плановые Cost TE</param>
        /// <param name="calculateFactCostTE">Необходимо ли пересчитывать значения фактические Cost TE</param>
        /// <param name="calculatePlanCostProd">Необходимо ли пересчитывать значения плановые Cost Production</param>
        /// <param name="calculateFactCostProd">Необходимо ли пересчитывать значения фактические Cost Production</param>
        public void CalculateBTLBudgetsCreateTask(string btlId, List<Guid> unlinkedPromoIds = null)
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

        [ClaimsAuthorize]
        [HttpPost]
        public async Task<IHttpActionResult> BTLPromoDelete(Guid key)
        {
            try
            {
                var btlPromo = Context.Set<BTLPromo>()
                    .Where(x => x.Id == key && !x.Disabled)
                    .ToList();
                if (btlPromo == null)
                {
                    return NotFound();
                }


                btlPromo[0].DeletedDate = System.DateTime.Now;
                btlPromo[0].Disabled = true;

                await Context.SaveChangesAsync();
                CalculateBTLBudgetsCreateTask(btlPromo[0].BTLId.ToString(), new List<Guid>() { btlPromo[0].Id });

                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, message = e.Message }));
            }
        }
    }
}