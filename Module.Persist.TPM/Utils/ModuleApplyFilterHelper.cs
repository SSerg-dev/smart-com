using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl.RoleStateMap;
using Persist.ScriptGenerator;
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
                promoToFilter = promoToFilter.Where(x => RoleStateUtil.RoleCanChangeState(role, x.PromoStatus.SystemName) && RoleStateUtil.IsOnApprovalRoleOrder(role, x));
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
        public static IQueryable<PromoGridView> ApplyFilter(IQueryable<PromoGridView> query, IQueryable<ClientTreeHierarchyView> hierarchy, IDictionary<string, IEnumerable<string>> filter = null, FilterQueryModes filterMode = FilterQueryModes.Active, string role = "") {
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
                IEnumerable<PromoGridView> promoToFilter = query.AsEnumerable();
                promoToFilter = promoToFilter.Where(x => RoleStateUtil.RoleCanChangeState(role, x.PromoStatusSystemName) && RoleStateUtil.IsOnApprovalRoleOrder(role, x));
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
        /// Применение фильтра BaseLine
        /// </summary>
        /// <param name="query"></param>
        /// <param name="startDate"></param>
        /// <param name="finishDate"></param>
        /// <param name="filter"></param>
        /// <param name="filterMode"></param>
        /// <returns></returns>
        public static IQueryable<ImportBaseLine> ApplyFilter(IQueryable<ImportBaseLine> query, DateTimeOffset startDate, DateTimeOffset finishDate, IDictionary<string, IEnumerable<string>> filter = null, FilterQueryModes filterMode = FilterQueryModes.Active) {
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            query = query.Where(x => x.StartDate >= startDate && x.StartDate <= finishDate);
            if (clientFilter.Any()) {
                query = query.Where(q => clientFilter.Contains(q.ClientTreeDemandCode));
            }
            return query;
        }

        /// <summary>
        /// Применить фильтр по клиентам к Assortment Matrix
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
                    hierarchy.Any(h => h.Id == x.ClientTree.ObjectId || h.Hierarchy.Contains(x.ClientTree.ObjectId.ToString())));
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
        public static PersistFilter BuildBaseLineFilter(DateTimeOffset? startDate, DateTimeOffset? finishDate, IDictionary<string, IEnumerable<string>> filter, FilterQueryModes filterMode = FilterQueryModes.Active) {

            PersistFilter result = new PersistFilter();
            result.QueryMode = filterMode;
            result.Where.Operator = Operators.And;

            if (startDate.HasValue) {
                result.Where.Rules.Add(new FilterRule("StartDate", Operations.MoreOrEquals, startDate.Value));
            }
            if (finishDate.HasValue) {
                result.Where.Rules.Add(new FilterRule("StartDate", Operations.LessOrEquals, finishDate.Value));
            }
            IEnumerable<string> clientFilter = FilterHelper.GetFilter(filter, ModuleFilterName.Client);
            if (clientFilter.Any()) {
                result.Where.Rules.Add(new FilterRule("ClientTree.DemandCode", Operations.In, clientFilter));
            }
            return result;
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
