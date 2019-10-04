using AutoMapper;
using Core.Security;
using Core.Security.Models;
using Core.Settings;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
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
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.Results;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers
{

    public class BudgetSubItemsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public BudgetSubItemsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<BudgetSubItem> GetConstraintedQuery()
        {

            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IQueryable<BudgetSubItem> query = Context.Set<BudgetSubItem>().Where(e => !e.Disabled);

            return query;
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<BudgetSubItem> GetBudgetSubItem([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<BudgetSubItem> GetBudgetSubItems()
        {
            return GetConstraintedQuery();
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<BudgetSubItem> GetBudgetSubItems([FromODataUri] int ClientTreeId, Guid BudgetItemId)
        {
            var budgetSubItems = GetConstraintedQuery();
            var budgetSubItemsType = budgetSubItems.Where(b => b.BudgetItemId == BudgetItemId).First().BudgetItem.Name;

            var queryClientTree = Context.Set<BudgetSubItemClientTree>().Where(e => e.BudgetSubItem.BudgetItem.Name == budgetSubItemsType);
            IQueryable<BudgetSubItem> query = Context.Set<BudgetSubItem>().Where(e => !e.Disabled && e.BudgetItem.Name == budgetSubItemsType && !queryClientTree.Any(x => x.BudgetSubItemId == e.Id));
            var BSICT = Context.Set<BudgetSubItemClientTree>().Where(x => x.ClientTreeId == ClientTreeId);
            budgetSubItems = budgetSubItems.Where(bsi => BSICT.Any(x => x.BudgetSubItemId == bsi.Id) && bsi.BudgetItemId == BudgetItemId);
            var result = budgetSubItems.ToList().Concat(query);
            return result.AsQueryable();
        }

        [ClaimsAuthorize]
        public IHttpActionResult Put([FromODataUri] System.Guid key, Delta<BudgetSubItem> patch)
        {
            var model = Context.Set<BudgetSubItem>().Find(key);
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
        public IHttpActionResult Post(BudgetSubItem model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var proxy = Context.Set<BudgetSubItem>().Create<BudgetSubItem>();
            var result = (BudgetSubItem)Mapper.Map(model, proxy, typeof(BudgetSubItem), proxy.GetType(), opts => opts.CreateMissingTypeMaps = true);

            Context.Set<BudgetSubItem>().Add(result);

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
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<BudgetSubItem> patch)
        {
            try
            {
                var model = Context.Set<BudgetSubItem>().Find(key);
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
                    return NotFound();
                else
                    throw;
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
                var model = Context.Set<BudgetSubItem>().Find(key);
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
            return Context.Set<BudgetSubItem>().Count(e => e.Id == key) > 0;
        }

        private IEnumerable<Column> GetExportSettings()
        {
            IEnumerable<Column> columns = new List<Column>() {                               
                new Column() { Order = 0, Field = "BudgetItem.Budget.Name", Header = "Budget", Quoting = false },
                new Column() { Order = 1, Field = "BudgetItem.Name", Header = "Item", Quoting = false },
                new Column() { Order = 2, Field = "Name", Header = "Sub-Item", Quoting = false }
            };
            return columns;
        }
        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<BudgetSubItem> options)
        {
            try
            {
                IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));
                IEnumerable<Column> columns = GetExportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                UserInfo user = authorizationManager.GetCurrentUser();
                string username = user == null ? "" : user.Login;
                string filePath = exporter.GetExportFileName("BudgetSubItem", username);
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

                CreateImportTask(fileName, "FullXLSXUpdateBudgetSubItemHandler");

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
                HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportBudgetSubItem), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportBudgetSubItem).Name, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(ImportBudgetSubItem), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UniqueFields", new List<String>() { "Name", "BudgetItemId" }, data);

                LoopHandler handler = new LoopHandler() {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Загрузка импорта из файла " + typeof(ImportBudgetSubItem).Name,
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
                string filename = string.Format("{0}Template.xlsx", "BudgetSubItem");
                if (!Directory.Exists(exportDir)) {
                    Directory.CreateDirectory(exportDir);
                }
                string filePath = Path.Combine(exportDir, filename);
                exporter.Export(Enumerable.Empty<BudgetSubItem>(), filePath);
                string file = Path.GetFileName(filePath);
                return Content(HttpStatusCode.OK, file);
            } catch (Exception e) {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }

        }

        private ExceptionResult GetErorrRequest(Exception e)
        {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
            {
                return InternalServerError(new Exception("This Budget Sub-Item has already existed"));
            }
            else
            {
                return InternalServerError(e.InnerException);
            }
        }
    }
}