using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers
{
    public class CalendarCompetitorCompanyController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public CalendarCompetitorCompanyController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<CalendarСompetitorCompany> GetConstraintedQuery()
        {
            var user = authorizationManager.GetCurrentUser();
            var role = authorizationManager.GetCurrentRole();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            var query = Context.Set<CalendarСompetitorCompany>().Where(c => !c.Disabled);

            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<CalendarСompetitorCompany> GetCalendarСompetitorCompany([FromODataUri] Guid key) => 
            SingleResult.Create(GetConstraintedQuery());

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<CalendarСompetitorCompany> GetCalendarСompetitorCompanies() =>
            GetConstraintedQuery();

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<CalendarСompetitorCompany> GetFilteredData(ODataQueryOptions<CalendarСompetitorCompany> options)
        {
            var query = GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<CalendarСompetitorCompany>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<CalendarСompetitorCompany>;
        }

        [ClaimsAuthorize]
        public IHttpActionResult Put([FromODataUri] Guid key, Delta<CalendarСompetitorCompany> patch) =>
            UpdateEntity(key, patch);

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] Guid key, Delta<CalendarСompetitorCompany> patch) =>
            UpdateEntity(key, patch);

        [ClaimsAuthorize]
        public IHttpActionResult Delete([FromODataUri] Guid key)
        {
            var model = Context.Set<CalendarСompetitorCompany>().Find(key);
            if (model == null)
            {
                return NotFound();
            }

            model.DeletedDate = DateTime.Now;
            model.Disabled = true;

            try
            {
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        private IHttpActionResult UpdateEntity(Guid key, Delta<CalendarСompetitorCompany> patch)
        {
            var model = Context.Set<CalendarСompetitorCompany>().Find(key);
            if (model == null)
            {
                return NotFound();
            }

            patch.Put(model);

            try
            {
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!EntityExist(key))
                {
                    return NotFound();
                }
                else
                {
                    return InternalServerError(ex);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Updated(model);
        }

        private bool EntityExist(Guid key) =>
            Context.Set<CalendarСompetitorCompany>().Count(e => e.Id == key) > 0;
    }
}
