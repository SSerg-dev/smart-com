using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.Interfaces;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl.RoleStateMap;
using Persist;
using Persist.Model;
using Persist.ScriptGenerator;
using Persist.ScriptGenerator.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace Module.Persist.TPM.Utils
{
    public static class ModuleApplyFilterHelper
    {
        /// <summary>
        /// Применение фильтра по ограничениям к Промо
        /// </summary>
        /// <param name="query"></param>
        /// <param name="hierarchy"></param>
        /// <param name="filter"></param>
        /// <param name="filterMode"></param>
        /// <returns></returns>
        public static IQueryable<Promo> ApplyFilter(IQueryable<Promo> query, IQueryable<ClientTreeHierarchyView> hierarchy, TPMmode mode = TPMmode.Current, IDictionary<string, IEnumerable<string>> filter = null, FilterQueryModes filterMode = FilterQueryModes.Active, string role = "")
        {
            if (filterMode == FilterQueryModes.Active)
            {
                query = query.Where(x => !x.Disabled);
            }
            if (filterMode == FilterQueryModes.Deleted)
            {
                query = query.Where(x => x.Disabled);
            }
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.ClientTreeId || h.Hierarchy.Contains(x.ClientTreeId.Value.ToString())));
            }
            if (!String.IsNullOrEmpty(role))
            {
                IEnumerable<Promo> promoToFilter = query.AsEnumerable();
                promoToFilter = promoToFilter.Where(x => RoleStateUtil.RoleCanChangeState(role, x.PromoStatus.SystemName) && RoleStateUtil.IsOnApprovalRoleOrder(role, x));
                query = promoToFilter.AsQueryable();
            }
            switch (mode)
            {
                case TPMmode.Current:
                    query = query.Where(x => x.TPMmode == TPMmode.Current);
                    break;
                case TPMmode.RS:
                    query = query.GroupBy(x => x.Number, (key, g) => g.OrderByDescending(e => e.TPMmode).FirstOrDefault());
                    //query = query.Where(x => x.TPMmode == TPMmode.RS);
                    break;
            }
            return query;
        }

        /// <summary>
        /// Применение фильтра по ограничениям к Промо конкурентов
        /// </summary>
        /// <param name="query"></param>
        /// <param name="hierarchy"></param>
        /// <param name="filter"></param>
        /// <param name="filterMode"></param>
        /// <returns></returns>
        public static IQueryable<CompetitorPromo> ApplyFilter(IQueryable<CompetitorPromo> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null, FilterQueryModes filterMode = FilterQueryModes.Active, string role = "")
        {
            if (filterMode == FilterQueryModes.Active)
            {
                query = query.Where(x => !x.Disabled);
            }
            if (filterMode == FilterQueryModes.Deleted)
            {
                query = query.Where(x => x.Disabled);
            }
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.ClientTree.ObjectId || h.Hierarchy.Contains(x.ClientTree.ObjectId.ToString())));
            }
            if (!String.IsNullOrEmpty(role))
            {
                IEnumerable<CompetitorPromo> promoToFilter = query.AsEnumerable();
                query = promoToFilter.AsQueryable();
            }
            return query;
        }

        /// <summary>
        /// Применение фильтра по ограничениям к Промо
        /// </summary>
        /// <param name="query"></param>
        /// <param name="hierarchy"></param>
        /// <param name="filter"></param>
        /// <param name="filterMode"></param>
        /// <returns></returns>
        public static IQueryable<PromoGridView> ApplyFilter(IQueryable<PromoGridView> query, IQueryable<ClientTreeHierarchyView> hierarchy, TPMmode mode, IDictionary<string, IEnumerable<string>> filter = null, FilterQueryModes filterMode = FilterQueryModes.Active, string role = "")
        {

            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.ClientTreeId));
            }
            if (!String.IsNullOrEmpty(role))
            {
                IEnumerable<PromoGridView> promoToFilter = query.AsEnumerable();
                promoToFilter = promoToFilter.Where(x => RoleStateUtil.RoleCanChangeState(role, x.PromoStatusSystemName) && RoleStateUtil.IsOnApprovalRoleOrder(role, x));
                if (role == "CustomerMarketing")
                {
                    promoToFilter = promoToFilter.Where(x => x.PromoStatusSystemName != "OnApproval");
                }
                query = promoToFilter.AsQueryable();
            }
            switch (mode)
            {
                case TPMmode.Current:
                    query = query.Where(x => x.TPMmode == TPMmode.Current);
                    if (filterMode == FilterQueryModes.Active)
                    {
                        query = query.Where(x => !x.Disabled);
                    }
                    if (filterMode == FilterQueryModes.Deleted)
                    {
                        query = query.Where(x => x.Disabled);
                    }
                    break;
                case TPMmode.RS:
                    query = query.GroupBy(x => x.Number, (key, g) => g.OrderByDescending(e => e.TPMmode).FirstOrDefault()).Where(g => !g.Disabled);
                    //query = query.Where(x => x.TPMmode == TPMmode.RS);
                    break;
            }
            return query;
        }

        /// <summary>
        /// Применение фильтра по ограничениям к Промо для Календаря
        /// </summary>
        /// <param name="query"></param>
        /// <param name="hierarchy"></param>
        /// <param name="filter"></param>
        /// <param name="filterMode"></param>
        /// <returns></returns>
        public static IQueryable<PromoView> ApplyFilter(IQueryable<PromoView> query, IQueryable<ClientTreeHierarchyView> hierarchy, TPMmode mode, IDictionary<string, IEnumerable<string>> filter = null, FilterQueryModes filterMode = FilterQueryModes.Active, string role = "")
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.ClientTreeId || h.Hierarchy.Contains(x.ClientTreeId.Value.ToString())));
            }
            switch (mode)
            {
                case TPMmode.Current:
                    query = query.Where(x => x.TPMmode == TPMmode.Current);
                    break;
                case TPMmode.RS:
                    query = query.GroupBy(x => x.Number, (key, g) => g.OrderByDescending(e => e.TPMmode).FirstOrDefault());

                    //query = query.Where(x => x.TPMmode == TPMmode.RS);
                    break;
            }
            return query;
        }
        /// <summary>
        /// Применение фильтра по ограничениям к иерархии клиентов
        /// </summary>
        /// <param name="query"></param>
        /// <param name="hierarchy"></param>
        /// <param name="filter"></param>
        /// <param name="filterMode"></param>
        /// <returns></returns>
        public static IQueryable<ClientTree> ApplyFilter(IQueryable<ClientTree> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null, FilterQueryModes filterMode = FilterQueryModes.Active, bool forTree = false)
        {
            List<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client).ToList();

            if (clientFilter.Any())
            {
                List<ClientTreeHierarchyView> hierarchyList = getFilteredHierarchy(hierarchy, clientFilter).ToList();
                List<int> filteredId = hierarchyList.Select(n => n.Id).ToList();
                List<string> filteredHierarchies = hierarchyList.Select(n => n.Hierarchy).ToList();

                // если грузим дерево, то родительские элементы тоже нужны
                if (forTree)
                {
                    foreach (string cf in clientFilter)
                    {
                        ClientTree constrainedNode = query.FirstOrDefault(n => n.ObjectId.ToString() == cf);
                        if (constrainedNode != null)
                        {
                            int parentObjectId = constrainedNode.parentId;
                            ClientTree currentNode;

                            do
                            {
                                currentNode = query.FirstOrDefault(n => n.ObjectId == parentObjectId);
                                if (currentNode != null)
                                {
                                    filteredId.Add(currentNode.ObjectId);
                                    parentObjectId = currentNode.parentId;
                                }

                            } while (currentNode != null && currentNode.Type.ToLower() != "root");
                        }

                    }
                }

                query = query.Where(x => filteredId.Contains(x.ObjectId) || filteredHierarchies.Contains(x.ObjectId.ToString()));
            }
            return query;
        }
        /// <summary>
        /// Применение фильтра по ограничениям к привязке EventClientTree
        /// </summary>
        /// <param name="query"></param>
        /// <param name="hierarchy"></param>
        /// <param name="filter"></param>
        /// <param name="filterMode"></param>
        /// <returns></returns>
        public static IQueryable<EventClientTree> ApplyFilter(IQueryable<EventClientTree> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null, FilterQueryModes filterMode = FilterQueryModes.Active)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.ClientTree.ObjectId));
            }
            return query;
        }
        /// <summary>
        /// Применение фильтра по ограничениям к базовым клиентам
        /// </summary>
        /// <param name="query"></param>
        /// <param name="hierarchy"></param>
        /// <param name="filter"></param>
        /// <param name="filterMode"></param>
        /// <returns></returns>
        public static IQueryable<BaseClientTreeView> ApplyFilter(IQueryable<BaseClientTreeView> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null, FilterQueryModes filterMode = FilterQueryModes.Active)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.BOI));
            }
            return query;
        }

        /// <summary>
        /// Применение фильтра по ограничениям
        /// </summary>
        /// <param name="query"></param>
        /// <param name="hierarchy"></param>
        /// <param name="filter"></param>
        /// <param name="filterMode"></param>
        /// <returns></returns>
        public static IQueryable<ClientTreeSharesView> ApplyFilter(IQueryable<ClientTreeSharesView> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null, FilterQueryModes filterMode = FilterQueryModes.Active)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.BOI));
            }
            return query;
        }

        /// <summary>
        /// Применение фильтра по ограничениям к базовым клиентам
        /// </summary>
        /// <param name="query"></param>
        /// <param name="hierarchy"></param>
        /// <param name="filter"></param>
        /// <param name="filterMode"></param>
        /// <returns></returns>
        public static IQueryable<ClientTreeBrandTech> ApplyFilter(IQueryable<ClientTreeBrandTech> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null, FilterQueryModes filterMode = FilterQueryModes.Active)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.ClientTree.ObjectId));
            }
            return query;
        }

        /// <summary>
        /// Применение фильтра по ограничениям к NoneNego
        /// </summary>
        /// <param name="query"></param>
        /// <param name="hierarchy"></param>
        /// <param name="filter"></param>
        /// <param name="filterMode"></param>
        /// <returns></returns>
        public static IQueryable<NoneNego> ApplyFilter(IQueryable<NoneNego> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null, FilterQueryModes filterMode = FilterQueryModes.Active)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.ClientTree.ObjectId));
            }
            return query;
        }

        /// <summary>
        /// Применение фильтра ImportBaseLine
        /// </summary>
        /// <param name="query"></param>
        /// <param name="startDate"></param>
        /// <param name="finishDate"></param>
        /// <param name="filter"></param>
        /// <param name="filterMode"></param>
        /// <returns></returns>
        public static IQueryable<ImportBaseLine> ApplyFilter(IQueryable<ImportBaseLine> query, DateTimeOffset startDate, DateTimeOffset finishDate, IDictionary<string, IEnumerable<string>> filter = null, FilterQueryModes filterMode = FilterQueryModes.Active)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            query = query.Where(x => x.StartDate >= startDate && x.StartDate <= finishDate);
            if (clientFilter.Any())
            {
                query = query.Where(q => clientFilter.Contains(q.ClientTreeDemandCode));
            }
            return query;
        }

        /// <summary>
        /// Применение фильтра ImportIncrementalPromo
        /// <param name="query"></param>
        /// <param name="filter"></param>
        /// <param name="filterMode"></param>
        /// <returns></returns>
        public static IList<ImportIncrementalPromo> ApplyFilter(IQueryable<ImportIncrementalPromo> query, DatabaseContext context, IDictionary<string, IEnumerable<string>> filter = null, FilterQueryModes filterMode = FilterQueryModes.Active)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                query = query.Where(x => context.Set<IncrementalPromo>()
                            .Any(y => clientFilter.Contains(y.Promo.ClientTreeId.ToString())));
            }
            return query.ToList();
        }

        /// <summary>
        /// Применить фильтр по клиентам к AssortmentMatrix
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="hierarchy">Иерархия</param>
        /// <param name="filter">Фильтр</param>

        public static IQueryable<AssortmentMatrix> ApplyFilter(IQueryable<AssortmentMatrix> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.ClientTree.ObjectId));
            }
            return query;
        }

        public static IQueryable<PLUDictionary> ApplyFilter(IQueryable<PLUDictionary> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                var ids = hierarchy.Select(x => x.Id).ToList();
                query = query.Where(x => ids.Contains(x.ObjectId));
            }
            return query;
        }

        /// <summary>
        /// Применить фильтр по клиентам к PromoSupport
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="hierarchy">Иерархия</param>
        /// <param name="filter">Фильтр</param>
        public static IQueryable<PromoSupport> ApplyFilter(IQueryable<PromoSupport> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {

                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.ClientTree.ObjectId));
            }
            return query;
        }

        /// <summary>
        /// Применить фильтр по клиентам к NonPromoSupport
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="hierarchy">Иерархия</param>
        /// <param name="filter">Фильтр</param>
        public static IQueryable<NonPromoSupport> ApplyFilter(IQueryable<NonPromoSupport> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.ClientTree.ObjectId));
            }
            return query;
        }
        /// <summary>
        /// Применить фильтр по клиентам к PromoProductsCorrection
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="hierarchy">Иерархия</param>
        /// <param name="filter">Фильтр</param>
        public static IQueryable<PromoProductsCorrection> ApplyFilter(IQueryable<PromoProductsCorrection> query, IQueryable<ClientTreeHierarchyView> hierarchy, TPMmode mode, IDictionary<string, IEnumerable<string>> filter = null)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.PromoProduct.Promo.ClientTree.ObjectId));
            }
            switch (mode)
            {
                case TPMmode.Current:
                    query = query.Where(x => x.TPMmode == TPMmode.Current && !x.Disabled);
                    break;
                case TPMmode.RS:
                    query = query.GroupBy(x => new { x.PromoProduct.Promo.Number, x.PromoProductId }, (key, g) => g.OrderByDescending(e => e.TPMmode).FirstOrDefault());
                    query = query.Where(x => !x.Disabled);
                    //query = query.ToList().AsQueryable();
                    //var deletedRSPromoes
                    break;
            }
            return query;
        }
        /// <summary>
        /// Применить фильтр по клиентам к ClienDashboard kpidata
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="hierarchy">Иерархия</param>
        /// <param name="filter">Фильтр</param>
        public static IQueryable<ClientDashboardView> ApplyFilter(IQueryable<ClientDashboardView> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.ObjectId));
            }
            return query;
        }
        /// <summary>
        /// Применить фильтр по клиентам к PreviousDayIncremental
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="hierarchy">Иерархия</param>
        /// <param name="filter">Фильтр</param>
        public static IQueryable<PreviousDayIncremental> ApplyFilter(IQueryable<PreviousDayIncremental> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.Promo.ClientTree.ObjectId));
            }
            return query;
        }
        /// <summary>
        /// Применить фильтр по клиентам к constraint
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="hierarchy">Иерархия</param>
        /// <param name="filter">Фильтр</param>
        public static IQueryable<Constraint> ApplyFilter(IQueryable<Constraint> query, IQueryable<ClientTreeHierarchyView> hierarchy)
        {

            query = query.Where(x =>
                hierarchy.Any(h => x.Value.Equals(h.Id.ToString())));

            return query;
        }

        /// <summary>
        /// Применить фильтр по клиентам к COGS
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="hierarchy">Иерархия</param>
        /// <param name="filter">Фильтр</param>
        public static IQueryable<COGS> ApplyFilter(IQueryable<COGS> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.ClientTree.ObjectId));
            }
            return query;
        }

        /// <summary>
        /// Применить фильтр по клиентам к COGS/Tn
        /// </summary>
        public static IQueryable<PlanCOGSTn> ApplyFilter(IQueryable<PlanCOGSTn> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.ClientTree.ObjectId));
            }
            return query;
        }

        /// <summary>
        /// Применить фильтр по клиентам к RATIShopper
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="hierarchy">Иерархия</param>
        /// <param name="filter">Фильтр</param>
        public static IQueryable<RATIShopper> ApplyFilter(IQueryable<RATIShopper> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.ClientTree.ObjectId));
            }
            return query;
        }

        /// <summary>
        /// Применить фильтр по клиентам к TradeInvestment
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="hierarchy">Иерархия</param>
        /// <param name="filter">Фильтр</param>
        public static IQueryable<TradeInvestment> ApplyFilter(IQueryable<TradeInvestment> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.ClientTree.ObjectId));
            }
            return query;
        }

        /// <summary>
        /// Применить фильтр по клиентам к ActualCOGS
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="hierarchy">Иерархия</param>
        /// <param name="filter">Фильтр</param>
        public static IQueryable<ActualCOGS> ApplyFilter(IQueryable<ActualCOGS> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.ClientTree.ObjectId));
            }
            return query;
        }

        /// <summary>
        /// Применить фильтр по клиентам к ActualCOGSTn
        /// </summary>
        public static IQueryable<ActualCOGSTn> ApplyFilterActualCOGSTn(IQueryable<ActualCOGSTn> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.ClientTree.ObjectId));
            }
            return query;
        }

        /// <summary>
        /// Применить фильтр по клиентам к ActualTradeInvestment
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="hierarchy">Иерархия</param>
        /// <param name="filter">Фильтр</param>
        public static IQueryable<ActualTradeInvestment> ApplyFilter(IQueryable<ActualTradeInvestment> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.ClientTree.ObjectId));
            }
            return query;
        }

        /// <summary>
        /// Применить фильтр по клиентам к PromoROIReport
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="hierarchy">Иерархия</param>
        /// <param name="filter">Фильтр</param>
        public static IQueryable<PromoROIReport> ApplyFilter(IQueryable<PromoROIReport> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.ClientTreeId));
            }
            return query;
        }

        /// <summary>
        /// Применить фильтр по клиентам к IncrementalPromo
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="hierarchy">Иерархия</param>
        /// <param name="filter">Фильтр</param>
        public static IQueryable<IncrementalPromo> ApplyFilter(IQueryable<IncrementalPromo> query, IQueryable<ClientTreeHierarchyView> hierarchy, TPMmode mode, IDictionary<string, IEnumerable<string>> filter = null)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.Promo.ClientTreeId));
            }
            switch (mode)
            {
                case TPMmode.Current:
                    query = query.Where(x => x.TPMmode == TPMmode.Current);
                    break;
                case TPMmode.RS:
                    query = query.GroupBy(x => new { x.Promo.Number, x.Product.Id }, (key, g) => g.OrderByDescending(e => e.TPMmode).FirstOrDefault());
                    //query = query.ToList().AsQueryable();
                    //var deletedRSPromoes
                    break;
            }
            return query;
        }

        /// <summary>
        /// Применить фильтр по клиентам к ActualLSV
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="hierarchy">Иерархия</param>
        /// <param name="filter">Фильтр</param>
        public static IQueryable<Promo> ApplyFilter(IQueryable<Promo> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.ClientTreeId));
            }
            return query;
        }

        /// <summary>
        /// Применить фильтр по клиентам к BTLPromo
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="hierarchy">Иерархия</param>
        /// <param name="filter">Фильтр</param>
        public static IQueryable<BTLPromo> ApplyFilter(IQueryable<BTLPromo> query, IQueryable<ClientTreeHierarchyView> hierarchy, TPMmode mode, IDictionary<string, IEnumerable<string>> filter = null)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.Promo.ClientTreeId));
            }
            query = query.Where(x => !x.Disabled || x.TPMmode == TPMmode.RS);
            switch (mode)
            {
                case TPMmode.Current:
                    query = query.Where(x => x.TPMmode == TPMmode.Current);
                    break;
                case TPMmode.RS:
                    query = query.GroupBy(x => new { x.BTLId, x.Promo.Number }, (key, g) => g.OrderByDescending(e => e.TPMmode).FirstOrDefault()).Where(x => !x.Disabled);
                    break;
            }
            return query;
        }

        /// <summary>
        /// Применить фильтр по клиентам к PlanPostPromoEffectReport
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="context">Контекст БД</param>
        /// <param name="hierarchy">Иерархия</param>
        /// <param name="filter">Фильтр</param>
        public static IQueryable<PlanPostPromoEffectReportWeekView> ApplyFilter(IQueryable<PlanPostPromoEffectReportWeekView> query, DatabaseContext context, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);

            if (clientFilter.Any())
            {
                IQueryable<int> numbers = query.Select(q => q.PromoNumber);

                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                IQueryable<string> queryIds = context.Set<Promo>()
                    .Where(x
                        => numbers.Contains(x.Number.Value)
                            && hierarchy.Any(h => h.Id == x.ClientTreeId))
                    .Select(x => x.Name + "#" + x.Number.ToString());

                query = query.Where(x => queryIds.Contains(x.PromoNameId));
            }
            return query;
        }

        /// <summary>
        /// Применить фильтр по клиентам к PlanIncrementalReport
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="context">Контекст БД</param>
        /// <param name="hierarchy">Иерархия</param>
        /// <param name="filter">Фильтр</param>
        public static IQueryable<PlanIncrementalReport> ApplyFilter(IQueryable<PlanIncrementalReport> query, DatabaseContext context, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null, bool forExport = false)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);

            if (clientFilter.Any())
            {
                IQueryable<int> numbers = query.Select(q => q.PromoNumber);

                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                IQueryable<string> queryIds = context.Set<Promo>()
                    .Where(x
                        => numbers.Contains(x.Number.Value)
                            && hierarchy.Any(h => h.Id == x.ClientTreeId))
                    .Select(x => x.Name + "#" + x.Number.ToString());

                query = query.Where(x => queryIds.Contains(x.PromoNameId));
            }
            //ToList().AsQueryable() необходим для ускорения группировки позже
            if (forExport)
            {
                return query;
            }
            else
            {
                return query.ToList().AsQueryable();
            }
        }

        /// <summary>
        /// Применить фильтр по клиентам к PlanPostPromoEffectReport
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="hierarchy">Иерархия</param>
        /// <param name="filter">Фильтр</param>
        public static IEnumerable<SimplePromoPromoProduct> ApplyFilter(IEnumerable<SimplePromoPromoProduct> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null)
        {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.ClientTreeId)).ToList();
            }
            return query;
        }

        /// <summary>
        /// Построение фильтра для генерации SQl-запроса
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="finishDate"></param>
        /// <param name="filter"></param>
        /// <param name="filterMode"></param>
        /// <returns></returns>
        public static PersistFilter BuildBaseLineFilter(DateTimeOffset? startDate, DateTimeOffset? finishDate, IDictionary<string, IEnumerable<string>> filter, FilterQueryModes filterMode = FilterQueryModes.Active)
        {

            PersistFilter result = new PersistFilter();
            result.QueryMode = filterMode;
            result.Where.Operator = Operators.And;

            if (startDate.HasValue)
            {
                result.Where.Rules.Add(new FilterRule("StartDate", Operations.MoreOrEquals, startDate.Value));
            }
            if (finishDate.HasValue)
            {
                result.Where.Rules.Add(new FilterRule("StartDate", Operations.LessOrEquals, finishDate.Value));
            }
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                result.Where.Rules.Add(new FilterRule("DemandCode", Operations.In, clientFilter));
            }
            return result;
        }

        /// <summary>
        /// Построение фильтра для генерации SQl-запроса
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="filterMode"></param>
        /// <returns></returns>
        public static PersistFilter BuildIncrementalPromoFilter(IDictionary<string, IEnumerable<string>> filter, FilterQueryModes filterMode = FilterQueryModes.Active)
        {
            PersistFilter result = new PersistFilter();
            result.QueryMode = filterMode;
            result.Where.Operator = Operators.And;

            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                result.Where.Rules.Add(new FilterRule("Promo.ClientTreeId", Operations.In, clientFilter));
            }
            return result;
        }


        /// <summary>
        /// Фильтрация иерархии - оставляем только узлы доступные данному пользователю
        /// </summary>
        /// <param name="hierarchy"></param>
        /// <param name="clientFilter"></param>
        /// <returns></returns>
        private static IQueryable<ClientTreeHierarchyView> getFilteredHierarchy(IQueryable<ClientTreeHierarchyView> hierarchy, IEnumerable<string> clientFilter)
        {
            return hierarchy.Where(h => clientFilter.Contains(h.Id.ToString()) || clientFilter.Any(c => h.Hierarchy.Contains(c)));
        }
        /// <summary>
        /// Применение фильтра по ограничениям к Промо
        /// </summary>
        /// <param name="query"></param>
        /// <param name="hierarchy"></param>
        /// <param name="filter"></param>
        /// <param name="filterMode"></param>
        /// <returns></returns>
        public static IQueryable<RollingScenario> ApplyFilter(IQueryable<RollingScenario> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null, FilterQueryModes filterMode = FilterQueryModes.Active, string role = "")
        {
            if (filterMode == FilterQueryModes.Active)
            {
                query = query.Where(x => !x.Disabled);
            }
            if (filterMode == FilterQueryModes.Deleted)
            {
                query = query.Where(x => x.Disabled);
            }
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any())
            {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.ClientTreeId || h.Hierarchy.Contains(x.ClientTreeId.ToString())));
            }
            //if (!string.IsNullOrEmpty(role))
            //{
            //    IEnumerable<RollingScenario> promoToFilter = query.AsEnumerable();
            //    promoToFilter = promoToFilter.Where(x => RoleStateUtil.RoleCanChangeState(role, x.PromoStatus.SystemName) && RoleStateUtil.IsOnApprovalRoleOrder(role, x));
            //    query = promoToFilter.AsQueryable();
            //}
            return query;
        }
    }
}
