using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using AutoMapper;
using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions.Export;
using Persist.Model;
using Module.Persist.TPM.Model.TPM;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Newtonsoft.Json;
using Utility;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Utils;
using System.Web.Http.Results;
using System.Data.SqlClient;
using Module.Frontend.TPM.Util;
using System.Web;

namespace Module.Frontend.TPM.Controllers
{
    public class PostPromoEffectsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public PostPromoEffectsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<PostPromoEffect> GetConstraintedQuery()
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IQueryable<PostPromoEffect> query = Context.Set<PostPromoEffect>().Where(e => !e.Disabled);

            return query;
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<PostPromoEffect> GetPostPromoEffect([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<PostPromoEffect> GetPostPromoEffects()
        {
            return GetConstraintedQuery();
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<PostPromoEffect> GetFilteredData(ODataQueryOptions<PostPromoEffect> options)
        {
            var query = GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<PostPromoEffect>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<PostPromoEffect>;
        }

        [ClaimsAuthorize]
        public IHttpActionResult Put([FromODataUri] System.Guid key, Delta<PostPromoEffect> patch)
        {
            var model = Context.Set<PostPromoEffect>().Find(key);
            if (model == null)
            {
                return NotFound();
            }

            patch.Put(model);
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
        public IHttpActionResult Post(PostPromoEffect model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var proxy = Context.Set<PostPromoEffect>().Create<PostPromoEffect>();
            var result = (PostPromoEffect)Mapper.Map(model, proxy, typeof(PostPromoEffect), proxy.GetType(), opts => opts.CreateMissingTypeMaps = true);
            Context.Set<PostPromoEffect>().Add(result);

            try
            {
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                return GetErorrRequest(e);
            }

            return Created(result);
        }

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<PostPromoEffect> patch)
        {            
            try
            {
                var model = Context.Set<PostPromoEffect>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                patch.Patch(model);
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
                var model = Context.Set<PostPromoEffect>().Find(key);
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
            return Context.Set<PostPromoEffect>().Count(e => e.Id == key) > 0;
        }

        private IEnumerable<Column> GetExportSettings()
        {
            IEnumerable<Column> columns = new List<Column>() {                

                new Column() { Order = 0, Field = "StartDate", Header = "Start date", Quoting = false, Format = "dd.MM.yyyy"  },
                new Column() { Order = 1, Field = "EndDate", Header = "End date", Quoting = false, Format = "dd.MM.yyyy"  },
                new Column() { Order = 2, Field = "ClientTree.FullPathName", Header = "Customer", Quoting = false },
                new Column() { Order = 3, Field = "ProductTree.FullPathName", Header = "Product", Quoting = false },
                new Column() { Order = 4, Field = "EffectWeek1", Header = "Effect Week 1, %", Quoting = false },
                new Column() { Order = 5, Field = "EffectWeek2", Header = "Effect Week 2, %", Quoting = false },
                new Column() { Order = 6, Field = "TotalEffect", Header = "Total Effect, %", Quoting = false },

            };
            return columns;
        }
        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<PostPromoEffect> options)
        {
            try
            {
                IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));
                IEnumerable<Column> columns = GetExportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                UserInfo user = authorizationManager.GetCurrentUser();
                string username = user == null ? "" : user.Login;
                string filePath = exporter.GetExportFileName("PostPromoEffect", username);
                exporter.Export(results, filePath);
                string filename = System.IO.Path.GetFileName(filePath);
                return Content<string>(HttpStatusCode.OK, filename);
            }
            catch (Exception e)
            {
                return Content<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private ExceptionResult GetErorrRequest(Exception e)
        {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
            {
                return InternalServerError(new Exception("This Post Promo Effect has already existed"));
            }
            else
            {
                return InternalServerError(e.InnerException);
            }
        }
    }
}