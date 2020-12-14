using AutoMapper;
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
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
        public IHttpActionResult Put([FromODataUri] System.Guid key, Delta<BTL> patch)
        {
            var model = Context.Set<BTL>().Find(key);
            if (model == null)
            {
                return NotFound();
            }
            patch.Put(model);
            try
            {
                var resultSaveChanges = Context.SaveChanges();
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
        public IHttpActionResult Post(BTL model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var proxy = Context.Set<BTL>().Create<BTL>();
            var result = (BTL)Mapper.Map(model, proxy, typeof(BTL), proxy.GetType(), opts => opts.CreateMissingTypeMaps = true);
            Context.Set<BTL>().Add(result);

            try
            {
                var resultSaveChanges = Context.SaveChanges();
            }
            catch (Exception e)
            {
                return GetErorrRequest(e);
            }

            return Created(result);
        }

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<BTL> patch)
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

                var resultSaveChanges = Context.SaveChanges();
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
        public IHttpActionResult Delete([FromODataUri] System.Guid key)
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
                var resultSaveChanges = Context.SaveChanges();
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return InternalServerError(e.InnerException);
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
        public IHttpActionResult ExportXLSX(ODataQueryOptions<BTL> options)
        {
            IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);
            using (DatabaseContext context = new DatabaseContext())
            {
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
                context.LoopHandlers.Add(handler);
                context.SaveChanges();
            }

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
                return InternalServerError(e);
            }
        }
    }
}
