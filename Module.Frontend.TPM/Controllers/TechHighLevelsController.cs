﻿using AutoMapper;
using Core.Security;
using Core.Security.Models;
using Core.Settings;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions.Export;
using Module.Persist.TPM.Model.TPM;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.Results;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers
{

    public class TechHighLevelsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public TechHighLevelsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }


        protected IQueryable<TechHighLevel> GetConstraintedQuery()
        {

            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IQueryable<TechHighLevel> query = Context.Set<TechHighLevel>().Where(e => !e.Disabled);

            return query;
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<TechHighLevel> GetTechHighLevel([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<TechHighLevel> GetTechHighLevels()
        {
            return GetConstraintedQuery();
        }


        [ClaimsAuthorize]
        public IHttpActionResult Put([FromODataUri] System.Guid key, Delta<TechHighLevel> patch)
        {
            var model = Context.Set<TechHighLevel>().Find(key);
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
        public IHttpActionResult Post(TechHighLevel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var proxy = Context.Set<TechHighLevel>().Create<TechHighLevel>();
            var result = (TechHighLevel)Mapper.Map(model, proxy, typeof(TechHighLevel), proxy.GetType(), opts => opts.CreateMissingTypeMaps = true);
            Context.Set<TechHighLevel>().Add(result);

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
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<TechHighLevel> patch)
        {            
            try
            {
                var model = Context.Set<TechHighLevel>().Find(key);
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
                var model = Context.Set<TechHighLevel>().Find(key);
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
            return Context.Set<TechHighLevel>().Count(e => e.Id == key) > 0;
        }

        private IEnumerable<Column> GetExportSettings()
        {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 0, Field = "Name", Header = "Tech high level", Quoting = false }
            };
            return columns;
        }
        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<TechHighLevel> options)
        {
            try
            {
                IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));
                IEnumerable<Column> columns = GetExportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                UserInfo user = authorizationManager.GetCurrentUser();
                string username = user == null ? "" : user.Login;
                string filePath = exporter.GetExportFileName("TechHighLevel", username);
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
                return InternalServerError(new Exception("This Tech High Level has already existed"));
            }
            else
            {
                return InternalServerError(e.InnerException);
            }
        }
    }
}