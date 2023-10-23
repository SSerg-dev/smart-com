using AutoMapper;
using Core.Security;
using Core.Security.Models;
using Core.Settings;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.FunctionalHelpers.HiddenMode;
using Module.Frontend.TPM.FunctionalHelpers.RA;
using Module.Frontend.TPM.FunctionalHelpers.Scenario;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Enum;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Interfaces;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Newtonsoft.Json;
using Persist.Model;
using Persist.ScriptGenerator.Filter;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;
using Utility.FileWorker;
using UserInfo = Core.Security.Models.UserInfo;

namespace Module.Frontend.TPM.Controllers
{
    public class ClientTreesController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;
        private IQueryable<ClientTree> activeTree;


        public ClientTreesController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<ClientTree> GetConstraintedQuery(DateTime? dateFilter = null)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            DateTime dt = dateFilter ?? DateTime.Now;

            IQueryable<ClientTree> query = Context.Set<ClientTree>().Where(x => x.Type == "root"
                || (DateTime.Compare(x.StartDate, dt) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dt) > 0)));

            foreach (ClientTree q in query)
            {
                if (q.DistrMarkUp == 0)
                {
                    q.DistrMarkUp = 1;
                }
            }

            //IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.SqlQuery<ClientTreeHierarchyView>(
                @"With RecursiveSearch (ObjectId, parentId, Hierarchy) AS (
		            Select ObjectId, parentId, CONVERT(varchar(255), '') 
		            FROM [DefaultSchemaSetting].[ClientTree] AS FirtGeneration 
		            WHERE [Type] = 'root' and ((Cast({0} AS Datetime) between StartDate and EndDate) or EndDate is NULL)  
		            union all 
		            select NextStep.ObjectId, NextStep.parentId, CAST(CASE WHEN Hierarchy = '' 
			            THEN 
				            (CAST(NextStep.parentId AS VARCHAR(255))) 
			            ELSE
            				(Hierarchy + '.' + CAST(NextStep.parentId AS VARCHAR(255))) 
			            END AS VARCHAR(255)) 
		            FROM [DefaultSchemaSetting].[ClientTree] AS NextStep INNER JOIN RecursiveSearch as bag on bag.ObjectId = NextStep.parentId 
		            where ( (Cast({0} AS Datetime) between NextStep.StartDate and NextStep.EndDate) or NextStep.EndDate is NULL) and [Type] <> 'root' 
	            ) 
                Select ObjectId as Id, Hierarchy from RecursiveSearch", dt.ToString("MM/dd/yyyy HH:mm:ss")).AsQueryable();

            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters, FilterQueryModes.Active, true);

            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public SingleResult<ClientTree> GetClientTree([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult GetClientTreeByObjectId(int? objectId)
        {
            var clientTree = Context.Set<ClientTree>().FirstOrDefault(x => x.ObjectId == objectId && x.EndDate == null);
            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, data = clientTree }, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }

        /// <summary>
        /// Получение иерархии для заданного узла
        /// </summary>
        /// <param name="node">Родительский узел для которого необходимо вернуть дочерние</param>
        /// <param name="filterParameter">Параметр фильтрации</param>
        /// <param name="clientObjectId">ObjectId узла для выделения при редактировании</param>
        /// <param name="dateFilter">Просмотр истории иерархии на конкретное время</param>
        /// <param name="view">True, если иерархия отркывается только на просмотр в промо</param>
        /// <returns></returns>
        [ClaimsAuthorize]
        [HttpGet, AcceptVerbs("GET")]
        public IHttpActionResult GetClientTrees()
        {
            DateTime? dateFilter = null;
            Guid? budgetSubItemId = null;
            bool needBaseClients = false;
            bool view = false;
            string node = "root";
            string filterParameter = null;
            string clientObjectId = null;

            try
            {
                // Получаем активные записи по диапазону дат
                activeTree = GetConstraintedQuery(dateFilter);
                List<ClientTree> budgetSubItemClientTrees = new List<ClientTree>();

                if (budgetSubItemId != null)
                {
                    budgetSubItemClientTrees = Context.Set<BudgetSubItemClientTree>().Where(x => x.BudgetSubItemId == budgetSubItemId).Select(x => x.ClientTree).ToList();
                }

                if (activeTree.Count() != 0)
                {
                    if (filterParameter == null && clientObjectId == null && !needBaseClients && budgetSubItemClientTrees.Count == 0)
                    {
                        return GetTreeForLevel(node);
                    }
                    else if (filterParameter != null || needBaseClients)
                    {
                        return GetFilteredNodes(clientObjectId, filterParameter, needBaseClients);
                    }
                    else if (budgetSubItemClientTrees.Count > 0)
                    {
                        return GetCheckedNodes(budgetSubItemClientTrees, filterParameter, needBaseClients);
                    }
                    else
                    {
                        return GetTreeForPromo(clientObjectId, view);
                    }
                }
                else
                {
                    ClientTree rootNode = Context.Set<ClientTree>().Where(x => x.ObjectId == 5000000 && !x.EndDate.HasValue).FirstOrDefault();

                    return Json(new
                    {
                        success = true,
                        children = new ClientTreeNode(rootNode, false, false, true)
                    });
                }
            }
            catch (Exception e)
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }
        [ClaimsAuthorize]
        [HttpGet, AcceptVerbs("GET")]
        public IHttpActionResult GetClientTrees(string node, string filterParameter, string clientObjectId, DateTime? dateFilter = null, bool view = false,
           bool needBaseClients = false, Guid? budgetSubItemId = null)
        {
            try
            {
                // Получаем активные записи по диапазону дат
                activeTree = GetConstraintedQuery(dateFilter);
                List<ClientTree> budgetSubItemClientTrees = new List<ClientTree>();

                if (budgetSubItemId != null)
                {
                    budgetSubItemClientTrees = Context.Set<BudgetSubItemClientTree>().Where(x => x.BudgetSubItemId == budgetSubItemId).Select(x => x.ClientTree).ToList();
                }

                if (activeTree.Count() != 0)
                {
                    if (filterParameter == null && clientObjectId == null && !needBaseClients && budgetSubItemClientTrees.Count == 0)
                    {
                        return GetTreeForLevel(node);
                    }
                    else if (filterParameter != null || needBaseClients)
                    {
                        return GetFilteredNodes(clientObjectId, filterParameter, needBaseClients);
                    }
                    else if (budgetSubItemClientTrees.Count > 0)
                    {
                        return GetCheckedNodes(budgetSubItemClientTrees, filterParameter, needBaseClients);
                    }
                    else
                    {
                        return GetTreeForPromo(clientObjectId, view);
                    }
                }
                else
                {
                    ClientTree rootNode = Context.Set<ClientTree>().Where(x => x.ObjectId == 5000000 && !x.EndDate.HasValue).FirstOrDefault();

                    return Json(new
                    {
                        success = true,
                        children = new ClientTreeNode(rootNode, false, false, true)
                    });
                }
            }
            catch (Exception e)
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
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
            ClientTree root = null;
            Object result;
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

            IQueryable<ClientTree> activeTreeList = activeTree.Where(x => x.parentId == parentId && x.parentId != x.ObjectId);
            List<ClientTreeNode> rootChilds = new List<ClientTreeNode>();

            // формируем список дочерних элементов
            foreach (ClientTree treeNode in activeTreeList)
            {
                bool leaf = !activeTree.Any(x => x.parentId == treeNode.ObjectId);
                rootChilds.Add(new ClientTreeNode(treeNode, false, leaf, false));
            }

            if (root != null)
            {
                bool haveChildren = rootChilds.Count() > 0;
                ClientTreeNode rootNode = new ClientTreeNode(root, haveChildren, false, true);

                if (haveChildren)
                {
                    rootNode.AddChild(rootChilds);
                }

                result = rootNode;
            }
            else
            {
                result = rootChilds;
            }

            return Json(new
            {
                success = true,
                children = result
            });
        }

        /// <summary>
        /// Получить отфильтрованное дерево
        /// </summary>
        /// <param name="clientObjectId">Целевой элемент</param>
        /// <param name="filterParameter">Параметр фильтрации</param>
        /// <param name="needBaseClients">Только базовые клиенты</param>
        /// <returns>Отфильтрованное дерево</returns>
        private IHttpActionResult GetFilteredNodes(string clientObjectId, string filterParameter, bool needBaseClients)
        {
            IQueryable<ClientTree> filterTreeList;

            if (filterParameter != null && needBaseClients)
            {
                filterTreeList = activeTree.Where(x => x.Name.StartsWith(filterParameter) && x.IsBaseClient);
            }
            else if (needBaseClients)
            {
                filterTreeList = activeTree.Where(x => x.IsBaseClient);
            }
            else
            {
                filterTreeList = activeTree.Where(x => x.Name.StartsWith(filterParameter));
            }

            ClientTree root = activeTree.First(n => n.Type == "root");
            ClientTreeNode tree = new ClientTreeNode(root, false, false, true); // формируемое дерево, начинается с root           
            List<ClientTreeNode> addedNodes = new List<ClientTreeNode>();
            List<ClientTree> filterList = filterTreeList.ToList();

            addedNodes.Add(tree);

            // если найден корень, возвращаем всё дерево
            if (!filterList.Any(n => n.Type == "root"))
            {
                for (int i = 0; i < filterList.Count(); i++)
                {
                    // оборачиваем найденный узел в класс
                    ClientTree treeNode = filterList[i];
                    bool leaf = !activeTree.Any(x => x.parentId == treeNode.ObjectId);
                    ClientTreeNode currentNode = new ClientTreeNode(treeNode, false, leaf, false);

                    //TODO: It is not a bad practice, it just means that you did not think your code through.
                    while (true)
                    {
                        // узел, к которому присоединяем получаемую ветвь
                        ClientTreeNode containsInTree = null;
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

                        ClientTree parent = activeTree.Where(x => x.ObjectId == currentNode.parentId).FirstOrDefault();
                        ClientTreeNode treeNodeParent = new ClientTreeNode(parent, true, false, false);

                        addedNodes.Add(currentNode);
                        treeNodeParent.AddChild(currentNode);
                        currentNode = treeNodeParent;
                    }
                }

                // если есть галочка, то подгружаем этот узел
                if (clientObjectId != null)
                {
                    int objectId = int.Parse(clientObjectId);
                    ClientTree checkedClient = activeTree.FirstOrDefault(n => n.ObjectId == objectId);

                    if (checkedClient != null)
                    {
                        ClientTreeNode currentNode = addedNodes.FirstOrDefault(n => n.ObjectId == objectId);
                        while (currentNode == null)
                        {
                            checkedClient = activeTree.First(n => n.ObjectId == checkedClient.parentId);
                            currentNode = addedNodes.FirstOrDefault(n => n.ObjectId == checkedClient.ObjectId);
                        }
                        if (currentNode.Type != "root" && filterList.Any(n => n.ObjectId == currentNode.ObjectId))
                            currentNode.AddChild(GetChildrenTreeNode(currentNode, activeTree, addedNodes, true, false));
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
        /// Получить дерево с выбранными узлами
        /// </summary>
        /// <param name="linkedNodes">Целевые узлы</param>
        /// <param name="filterParameter">Параметр фильтрации</param>
        /// <param name="needBaseClients">Только базовые клиенты</param>
        /// <returns>Дерево с выбранными узлами</returns>
        private IHttpActionResult GetCheckedNodes(List<ClientTree> linkedNodes, string filterParameter, bool needBaseClients)
        {
            if (filterParameter != null && needBaseClients)
            {
                linkedNodes = linkedNodes.Where(x => x.Name.StartsWith(filterParameter) && x.IsBaseClient).ToList();
            }
            else if (needBaseClients)
            {
                linkedNodes = linkedNodes.Where(x => x.IsBaseClient).ToList();
            }
            else if (filterParameter != null)
            {
                linkedNodes = linkedNodes.Where(x => x.Name.StartsWith(filterParameter)).ToList();
            }

            ClientTree root = activeTree.First(n => n.Type == "root");
            ClientTreeNode tree = new ClientTreeNode(root, false, false, true); // формируемое дерево, начинается с root           
            List<ClientTreeNode> addedNodes = new List<ClientTreeNode>();

            addedNodes.Add(tree);

            for (int i = 0; i < linkedNodes.Count(); i++)
            {
                // оборачиваем найденный узел в класс
                ClientTree treeNode = linkedNodes[i];
                bool leaf = !activeTree.Any(x => x.parentId == treeNode.ObjectId);
                ClientTreeNode currentNode = new ClientTreeNode(treeNode, false, leaf, false, true);

                //TODO: It is not a bad practice, it just means that you did not think your code through.
                while (true)
                {
                    // узел, к которому присоединяем получаемую ветвь
                    ClientTreeNode containsInTree = null;
                    // ищем узел в дереве
                    containsInTree = addedNodes.FirstOrDefault(n => n.ObjectId == currentNode.ObjectId);
                    if (containsInTree != null)
                    {
                        // если есть дети, обновляем
                        if (currentNode.children != null)
                        {
                            containsInTree.AddChild(currentNode.children);

                            IQueryable<ClientTree> childNodes = activeTree.Where(x => x.parentId == currentNode.ObjectId);
                            bool isCheckedNodeExist = linkedNodes.Select(x => x.ObjectId).Intersect(childNodes.Select(x => x.ObjectId)).Count() != 0;
                            if (isCheckedNodeExist)
                            {
                                containsInTree.AddChild(GetChildrenTreeNode(currentNode, activeTree, addedNodes, false, false, linkedNodes));
                            }

                            containsInTree.expanded = true;
                            containsInTree.loaded = true;
                        }

                        break;
                    }

                    ClientTree parent = activeTree.Where(x => x.ObjectId == currentNode.parentId).FirstOrDefault();
                    bool isNodeChecked = linkedNodes.Any(x => x.ObjectId == parent.ObjectId);
                    ClientTreeNode treeNodeParent = new ClientTreeNode(parent, true, false, false, isNodeChecked);

                    addedNodes.Add(currentNode);
                    treeNodeParent.AddChild(currentNode);

                    // выбираем узлы, которые располагаются на одном уровне с текущим и добавляем их в дерево (чтобы вид открывшегося дерева был такой же, каким был когда производится выбор узлов)
                    List<ClientTree> currentNodeFellows = activeTree.Where(x => x.parentId == currentNode.parentId
                            && x.ObjectId != currentNode.ObjectId
                            && x.Type != "root").ToList();
                    foreach (var node in currentNodeFellows)
                    {
                        if (!addedNodes.Any(x => x.ObjectId == node.ObjectId))
                        {
                            leaf = !activeTree.Any(x => x.parentId == node.ObjectId);
                            isNodeChecked = linkedNodes.Any(x => x.ObjectId == node.ObjectId); ;
                            ClientTreeNode n = new ClientTreeNode(node, false, leaf, false, isNodeChecked);
                            addedNodes.Add(n);
                            treeNodeParent.AddChild(n);
                        }
                    }
                    currentNode = treeNodeParent;
                }
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
        /// <param name="clientObjectId">Целевой элемент</param>
        /// <param name="view">True, если промо открывается на только просмотр</param>
        /// <returns>Дерево для промо</returns>
        private IHttpActionResult GetTreeForPromo(string clientObjectId, bool view)
        {
            List<ClientTreeNode> children = new List<ClientTreeNode>();
            List<ClientTreeNode> nodeList = new List<ClientTreeNode>();
            List<ClientTreeNode> outList = new List<ClientTreeNode>();
            int objectId = int.Parse(clientObjectId);
            int rootObjectId = 5000000;
            ClientTree rootNode = activeTree.Where(x => x.ObjectId == rootObjectId).FirstOrDefault();
            ClientTree targetNode = activeTree.Where(x => x.ObjectId == objectId).FirstOrDefault();
            ClientTreeNode branch = null;

            if (targetNode != null)
            {
                List<ClientTree> currentLevelNodes = view ? new List<ClientTree> { targetNode } :
                    activeTree.Where(x => x.parentId == targetNode.parentId && x.Type != "root").ToList();

                foreach (ClientTree treeNode in currentLevelNodes)
                {
                    bool leaf = !activeTree.Any(x => x.parentId == treeNode.ObjectId);
                    children.Add(new ClientTreeNode(treeNode, false, leaf, false));
                }

                //----получаем всех предков ----
                bool first = true;
                ClientTree parentTargetNode = activeTree.Where(x => x.ObjectId == targetNode.parentId).FirstOrDefault();

                while (parentTargetNode != null && parentTargetNode.Type != "root")
                {
                    List<ClientTree> parentList = view ? new List<ClientTree> { parentTargetNode } :
                        activeTree.Where(x => x.parentId == parentTargetNode.parentId && x.Type != "root").ToList();

                    foreach (ClientTree treeNode in parentList)
                    {
                        //если узел из текущей(редактируемой) ветки, то возвращаем его развернутым
                        if (treeNode.ObjectId == parentTargetNode.ObjectId)
                        {
                            ClientTreeNode nodeParent = new ClientTreeNode(treeNode, true, false, true);

                            nodeParent.AddChild(first ? children : outList);
                            nodeList.Add(nodeParent);
                            first = false;
                        }
                        else
                        {
                            bool leaf = !activeTree.Any(x => x.parentId == treeNode.ObjectId);
                            nodeList.Add(new ClientTreeNode(treeNode, false, leaf, false));
                        }
                    }

                    parentTargetNode = activeTree.Where(x => x.ObjectId == parentTargetNode.parentId).FirstOrDefault();
                    outList = nodeList;
                    nodeList = new List<ClientTreeNode>();
                }

                //добавляем в ветку корневой узел для коррентной отрисовки дерева на клиенте
                branch = new ClientTreeNode(rootNode, true, false, true);
                branch.AddChild(outList.Count == 0 ? children : outList);
            }

            if (branch == null)
            {
                return GetTreeForLevel("root");
            }
            else
            {
                return Json(new
                {
                    success = branch != null,
                    children = branch
                });
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
        private List<ClientTreeNode> GetChildrenTreeNode(ClientTreeNode clientTree, IQueryable<ClientTree> activeTree, List<ClientTreeNode> addedNodes, bool full, bool expandAll, List<ClientTree> linkedNodes = null)
        {
            List<ClientTreeNode> children = new List<ClientTreeNode>();
            IQueryable<ClientTree> clientTreeList = activeTree.Where(x => x.parentId == clientTree.ObjectId && x.parentId != x.ObjectId);

            foreach (ClientTree treeNode in clientTreeList)
            {
                bool leaf = !activeTree.Any(x => x.parentId == treeNode.ObjectId);
                //избегаем дубликатов
                if (clientTree.children == null || !addedNodes.Any(n => n.ObjectId == treeNode.ObjectId)/*!clientTree.children.Any(n => n.ObjectId == treeNode.ObjectId)*/)
                {
                    // если есть несколько выделенных узлов, то каждый из них надо отметить галочкой
                    bool isNodeChecked = false;
                    if (linkedNodes != null)
                    {
                        isNodeChecked = linkedNodes.Any(x => x.ObjectId == treeNode.ObjectId);
                    }

                    ClientTreeNode child = new ClientTreeNode(treeNode, expandAll, leaf, false, isNodeChecked);
                    if (full)
                        child.AddChild(GetChildrenTreeNode(child, activeTree, addedNodes, full, expandAll, linkedNodes));

                    children.Add(child);
                    addedNodes.Add(child);
                }
            }

            return children;
        }

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> Post(ClientTree model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            activeTree = GetConstraintedQuery();
            ClientTree parentCheckCode = activeTree.FirstOrDefault(x => x.ObjectId == model.parentId);

            //Проверяем на наличие DemandCode у Parents            
            if (model.DemandCode == "" && model.IsBaseClient == true)
            {
                while (parentCheckCode != null)
                {
                    if (parentCheckCode.DemandCode != null)
                    {
                        break;
                    }
                    else if (parentCheckCode.DemandCode == null && parentCheckCode.Type == "root")
                    {
                        string msg = "This ClientTree not found DemandCode. Please add DemandCode!";
                        return Json(new { success = false, message = msg });
                    }
                    parentCheckCode = activeTree.FirstOrDefault(x => x.ObjectId == parentCheckCode.parentId);
                }
            }

            ClientTree parent = activeTree.FirstOrDefault(x => x.ObjectId == model.parentId);
            string fullPathClientName = model.Name;
            model.StartDate = DateTime.Now; // Устанавливаем время сервера

            while (parent != null && parent.Type != "root")
            {
                fullPathClientName = fullPathClientName.Insert(0, " > ").Insert(0, parent.Name);
                parent = activeTree.FirstOrDefault(x => x.ObjectId == parent.parentId);
            }

            model.FullPathName = fullPathClientName;
            model.DeviationCoefficient = model.IsBaseClient
                                                ? model.DeviationCoefficient / -10000
                                                : 0;

            var proxy = Context.Set<ClientTree>().Create<ClientTree>();
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<ClientTree, ClientTree>().ReverseMap());
            var mapper = configuration.CreateMapper();
            var result = mapper.Map(model, proxy);

            //Проверка DemandCode
            if (!CheckDemandCode(model))
            {
                string msg = "There is a ClientTree with such DemandCode";
                return Json(new { success = false, message = msg });
            }
            var tt = (!string.IsNullOrEmpty(model.GHierarchyCode) ? model.GHierarchyCode : null);
            var checkDouble = activeTree.Where(
                x => x.Name == model.Name &&
                x.Type == model.Type &&
                x.IsOnInvoice == model.IsOnInvoice &&
                x.IsBaseClient == model.IsBaseClient &&
                x.DistrMarkUp == model.DistrMarkUp &&
                x.parentId == model.parentId &&
                x.DemandCode == (!string.IsNullOrEmpty(model.DemandCode) ? model.DemandCode : null) &&
                x.GHierarchyCode == model.GHierarchyCode &&
                x.DMDGroup == model.DMDGroup &&
                x.SFAClientCode == model.SFAClientCode).Count();

            if (checkDouble > 0)
            {
                string msg = "There is already such a record!";
                return Json(new { success = false, message = msg });
            }
            result.ObjectId = new int();
            Context.Set<ClientTree>().Add(result);
            var resultSaveChanges = await Context.SaveChangesAsync();

            if (result.DemandCode != null && result.DemandCode != "")
            {
                await CreateCoefficientSI2SOHandler(null, result.DemandCode, 1);
            }

            if (resultSaveChanges > 0)
            {
                await ClientTreeBrandTechesController.FillClientTreeBrandTechTableAsync(Context);
            }

            Context.Entry(result).Reload();

            return Json(new { success = true, children = result }, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> UpdateNode([FromBody] ClientTree model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.DemandCode != null)
            {
                if (model.DemandCode == "" || model.DemandCode == " ")
                {
                    model.DemandCode = null;
                }
            }
            try
            {
                activeTree = GetConstraintedQuery();
                var oldDemandCode = activeTree.Where(x => x.ObjectId == model.ObjectId && x.EndDate == null).Select(x => x.DemandCode).First();
                if (!string.IsNullOrEmpty(oldDemandCode) && model.DemandCode == null)
                {
                    var childsNull = activeTree.Where(x => x.parentId == model.ObjectId && x.DemandCode == null && x.IsBaseClient == true).Count();

                    if (childsNull > 0 && (model.Type == "Group chain" || model.Type == "Client" || model.Type == "Group"))
                    {
                        string msg = "This ClientTree not found DemandCode. The change is not possible!";
                        return Json(new { success = false, message = msg });
                    }
                }

                ClientTree currentRecord = activeTree.FirstOrDefault(x => x.ObjectId == model.ObjectId);

                if (currentRecord == null)
                {
                    return NotFound();
                }

                DateTime dt = DateTime.Now;
                ClientTree oldRecord = (ClientTree)currentRecord.Clone();
                oldRecord.EndDate = dt;

                string oldFullPath = currentRecord.FullPathName;
                int ind = oldFullPath.LastIndexOf(">");
                ind = ind < 0 ? 0 : ind + 2;

                model.FullPathName = oldFullPath.Substring(0, ind) + model.Name;
                model.StartDate = dt;
                if (!model.IsBaseClient)
                {
                    model.IsOnInvoice = null;
                    model.DistrMarkUp = null;
                }
                //Проверка DemandCode
                if (!CheckDemandCode(model))
                {
                    string msg = "There is a ClientTree with such DemandCode";
                    return Json(new { success = false, message = msg });
                }

                if (model.DemandCode != currentRecord.DemandCode || model.IsBaseClient != currentRecord.IsBaseClient)
                {
                    ChangesIncident changesIncident = new ChangesIncident
                    {
                        Disabled = false,
                        DeletedDate = null,
                        DirectoryName = "ClientTree",
                        ItemId = model.Id.ToString(),
                        CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                        ProcessDate = null
                    };
                    Context.Set<ChangesIncident>().Add(changesIncident);

                    List<ClientTree> childNodes = new List<ClientTree>();
                    var firstchildNodes = activeTree.Where(x => x.parentId == currentRecord.ObjectId).ToList();

                    GetChildNodes(firstchildNodes, childNodes);
                    if (model.DemandCode != currentRecord.DemandCode)
                    {
                        if (!currentRecord.IsBaseClient)
                            ClientTreeBrandTechesController.ResetClientTreeBrandTechDemandGroup(model.DemandCode, firstchildNodes, currentRecord, Context);
                        else
                            ClientTreeBrandTechesController.ResetClientTreeBrandTechDemandGroup(model.DemandCode, new List<ClientTree>() { currentRecord }, currentRecord, Context);
                    }
                    foreach (var node in childNodes)
                    {
                        if (node.IsBaseClient)
                        {
                            changesIncident = new ChangesIncident
                            {
                                Disabled = false,
                                DeletedDate = null,
                                DirectoryName = "ClientTree",
                                ItemId = node.Id.ToString(),
                                CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                                ProcessDate = null
                            };
                            Context.Set<ChangesIncident>().Add(changesIncident);
                        }
                    }
                }
                model.DeviationCoefficient = model.IsBaseClient
                                                ? model.DeviationCoefficient / -10000
                                                : 0;

                Context.Entry(currentRecord).CurrentValues.SetValues(model);
                UpdateFullPathClientTree(currentRecord, Context.Set<ClientTree>());
                Context.Set<ClientTree>().Add(oldRecord);
                var resultSaveChanges = await Context.SaveChangesAsync();

                if (currentRecord.DemandCode != null && currentRecord.DemandCode != "")
                {
                    await CreateCoefficientSI2SOHandler(null, currentRecord.DemandCode, 1);
                }

                if (resultSaveChanges > 0)
                {
                    await ClientTreeBrandTechesController.FillClientTreeBrandTechTableAsync(Context);
                    await ClientTreeBrandTechesController.DisableNotActualClientTreeBrandTech(Context);

                }

                return Created(currentRecord);
            }
            catch (Exception e)
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }

        [ClaimsAuthorize]
        [HttpPost]
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            try
            {
                activeTree = GetConstraintedQuery();
                ClientTree record = activeTree.FirstOrDefault(x => x.Id == key);
                List<ClientTree> childs = activeTree.Where(x => x.parentId == record.ObjectId).ToList();
                List<ClientTree> recordsToDelete = new List<ClientTree>();

                recordsToDelete.Add(record);

                while (childs.Count() > 0)
                {
                    recordsToDelete.AddRange(childs);
                    List<int> parents = childs.Select(ch => ch.ObjectId).ToList();
                    childs = activeTree.Where(x => parents.Contains(x.parentId)).ToList();
                }

                recordsToDelete.ForEach(x => x.EndDate = DateTime.Now);

                var resultSaveChanges = await Context.SaveChangesAsync();
                if (resultSaveChanges > 0)
                {
                    await ClientTreeBrandTechesController.FillClientTreeBrandTechTableAsync(Context);
                    await ClientTreeBrandTechesController.DisableNotActualClientTreeBrandTech(Context);
                    await ClientTreeBrandTechesController.DeleteInvalidClientBrandTech(key, Context);
                }
                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
            }
            catch (Exception e)
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }

        private void GetChildNodes(List<ClientTree> clientTreeList, List<ClientTree> nodes)
        {
            foreach (var client in clientTreeList)
            {
                bool leaf = !activeTree.Any(x => x.parentId == client.ObjectId);
                if (!leaf)
                {
                    var childNodes = activeTree.Where(x => x.parentId == client.ObjectId).ToList();
                    GetChildNodes(childNodes, nodes);
                }
                else if (client.IsBaseClient && !nodes.Any(x => x.ObjectId == client.ObjectId))
                {
                    nodes.Add(client);
                }
            }

            return;
        }

        private bool EntityExists(int key)
        {
            return Context.Set<ClientTree>().Count(e => e.ObjectId == key) > 0;
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult GetHierarchyDetail([FromODataUri] int key)
        {
            activeTree = GetConstraintedQuery();
            ClientTree clientTree = activeTree.Where(x => x.Id == key).FirstOrDefault();
            List<ClientTreeNode> nodes = new List<ClientTreeNode>();

            //получаем всех предков
            while (clientTree.Type != "root")
            {
                nodes.Add(new ClientTreeNode(clientTree, false, false, false));
                clientTree = activeTree.Where(x => x.ObjectId == clientTree.parentId).FirstOrDefault();
            }

            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, data = nodes }, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }

        /// <summary>
        /// Перемещение узлов
        /// </summary>
        /// <param name="nodeToMove"> Id узла который нужно переместить</param>
        /// <param name="destinationNode"> Id узла в который необходимо переместить узел</param>
        /// <returns></returns>
        [ClaimsAuthorize]
        [HttpPost]
        public async Task<IHttpActionResult> Move([FromODataUri] int nodeToMove, int destinationNode)
        {
            try
            {
                activeTree = GetConstraintedQuery();
                ClientTree recordToMove = activeTree.FirstOrDefault(x => x.Id == nodeToMove);
                ClientTree destinationRecord = activeTree.FirstOrDefault(x => x.Id == destinationNode);
                DateTime dt = DateTime.Now;

                if (recordToMove == null || destinationRecord == null)
                {
                    string msg = recordToMove == null ? "Can't find record to move" : "Can't find destination node";
                    return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, message = msg }));
                }

                // Создаём новую запись, старой устанавоиваем EndDate                
                ClientTree newRecord = (ClientTree)recordToMove.Clone();
                newRecord.parentId = destinationRecord.ObjectId;
                newRecord.StartDate = dt;
                newRecord.depth = destinationRecord.depth + 1;

                recordToMove.EndDate = dt;
                Context.Set<ClientTree>().Add(newRecord);
                await Context.SaveChangesAsync();

                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
            }
            catch (Exception e)
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }

        [ClaimsAuthorize]
        [HttpPost]
        // Проверка на возможность создать еще одного базового клиента в ветке.
        public IHttpActionResult CanCreateBaseClient(int? objectId, bool isCreateMode)
        {
            if (objectId != null)
            {
                var allClients = this.GetConstraintedQuery().ToList();
                var currentClient = allClients.FirstOrDefault(x => x.ObjectId == objectId);
                var newCurrentClient = (ClientTree)currentClient.Clone();
                var stack = new Stack<ClientTree>();

                // Поднимаемся в верх от текущего элемента.                
                while (newCurrentClient != null && newCurrentClient.Type != "root")
                {
                    // Если встречается базовый клиент.
                    if (newCurrentClient.IsBaseClient)
                    {
                        // То нельзя создать еще одного базового клиента.
                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false }));
                    }

                    // Родительсктй элемент становится текущим.
                    newCurrentClient = allClients.FirstOrDefault(x => x.ObjectId == newCurrentClient.parentId);
                }

                newCurrentClient = (ClientTree)currentClient.Clone();

                // Если форма открыта на редактирование.
                if (!isCreateMode)
                {
                    do
                    {
                        // Ищем потомков и добавляем их в стек.
                        foreach (var client in GetAllChildrenCurrentClient(allClients, newCurrentClient))
                        {
                            stack.Push(client);
                        }

                        // Если в стеке что-то есть.
                        if (stack.Count != 0)
                        {
                            // Извлекаем из стека последнего добавленного клиента.
                            newCurrentClient = stack.Pop();
                            // Проверяем его на налачие флага базового клиента.
                            if (newCurrentClient.IsBaseClient)
                            {
                                // Если он помечен флагом базового клиента, то нельзя создать еще одного.
                                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false }));
                            }

                            // Ищем потомков и добавляем их в стек.
                            foreach (var client in GetAllChildrenCurrentClient(allClients, newCurrentClient))
                            {
                                stack.Push(client);
                            }
                        }
                    }
                    // Пока стек не пустой.
                    while (stack.Count != 0);
                }

                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
            }

            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false }));
        }

        // Получаем всех потомков элемента.
        public Stack<ClientTree> GetAllChildrenCurrentClient(IEnumerable<ClientTree> allClients, ClientTree currentClient)
        {
            var stack = new Stack<ClientTree>();
            // Собираем всех потомков текущего клиента.
            var childrens = allClients.Where(x => x.parentId == currentClient.ObjectId);

            // Если потомки есть.
            if (childrens.Count() != 0)
            {
                // Проходимся по ним и добавляем в стек.
                foreach (var children in childrens)
                {
                    stack.Push(children);
                }
            }

            return stack;
        }

        /// <summary>
        /// Обновить FullPathName для потомков в ClientTree
        /// </summary>
        /// <param name="node">Родительский узел</param>
        private void UpdateFullPathClientTree(ClientTree node, IQueryable<ClientTree> tree)
        {
            ClientTree[] children = tree.Where(n => n.parentId == node.ObjectId).ToArray();

            for (int i = 0; i < children.Length; i++)
            {
                children[i].FullPathName = node.FullPathName + " > " + children[i].Name;
                UpdateFullPathClientTree(children[i], tree);
            }
        }

        /// <summary>
        /// Проверка DemandCode
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        public bool CheckDemandCode(ClientTree tree)
        {
            return String.IsNullOrEmpty(tree.DemandCode) || !GetConstraintedQuery().Any(y => y.DemandCode == tree.DemandCode && tree.ObjectId != y.ObjectId);
        }

        [ClaimsAuthorize]
        [HttpPost]
        public async Task<IHttpActionResult> UploadLogoFile(int clientTreeId)
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                FileDispatcher fileDispatcher = new FileDispatcher();
                string directory = Core.Settings.AppSettingsManager.GetSetting("CLIENT_TREE_DIRECTORY", "ClientTreeLogoFiles");
                string fullPathfile = await FileUtility.UploadFile(Request, directory);
                string fileName = fullPathfile.Split('\\').Last();

                // так себе проверка, но лучше что-то, чем ничего
                string typeFile = fullPathfile.Split('.').Last().ToLower();
                if (typeFile != "png" && typeFile != "jpg")
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

                ClientTree clientTree = Context.Set<ClientTree>().Find(clientTreeId);
                if (clientTree == null)
                    return NotFound();

                // удаляем старую картинку если была
                if (clientTree.LogoFileName != null)
                {
                    if (fileDispatcher.IsExists(directory, clientTree.LogoFileName))
                    {
                        fileDispatcher.DeleteFile(Path.Combine(directory, clientTree.LogoFileName));
                    }
                }

                clientTree.LogoFileName = fileName;
                await Context.SaveChangesAsync();

                return Json(new { success = true, fileName });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        [ClaimsAuthorize]
        [HttpGet]
        [Route("odata/ClientTrees/DownloadLogoFile")]
        public HttpResponseMessage DownloadLogoFile(string fileName)
        {
            try
            {
                string directory = Core.Settings.AppSettingsManager.GetSetting("CLIENT_TREE_DIRECTORY", "ClientTreeLogoFiles");
                string logDir = Core.Settings.AppSettingsManager.GetSetting<string>("HANDLER_LOG_TYPE", "File");

                if (logDir == "Azure")
                {
                    return FileUtility.DownloadFileAzure(directory, fileName);
                }
                return FileUtility.DownloadFile(directory, fileName);

            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.Accepted);
            }
        }

        [ClaimsAuthorize]
        [HttpPost]
        public async Task<IHttpActionResult> DeleteLogo(int id)
        {
            var currentClient = Context.Set<ClientTree>().Find(id);

            if (currentClient != null && !String.IsNullOrEmpty(currentClient.LogoFileName))
            {
                // удаляем старое лого
                string directory = Core.Settings.AppSettingsManager.GetSetting("CLIENT_TREE_DIRECTORY", "ClientTreeLogoFiles");
                FileDispatcher fileDispatcher = new FileDispatcher();
                if (fileDispatcher.IsExists(directory, currentClient.LogoFileName))
                {
                    fileDispatcher.DeleteFile(Path.Combine(directory, currentClient.LogoFileName));
                }

                currentClient.LogoFileName = null;
                await Context.SaveChangesAsync();
                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, message = "The file from selected client was removed successfully." }));
            }
            else
            {
                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, message = "The logo is not exists for current client." }));
            }
        }

        [HttpPost]
        public IHttpActionResult GetUploadingClients()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            IQueryable<JobFlagView> jobs = Context.SqlQuery<JobFlagView>(
                $@"SELECT Prefix, Value FROM {defaultSchema}.JobFlag WHERE Description LIKE 'Upload client%'").AsQueryable();
            var uploadingClients = jobs.Where(x => x.Value == 1).Select(x => x.Prefix).ToList();
            var availableStatuses = new List<string> { RSstateNames.ON_APPROVAL, RSstateNames.DRAFT };
            var availableClients = Context.Set<RollingScenario>()
                .Where(x => !x.Disabled && x.ScenarioType == ScenarioType.RA && availableStatuses.Contains(x.RSstatus))
                .Select(x => x.ClientTree.ObjectId.ToString())
                .ToList();
            availableClients = availableClients.Except(uploadingClients).ToList();
            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, availableClients }));
        }

        [ClaimsAuthorize]
        public IHttpActionResult SaveScenario(string ClientName, string ObjectId, string ScenarioType)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            var scenario = ScenarioHelper.GetActiveScenario(Int32.Parse(ObjectId), Context);

            if (scenario == null) return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, message = "RA scenario not found" }));
            List<Guid> PromoRSIds = scenario.Promoes.Select(f => f.Id).ToList();
            if (Context.Set<BlockedPromo>().Any(x => x.Disabled == false && PromoRSIds.Contains(x.PromoId)))
            {
                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, message = "there is a blocked Promo" }));
            }
            var email = NotificationsHelper.GetUsersEmail(new List<Guid>() { (Guid)user.Id }, Context).First();
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");

            var scenarioName = $"{ObjectId}_{DateTime.Now:yyyyMMddhhmmss}";
            try
            {
                var createRunScript = $@"
                    INSERT INTO [{defaultSchema}].[ScenarioCopyTask]
                            ([Disabled]
                            ,[DeletedDate]
                            ,[ClientPrefix]
                            ,[ClientObjectId]
                            ,[Schema]
                            ,[CreateDate]
                            ,[ProcessDate]
                            ,[ScenarioType]
                            ,[ScenarioName]
                            ,[Status]
                            ,[Email])
                        VALUES
                            (0
                            ,NULL
                            ,'{ClientName}'
                            ,{ObjectId}
                            ,'{defaultSchema}'
                            ,GETDATE()
                            ,NULL
                            ,'{ScenarioType}'
                            ,'{scenarioName}'
                            ,'WAITING'
                            ,'{email ?? "NULL"}')
                ";
                //copy scenario
                var newSavedScenario = new SavedScenario
                {
                    RollingScenario = scenario,
                    ScenarioName = scenarioName
                };
                Context.Set<SavedScenario>().Add(newSavedScenario);
                Context.SaveChanges();
                List<int> promoHiddennumbers = scenario.Promoes.Select(h => h.Number).Cast<int>().ToList();
                List<Promo> promos = Context.Set<Promo>()
                        //.Include(g => g.BTLPromoes)
                        .Include(g => g.PromoSupportPromoes)
                        .Include(g => g.PromoProductTrees)
                        .Include(g => g.IncrementalPromoes)
                        .Include(x => x.PromoProducts.Select(y => y.PromoProductsCorrections))
                        .Include(g => g.PromoPriceIncrease.PromoProductPriceIncreases.Select(f => f.ProductCorrectionPriceIncreases))
                        .Where(x => promoHiddennumbers.Contains((int)x.Number) && x.TPMmode == TPMmode.RA).ToList();
                HiddenModeHelper.CopyPromoesToHidden(Context, promos, newSavedScenario);
                Context.ExecuteSqlCommand(createRunScript);
                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, message = "Create run success" }));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = false, message = "Create run failure " + ex.Message }));
            }
        }
        [ClaimsAuthorize]
        public async Task<IHttpActionResult> CopyYearScenario(string ObjectId, bool CheckedDate)
        {
            int objId = int.Parse(ObjectId);
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id ?? Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id ?? Guid.Empty);

            HandlerData handlerData = new HandlerData();
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, handlerData, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, handlerData, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, handlerData, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ObjectId", objId, handlerData, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("CheckedDate", CheckedDate, handlerData, visible: false, throwIfNotExists: false);

            string clientTreeName = Context.Set<ClientTree>().FirstOrDefault(g => g.ObjectId == objId && g.EndDate == null).Name;

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = $"Copy prevous year for client {clientTreeName}",
                Name = "Module.Host.TPM.Handlers.CopyPrevousYearHandler",
                ExecutionPeriod = null,
                CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                LastExecutionDate = null,
                NextExecutionDate = null,
                ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                UserId = userId,
                RoleId = roleId
            };
            handler.SetParameterData(handlerData);
            Context.LoopHandlers.Add(handler);
            await Context.SaveChangesAsync();
            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));

        }
        /// <summary>
        /// Создание задачи на добавление новых записей коэффицициентов
        /// </summary>
        /// <param name="promo"></param>
        private async Task CreateCoefficientSI2SOHandler(string brandTechCode, string demandCode, double cValue)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            HandlerData data = new HandlerData();
            HandlerDataHelper.SaveIncomingArgument("brandTechCode", brandTechCode, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("demandCode", demandCode, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("cValue", cValue, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = "Adding new records for coefficients SI/SO",
                Name = "Module.Host.TPM.Handlers.CreateCoefficientSI2SOHandler",
                ExecutionPeriod = null,
                RunGroup = "CreateCoefficientSI2SO",
                CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                LastExecutionDate = null,
                NextExecutionDate = null,
                ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                UserId = userId,
                RoleId = roleId
            };
            handler.SetParameterData(data);
            Context.LoopHandlers.Add(handler);
            await Context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Класс-обертка для дерева (в ExtJS)
    /// </summary>
    public class ClientTreeNode
    {
        public int Id { get; set; }
        public int ObjectId { get; set; }
        public string RetailTypeName { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string FullPathName { get; set; }
        public bool? IsOnInvoice { get; set; }
        public int parentId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string GHierarchyCode { get; set; }
        public string DemandCode { get; set; }
        public string DMDGroup { get; set; }
        public bool IsBaseClient { get; set; }
        public bool leaf { get; set; }
        public bool loaded { get; set; }
        public bool expanded { get; set; }
        public bool _checked { get; set; }
        public List<ClientTreeNode> children { get; set; }
        public int depth { get; set; }
        public bool? IsBeforeStart { get; set; }
        public int? DaysStart { get; set; }
        public bool? IsDaysStart { get; set; }
        public bool? IsBeforeEnd { get; set; }
        public int? DaysEnd { get; set; }
        public bool? IsDaysEnd { get; set; }
        public double? DistrMarkUp { get; set; }
        public string SFAClientCode { get; set; }
        public double? DeviationCoefficient { get; set; }
        public string LogoFileName { get; set; }

        public ClientTreeNode(ClientTree treeNode, bool expanded, bool leaf, bool loaded, bool _checked = false)
        {
            Id = treeNode.Id;
            ObjectId = treeNode.ObjectId;
            RetailTypeName = treeNode.RetailTypeName;
            Type = treeNode.Type;
            Name = treeNode.Name;
            FullPathName = treeNode.FullPathName;
            IsOnInvoice = treeNode.IsOnInvoice;
            parentId = treeNode.parentId;
            StartDate = treeNode.StartDate;
            EndDate = treeNode.EndDate;
            GHierarchyCode = treeNode.GHierarchyCode;
            DemandCode = treeNode.DemandCode;
            IsBaseClient = treeNode.IsBaseClient;
            depth = treeNode.depth;
            IsBeforeStart = treeNode.IsBeforeStart;
            DaysStart = treeNode.DaysStart;
            IsDaysStart = treeNode.IsDaysStart;
            IsBeforeEnd = treeNode.IsBeforeEnd;
            DaysEnd = treeNode.DaysEnd;
            IsDaysEnd = treeNode.IsDaysEnd;
            LogoFileName = treeNode.LogoFileName;
            DMDGroup = treeNode.DMDGroup;
            DistrMarkUp = treeNode.DistrMarkUp;
            SFAClientCode = treeNode.SFAClientCode;
            DeviationCoefficient = treeNode.DeviationCoefficient;

            this.leaf = leaf;
            this.loaded = loaded;
            this.expanded = expanded;
            this._checked = _checked;
        }

        /// <summary>
        /// Добавить элемент-потомок
        /// </summary>
        /// <param name="child">Потомок</param>
        public void AddChild(ClientTreeNode child)
        {
            if (children == null)
                children = new List<ClientTreeNode>();

            children.Add(child);
        }

        /// <summary>
        /// Добавить несколько потомков
        /// </summary>
        /// <param name="child">Список потомков</param>
        public void AddChild(List<ClientTreeNode> child)
        {
            if (children == null)
                children = new List<ClientTreeNode>();

            children.AddRange(child);
        }
    }
}