using AutoMapper;
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
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Thinktecture.IdentityModel.Authorization.WebApi;
using System.Net.Http;
using System.Threading.Tasks;
using Frontend.Core.Extensions;
using Persist;
using Looper.Parameters;
using Looper.Core;
using Module.Persist.TPM.Model.Import;
using System.Web.Http.Results;
using System.Data.SqlClient;
using System.Net.Http.Headers;
using Module.Persist.TPM.Utils;
using Module.Frontend.TPM.Util;
using System.Web;

namespace Module.Frontend.TPM.Controllers
{
    public class DemandsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public DemandsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<Demand> GetConstraintedQuery()
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IQueryable<Demand> query = Context.Set<Demand>().Where(e => !e.Disabled);

            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public SingleResult<Demand> GetDemand([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<Demand> GetDemands()
        {
            return GetConstraintedQuery();
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<Demand> GetFilteredData(ODataQueryOptions<Demand> options)
        {
            var query = GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<Demand>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<Demand>;
        }

        [ClaimsAuthorize]
        public IHttpActionResult Put([FromODataUri] System.Guid key, Delta<Demand> patch)
        {
            var model = Context.Set<Demand>().Find(key);
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
        public IHttpActionResult Post(Demand model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var proxy = Context.Set<Demand>().Create<Demand>();
            var result = (Demand)Mapper.Map(model, proxy, typeof(Demand), proxy.GetType(), opts => opts.CreateMissingTypeMaps = true);
            Context.Set<Demand>().Add(result);

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
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<Demand> patch)
        {            
            try
            {
                var model = Context.Set<Demand>().Find(key);
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
                var model = Context.Set<Demand>().Find(key);
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
            return Context.Set<Demand>().Count(e => e.Id == key) > 0;
        }

        public static IEnumerable<Column> GetExportSettings()
        {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 0, Field = "Number", Header = "Promo ID", Quoting = false },
                new Column() { Order = 1, Field = "Name", Header = "Promo name", Quoting = false },
                new Column() { Order = 2, Field = "Client.CommercialSubnet.CommercialNet.Name", Header = "Customer", Quoting = false },
                new Column() { Order = 3, Field = "Brand.Name", Header = "Brand", Quoting = false },
                new Column() { Order = 4, Field = "BrandTech.BrandsegTechsub", Header = "BrandTech", Quoting = false },
                new Column() { Order = 5, Field = "StartDate", Header = "StartDate", Quoting = false, Format = "dd.MM.yyyy HH:mm:ss" },
                new Column() { Order = 6, Field = "EndDate", Header = "EndDate", Quoting = false, Format = "dd.MM.yyyy HH:mm:ss" },
                new Column() { Order = 7, Field = "DispatchesStart", Header = "DispatchesStart", Quoting = false, Format = "dd.MM.yyyy HH:mm:ss" },
                new Column() { Order = 8, Field = "DispatchesEnd", Header = "DispatchesEnd", Quoting = false, Format = "dd.MM.yyyy HH:mm:ss" },
                new Column() { Order = 9, Field = "PlanBaseline", Header = "PlanBaseline", Quoting = false },
                new Column() { Order = 10, Field = "PlanDuration", Header = "PlanDuration", Quoting = false },
                new Column() { Order = 11, Field = "PlanUplift", Header = "PlanUplift", Quoting = false },
                new Column() { Order = 12, Field = "PlanIncremental", Header = "PlanIncremental", Quoting = false },
                new Column() { Order = 13, Field = "PlanActivity", Header = "PlanActivity", Quoting = false },
                new Column() { Order = 14, Field = "PlanSteal", Header = "PlanSteal", Quoting = false },
                new Column() { Order = 15, Field = "FactBaseline", Header = "FactBaseline", Quoting = false },
                new Column() { Order = 16, Field = "FactDuration", Header = "FactDuration", Quoting = false },
                new Column() { Order = 17, Field = "FactUplift", Header = "FactUplift", Quoting = false },
                new Column() { Order = 18, Field = "FactIncremental", Header = "FactIncremental", Quoting = false },
                new Column() { Order = 19, Field = "FactActivity", Header = "FactActivity", Quoting = false },
                new Column() { Order = 20, Field = "FactSteal", Header = "FactSteal", Quoting = false },
            };
            return columns;
        }

        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<Demand> options)
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
                HandlerDataHelper.SaveIncomingArgument("TModel", typeof(Demand), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(DemandsController), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(DemandsController.GetExportSettings), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = $"Export {nameof(Demand)} dictionary",
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

        [ClaimsAuthorize]
        public async Task<HttpResponseMessage> FullImportXLSX() {
            try {
                if (!Request.Content.IsMimeMultipartContent()) {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string importDir = Core.Settings.AppSettingsManager.GetSetting("IMPORT_DIRECTORY", "ImportFiles");
                string fileName = await FileUtility.UploadFile(Request, importDir);

                CreateImportTask(fileName, "FullXLSXUpdateImportDemandHandler");

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StringContent("success = true");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

                return result;
            } catch (Exception e) {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private void CreateImportTask(string fileName, string importHandler)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            using (DatabaseContext context = new DatabaseContext())
            {
                ImportResultFilesModel resiltfile = new ImportResultFilesModel();
                ImportResultModel resultmodel = new ImportResultModel();

                HandlerData data = new HandlerData();
                FileModel file = new FileModel()
                {
                    LogicType = "Import",
                    Name = System.IO.Path.GetFileName(fileName),
                    DisplayName = System.IO.Path.GetFileName(fileName)
                };

                HandlerDataHelper.SaveIncomingArgument("File", file, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportDemand), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportDemand).Name, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(Demand), data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Загрузка импорта из файла " + typeof(ImportDemand).Name,
                    Name = "Module.Host.TPM.Handlers." + importHandler,
                    ExecutionPeriod = null,
                    RunGroup = typeof(ImportDemand).Name,
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
        }

        private ExceptionResult GetErorrRequest(Exception e)
        {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
            {
                return InternalServerError(new Exception("This Demand has already existed"));
            }
            else
            {
                return InternalServerError(e.InnerException);
            }
        }
    }
}
