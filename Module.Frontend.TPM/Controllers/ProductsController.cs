using AutoMapper;
using Core.Security;
using Core.Security.Models;
using Core.Settings;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Persist.TPM.Model.TPM;
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
    public class ProductsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public ProductsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<Product> GetConstraintedQuery()
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IQueryable<Product> query = Context.Set<Product>().Where(e => !e.Disabled);

            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<Product> GetProduct([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<Product> GetProducts()
        {
            return GetConstraintedQuery();
        }

        [ClaimsAuthorize]
        public IHttpActionResult Put([FromODataUri] System.Guid key, Delta<Product> patch)
        {
            var model = Context.Set<Product>().Find(key);
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
        public IHttpActionResult Post(Product model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var proxy = Context.Set<Product>().Create<Product>();
            var result = (Product)Mapper.Map(model, proxy, typeof(Product), proxy.GetType(), opts => opts.CreateMissingTypeMaps = true);

            try
            {
                CheckNewBrandTech(result.BrandFlag, result.SupplySegment);
                Context.Set<Product>().Add(result);
                Context.Set<ProductChangeIncident>().Add(CreateIncident(result, true, false));
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
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<Product> patch)
        {
            
            try
            {
                var model = Context.Set<Product>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                patch.Patch(model);
                CheckNewBrandTech(model.BrandFlag, model.SupplySegment);
                Context.Set<ProductChangeIncident>().Add(CreateIncident(model, false, false));
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
                var model = Context.Set<Product>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                model.DeletedDate = System.DateTime.Now;
                model.Disabled = true;
                Context.Set<ProductChangeIncident>().Add(CreateIncident(model, true, false));
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
            return Context.Set<Product>().Count(e => e.Id == key) > 0;
        }

        private IEnumerable<Column> GetExportSettings()
        {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 0, Field = "ZREP", Header = "ZREP", Quoting = false },
                new Column() { Order = 1, Field = "EAN", Header = "EAN", Quoting = false },
                new Column() { Order = 2, Field = "ProductRU", Header = "Product RU", Quoting = false },
                new Column() { Order = 3, Field = "ProductEN", Header = "Product EN", Quoting = false },
                new Column() { Order = 4, Field = "BrandFlagAbbr", Header = "Brand flag abbr", Quoting = false },
                new Column() { Order = 5, Field = "BrandFlag", Header = "Brand flag", Quoting = false },
                new Column() { Order = 6, Field = "SubmarkFlag", Header = "Submark flag", Quoting = false },
                new Column() { Order = 7, Field = "IngredientVariety", Header = "Ingredient variety", Quoting = false },
                new Column() { Order = 8, Field = "ProductCategory", Header = "Product category", Quoting = false },
                new Column() { Order = 9, Field = "ProductType", Header = "Product type", Quoting = false },
                new Column() { Order = 10, Field = "MarketSegment", Header = "Market segment", Quoting = false },
                new Column() { Order = 11, Field = "SupplySegment", Header = "Supply segment", Quoting = false },
                new Column() { Order = 12, Field = "FunctionalVariety", Header = "Functional variety", Quoting = false },
                new Column() { Order = 13, Field = "Size", Header = "Size", Quoting = false },
                new Column() { Order = 14, Field = "BrandEssence", Header = "Brand essence", Quoting = false },
                new Column() { Order = 15, Field = "PackType", Header = "Pack type", Quoting = false },
                new Column() { Order = 16, Field = "GroupSize", Header = "Group size", Quoting = false },
                new Column() { Order = 17, Field = "TradedUnitFormat", Header = "Traded unit format", Quoting = false },
                new Column() { Order = 18, Field = "ConsumerPackFormat", Header = "Consumer pack format", Quoting = false },
                new Column() { Order = 19, Field = "UOM_PC2Case", Header = "UOM_PC2Case", Quoting = false }
            };
            return columns;
        }

        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<Product> options)
        {
            try
            {
                IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));
                IEnumerable<Column> columns = GetExportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                UserInfo user = authorizationManager.GetCurrentUser();
                string username = user == null ? "" : user.Login;
                string filePath = exporter.GetExportFileName("Product", username);
                exporter.Export(results, filePath);
                string filename = System.IO.Path.GetFileName(filePath);
                return Content<string>(HttpStatusCode.OK, filename);
            }
            catch (Exception e)
            {
                return Content<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Проверка на наличие Бренда и Технологии
        /// </summary>
        /// <param name="brand">Наименование Бренда</param>
        /// <param name="tech">Наименование Технологии</param>
        private void CheckNewBrandTech(string brand, string tech)
        {
            if (brand.Length != 0 && tech.Length != 0)
            {
                Brand checkBrand = Context.Set<Brand>().FirstOrDefault(n => n.Name.ToLower() == brand.ToLower() && !n.Disabled);
                Technology checkTech = Context.Set<Technology>().FirstOrDefault(n => n.Name.ToLower() == tech.ToLower() && !n.Disabled);

                if (checkBrand == null || checkTech == null)
                {
                    if (checkBrand == null)
                    {
                        checkBrand = new Brand { Disabled = false, Name = brand };
                        Context.Set<Brand>().Add(checkBrand);
                    }

                    if (checkTech == null)
                    {
                        checkTech = new Technology { Disabled = false, Name = tech };
                        Context.Set<Technology>().Add(checkTech);
                    }

                    Context.SaveChanges();

                    BrandTech newBrandTech = new BrandTech { Disabled = false, BrandId = checkBrand.Id, TechnologyId = checkTech.Id };
                    Context.Set<BrandTech>().Add(newBrandTech);
                    Context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Создание записи об создании/удалении/изменении продукта
        /// </summary>
        /// <param name="product"></param>
        /// <param name="isCreate"></param>
        /// <param name="isDelete"></param>
        /// <returns></returns>
        private ProductChangeIncident CreateIncident(Product product, bool isCreate, bool isDelete) {
            return new ProductChangeIncident {
                CreateDate = DateTime.Now,
                ProductId = product.Id,
                IsCreate = isCreate,
                IsDelete = isDelete
            };
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
                HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportProduct), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportProduct).Name, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(Product), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UniqueFields", new List<String>() { "ZREP" }, data);

                LoopHandler handler = new LoopHandler() {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Загрузка импорта из файла " + typeof(ImportProduct).Name,
                    Name = "Module.Host.TPM.Handlers." + importHandler,
                    ExecutionPeriod = null,
                    CreateDate = DateTimeOffset.Now,
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
                string filename = string.Format("{0}Template.xlsx", "Product");
                if (!Directory.Exists(exportDir)) {
                    Directory.CreateDirectory(exportDir);
                }
                string filePath = Path.Combine(exportDir, filename);
                exporter.Export(Enumerable.Empty<Product>(), filePath);
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
                return InternalServerError(new Exception("This Product has already existed"));
            }
            else
            {
                return InternalServerError(e.InnerException);
            }
        }
    }
}
