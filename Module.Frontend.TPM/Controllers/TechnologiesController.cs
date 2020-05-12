using AutoMapper;
using Core.Security;
using Core.Security.Models;
using Core.Settings;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
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
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.Results;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers
{

    public class TechnologiesController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public TechnologiesController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }


        protected IQueryable<Technology> GetConstraintedQuery()
        {

            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IQueryable<Technology> query = Context.Set<Technology>().Where(e => !e.Disabled);

            return query;
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<Technology> GetTechnology([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<Technology> GetTechnologies()
        {
            return GetConstraintedQuery();
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<Technology> GetFilteredData(ODataQueryOptions<Technology> options)
        {
            var query = GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<Technology>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<Technology>;
        }

        [ClaimsAuthorize]
        public IHttpActionResult Put([FromODataUri] System.Guid key, Delta<Technology> patch)
        {
            var model = Context.Set<Technology>().Find(key);
            if (model == null)
            {
                return NotFound();
            }
            patch.Put(model);
            try
            {
                var resultSaveChanges = Context.SaveChanges();
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
        public IHttpActionResult Post(Technology model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var proxy = Context.Set<Technology>().Create<Technology>();
            var result = (Technology)Mapper.Map(model, proxy, typeof(Technology), proxy.GetType(), opts => opts.CreateMissingTypeMaps = true);
            Context.Set<Technology>().Add(result);

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

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<Technology> patch)
        {            
            try
            {
                var model = Context.Set<Technology>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }
                var oldName = model.Name;
                patch.Patch(model);
                var newName = model.Name;
                //Асинхронно, т.к. долго выполняется и иначе фронт не дождется ответа
                Task.Run(() => PromoHelper.UpdateProductHierarchy("Technology", newName, oldName, key));
                UpdateProductTrees(model.Id, model.Name);

                var resultSaveChanges = Context.SaveChanges();
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
                var model = Context.Set<Technology>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }
                model.DeletedDate = System.DateTime.Now;
                model.Disabled = true;

                var resultSaveChanges = Context.SaveChanges();
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return InternalServerError(e.InnerException);
            }
        }

        private bool EntityExists(System.Guid key)
        {
            return Context.Set<Technology>().Count(e => e.Id == key) > 0;
        }

        private IEnumerable<Column> GetExportSettings()
        {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 0, Field = "Name", Header = "Technology", Quoting = false },
                new Column() { Order = 1, Field = "Technology", Header = "Tech Code", Quoting = false }
            };
            return columns;
        }
        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<Technology> options)
        {
            try
            {
                IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));
                IEnumerable<Column> columns = GetExportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                UserInfo user = authorizationManager.GetCurrentUser();
                string username = user == null ? "" : user.Login;
                string filePath = exporter.GetExportFileName("Technology", username);
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
        public async Task<HttpResponseMessage> FullImportXLSX() {
            try {
                if (!Request.Content.IsMimeMultipartContent()) {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string importDir = Core.Settings.AppSettingsManager.GetSetting("IMPORT_DIRECTORY", "ImportFiles");
                string fileName = await FileUtility.UploadFile(Request, importDir);

                CreateImportTask(fileName, "FullXLSXUpdateAllHandler");

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
                HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportTechnology), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportTechnology).Name, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(Technology), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UniqueFields", new List<String>() { "Name" }, data);

                LoopHandler handler = new LoopHandler() {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Загрузка импорта из файла " + typeof(ImportTechnology).Name,
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

        [ClaimsAuthorize]
        public IHttpActionResult DownloadTemplateXLSX() {
            try {
                IEnumerable<Column> columns = GetExportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
                string filename = string.Format("{0}Template.xlsx", "Technology");
                if (!Directory.Exists(exportDir)) {
                    Directory.CreateDirectory(exportDir);
                }
                string filePath = Path.Combine(exportDir, filename);
                exporter.Export(Enumerable.Empty<Technology>(), filePath);
                string file = Path.GetFileName(filePath);
                return Content(HttpStatusCode.OK, file);
            } catch (Exception e) {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }

        }

        /// <summary>
        /// Обновить имена узлов в ProductTree
        /// </summary>
        /// <param name="id">Id Технологии</param>
        /// <param name="newName">Новое наименование Технологии</param>
        private void UpdateProductTrees(Guid id, string newName)
        {
            ProductTree[] productTree = Context.Set<ProductTree>().Where(n => n.TechnologyId == id).ToArray();
            for (int i = 0; i < productTree.Length; i++)
            {
                // меняем также FullPathName
                string oldFullPath = productTree[i].FullPathName;
                int ind = oldFullPath.LastIndexOf(">");
                ind = ind < 0 ? 0 : ind + 2;

                productTree[i].FullPathName = oldFullPath.Substring(0, ind) + newName;
                productTree[i].Name = newName;
                ProductTreesController.UpdateFullPathProductTree(productTree[i], Context.Set<ProductTree>());
            }
        }

        private ExceptionResult GetErorrRequest(Exception e)
        {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
            {
                return InternalServerError(new Exception("This Technology has already existed"));
            }
            else
            {
                return InternalServerError(e.InnerException);
            }
        }
    }
}