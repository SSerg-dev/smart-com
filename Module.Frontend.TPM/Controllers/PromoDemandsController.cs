using AutoMapper;
using Core.Security;
using Core.Security.Models;
using Core.Settings;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions.Export;
using Module.Frontend.TPM.Model;
using Module.Persist.TPM.Model.TPM;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Core.MarsCalendar;
using System.Data.SqlClient;
using System.Web.Http.Results;
using Module.Frontend.TPM.Util;
using System.Web;

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
        public IHttpActionResult Put([FromODataUri] System.Guid key, Delta<PromoDemand> patch)
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
                Context.SaveChanges();
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
        public IHttpActionResult Post(PromoDemand model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SetDemandMarsDates(model);
            var proxy = Context.Set<PromoDemand>().Create<PromoDemand>();
            var result = (PromoDemand)Mapper.Map(model, proxy, typeof(PromoDemand), proxy.GetType(), opts => opts.CreateMissingTypeMaps = true);
            Context.Set<PromoDemand>().Add(result);

            try
            {
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                return GetErorrRequest(e);
            }

            return Created(model);
        }

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<PromoDemand> patch)
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
                Context.SaveChanges();

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
                var model = Context.Set<PromoDemand>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                model.DeletedDate = System.DateTime.Now;
                model.Disabled = true;
                Context.SaveChanges();

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return InternalServerError(e.InnerException);
            }
        }

        private bool EntityExists(System.Guid key)
        {
            return Context.Set<PromoDemand>().Count(e => e.Id == key) > 0;
        }

        private IEnumerable<Column> GetExportSettings()
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
        public IHttpActionResult ExportXLSX(ODataQueryOptions<PromoDemand> options)
        {
            try
            {
                IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));
                IEnumerable<Column> columns = GetExportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                UserInfo user = authorizationManager.GetCurrentUser();
                string username = user == null ? "" : user.Login;
                string filePath = exporter.GetExportFileName("PromoDemand", username);
                exporter.Export(results, filePath);
                string filename = System.IO.Path.GetFileName(filePath);
                return Content<string>(HttpStatusCode.OK, filename);
            }
            catch (Exception e)
            {
                return Content<string>(HttpStatusCode.InternalServerError, e.Message);
            }
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
                return InternalServerError(e.InnerException);
            }
        }
    }
}
