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

            model.Name = model.Name.Trim();
            model.Tech_code = model.Tech_code?.Trim();
            model.SubBrand = model.SubBrand?.Trim();
            model.SubBrand_code = model.SubBrand_code?.Trim();

            model.SubBrand_code = String.IsNullOrWhiteSpace(model.SubBrand_code) ? null : model.SubBrand_code;
            model.SubBrand = String.IsNullOrWhiteSpace(model.SubBrand) ? null : model.SubBrand;
            if (Context.Set<Technology>().Any(t=>t.Tech_code == model.Tech_code && t.SubBrand_code == model.SubBrand_code && !t.Disabled))
            {
                var errorText = !String.IsNullOrEmpty(model.SubBrand_code) ?
                    $"Technology with tech_code {model.Tech_code} and sub_code {model.SubBrand_code} already exists"
                    : $"Technology with tech_code {model.Tech_code} already exists";
                ModelState.AddModelError("Error", errorText);
                return BadRequest(ModelState);
            }
            if (!String.IsNullOrEmpty(model.SubBrand_code) && String.IsNullOrEmpty(model.SubBrand))
            {
                var errorText = $"Sub Brand required";
                ModelState.AddModelError("Error", errorText);
                return BadRequest(ModelState);
            }else if (String.IsNullOrEmpty(model.SubBrand_code) && !String.IsNullOrEmpty(model.SubBrand))
            {
                var errorText = $"Please, insert Sub Brand Code or delete Sub Brand";
                ModelState.AddModelError("Error", errorText);
                return BadRequest(ModelState);
            }
            var proxy = Context.Set<Technology>().Create<Technology>();
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<Technology, Technology>().ReverseMap());
            var mapper = configuration.CreateMapper();
            var result = mapper.Map(model, proxy);
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

                var patchModel = patch.GetEntity();
                patchModel.Name = patchModel.Name ?? patchModel.Name?.Trim();
                patchModel.Tech_code = patchModel.Tech_code ?? patchModel.Tech_code?.Trim();
                patchModel.SubBrand = patchModel.SubBrand ?? patchModel.SubBrand?.Trim();
                patchModel.SubBrand_code = patchModel.SubBrand_code ?? patchModel.SubBrand_code?.Trim();
                patchModel.IsSplittable = patchModel.IsSplittable;

                var oldTC = model.Tech_code;
                var oldTN = model.Name;
                var oldSC = model.SubBrand_code;
                var oldName = model.Name;
                var oldIsSplittable = model.IsSplittable;
                patch.Patch(model);

                var newName = model.Name;
                if (!String.IsNullOrEmpty(model.SubBrand))
                    newName = String.Format("{0} {1}", model.Name, model.SubBrand);

                //Асинхронно, т.к. долго выполняется и иначе фронт не дождется ответа
                Task.Run(() => PromoHelper.UpdateProductHierarchy("Technology", newName, oldName, key));
                UpdateProductTrees(model.Id, newName);
                //Асинхронно, т.к. долго выполняется и иначе фронт не дождется ответа
                Task.Run(() => UpdateProducts(model, oldTC, oldTN, oldSC));

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

        public static IEnumerable<Column> GetExportSettings()
        {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 0, Field = "Name", Header = "Technology", Quoting = false },
                new Column() { Order = 1, Field = "Description_ru", Header = "Technology RU", Quoting = false },
                new Column() { Order = 2, Field = "Tech_code", Header = "Tech Code", Quoting = false },
                new Column() { Order = 3, Field = "SubBrand", Header = "Sub Brand", Quoting = false },
                new Column() { Order = 4, Field = "SubBrand_code", Header = "Sub Brand Code", Quoting = false },
                new Column() { Order = 5, Field = "Splittable", Header = "Splittable", Quoting = false }
            };
            return columns;
        }

        public static IEnumerable<Column> GetImportSettings()
        {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 0, Field = "Name", Header = "Technology", Quoting = false },
                new Column() { Order = 1, Field = "Description_ru", Header = "Technology RU", Quoting = false },
                new Column() { Order = 2, Field = "Tech_code", Header = "Tech Code", Quoting = false },
                new Column() { Order = 3, Field = "SubBrand", Header = "Sub Brand", Quoting = false },
                new Column() { Order = 4, Field = "SubBrand_code", Header = "Sub Brand Code", Quoting = false },
                new Column() { Order = 5, Field = "Splittable", Header = "Splittable", Quoting = false },
                new Column() { Order = 6, Header = "", Quoting = false },
                new Column() { Order = 7, Header = "*Column Splittable: '+' - splittable, '-' - not splittable", Quoting = false }
            };
            return columns;
        }

        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<Technology> options)
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
                HandlerDataHelper.SaveIncomingArgument("TModel", typeof(TechnologyExport), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(TechnologiesController), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(TechnologiesController.GetExportSettings), data, visible: false, throwIfNotExists: false);
                string query = results.ToTraceQuery().Replace("[Extent1].[IsSplittable] AS [IsSplittable]", "IIF ([Extent1].[IsSplittable] = 1, '+', '-') AS [Splittable]");
                query = query.Replace("[Project1].[IsSplittable] AS [IsSplittable]", "[Project1].[Splittable] AS [Splittable]");
                HandlerDataHelper.SaveIncomingArgument("SqlString", query, data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = $"Export {nameof(Technology)} dictionary",
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
        public async Task<HttpResponseMessage> FullImportXLSX()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string importDir = Core.Settings.AppSettingsManager.GetSetting("IMPORT_DIRECTORY", "ImportFiles");
                string fileName = await FileUtility.UploadFile(Request, importDir);

                CreateImportTask(fileName, "FullXLSXUpdateTechnologyHandler");

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StringContent("success = true");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

                return result;
            }
            catch (Exception e)
            {
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
                HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportTechnology), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportTechnology).Name, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(Technology), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UniqueFields", new List<String>() { "Name" }, data);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Загрузка импорта из файла " + typeof(ImportTechnology).Name,
                    Name = "Module.Host.TPM.Handlers." + importHandler,
                    ExecutionPeriod = null,
                    RunGroup = typeof(ImportTechnology).Name,
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
        public IHttpActionResult DownloadTemplateXLSX()
        {
            try
            {
                IEnumerable<Column> columns = GetImportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
                string filename = string.Format("{0}Template.xlsx", "Technology");
                if (!Directory.Exists(exportDir))
                {
                    Directory.CreateDirectory(exportDir);
                }
                string filePath = Path.Combine(exportDir, filename);
                exporter.Export(Enumerable.Empty<Technology>(), filePath);
                string file = Path.GetFileName(filePath);
                return Content(HttpStatusCode.OK, file);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }

        }

        /// <summary>
        /// Обновить имена узлов в ProductTree
        /// </summary>
        /// <param name="id">Id Технологии</param>
        /// <param name="newName">Новое наименование Технологии</param>
        public static void UpdateProductTrees(Guid id, string newName)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                ProductTree[] productTree = context.Set<ProductTree>().Where(n => n.TechnologyId == id).ToArray();
                for (int i = 0; i < productTree.Length; i++)
                {
                    // меняем также FullPathName
                    string oldFullPath = productTree[i].FullPathName;
                    int ind = oldFullPath.LastIndexOf(">");
                    ind = ind < 0 ? 0 : ind + 2;

                    productTree[i].FullPathName = oldFullPath.Substring(0, ind) + newName;
                    productTree[i].Name = newName;
                    ProductTreesController.UpdateFullPathProductTree(productTree[i], context.Set<ProductTree>());
                }
            }
        }

        public static void UpdateProducts(Technology model, string oldTC, string oldTN, string oldSC)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var products = context.Set<Product>().Where(p => p.Tech_code.Equals(oldTC) && p.Technology.Equals(oldTN) && p.SubBrand_code.Equals(oldSC) && !p.Disabled).ToList();
                foreach (var product in products)
                {
                    Delta<Product> newProduct = new Delta<Product>();
                    newProduct.TrySetPropertyValue("SubBrand_code", model.SubBrand_code);
                    newProduct.TrySetPropertyValue("Tech_code", model.Tech_code);
                    newProduct.Patch(product);
                }

                context.SaveChanges();
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