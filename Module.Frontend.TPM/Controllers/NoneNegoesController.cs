using AutoMapper;
using Core.Security;
using Core.Security.Models;
using Core.Settings;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Model;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Newtonsoft.Json;
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
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.Results;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;

namespace Module.Frontend.TPM.Controllers
{

    public class NoneNegoesController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public NoneNegoesController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }


        protected IQueryable<NoneNego> GetConstraintedQuery()
        {

            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();

            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<NoneNego> query = Context.Set<NoneNego>().Where(e => !e.Disabled);
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);
            return query;
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<NoneNego> GetNoneNego([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<NoneNego> GetNoneNegoes()
        {
            return GetConstraintedQuery();
        }


        [ClaimsAuthorize]
        public IHttpActionResult Put([FromODataUri] System.Guid key, Delta<NoneNego> patch)
        {
            var model = Context.Set<NoneNego>().Find(key);
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
        public IHttpActionResult Post(NoneNego model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // делаем UTC +3
            model.CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
            model.FromDate = ChangeTimeZoneUtil.ResetTimeZone(model.FromDate);
            model.ToDate = ChangeTimeZoneUtil.ResetTimeZone(model.ToDate);

            var proxy = Context.Set<NoneNego>().Create<NoneNego>();
            var result = (NoneNego)Mapper.Map(model, proxy, typeof(NoneNego), proxy.GetType(), opts => opts.CreateMissingTypeMaps = true);
            Context.Set<NoneNego>().Add(result);

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
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<NoneNego> patch)
        {            
            try
            {
                var model = Context.Set<NoneNego>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                patch.Patch(model);
                // делаем UTC +3
                model.CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                model.FromDate = ChangeTimeZoneUtil.ResetTimeZone(model.FromDate);
                model.ToDate = ChangeTimeZoneUtil.ResetTimeZone(model.ToDate);

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
                var model = Context.Set<NoneNego>().Find(key);
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
            return Context.Set<NoneNego>().Count(e => e.Id == key) > 0;
        }

        private IEnumerable<Column> GetExportSettings()
        {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 0, Field = "ClientTree.FullPathName", Header = "Client", Quoting = false },
                new Column() { Order = 1, Field = "ClientTree.ObjectId", Header = "Client hierarchy code", Quoting = false },
                new Column() { Order = 2, Field = "ProductTree.FullPathName", Header = "Product", Quoting = false },
                new Column() { Order = 3, Field = "ProductTree.ObjectId", Header = "Product hierarchy code", Quoting = false },
                new Column() { Order = 4, Field = "Mechanic.Name", Header = "Mechanic", Quoting = false },
                new Column() { Order = 5, Field = "MechanicType.Name", Header = "Mechanic type", Quoting = false },
                new Column() { Order = 6, Field = "Discount", Header = "Discount", Quoting = false },
                new Column() { Order = 7, Field = "FromDate", Header = "From date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 8, Field = "ToDate", Header = "To date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 9, Field = "CreateDate", Header = "Create date", Quoting = false, Format = "dd.MM.yyyy" }
            };
            return columns;
        }
        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<NoneNego> options)
        {
            try
            {
                IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));
                IEnumerable<Column> columns = GetExportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                UserInfo user = authorizationManager.GetCurrentUser();
                string username = user == null ? "" : user.Login;
                string filePath = exporter.GetExportFileName("NoneNego", username);
                exporter.Export(results, filePath);
                string filename = System.IO.Path.GetFileName(filePath);
                return Content<string>(HttpStatusCode.OK, filename);
            }
            catch (Exception e)
            {
                return Content<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [ClaimsAuthorize]
        public IHttpActionResult DownloadTemplateXLSX() {
            try {
                IEnumerable<Column> columns = GetExportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
                string filename = string.Format("{0}Template.xlsx", "NoNego");
                if (!Directory.Exists(exportDir)) {
                    Directory.CreateDirectory(exportDir);
                }
                string filePath = Path.Combine(exportDir, filename);
                exporter.Export(Enumerable.Empty<RejectReason>(), filePath);
                string file = Path.GetFileName(filePath);
                return Content(HttpStatusCode.OK, file);
            } catch (Exception e) {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }

        }

        [ClaimsAuthorize]
        public async Task<HttpResponseMessage> FullImportXLSX() {
            try {
                if (!Request.Content.IsMimeMultipartContent()) {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string importDir = Core.Settings.AppSettingsManager.GetSetting("IMPORT_DIRECTORY", "ImportFiles");
                string fileName = await FileUtility.UploadFile(Request, importDir);

                CreateImportTask(fileName, "FullXLSXNoNegoUpdateImporHandler");

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StringContent("success = true");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

                return result;
            } catch (Exception e) {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private void CreateImportTask(string fileName, string importHandler) {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            using (DatabaseContext context = new DatabaseContext()) {
                ImportResultFilesModel resiltfile = new ImportResultFilesModel();
                ImportResultModel resultmodel = new ImportResultModel();

                HandlerData data = new HandlerData();
                FileModel file = new FileModel() {
                    LogicType = "Import",
                    Name = System.IO.Path.GetFileName(fileName),
                    DisplayName = System.IO.Path.GetFileName(fileName)
                };

                HandlerDataHelper.SaveIncomingArgument("File", file, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportNoNego), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportNoNego).Name, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(ImportNoNego), data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler() {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Загрузка импорта из файла " + typeof(ImportNoNego).Name,
                    Name = "Module.Host.TPM.Handlers." + importHandler,
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
        }

        [HttpPost]
        [ClaimsAuthorize]
        public IHttpActionResult IsValidPeriod([FromODataUri] DateTimeOffset? fromDate, [FromODataUri] DateTimeOffset? toDate, Guid? noneNegoId, int clientTreeId, int productTreeId, Guid mechanicId)
        {
            List<NoneNego> neededNoneNegoes = null;

            fromDate = ChangeTimeZoneUtil.ChangeTimeZone(fromDate);
            toDate = ChangeTimeZoneUtil.ChangeTimeZone(toDate);

            // При создании новой записи.
            if (noneNegoId == null)
            {
                neededNoneNegoes = GetNoneNegoes().Where(x => 
                    (x.ClientTreeId == clientTreeId) && (x.ProductTreeId == productTreeId) 
                        && (x.MechanicId.Value == mechanicId)).ToList();
            }
            // При редактировании выбранной записи.
            else
            {
                neededNoneNegoes = GetNoneNegoes().Where(x => 
                    (x.ClientTreeId == clientTreeId) && (x.ProductTreeId == productTreeId) 
                        && (x.MechanicId.Value == mechanicId) && (x.Id != noneNegoId.Value)).ToList();
            }

            if (neededNoneNegoes.Count != 0)
            {
                if (toDate == null)
                {
                    foreach (var noneNego in neededNoneNegoes)
                    {
                        // [---] [===]
                        if (noneNego.ToDate > fromDate.Value.AddDays(-1))
                        {
                            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false }));
                        }
                    }

                    return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
                }

                foreach (var noneNego in neededNoneNegoes)
                {
                    // ([---] [===]) || ([===] [---])
                    if (!(((noneNego.FromDate < fromDate) && (noneNego.ToDate < fromDate)) || ((noneNego.FromDate > toDate) && (noneNego.ToDate > toDate))))
                    {
                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false }));
                    }
                }
            }

            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
        }

        private ExceptionResult GetErorrRequest(Exception e)
        {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
            {
                return InternalServerError(new Exception("This NoneNego has already existed"));
            }
            else
            {
                return InternalServerError(e.InnerException);
            }
        }
    }
}