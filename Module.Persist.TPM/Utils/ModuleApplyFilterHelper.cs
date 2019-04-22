using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl.RoleStateMap;
using Persist.ScriptGenerator.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace Module.Persist.TPM.Utils {
    public static class ModuleApplyFilterHelper {
        /// <summary>
        /// Применение фильтра по ограничениям к Промо
        /// </summary>
        /// <param name="query"></param>
        /// <param name="hierarchy"></param>
        /// <param name="filter"></param>
        /// <param name="filterMode"></param>
        /// <returns></returns>
        public static IQueryable<Promo> ApplyFilter(IQueryable<Promo> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null, FilterQueryModes filterMode = FilterQueryModes.Active, string role = "") {
            if (filterMode == FilterQueryModes.Active) {
                query = query.Where(x => !x.Disabled);
            }
            if (filterMode == FilterQueryModes.Deleted) {
                query = query.Where(x => x.Disabled);
            }
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any()) {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.ClientTreeId || h.Hierarchy.Contains(x.ClientTreeId.Value.ToString())));
            }
            if (!String.IsNullOrEmpty(role)) {
                IEnumerable<Promo> promoToFilter = query.AsEnumerable();
                promoToFilter = promoToFilter.Where(x => RoleStateUtil.RoleCanChangeState(role, x.PromoStatus.SystemName));
                query = promoToFilter.AsQueryable();
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
        public static IQueryable<PromoView> ApplyFilter(IQueryable<PromoView> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null, FilterQueryModes filterMode = FilterQueryModes.Active, string role = "") {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any()) {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.ClientTreeId || h.Hierarchy.Contains(x.ClientTreeId.Value.ToString())));
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
        public static IQueryable<ClientTree> ApplyFilter(IQueryable<ClientTree> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null, FilterQueryModes filterMode = FilterQueryModes.Active) {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any()) {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.ObjectId || h.Hierarchy.Contains(x.ObjectId.ToString())));
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
        public static IQueryable<BaseClientTreeView> ApplyFilter(IQueryable<BaseClientTreeView> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null, FilterQueryModes filterMode = FilterQueryModes.Active) {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any()) {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.BOI || h.Hierarchy.Contains(x.BOI.ToString())));
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
        public static IQueryable<ClientTreeSharesView> ApplyFilter(IQueryable<ClientTreeSharesView> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null, FilterQueryModes filterMode = FilterQueryModes.Active) {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any()) {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.BOI || h.Hierarchy.Contains(x.BOI.ToString())));
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
        public static IQueryable<NoneNego> ApplyFilter(IQueryable<NoneNego> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null, FilterQueryModes filterMode = FilterQueryModes.Active) {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any()) {
                hierarchy = getFilteredHierarchy(hierarchy, clientFilter);
                query = query.Where(x =>
                    hierarchy.Any(h => h.Id == x.ClientTree.ObjectId || h.Hierarchy.Contains(x.ClientTree.ObjectId.ToString())));
            }
            return query;
        }

        /// <summary>
        /// ФИльтрация иерархии - оставляем только узлы доступные данному пользователю
        /// </summary>
        /// <param name="hierarchy"></param>
        /// <param name="clientFilter"></param>
        /// <returns></returns>
        private static IQueryable<ClientTreeHierarchyView> getFilteredHierarchy(IQueryable<ClientTreeHierarchyView> hierarchy, IEnumerable<string> clientFilter) {
            return hierarchy.Where(h => clientFilter.Contains(h.Id.ToString()) || clientFilter.Any(c => h.Hierarchy.Contains(c)));
        }
    }
}
