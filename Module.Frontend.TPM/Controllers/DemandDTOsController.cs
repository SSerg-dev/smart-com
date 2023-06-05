using AutoMapper;
using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.DTO;
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
    [Route("odata/DemandDTOs")]
    public class DemandDTOsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;
        private readonly IMapper mapper;

        public DemandDTOsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<Promo, DemandDTO>().ReverseMap());
            mapper = configuration.CreateMapper();
        }


        protected IQueryable<Promo> GetConstraintedQuery()
        {

            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IQueryable<Promo> query = Context.Set<Promo>().Where(e => !e.Disabled);

            return query;
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public SingleResult<DemandDTO> GetPromo([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(mapper.ProjectTo<DemandDTO>(GetConstraintedQuery()));
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<DemandDTO> GetPromoes()
        {
            return mapper.ProjectTo<DemandDTO>(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<DemandDTO> GetFilteredData(ODataQueryOptions<DemandDTO> options)
        {
            var query = mapper.ProjectTo<DemandDTO>(GetConstraintedQuery());

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<DemandDTO>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<DemandDTO>;
        }

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> Put([FromODataUri] System.Guid key, Delta<DemandDTO> patch)
        {
            var model = Context.Set<Promo>().Find(key);
            if (model == null)
            {
                return NotFound();
            }
            var dto = mapper.Map<DemandDTO>(model);
            patch.Put(dto);
            mapper.Map(dto, model);
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
        public async Task<IHttpActionResult> Post(DemandDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var proxy = Context.Set<Promo>().Create<Promo>();
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<Promo, Promo>().ReverseMap());
            var mapper = configuration.CreateMapper();
            var result = mapper.Map(model, proxy);
            Context.Set<Promo>().Add(result);

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
        public async Task<IHttpActionResult> Patch([FromODataUri] System.Guid key, Delta<DemandDTO> patch)
        {
            try
            {
                var model = Context.Set<Promo>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                var dto = mapper.Map<DemandDTO>(model);
                patch.Patch(dto);
                mapper.Map(dto, model);
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
                var model = Context.Set<Promo>().Find(key);
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
            return Context.Set<Promo>().Count(e => e.Id == key) > 0;
        }

        public static IEnumerable<Column> GetExportSettings()
        {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 0, Field = "Name", Header = "Program", Quoting = false },
                new Column() { Order = 1, Field = "Client.CommercialSubnet.CommercialNet.Name", Header = "Customer", Quoting = false },
                new Column() { Order = 2, Field = "Brand.Name", Header = "Brand", Quoting = false },
                new Column() { Order = 3, Field = "BrandTech.BrandsegTechsub", Header = "BrandTech", Quoting = false },
                new Column() { Order = 4, Field = "Product.Name", Header = "Product", Quoting = false },
                new Column() { Order = 5, Field = "PromoStatus.Name", Header = "PromoStatus", Quoting = false },
                new Column() { Order = 6, Field = "Mechanic.MechanicName", Header = "Mechanic", Quoting = false },
                new Column() { Order = 7, Field = "Mechanic.Discount", Header = "Mechanic, %", Quoting = false },
                new Column() { Order = 8, Field = "Mechanic.Comment", Header = "Mechanic comment", Quoting = false },
                new Column() { Order = 9, Field = "StartDate", Header = "StartDate", Quoting = false },
                new Column() { Order = 10, Field = "EndDate", Header = "EndDate", Quoting = false },
                new Column() { Order = 11, Field = "DispatchesStart", Header = "DispatchesStart", Quoting = false },
                new Column() { Order = 12, Field = "DispatchesEnd", Header = "DispatchesEnd", Quoting = false },
                new Column() { Order = 13, Field = "EventName", Header = "EventName", Quoting = false },
                new Column() { Order = 14, Field = "PlanBaseline", Header = "PlanBaseline", Quoting = false },
                new Column() { Order = 15, Field = "PlanDuration", Header = "PlanDuration", Quoting = false },
                new Column() { Order = 16, Field = "PlanUplift", Header = "PlanUplift", Quoting = false },
                new Column() { Order = 17, Field = "PlanIncremental", Header = "PlanIncremental", Quoting = false },
                new Column() { Order = 18, Field = "PlanActivity", Header = "PlanActivity", Quoting = false },
                new Column() { Order = 19, Field = "PlanSteal", Header = "PlanSteal", Quoting = false },
                new Column() { Order = 20, Field = "FactBaseline", Header = "FactBaseline", Quoting = false },
                new Column() { Order = 21, Field = "FactDuration", Header = "FactDuration", Quoting = false },
                new Column() { Order = 22, Field = "FactUplift", Header = "FactUplift", Quoting = false },
                new Column() { Order = 23, Field = "FactIncremental", Header = "FactIncremental", Quoting = false },
                new Column() { Order = 24, Field = "FactActivity", Header = "FactActivity", Quoting = false },
                new Column() { Order = 25, Field = "FactSteal", Header = "FactSteal", Quoting = false }
            };
            return columns;
        }
        [ClaimsAuthorize]
        public async Task<IHttpActionResult> ExportXLSX(ODataQueryOptions<DemandDTO> options)
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
            HandlerDataHelper.SaveIncomingArgument("TModel", typeof(DemandDTO), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(DemandDTOsController), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(DemandDTOsController.GetExportSettings), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = $"Export {nameof(DemandDTO)} dictionary",
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

        private ExceptionResult GetErorrRequest(Exception e)
        {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
            {
                return InternalServerError(new Exception("This Demand DTO has already existed"));
            }
            else
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }
    }
}