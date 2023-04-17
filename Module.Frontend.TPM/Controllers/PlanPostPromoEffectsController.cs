using AutoMapper;
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
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.SimpleModel;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Persist;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.Results;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;
using Utility.FileWorker;

namespace Module.Frontend.TPM.Controllers
{
    public class PlanPostPromoEffectsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;
        
        public PlanPostPromoEffectsController(IAuthorizationManager authorizationManager) {
            this.authorizationManager = authorizationManager;
        }
        
        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<PlanPostPromoEffect> GetPlanPostPromoEffects() {
            return GetConstraintedQuery();
        }        

        protected IQueryable<PlanPostPromoEffect> GetConstraintedQuery() {

            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();

            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<PlanPostPromoEffect> query = Context.Set<PlanPostPromoEffect>().Where(e => !e.Disabled);
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();

            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

            return query;
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<PlanPostPromoEffect> GetFilteredData(ODataQueryOptions<PlanPostPromoEffect> options)
        {
            var query = GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<PlanPostPromoEffect>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<PlanPostPromoEffect>;
        }
        
        [ClaimsAuthorize]
        public IHttpActionResult Put([FromODataUri] System.Guid key, Delta<COGS> patch) {
            var model = Context.Set<COGS>().Find(key);
            if (model == null) {
                return NotFound();
            }
            patch.Put(model);
            try {
                Context.SaveChanges();
            } catch (DbUpdateConcurrencyException) {
                if (!EntityExists(key)) {
                    return NotFound();
                } else {
                    throw;
                }
            }
            return Updated(model);
        }
        
        [ClaimsAuthorize]
        public IHttpActionResult Post(PlanPostPromoEffect model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            
            var proxy = Context.Set<PlanPostPromoEffect>().Create<PlanPostPromoEffect>();
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<PlanPostPromoEffect, PlanPostPromoEffect>().ReverseMap());
            var mapper = configuration.CreateMapper();
            var result = mapper.Map(model, proxy);

            Context.Set<PlanPostPromoEffect>().Add(result);

            try {
                Context.SaveChanges();
            } catch (Exception e) {
                return GetErorrRequest(e);
            }

            return Created(model);
        }
        
        [ClaimsAuthorize]
        public IHttpActionResult Delete([FromODataUri] System.Guid key) {
            try {
                var model = Context.Set<PlanPostPromoEffect>().Find(key);
                if (model == null) {
                    return NotFound();
                }

                model.DeletedDate = System.DateTime.Now;
                model.Disabled = true;
                
                Context.SaveChanges();

                return StatusCode(HttpStatusCode.NoContent);
            } catch (Exception e) {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<PlanPostPromoEffect> patch) {
            return NotFound();
        }
        
        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<COGS> options) 
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
                HandlerDataHelper.SaveIncomingArgument("TModel", typeof(COGS), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(COGSsController), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(COGSsController.GetExportSettings), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = $"Export {nameof(COGS)} dictionary",
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

                NameValueCollection form = System.Web.HttpContext.Current.Request.Form;
                CreateImportTask(fileName, "FullXLSXPlanPostPromoEffectUpdateImportHandler", form);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StringContent("success = true");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

                return result;
            } catch (Exception e) {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private void CreateImportTask(string fileName, string importHandler, NameValueCollection paramForm)
        {
        }

        private bool EntityExists(System.Guid key) {
            return Context.Set<COGS>().Count(e => e.Id == key) > 0;
        }
        
        private ExceptionResult GetErorrRequest(Exception e) {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601)) {
                return InternalServerError(new Exception("This PlanPostPromoEffect has already existed"));
            } else {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }
    }
}