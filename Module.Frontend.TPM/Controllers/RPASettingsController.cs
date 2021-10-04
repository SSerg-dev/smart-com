using AutoMapper;
using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Module.Frontend.TPM.Model;
using Module.Persist.TPM.Model.TPM;
using Newtonsoft.Json;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.Results;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers
{
    public class RPASettingsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public RPASettingsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<RPASetting> GetConstraintedQuery()
        {

            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();

            IQueryable<RPASetting> query = Context.Set<RPASetting>();
            IQueryable<RPASetting> resultQuery = query.ToList()
                 .Where(x => JsonConvert.DeserializeObject<RPAEventJsonField>(x.Json).roles.Contains(role))
                 .AsQueryable();
            
            return resultQuery;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<RPASetting> GetRPASetting([FromODataUri] System.Guid key)
        {
            return SingleResult.Create<RPASetting>(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<RPASetting> GetRPASettings()
        {
            return GetConstraintedQuery();
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<RPASetting> GetFilteredData(ODataQueryOptions<RPASetting> options)
        {
            var query = GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<RPASetting>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<RPASetting>;
        }

        [ClaimsAuthorize]
        public IHttpActionResult Post(RPASetting model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var proxy = Context.Set<RPASetting>().Create<RPASetting>();
            var result = (RPASetting)Mapper.Map(model, proxy, typeof(RPASetting), proxy.GetType(), opts => opts.CreateMissingTypeMaps = true);
            Context.Set<RPASetting>().Add(result);

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
        private ExceptionResult GetErorrRequest(Exception e)
        {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
            {
                return InternalServerError(new Exception("This RPASetting has already existed"));
            }
            else
            {
                return InternalServerError(e.InnerException);
            }
        }
    }
}
