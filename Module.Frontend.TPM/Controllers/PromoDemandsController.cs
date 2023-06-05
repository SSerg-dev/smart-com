using AutoMapper;
using Core.MarsCalendar;
using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist.Model;
using System;
using System.Collections.Generic;
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
    public class PromoDemandsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public PromoDemandsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<PromoDemand> GetConstraintedQuery()
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();

            IQueryable<PromoDemand> query = Context.Set<PromoDemand>().Where(e => !e.Disabled);

            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<PromoDemand> GetPromoDemand([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<PromoDemand> GetPromoDemands()
        {
            return GetConstraintedQuery();
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<PromoDemand> GetFilteredData(ODataQueryOptions<PromoDemand> options)
        {
            var query = GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<PromoDemand>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<PromoDemand>;
        }

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> Put([FromODataUri] System.Guid key, Delta<PromoDemand> patch)
        {
            var model = Context.Set<PromoDemand>().Find(key);
            if (model == null)
            {
                return NotFound();
            }

            patch.Put(model);
            SetDemandMarsDates(model);

            try
            {
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
        public async Task<IHttpActionResult> Post(PromoDemand model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SetDemandMarsDates(model);
            var proxy = Context.Set<PromoDemand>().Create<PromoDemand>();
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<PromoDemand, PromoDemand>().ReverseMap());
            var mapper = configuration.CreateMapper();
            var result = mapper.Map(model, proxy);
            Context.Set<PromoDemand>().Add(result);

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return GetErorrRequest(e);
            }

            return Created(model);
        }

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] System.Guid key, Delta<PromoDemand> patch)
        {
            try
            {
                var model = Context.Set<PromoDemand>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                patch.Patch(model);
                SetDemandMarsDates(model);
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
        public async Task<IHttpActionResult> Delete([FromODataUri] System.Guid key)
        {
            try
            {
                var model = Context.Set<PromoDemand>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                model.DeletedDate = System.DateTime.Now;
                model.Disabled = true;
                await Context.SaveChangesAsync();

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }

        private bool EntityExists(System.Guid key)
        {
            return Context.Set<PromoDemand>().Count(e => e.Id == key) > 0;
        }

        public static IEnumerable<Column> GetExportSettings()
        {
            IEnumerable<Column> columns = new List<Column>()
            {
                new Column() { Order = 0, Field = "BrandTech.Brand.Name", Header = "Brand", Quoting = false },
                new Column() { Order = 1, Field = "BrandTech.BrandsegTechsub", Header = "Brand Tech", Quoting = false },
                new Column() { Order = 2, Field = "Account", Header = "Account", Quoting = false },
                new Column() { Order = 3, Field = "Mechanic.Name", Header = "Mechanic", Quoting = false },
                new Column() { Order = 4, Field = "MechanicType.Name", Header = "Mechanic Type", Quoting = false },
                new Column() { Order = 5, Field = "Discount", Header = "Discount", Quoting = false },
                new Column() { Order = 6, Field = "Week", Header = "Week", Quoting = false },
                new Column() { Order = 7, Field = "Baseline", Header = "Baseline", Quoting = false },
                new Column() { Order = 8, Field = "Uplift", Header = "Uplift", Quoting = false },
                new Column() { Order = 9, Field = "Incremental", Header = "Incremental", Quoting = false },
                new Column() { Order = 10, Field = "Activity", Header = "Activity", Quoting = false }
            };

            return columns;
        }

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> ExportXLSX(ODataQueryOptions<PromoDemand> options)
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
            HandlerDataHelper.SaveIncomingArgument("TModel", typeof(PromoDemand), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(PromoDemandsController), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(PromoDemandsController.GetExportSettings), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = $"Export {nameof(PromoDemand)} dictionary",
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

        private void SetDemandMarsDates(PromoDemand demand)
        {
            MarsDate week = new MarsDate(demand.Week);

            // без установки 12:00 AM + часовой пояс фильтрация не работает
            demand.MarsStartDate = ChangeUtc(week.StartDate());
            demand.MarsEndDate = ChangeUtc(week.EndDate());
        }

        /// <summary>
        /// Установка времени 12:00 AM + часовой пояс без смены даты
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        private DateTimeOffset ChangeUtc(DateTimeOffset original)
        {
            TimeZoneInfo timeZone = TimeZoneInfo.Local;
            return new DateTimeOffset(original.Year, original.Month, original.Day, 0, 0, 0, timeZone.BaseUtcOffset);
        }

        private ExceptionResult GetErorrRequest(Exception e)
        {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
            {
                return InternalServerError(new Exception("This PromoDemand has already existed"));
            }
            else
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }
    }
}
