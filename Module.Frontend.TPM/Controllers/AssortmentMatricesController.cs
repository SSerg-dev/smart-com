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
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.Results;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;

namespace Module.Frontend.TPM.Controllers
{

    public class AssortmentMatricesController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public AssortmentMatricesController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }


        protected IQueryable<AssortmentMatrix> GetConstraintedQuery(bool needActualAssortmentMatrix = false)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();

            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);         
            IQueryable<AssortmentMatrix> query = Context.Set<AssortmentMatrix>().Where(e => !e.Disabled);
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();

            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

            if (needActualAssortmentMatrix)
            {
                List<AssortmentMatrix> materializedQuery = query.ToList();
                List<AssortmentMatrix> actualAssortmentMatrix = GetActualAssortmentMatrix();
                query = materializedQuery.Intersect(actualAssortmentMatrix).AsQueryable();
            }

            return query;
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<AssortmentMatrix> GetAssortmentMatrix([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<AssortmentMatrix> GetAssortmentMatrices(bool needActualAssortmentMatrix)
        {
            return GetConstraintedQuery(needActualAssortmentMatrix);
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<AssortmentMatrix> GetFilteredData(ODataQueryOptions<AssortmentMatrix> options)
        {
            var bodyText = HttpContext.Current.Request.GetRequestBody();
            
            var query = JsonHelper.IsValueExists(bodyText, "needActualAssortmentMatrix") 
                ? GetConstraintedQuery(JsonHelper.GetValueIfExists<bool>(bodyText, "needActualAssortmentMatrix")) 
                : GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<AssortmentMatrix>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<AssortmentMatrix>;
        }

        [ClaimsAuthorize]
        public IHttpActionResult Put([FromODataUri] System.Guid key, Delta<AssortmentMatrix> patch)
        {
            var model = Context.Set<AssortmentMatrix>().Find(key);
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
        public IHttpActionResult Post(AssortmentMatrix model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            model.CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
            // делаем UTC +3
            model.StartDate = ChangeTimeZoneUtil.ResetTimeZone(model.StartDate);
            model.EndDate = ChangeTimeZoneUtil.ResetTimeZone(model.EndDate);

            var proxy = Context.Set<AssortmentMatrix>().Create<AssortmentMatrix>();
            var result = (AssortmentMatrix)Mapper.Map(model, proxy, typeof(AssortmentMatrix), proxy.GetType(), opts => opts.CreateMissingTypeMaps = true);

            Context.Set<AssortmentMatrix>().Add(result);

			var pci = new ProductChangeIncident
			{
				CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
				IsCreate = false,
				IsDelete = false,
				IsCreateInMatrix = true,
				IsDeleteInMatrix = false,
				IsChecked = false,
				Product = result.Product,
				ProductId = result.ProductId
			};
			Context.Set<ProductChangeIncident>().Add(pci);

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
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<AssortmentMatrix> patch)
        {            
            try
            {
                var model = Context.Set<AssortmentMatrix>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                var oldAssortmentMatrix = new AssortmentMatrix
                {
                    Id = new Guid(),
                    Disabled = true,
                    Number = model.Number,
                    DeletedDate = null,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    CreateDate = model.CreateDate,
                    ClientTreeId = model.ClientTreeId,
                    ProductId = model.ProductId,
                    Product = model.Product,
                    ClientTree = model.ClientTree
                };
                Context.Set<AssortmentMatrix>().Add(oldAssortmentMatrix);

				var pci = new ProductChangeIncident
				{
					CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
					IsCreate = false,
					IsDelete = false,
					IsCreateInMatrix = true,
					IsDeleteInMatrix = true,
					IsChecked = false,
					Product = model.Product,
					ProductId = model.ProductId
				};
				Context.Set<ProductChangeIncident>().Add(pci);

                //запись в таблицу ChangesIncident здесь создавать не надо, т.к. есть триггер на UPDATE

                model.CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);

                patch.Patch(model);

                // делаем UTC +3
                model.StartDate = ChangeTimeZoneUtil.ResetTimeZone(model.StartDate);
                model.EndDate = ChangeTimeZoneUtil.ResetTimeZone(model.EndDate);

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
                var model = Context.Set<AssortmentMatrix>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

				var pci = new ProductChangeIncident
				{
					CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
					IsCreate = false,
					IsDelete = false,
					IsCreateInMatrix = false,
					IsDeleteInMatrix = true,
					IsChecked = false,
					Product = model.Product,
					ProductId = model.ProductId
				};
				Context.Set<ProductChangeIncident>().Add(pci);

				model.DeletedDate = DateTime.Now;
                model.Disabled = true;
                Context.SaveChanges();

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return InternalServerError(e.InnerException);
            }
        }
        
        private bool EntityExists(Guid key)
        {
            return Context.Set<AssortmentMatrix>().Count(e => e.Id == key) > 0;
        }

        private IEnumerable<Column> GetExportSettings()
        {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 0, Field = "Number", Header = "ID", Quoting = false },
                new Column() { Order = 1, Field = "ClientTree.FullPathName", Header = "Client", Quoting = false },
                new Column() { Order = 2, Field = "ClientTree.ObjectId", Header = "Client hierarchy code", Quoting = false },
                new Column() { Order = 3, Field = "Product.EAN_PC", Header = "EAN PC", Quoting = false },
                new Column() { Order = 5, Field = "StartDate", Header = "Start Date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 6, Field = "EndDate", Header = "End Date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 7, Field = "CreateDate", Header = "Create Date", Quoting = false, Format = "dd.MM.yyyy" }
            };
            return columns;
        }

        private IEnumerable<Column> GetColumnsImportSettings()
        {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 0, Field = "ClientTree.FullPathName", Header = "Client", Quoting = false },
                new Column() { Order = 1, Field = "ClientTree.ObjectId", Header = "Client hierarchy code", Quoting = false },
                new Column() { Order = 2, Field = "Product.EAN_PC", Header = "EAN PC", Quoting = false },
                new Column() { Order = 3, Field = "StartDate", Header = "Start Date", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 4, Field = "EndDate", Header = "End Date", Quoting = false, Format = "dd.MM.yyyy" }
            };
            return columns;
        }

        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<AssortmentMatrix> options)
        {
            try
            {
                IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));
                IEnumerable<Column> columns = GetExportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                UserInfo user = authorizationManager.GetCurrentUser();
                string username = user == null ? "" : user.Login;
                string filePath = exporter.GetExportFileName("AssortmentMatrix", username);
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
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string importDir = Core.Settings.AppSettingsManager.GetSetting("IMPORT_DIRECTORY", "ImportFiles");
                string fileName = await FileUtility.UploadFile(Request, importDir);

                CreateImportTask(fileName, "FullXLSXAssortmentMatrixImportHandler");

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
                HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportAssortmentMatrix), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportAssortmentMatrix).Name, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(ImportAssortmentMatrix), data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler() {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Загрузка импорта из файла " + typeof(ImportAssortmentMatrix).Name,
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
                IEnumerable<Column> columns = GetColumnsImportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
                string filename = string.Format("{0}Template.xlsx", "AssortmentMatrix");
                if (!Directory.Exists(exportDir)) {
                    Directory.CreateDirectory(exportDir);
                }
                string filePath = Path.Combine(exportDir, filename);
                exporter.Export(Enumerable.Empty<AssortmentMatrix>(), filePath);
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
                return InternalServerError(new Exception("This AssortmentMatrix has already existed"));
            }
            else
            {
                return InternalServerError(e.InnerException);
            }
        }

        /// <summary>
        /// This method returns the actual assortment matrix. 
        /// </summary>
        private List<AssortmentMatrix> GetActualAssortmentMatrix()
        {
            var resultAssortmentMatrix = new List<AssortmentMatrix>();
            var clientProductAssortmentMatrixGroups = Context.Set<AssortmentMatrix>().GroupBy(x => new { x.ClientTreeId, x.ProductId });

            foreach (var clientProductAssortmentMatrixGroup in clientProductAssortmentMatrixGroups)
            {
                var record = clientProductAssortmentMatrixGroup
                    .Where(x => x.EndDate >= new DateTime(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTimeOffset.Now.Day) && !x.Disabled)
                    .OrderByDescending(x => x.CreateDate).FirstOrDefault();

                if (record != null)
                {
                    resultAssortmentMatrix.Add(record);
                }
            }

            return resultAssortmentMatrix;
        }
    }
}