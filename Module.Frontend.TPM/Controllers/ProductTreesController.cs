using AutoMapper;
using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Module.Persist.TPM.Utils.Filter;
using Newtonsoft.Json;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility.FileWorker;

namespace Module.Frontend.TPM.Controllers
{
    public class ProductTreesController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;
        private IQueryable<ProductTree> activeTree;

        public ProductTreesController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<ProductTree> GetConstraintedQuery(DateTime? dateFilter = null)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();

            DateTime dt = dateFilter ?? DateTime.Now;
            IQueryable<ProductTree> query = Context.Set<ProductTree>().Where(x => x.Type == "root"
                || (DateTime.Compare(x.StartDate, dt) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dt) > 0)));

            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public SingleResult<ProductTree> GetProductTree([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }

        /// <summary>
        /// Получение иерархии для заданного узла
        /// </summary>
        /// <param name="node">Родительский узел для которого необходимо вернуть дочерние</param>
        /// <param name="filterParameter">Параметр фильтрации</param>
        /// <param name="promoId">Id Промо</param>
        /// <param name="dateFilter">Просмотр истории иерархии на конкретное время</param>
        /// <param name="view">True, если иерархия отркывается только на просмотр в промо</param>
        /// <param name="productTreeObjectIds">Список Object Id для выделения при редактировании</param>
        /// <returns></returns>
        [ClaimsAuthorize]
        [HttpGet, AcceptVerbs("GET")]
        public IHttpActionResult GetProductTrees(string node, string filterParameter, Guid? promoId, DateTime? dateFilter = null, bool view = false, string productTreeObjectIds = null)
        {
            try
            {
                // Получаем активные записи по диапазону дат
                activeTree = GetConstraintedQuery(dateFilter);
                bool existProductTreeForPromo = promoId == null ? false : Context.Set<PromoProductTree>().Any(n => n.PromoId == promoId.Value && !n.Disabled);

                if (filterParameter == null && !promoId.HasValue && productTreeObjectIds == null)
                {
                    return GetTreeForLevel(node);
                }
                else if (filterParameter != null)
                {
                    return GetFilteredNodes(productTreeObjectIds, filterParameter);
                }
                else
                {
                    return GetTreeForPromo(promoId, productTreeObjectIds, view);
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        /// <summary>
        /// Получить дерево для определенного уровня
        /// </summary>
        /// <param name="node">Узел, определяет уровень</param>
        /// <returns>Дерево для определенного уровня</returns>
        private IHttpActionResult GetTreeForLevel(string node)
        {
            int? parentId;
            ProductTree root = null;
            // Получаем записи у которых родительский элемент = разворачиваемому
            if (node == "root")
            {
                root = activeTree.FirstOrDefault(x => x.Type == node);
                parentId = root.ObjectId;
            }
            else
            {
                parentId = int.Parse(node);
            }
            if (!parentId.HasValue)
            {
                throw new Exception("Unable to find root node");
            }

            IQueryable<ProductTree> activeTreeList = activeTree.Where(x => x.parentId == parentId && x.parentId != x.ObjectId).OrderBy(x => x.NodePriority);
            List<ProductTreeNode> rootChilds = new List<ProductTreeNode>();
            // формируем список дочерних элементов
            foreach (ProductTree treeNode in activeTreeList)
            {
                bool leaf = !activeTree.Any(x => x.parentId == treeNode.ObjectId);
                treeNode.Technology = null; // из-за loopreference, нужно чтобы Context был в приложении а не ядре
                treeNode.TechnologyId = null; //
                rootChilds.Add(new ProductTreeNode(treeNode, false, leaf, false));
            }

            if (root != null)
            {
                bool haveChildren = rootChilds.Count() > 0;
                ProductTreeNode rootNode = new ProductTreeNode(root, haveChildren, false, true);

                if (haveChildren)
                {
                    rootNode.AddChild(rootChilds);
                }

                return Json(new
                {
                    success = true,
                    children = rootNode
                });
            }
            else
            {
                return Json(new
                {
                    success = true,
                    children = rootChilds
                });
            }

        }

        /// <summary>
        /// Получить отфильтрованное дерево
        /// </summary>
        /// <param name="productObjectId">Целевой элемент</param>
        /// <param name="filterParameter">Параметр фильтрации</param>
        /// <returns>Отфильтрованное дерево</returns>
        private IHttpActionResult GetFilteredNodes(string productTreeObjectIds, string filterParameter)
        {
            IQueryable<ProductTree> filterTreeList = activeTree.Where(x => x.Name.StartsWith(filterParameter));
            ProductTree root = activeTree.First(n => n.Type == "root");
            ProductTreeNode tree = new ProductTreeNode(root, false, false, true); // формируемое дерево, начинается с root           
            List<ProductTreeNode> addedNodes = new List<ProductTreeNode>();
            List<ProductTree> filterList = filterTreeList.ToList();

            addedNodes.Add(tree);

            // если найден корень, возвращаем всё дерево
            if (!filterList.Any(n => n.Type == "root"))
            {
                for (int i = 0; i < filterList.Count(); i++)
                {
                    // оборачиваем найденный узел в класс
                    ProductTree treeNode = filterList[i];
                    bool leaf = !activeTree.Any(x => x.parentId == treeNode.ObjectId);
                    ProductTreeNode currentNodeFilter = new ProductTreeNode(treeNode, false, leaf, false);
                    ProductTreeNode currentNode = currentNodeFilter;

                    //TODO: It is not a bad practice, it just means that you did not think your code through.
                    while (true)
                    {
                        // узел, к которому присоединяем получаемую ветвь
                        ProductTreeNode containsInTree = null;
                        // ищем узел в дереве
                        containsInTree = addedNodes.FirstOrDefault(n => n.ObjectId == currentNode.ObjectId);
                        if (containsInTree != null)
                        {
                            // если есть дети, обновляем
                            if (currentNode.children != null)
                            {
                                containsInTree.AddChild(currentNode.children);

                                if (filterList.Any(n => n.ObjectId == currentNode.ObjectId))
                                    containsInTree.AddChild(GetChildrenTreeNode(currentNode, activeTree, addedNodes, false, false));

                                containsInTree.expanded = true;
                                containsInTree.loaded = true;
                            }

                            break;
                        }

                        ProductTree parent = activeTree.Where(x => x.ObjectId == currentNode.parentId).FirstOrDefault();
                        ProductTreeNode treeNodeParent = new ProductTreeNode(parent, true, false, false);

                        addedNodes.Add(currentNode);
                        treeNodeParent.AddChild(currentNode);
                        currentNode = treeNodeParent;
                    }

                }

                // если есть галочки, то подгружаем эти узлы
                if (productTreeObjectIds != null)
                {
                    List<int> objectIds = productTreeObjectIds.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(n => Int32.Parse(n)).ToList();

                    foreach (int objectId in objectIds)
                    {
                        ProductTree checkedProduct = activeTree.First(n => n.ObjectId == objectId);
                        ProductTreeNode currentNode = addedNodes.FirstOrDefault(n => n.ObjectId == objectId);

                        while (currentNode == null)
                        {
                            checkedProduct = activeTree.First(n => n.ObjectId == checkedProduct.parentId);
                            currentNode = addedNodes.FirstOrDefault(n => n.ObjectId == checkedProduct.ObjectId);
                        }

                        if (currentNode.Type != "root" && filterList.Any(n => n.ObjectId == currentNode.ObjectId))
                            currentNode.AddChild(GetChildrenTreeNode(currentNode, activeTree, addedNodes, true, false));

                        currentNode = addedNodes.FirstOrDefault(n => n.ObjectId == objectId);
                        if (currentNode != null)
                            currentNode.Target = true;
                    }
                }
            }
            else
            {
                tree.AddChild(GetChildrenTreeNode(tree, activeTree, addedNodes, true, true));
                tree.expanded = true;
            }

            return Json(new
            {
                success = true,
                children = tree
            });
        }

        /// <summary>
        /// Получить дерево для промо
        /// </summary>
        /// <param name="productObjectId">Целевой элемент</param>
        /// <param name="view">True, если промо открывается на только просмотр</param>
        /// <returns>Дерево для промо</returns>
        private IHttpActionResult GetTreeForPromo(Guid? promoId, string productTreeObjectIds, bool view)
        {
            List<ProductTreeNode> children = new List<ProductTreeNode>();
            List<ProductTreeNode> nodeList = new List<ProductTreeNode>();
            List<ProductTreeNode> outList = new List<ProductTreeNode>();
            int rootObjectId = 1000000;
            ProductTree rootNode = activeTree.Where(x => x.ObjectId == rootObjectId).FirstOrDefault();
            ProductTreeNode branch = null;

            List<ProductTree> targetNodes = new List<ProductTree>();

            // если промо открывается в первый раз, то ищем нужные узлы через PromoId
            // иначе ищем те, которые отметили
            if (productTreeObjectIds == null)
            {
                targetNodes = activeTree.Where(n => Context.Set<PromoProductTree>().Where(t => t.PromoId == promoId.Value && !t.Disabled)
                    .Select(t => t.ProductTreeObjectId).Contains(n.ObjectId)).ToList();
            }
            else
            {
                List<int> objectIds = productTreeObjectIds.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(n => Int32.Parse(n)).ToList();

                foreach (int objectId in objectIds)
                {
                    ProductTree productTreeNode = activeTree.Where(n => n.ObjectId == objectId).FirstOrDefault();
                    if (productTreeNode != default(ProductTree))
                    {
                        targetNodes.Add(activeTree.Where(n => n.ObjectId == objectId).First());
                    }
                }
            }

            if (targetNodes.Count > 0)
            {
                int parentId = targetNodes.First().parentId;

                List<ProductTree> currentLevelNodes = view ? targetNodes :
                    activeTree.Where(x => x.parentId == parentId && x.Type != "root").ToList();

                foreach (ProductTree treeNode in currentLevelNodes)
                {
                    bool leaf = !activeTree.Any(x => x.parentId == treeNode.ObjectId);
                    bool target = targetNodes.Any(n => n.Id == treeNode.Id);
                    children.Add(new ProductTreeNode(treeNode, false, leaf, false, target));
                }

                children = children.OrderBy(x => x.NodePriority).ToList();
                //----получаем всех предков ----
                bool first = true;

                ProductTree parentTargetNode = activeTree.Where(x => x.ObjectId == parentId).FirstOrDefault();

                while (parentTargetNode != null && parentTargetNode.Type != "root")
                {
                    List<ProductTree> parentList = view ? new List<ProductTree> { parentTargetNode } :
                        activeTree.Where(x => x.parentId == parentTargetNode.parentId && x.Type != "root").ToList();

                    foreach (ProductTree treeNode in parentList)
                    {
                        //если узел из текущей(редактируемой) ветки, то возвращаем его развернутым
                        if (treeNode.ObjectId == parentTargetNode.ObjectId)
                        {
                            ProductTreeNode nodeParent = new ProductTreeNode(treeNode, true, false, true);

                            nodeParent.AddChild(first ? children : outList);
                            nodeList.Add(nodeParent);
                            first = false;
                        }
                        else
                        {
                            bool leaf = !activeTree.Any(x => x.parentId == treeNode.ObjectId);
                            nodeList.Add(new ProductTreeNode(treeNode, false, leaf, false));
                        }
                    }

                    parentTargetNode = activeTree.Where(x => x.ObjectId == parentTargetNode.parentId).FirstOrDefault();
                    outList = nodeList.OrderBy(x => x.NodePriority).ToList();
                    nodeList = new List<ProductTreeNode>();
                }

                //добавляем в ветку корневой узел для коррентной отрисовки дерева на клиенте
                branch = new ProductTreeNode(rootNode, true, false, true);
                branch.AddChild(outList.Count == 0 ? children : outList);
            }
            var config = new MapperConfiguration(cfg => {
                cfg.AllowNullCollections = true; // потому что в children должен быть null а не пустая коллекция
                cfg.CreateMap<ProductTreeNode, ProductTreeNode>();
                cfg.CreateMap<Technology, Technology>()
                .ForMember(pTo => pTo.BrandTeches, opt => opt.Ignore());
                cfg.CreateMap<ProductTree, ProductTree>()
                .ForMember(pTo => pTo.NoneNegoes, opt => opt.Ignore())
                .MaxDepth(1);
            });
            var mapper = config.CreateMapper();
            var branchMap = mapper.Map<ProductTreeNode>(branch);
            if (branch == null)
            {
                return GetTreeForLevel("root");
            }
            else
            {
                return Json(new
                {
                    success = branchMap != null,
                    children = branchMap
                }, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            }
        }

        [ClaimsAuthorize]
        public IHttpActionResult Post(ProductTree model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            activeTree = GetConstraintedQuery();
            ProductTree parent = activeTree.FirstOrDefault(x => x.ObjectId == model.parentId);
            string fullPathClientName = model.Name;
            model.StartDate = DateTime.Now; // Устанавливаем время сервера

            while (parent != null && parent.Type != "root")
            {
                fullPathClientName = fullPathClientName.Insert(0, " > ").Insert(0, parent.Name);
                parent = activeTree.FirstOrDefault(x => x.ObjectId == parent.parentId);
            }

            model.FullPathName = fullPathClientName;

            var proxy = Context.Set<ProductTree>().Create<ProductTree>();
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<ProductTree, ProductTree>().ReverseMap());
            var mapper = configuration.CreateMapper();
            var result = mapper.Map(model, proxy);

            result.ObjectId = new int();
            Context.Set<ProductTree>().Add(result);
            Context.SaveChanges();
            Context.Entry(result).Reload();

            return Json(new { success = true, children = result });
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IHttpActionResult UpdateNode([FromBody] ProductTree model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                activeTree = GetConstraintedQuery();
                ProductTree currentRecord = activeTree.FirstOrDefault(x => x.Id == model.Id);

                if (currentRecord == null)
                {
                    return NotFound();
                }

                DateTime dt = DateTime.Now;
                ProductTree oldRecord = (ProductTree)currentRecord.Clone();
                oldRecord.EndDate = dt;

                string oldFullPath = currentRecord.FullPathName;
                int ind = oldFullPath.LastIndexOf(">");
                ind = ind < 0 ? 0 : ind + 2;

                //при изменеии названия бренда в узле необходимо обновить фильтры дочерних технологий
                if (currentRecord.Name != model.Name && currentRecord.Type == "Brand")
                {
                    List<ProductTree> childNodes = activeTree.Where(x => x.parentId == model.ObjectId && x.Type == "Technology").ToList();
                    if (childNodes.Count != 0)
                    {
                        foreach (ProductTree child in childNodes)
                        {
                            child.Filter = child.Filter.Replace(currentRecord.Name, model.Name);
                        }
                    }
                }

                model.FullPathName = oldFullPath.Substring(0, ind) + model.Name;
                model.StartDate = dt;

                if (model.Filter != currentRecord.Filter)
                {
                    ChangesIncident changesIncident = new ChangesIncident
                    {
                        Disabled = false,
                        DeletedDate = null,
                        DirectoryName = "ProductTree",
                        ItemId = model.Id.ToString(),
                        CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                        ProcessDate = null
                    };

                    Context.Set<ChangesIncident>().Add(changesIncident);

                    List<Product> products = Context.Set<Product>().Where(x => !x.Disabled).ToList();
                    List<Func<Product, bool>> oldExpression = GetExpressionList(currentRecord);
                    List<Func<Product, bool>> newExpression = GetExpressionList(model);
                    if (oldExpression != null && newExpression != null)
                    {
                        List<Product> oldProductsList = products.Where(p => oldExpression.Any(e => e.Invoke(p))).ToList();
                        List<Product> newProductsList = products.Where(x => newExpression.Any(e => e.Invoke(x))).ToList();

                        var forIncident = new List<Product>();
                        forIncident.AddRange(oldProductsList.Except(newProductsList));
                        forIncident.AddRange(newProductsList.Except(oldProductsList));
                        if (forIncident.Any())
                        {
                            foreach (var product in forIncident)
                            {
                                var pci = new ProductChangeIncident
                                {
                                    CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                                    IsCreate = false,
                                    IsDelete = false,
                                    IsCreateInMatrix = false,
                                    IsDeleteInMatrix = false,
                                    IsChecked = false,
                                    Product = product,
                                    ProductId = product.Id
                                };
                                Context.Set<ProductChangeIncident>().Add(pci);
                            }
                        }
                    }
                }

                if (currentRecord.Type == "Subrange")
                {
                    var oldName = currentRecord.FullPathName;
                    var newName = model.FullPathName;
                    //Асинхронно, т.к. долго выполняется и иначе фронт не дождется ответа
                    Task.Run(() => PromoHelper.UpdateProductHierarchy(currentRecord.Type, newName, oldName));
                }
                Context.Entry(currentRecord).CurrentValues.SetValues(model);
                UpdateFullPathProductTree(currentRecord, Context.Set<ProductTree>());
                Context.Set<ProductTree>().Add(oldRecord);
                Context.SaveChanges();

                return Created(currentRecord);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            try
            {
                activeTree = GetConstraintedQuery();
                ProductTree record = activeTree.FirstOrDefault(x => x.Id == key);
                List<ProductTree> recordsToDelete = new List<ProductTree>();
                List<ProductTree> childs = activeTree.Where(x => x.parentId == record.ObjectId).ToList();

                recordsToDelete.Add(record);

                while (childs.Count() > 0)
                {
                    recordsToDelete.AddRange(childs);
                    List<int> parents = childs.Select(ch => ch.ObjectId).ToList();
                    childs = activeTree.Where(x => parents.Contains(x.parentId)).ToList();
                }

                recordsToDelete.ForEach(x => x.EndDate = DateTime.Now);
                Context.SaveChanges();

                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult GetHierarchyDetail([FromODataUri] int key)
        {
            activeTree = GetConstraintedQuery();
            ProductTree productTree = activeTree.Where(x => x.Id == key).FirstOrDefault();
            List<ProductTreeNode> nodes = new List<ProductTreeNode>();

            //получаем всех предков
            while (productTree.Type != "root")
            {
                nodes.Add(new ProductTreeNode(productTree, false, false, false));
                productTree = activeTree.Where(x => x.ObjectId == productTree.parentId).FirstOrDefault();
            }

            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, data = nodes }, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore}));
        }

        /// <summary>
        /// Перемещение узлов
        /// </summary>
        /// <param name="nodeToMove"> Id узла который нужно переместить</param>
        /// <param name="destinationNode"> Id узла в который необходимо переместить узел</param>
        /// <returns></returns>
        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult Move([FromODataUri] int nodeToMove, int destinationNode)
        {
            try
            {
                activeTree = GetConstraintedQuery();
                ProductTree recordToMove = activeTree.FirstOrDefault(x => x.Id == nodeToMove);
                ProductTree destinationRecord = activeTree.FirstOrDefault(x => x.Id == destinationNode);
                DateTime dt = DateTime.Now;

                if (recordToMove == null || destinationRecord == null)
                {
                    string msg = recordToMove == null ? "Unable to find record to move" : "Unable to find destination node";
                    return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, message = msg }));
                }

                // Создаём новую запись, старой устанавоиваем EndDate                
                ProductTree newRecord = (ProductTree)recordToMove.Clone();
                newRecord.parentId = destinationRecord.ObjectId;
                newRecord.StartDate = dt;
                newRecord.depth = destinationRecord.depth + 1;

                recordToMove.EndDate = dt;
                Context.Set<ProductTree>().Add(newRecord);
                Context.SaveChanges();

                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        /// <summary>
        /// Получить потомков
        /// </summary>
        /// <param name="clientTree">Текущий узел</param>
        /// <param name="activeTree">Активное дерево</param>
        /// <param name="addedNodes">Отфильтрованные узлы</param>
        /// <param name="full">Получить всех потомков или толькло для текущего уровня</param>
        /// <param name="expandAll">Раскрывать ли потомков</param>
        /// <returns></returns>
        private List<ProductTreeNode> GetChildrenTreeNode(ProductTreeNode productTree, IQueryable<ProductTree> activeTree, List<ProductTreeNode> addedNodes, bool full, bool expandAll)
        {
            List<ProductTreeNode> children = new List<ProductTreeNode>();
            IQueryable<ProductTree> productTreeList = activeTree.Where(x => x.parentId == productTree.ObjectId && x.parentId != x.ObjectId);

            foreach (ProductTree treeNode in productTreeList)
            {
                bool leaf = !activeTree.Any(x => x.parentId == treeNode.ObjectId);
                //избегаем дубликатов
                if (productTree.children == null || !productTree.children.Any(n => n.ObjectId == treeNode.ObjectId))
                {
                    ProductTreeNode child = new ProductTreeNode(treeNode, expandAll, leaf, false);
                    if (full)
                        child.AddChild(GetChildrenTreeNode(child, activeTree, addedNodes, full, expandAll));

                    children.Add(child);
                    addedNodes.Add(child);
                }
            }

            return children;
        }

        /// <summary>
        /// Обновить FullPathName для потомков в ProductTree
        /// </summary>
        /// <param name="node">Родительский узел</param>
        public static void UpdateFullPathProductTree(ProductTree node, IQueryable<ProductTree> tree)
        {
            // метод статический т.к. вызывается в контроллерах Brand и Technology
            ProductTree[] children = tree.Where(n => n.parentId == node.ObjectId).ToArray();

            for (int i = 0; i < children.Length; i++)
            {
                children[i].FullPathName = node.FullPathName + " > " + children[i].Name;
                UpdateFullPathProductTree(children[i], tree);
            }
        }

        [ClaimsAuthorize]
        [HttpPost]
        public async Task<IHttpActionResult> UploadLogoFile(int productTreeId)
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                FileDispatcher fileDispatcher = new FileDispatcher();

                string directory = Core.Settings.AppSettingsManager.GetSetting("PRODUCT_TREE_DIRECTORY", "ProductTreeLogoFiles");
                string fullPathfile = await FileUtility.UploadFile(Request, directory);
                string fileName = fullPathfile.Split('\\').Last();

                // так себе проверка, но лучше что-то, чем ничего
                string typeFile = fullPathfile.Split('.').Last().ToLower();
                if (typeFile != "png" && typeFile != "jpg")
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

                ProductTree productTree = Context.Set<ProductTree>().Find(productTreeId);
                if (productTree == null)
                    return NotFound();

                // удаляем старую картинку если была
                if (productTree.LogoFileName != null)
                {
                    if (fileDispatcher.IsExists(directory, productTree.LogoFileName))
                    {
                        fileDispatcher.DeleteFile(Path.Combine(directory, productTree.LogoFileName));
                    }
                }

                productTree.LogoFileName = fileName;
                Context.SaveChanges();

                return Json(new { success = true, fileName });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        [ClaimsAuthorize]
        [HttpGet]
        [Route("odata/ProductTrees/DownloadLogoFile")]
        public HttpResponseMessage DownloadLogoFile(string fileName)
        {
            try
            {
                string directory = Core.Settings.AppSettingsManager.GetSetting("PRODUCT_TREE_DIRECTORY", "ProductTreeLogoFiles");
                string logDir = Core.Settings.AppSettingsManager.GetSetting<string>("HANDLER_LOG_TYPE", "File");

                if (logDir == "Azure")
                {
                    return FileUtility.DownloadFileAzure(directory, fileName);
                }
                return FileUtility.DownloadFile(directory, fileName);
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [ClaimsAuthorize]
        [HttpPost]
        public async Task<IHttpActionResult> DeleteLogo(int id)
        {
            var currentProduct = Context.Set<ProductTree>().Find(id);

            if (currentProduct != null && !String.IsNullOrEmpty(currentProduct.LogoFileName))
            {
                // удаляем старое лого
                string directory = Core.Settings.AppSettingsManager.GetSetting("PRODUCT_TREE_DIRECTORY", "ProductTreeLogoFiles");
                FileDispatcher fileDispatcher = new FileDispatcher();
                if (fileDispatcher.IsExists(directory, currentProduct.LogoFileName))
                {
                    fileDispatcher.DeleteFile(Path.Combine(directory, currentProduct.LogoFileName));
                }
                currentProduct.LogoFileName = null;
                await Context.SaveChangesAsync();
                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, message = "The file from selected client was removed successfully." }));
            }
            else
            {
                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, message = "The logo is not exists for current client." }));
            }
        }

        /// <summary>
        /// Список преобразованных в функции фильтров из узлов иерархии
        /// </summary>
        /// <param name="productTreeNodes"></param>
        /// <returns></returns>
        private List<Func<Product, bool>> GetExpressionList(ProductTree productTreeNodes)
        {
            List<Func<Product, bool>> expressionsList = new List<Func<Product, bool>>();
            if (productTreeNodes != null && !String.IsNullOrEmpty(productTreeNodes.Filter))
            {
                string stringFilter = productTreeNodes.Filter;
                // Преобразованиестроки фильтра в соответствующий класс
                FilterNode filter = stringFilter.ConvertToNode();
                // Создание функции фильтрации на основе построенного фильтра
                var expr = filter.ToExpressionTree<Product>();
                expressionsList.Add(expr.Compile());
            }
            return expressionsList;
        }
    }
    /// <summary>
    /// Класс-обертка для дерева (в ExtJS)
    /// </summary>
    public class ProductTreeNode
    {
        public int Id { get; set; }
        public int ObjectId { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? TechnologyId { get; set; }
        public Technology Technology { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description_ru { get; set; }
        public string Filter { get; set; }
        public string Abbreviation { get; set; }
        public string FullPathName { get; set; }
        public int parentId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool leaf { get; set; }
        public bool loaded { get; set; }
        public bool expanded { get; set; }
        public int depth { get; set; }
        public bool root { get; set; }
        public bool Target { get; set; }
        public List<ProductTreeNode> children { get; set; }
        public int? NodePriority { get; set; }
        public string LogoFileName { get; set; }

        public ProductTreeNode(ProductTree treeNode, bool expanded, bool leaf, bool loaded, bool target = false)
        {
            Id = treeNode.Id;
            ObjectId = treeNode.ObjectId;
            BrandId = treeNode.BrandId;
            TechnologyId = treeNode.TechnologyId;
            Technology = treeNode.Technology;
            Type = treeNode.Type;
            Name = treeNode.Name;
            Description_ru = treeNode.Description_ru;
            FullPathName = treeNode.FullPathName;
            Abbreviation = treeNode.Abbreviation;
            parentId = treeNode.parentId;
            StartDate = treeNode.StartDate;
            EndDate = treeNode.EndDate;
            Filter = treeNode.Filter;
            depth = treeNode.depth;
            NodePriority = treeNode.NodePriority;
            LogoFileName = treeNode.LogoFileName;

            root = Type == "root";
            this.expanded = expanded;
            this.leaf = leaf;
            this.loaded = loaded;
            this.Target = target;
        }

        public ProductTreeNode()
        {
        }

        /// <summary>
        /// Добавить элемент-потомок
        /// </summary>
        /// <param name="child">Потомок</param>
        public void AddChild(ProductTreeNode child)
        {
            if (children == null)
                children = new List<ProductTreeNode>();

            children.Add(child);
        }

        /// <summary>
        /// Добавить несколько потомков
        /// </summary>
        /// <param name="child">Список потомков</param>
        public void AddChild(List<ProductTreeNode> child)
        {
            if (children == null)
                children = new List<ProductTreeNode>();

            children.AddRange(child);
        }
    }
}