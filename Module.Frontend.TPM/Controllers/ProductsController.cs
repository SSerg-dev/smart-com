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
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Module.Persist.TPM.Utils.Filter;
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
using System.Web.Http.OData.Extensions;
using System.Web.Http.OData.Query;
using System.Web.Http.Results;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;

namespace Module.Frontend.TPM.Controllers
{
    public class ProductsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public ProductsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<Product> GetConstraintedQuery(string inOutProductTreeObjectIds = "", bool needInOutFilteredProducts = false, bool needInOutExcludeAssortmentMatrixProducts = false)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IQueryable<Product> query = Context.Set<Product>().Where(e => !e.Disabled);

            if (needInOutFilteredProducts)
            {
                var productTreeObjectStringIds = inOutProductTreeObjectIds.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                var productIreeObjectIntIds = new List<int>();
                foreach (var productTreeObjectStringId in productTreeObjectStringIds)
                {
                    int productTreeObjectIntId;
                    if (int.TryParse(productTreeObjectStringId, out productTreeObjectIntId))
                    {
                        productIreeObjectIntIds.Add(productTreeObjectIntId);
                    }
                }

                var filteredProducts = GetFilteredProducts(productIreeObjectIntIds);
                if (!needInOutExcludeAssortmentMatrixProducts)
                {
                    query = filteredProducts.AsQueryable();
                }
                else
                {
                    var productsFromAssortmentMatrix = GetProductsFromAssortmentMatrix();
                    query = filteredProducts.Except(productsFromAssortmentMatrix).AsQueryable();
                }
            }

            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<Product> GetProduct([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }

        //Получаем продукты, привязанные к промо
        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<Product> GetProducts([FromODataUri] Guid promoId)
        {
            IQueryable<PromoProduct> promoProducts = Context.Set<PromoProduct>().Where(x => x.PromoId == promoId && !x.Disabled);
            IQueryable<Product> products = GetConstraintedQuery();
            products = products.Where(x => promoProducts.Any(y => x.Id == y.ProductId) && !x.Disabled);
            return products;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<Product> GetProducts(string inOutProductTreeObjectIds, bool needInOutFilteredProducts, bool needInOutExcludeAssortmentMatrixProducts)
        {
            return GetConstraintedQuery(inOutProductTreeObjectIds, needInOutFilteredProducts, needInOutExcludeAssortmentMatrixProducts);
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<Product> GetFilteredData(ODataQueryOptions<Product> options)
        {
            string bodyText = Helper.GetRequestBody(HttpContext.Current.Request);
            var isPromoIdExists = JsonHelper.IsValueExists(bodyText, "promoId");

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<Product>(options.Context, Request, HttpContext.Current.Request);
            if (isPromoIdExists)
            {
                Guid promoId = Helper.GetValueIfExists<Guid>(bodyText, "promoId");

                IQueryable<Product> query = GetConstraintedQuery();
                IQueryable<PromoProduct> promoProducts = Context.Set<PromoProduct>().Where(x => x.PromoId == promoId && !x.Disabled);
                query = query.Where(x => promoProducts.Any(y => x.Id == y.ProductId) && !x.Disabled);
                return optionsPost.ApplyTo(query, querySettings) as IQueryable<Product>;
            }
            else
            {
                string inOutProductTreeObjectIds = Helper.GetValueIfExists<string>(bodyText, "inOutProductTreeObjectIds");
                bool needInOutFilteredProducts = Helper.GetValueIfExists<bool>(bodyText, "needInOutFilteredProducts");
                bool needInOutExcludeAssortmentMatrixProducts = Helper.GetValueIfExists<bool>(bodyText, "needInOutExcludeAssortmentMatrixProducts");

                IQueryable<Product> query = GetConstraintedQuery(inOutProductTreeObjectIds, needInOutFilteredProducts, needInOutExcludeAssortmentMatrixProducts);
                return optionsPost.ApplyTo(query, querySettings) as IQueryable<Product>;
            }
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
            IList<string> errors = new List<string>();
            var validBrandTech = BrandTechExist(Context, result.Brand_code, result.Segmen_code, result.SubBrand_code, result.Tech_code, ref errors);
            if (!validBrandTech)
            {
                return InternalServerError(new Exception(string.Join(". ", errors)));
            }
            try
            {
                CheckNewBrandTech(result.BrandFlag, result.SupplySegment, model.SubBrand_code);
                Context.Set<Product>().Add(result);
                // добавлен триггер на INSERT для импорта, поэтому создавать инцидент еще раз тут не надо
                //Context.Set<ProductChangeIncident>().Add(CreateIncident(result, true, false));
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
                model.SubBrand_code = !String.IsNullOrEmpty(model.SubBrand_code) ? model.SubBrand_code : null;
                IList<string> errors = new List<string>();
                var validBrandTech = BrandTechExist(Context, model.Brand_code, model.Segmen_code, model.SubBrand_code, model.Tech_code, ref errors);
                if (!validBrandTech)
                {
                    return InternalServerError(new Exception(string.Join(". ", errors)));
                }
                CheckNewBrandTech(model.BrandFlag, model.SupplySegment, model.SubBrand_code);
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
                Context.Set<ProductChangeIncident>().Add(CreateIncident(model, false, true));

                // удаляем продукты и incremental у промо в статусе Draft (пока иерархия не зафиксирована)
                var productsInDraftPromoes = Context.Set<PromoProduct>().Where(n => n.ProductId == key && n.Promo.PromoStatus != null && n.Promo.PromoStatus.SystemName.ToLower() == "draft");
                var incrementalInDraftPromoes = Context.Set<IncrementalPromo>().Where(n => n.ProductId == key && n.Promo.PromoStatus != null && n.Promo.PromoStatus.SystemName.ToLower() == "draft");

                foreach (PromoProduct pp in productsInDraftPromoes)
                {
                    pp.Disabled = true;
                    pp.DeletedDate = DateTimeOffset.Now;
                }

                foreach (IncrementalPromo ip in incrementalInDraftPromoes)
                {
                    ip.Disabled = true;
                    ip.DeletedDate = DateTimeOffset.Now;
                }

                Context.SaveChanges();
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return InternalServerError(e.InnerException);
            }
        }

        [HttpPost]
        public IHttpActionResult GetIfAllProductsInSubrange(ODataActionParameters data)
        {
            try
            {
                var answer = new List<Tuple<int, bool>>();
                if (data.ContainsKey("ProductIds") && data["ProductIds"] != null)
                {
                    var productIdsString = data["ProductIds"] as string;

                    IEnumerable<Product> productsFromAssortmentMatrixForCurrentPromo = null;
                    if (data.ContainsKey("PromoId") && data["PromoId"] != null)
                    {
                        var promoId = data["PromoId"] as string;
                        var promoGuidId = Guid.Parse(promoId);
                        var promo = Context.Set<Promo>().FirstOrDefault(x => x.Id == promoGuidId);

                        var eanPCsFromAssortmentMatrixForCurrentPromo = PlanProductParametersCalculation.GetProductListFromAssortmentMatrix(promo, Context);
                        productsFromAssortmentMatrixForCurrentPromo = Context.Set<Product>().Where(x => eanPCsFromAssortmentMatrixForCurrentPromo.Contains(x.EAN_PC));
                    }
                    else if (data.ContainsKey("ClientTreeKeyId") && data["ClientTreeKeyId"] != null &&
                        data.ContainsKey("DispatchesStart") && data["DispatchesStart"] != null &&
                        data.ContainsKey("DispatchesEnd") && data["DispatchesEnd"] != null)
                    {
                        var clientTreeKeyId = int.Parse(data["ClientTreeKeyId"] as string);
                        var dispatchesStart = DateTimeOffset.Parse(data["DispatchesStart"] as string);
                        var dispatchesEnd = DateTimeOffset.Parse(data["DispatchesEnd"] as string);

                        var eanPCsFromAssortmentMatrixForCurrentPromo =
                            PlanProductParametersCalculation.GetProductListFromAssortmentMatrix(Context, clientTreeKeyId, dispatchesStart, dispatchesEnd);

                        productsFromAssortmentMatrixForCurrentPromo = Context.Set<Product>().Where(x => eanPCsFromAssortmentMatrixForCurrentPromo.Contains(x.EAN_PC));
                    }

                    var ResultList = productIdsString.Split(new string[] { ";!;" }, StringSplitOptions.None).ToList();
                    var productIds = ResultList[0].Split(';').Select(Guid.Parse).ToList();
                    var productTreeObjectIds = ResultList[1].Split(';').Select(Int32.Parse).ToList();
                    List<int> idL;
                    foreach (var productTreeObjectId in productTreeObjectIds)
                    {
                        idL = new List<int>();
                        idL.Add(productTreeObjectId);
                        var filteredProducts = GetFilteredProducts(idL);
                        var resultProductList = productsFromAssortmentMatrixForCurrentPromo != null ? filteredProducts.Intersect(productsFromAssortmentMatrixForCurrentPromo) : filteredProducts;
                        if (resultProductList.All(x => productIds.Contains(x.Id)))
                        {
                            answer.Add(new Tuple<int, bool>(productTreeObjectId, true));
                        }
                        else
                        {
                            answer.Add(new Tuple<int, bool>(productTreeObjectId, false));
                        }
                    }
                }

                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, answer }));
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, message = e.Message }));
            }
        }

        private bool EntityExists(System.Guid key)
        {
            return Context.Set<Product>().Count(e => e.Id == key) > 0;
        }

        private IEnumerable<Column> GetExportSettings()
        {
            int orderNum = 0;
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = orderNum++, Field = "ZREP", Header = "ZREP", Quoting = false },
                new Column() { Order = orderNum++, Field = "EAN_Case", Header = "EAN Case", Quoting = false },
                new Column() { Order = orderNum++, Field = "EAN_PC", Header = "EAN PC", Quoting = false },
                new Column() { Order = orderNum++, Field = "ProductEN", Header = "Product EN", Quoting = false },
				//--
				new Column() { Order = orderNum++, Field = "Brand", Header = "Brand", Quoting = false },
                new Column() { Order = orderNum++, Field = "Brand_code", Header = "Brand code", Quoting = false },
                new Column() { Order = orderNum++, Field = "Technology", Header = "Technology", Quoting = false },
                new Column() { Order = orderNum++, Field = "Tech_code", Header = "Technology code", Quoting = false },
                new Column() { Order = orderNum++, Field = "BrandTech", Header = "Brand Tech", Quoting = false },
                new Column() { Order = orderNum++, Field = "BrandTech_code", Header = "Brand Tech code", Quoting = false },
                new Column() { Order = orderNum++, Field = "Segmen_code", Header = "Segmen code", Quoting = false },
				//--
                new Column() { Order = orderNum++, Field = "BrandsegTech_code", Header = "Brand Seg Tech Code", Quoting = false },
                new Column() { Order = orderNum++, Field = "Brandsegtech", Header = "Brand Seg Tech", Quoting = false },
                new Column() { Order = orderNum++, Field = "BrandsegTechsub_code", Header = "Brand Seg Tech Sub Code", Quoting = false },
                new Column() { Order = orderNum++, Field = "BrandsegTechsub", Header = "Brand Seg Tech Sub", Quoting = false },
                new Column() { Order = orderNum++, Field = "SubBrand_code", Header = "Sub Code", Quoting = false },
                new Column() { Order = orderNum++, Field = "SubBrand", Header = "Sub", Quoting = false },
                new Column() { Order = orderNum++, Field = "BrandFlagAbbr", Header = "Brand flag abbr", Quoting = false },
                new Column() { Order = orderNum++, Field = "BrandFlag", Header = "Brand flag", Quoting = false },
                new Column() { Order = orderNum++, Field = "SubmarkFlag", Header = "Submark flag", Quoting = false },
                new Column() { Order = orderNum++, Field = "IngredientVariety", Header = "Ingredient variety", Quoting = false },
                new Column() { Order = orderNum++, Field = "ProductCategory", Header = "Product category", Quoting = false },
                new Column() { Order = orderNum++, Field = "ProductType", Header = "Product type", Quoting = false },
                new Column() { Order = orderNum++, Field = "MarketSegment", Header = "Market segment", Quoting = false },
                new Column() { Order = orderNum++, Field = "SupplySegment", Header = "Supply segment", Quoting = false },
                new Column() { Order = orderNum++, Field = "FunctionalVariety", Header = "Functional variety", Quoting = false },
                new Column() { Order = orderNum++, Field = "Size", Header = "Size", Quoting = false },
                new Column() { Order = orderNum++, Field = "BrandEssence", Header = "Brand essence", Quoting = false },
                new Column() { Order = orderNum++, Field = "PackType", Header = "Pack type", Quoting = false },
                new Column() { Order = orderNum++, Field = "GroupSize", Header = "Group size", Quoting = false },
                new Column() { Order = orderNum++, Field = "TradedUnitFormat", Header = "Traded unit format", Quoting = false },
                new Column() { Order = orderNum++, Field = "ConsumerPackFormat", Header = "Consumer pack format", Quoting = false },
                new Column() { Order = orderNum++, Field = "UOM_PC2Case", Header = "UOM_PC2Case", Quoting = false },
                new Column() { Order = orderNum++, Field = "Division", Header = "Division", Quoting = false }
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
        private void CheckNewBrandTech(string brand, string tech, string subCode)
        {
            if (brand.Length != 0 && tech.Length != 0)
            {
                Brand checkBrand = Context.Set<Brand>().FirstOrDefault(n => n.Name.ToLower() == brand.ToLower() && !n.Disabled);
                Technology checkTech = Context.Set<Technology>().FirstOrDefault(n => n.Name.ToLower() == tech.ToLower() && 
                                                                                    n.SubBrand_code.Equals(subCode) &&
                                                                                    !n.Disabled);

                if (checkBrand == null || checkTech == null)
                {
                    if (checkBrand == null)
                    {
                        checkBrand = new Brand { Disabled = false, Name = brand };
                        Context.Set<Brand>().Add(checkBrand);
                    }

                    if (checkTech == null)
                    {
                        checkTech = new Technology { Disabled = false, Name = tech, SubBrand_code = subCode };
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
        private ProductChangeIncident CreateIncident(Product product, bool isCreate, bool isDelete)
        {
            return new ProductChangeIncident
            {
                CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                ProductId = product.Id,
                IsCreate = isCreate,
                IsDelete = isDelete,
                IsCreateInMatrix = false,
                IsDeleteInMatrix = false,
                IsChecked = false,
            };
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

                CreateImportTask(fileName, "FullXLSXUpdateAllHandler");

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
                HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportProduct), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportProduct).Name, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(Product), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UniqueFields", new List<String>() { "ZREP" }, data);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Загрузка импорта из файла " + typeof(ImportProduct).Name,
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
        public IHttpActionResult DownloadTemplateXLSX()
        {
            try
            {
                IEnumerable<Column> columns = GetExportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
                string filename = string.Format("{0}Template.xlsx", "Product");
                if (!Directory.Exists(exportDir))
                {
                    Directory.CreateDirectory(exportDir);
                }
                string filePath = Path.Combine(exportDir, filename);
                exporter.Export(Enumerable.Empty<Product>(), filePath);
                string file = Path.GetFileName(filePath);
                return Content(HttpStatusCode.OK, file);
            }
            catch (Exception e)
            {
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

        private IEnumerable<Product> GetFilteredProducts(IEnumerable<int> productTreeObjectIds)
        {
            var productTreeNodes = Context.Set<ProductTree>().Where(x => productTreeObjectIds.Any(y => x.ObjectId == y) && !x.EndDate.HasValue);
            var expressionList = new List<Func<Product, bool>>();
            try
            {
                expressionList = GetExpressionList(productTreeNodes);
            }
            // На случай некорректного фильтра.
            catch { }

            var filteredProducts = Context.Set<Product>().Where(x => !x.Disabled).ToList();
            return filteredProducts.Where(x => expressionList.Any(y => y.Invoke(x)));
        }

        private IEnumerable<Product> GetProductsFromAssortmentMatrix()
        {
            return Context.Set<AssortmentMatrix>().Where(x => !x.Disabled).Select(x => x.Product);
        }

        private static List<Func<Product, bool>> GetExpressionList(IEnumerable<ProductTree> productTreeNodes)
        {
            var expressionsList = new List<Func<Product, bool>>();
            foreach (ProductTree node in productTreeNodes)
            {
                if (node != null && !String.IsNullOrEmpty(node.Filter))
                {
                    string stringFilter = node.Filter;
                    // Преобразованиестроки фильтра в соответствующий класс
                    FilterNode filter = stringFilter.ConvertToNode();
                    // Создание функции фильтрации на основе построенного фильтра
                    var expr = filter.ToExpressionTree<Product>();
                    expressionsList.Add(expr.Compile());
                }
            }
            return expressionsList;
        }

        public static bool BrandTechExist(DatabaseContext context, string brandCode, string segmenCode, string subBrandCode, string techCode, ref IList<string> errors)
        {
            bool exist = true;

            var technology = context.Set<Technology>().Where(t => 
                                                            t.Tech_code == techCode
                                                            && t.SubBrand_code == subBrandCode
                                                            && !t.Disabled).FirstOrDefault();
            var brand = context.Set<Brand>().Where(b => 
                                                b.Segmen_code == segmenCode
                                                && b.Brand_code == brandCode
                                                && !b.Disabled).FirstOrDefault();
            var brandTech = context.Set<BrandTech>().Where(bt =>
                                                        bt.Technology != null && bt.Brand != null
                                                       && bt.Brand.Brand_code == brandCode
                                                       && bt.Brand.Segmen_code == segmenCode
                                                       && bt.Technology.Tech_code == techCode
                                                       && bt.Technology.SubBrand_code == subBrandCode                                                    
                                                       && !bt.Brand.Disabled
                                                       && !bt.Technology.Disabled
                                                       && !bt.Disabled).FirstOrDefault();
            if (brand == null) 
            { 
                errors.Add("Brand was not found"); 
                exist = false; 
            }
            if (technology == null) 
            {
                errors.Add("Technology was not found"); 
                exist = false; 
            }
            if (brandTech == null) 
            {
                errors.Add("Brand Tech was not found"); 
                exist = false; 
            }

            return exist;
        }
    }
}
