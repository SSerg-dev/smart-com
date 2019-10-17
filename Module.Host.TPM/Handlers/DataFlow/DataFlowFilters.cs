using Model.Host.TPM.Handlers.DataFlow;
using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Handlers.DataFlow
{
    public class DataFlowFilterCollection
    {
        private IEnumerable<ChangesIncident> ChangesIncidents { get; } 
        private DataFlowModuleCollection DataFlowModuleCollection { get; }

        public IEnumerable<DataFlowFilter> Filters { get; } 

        public DataFlowFilterCollection(DatabaseContext databaseContext, DataFlowModuleCollection dataFlowModelCollection)
        {
            this.ChangesIncidents = databaseContext.Set<ChangesIncident>().Where(x => x.ProcessDate == null);
            this.DataFlowModuleCollection = dataFlowModelCollection;

            this.Filters = new List<DataFlowFilter>
            {
                new BaseLineDataFlowFilter(this.ChangesIncidents, this.DataFlowModuleCollection),
                new AssortmentMatrixDataFlowFilter(this.ChangesIncidents, this.DataFlowModuleCollection),
                new ClientTreeDataFlowFilter(this.ChangesIncidents, this.DataFlowModuleCollection),
                new ProductTreeDataFlowFilter(this.ChangesIncidents, this.DataFlowModuleCollection),
                new IncrementalPromoDataFlowFilter(this.ChangesIncidents, this.DataFlowModuleCollection)
            };
        }

        public bool Apply(PromoDataFlowModule.PromoDataFlowSimpleModel promo)
        {
            foreach (var filter in this.Filters)
            {
                if (filter.Apply(promo))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public class DataFlowFilter
    {
        protected DataFlowModuleCollection DataFlowModuleCollection { get; }
        public DataFlowFilter(DataFlowModuleCollection dataFlowModuleCollection)
        {
            this.DataFlowModuleCollection = dataFlowModuleCollection;
        }
        public virtual bool Apply(PromoDataFlowModule.PromoDataFlowSimpleModel promo)
        {
            return true;
        }

        protected virtual IEnumerable<int> GetChangedModelsInt(IEnumerable<ChangesIncident> changesIncidents)
        {
            var changedIncidentsIntIds = changesIncidents.Select(x =>
            {
                int itemIntId;
                bool success = Int32.TryParse(x.ItemId, out itemIntId);
                return new { itemIntId, success };
            })
            .Where(x => x.success).Select(x => x.itemIntId);

            return changedIncidentsIntIds;
        }

        protected virtual IEnumerable<Guid> GetChangedModelsGuid(IEnumerable<ChangesIncident> changesIncidents)
        {
            var changedIncidentsGuidIds = changesIncidents.Select(x =>
            {
                Guid itemGuidId;
                bool success = Guid.TryParse(x.ItemId, out itemGuidId);
                return new { itemGuidId, success };
            })
            .Where(x => x.success).Select(x => x.itemGuidId).ToList();

            return changedIncidentsGuidIds;
        }
    }

    public class BaseLineDataFlowFilter : DataFlowFilter
    {
        private IEnumerable<BaseLineDataFlowModule.BaseLineDataFlowSimpleModel> ChangedModels { get; }

        public BaseLineDataFlowFilter(IEnumerable<ChangesIncident> changesIncidents, DataFlowModuleCollection dataFlowModuleCollection) 
            : base(dataFlowModuleCollection)
        {
            var currentChangesIncidents = changesIncidents.Where(x => x.DirectoryName == nameof(BaseLine));
            var currentChangesIncidentsIds = base.GetChangedModelsGuid(currentChangesIncidents);

            this.ChangedModels = this.DataFlowModuleCollection.BaseLineDataFlowModule.Collection
                .Where(x => currentChangesIncidentsIds.Contains(x.Id));
        }

        /// <summary>
        /// StartDate в BaseLine лежит в промежутке дат промо (включительно). 
        /// В промо входит продукт с таким же ZREP как и в Baseline.
        /// Клиент в BaseLine cовпадает с клиентом в промо (смотрим по всей иерархии).
        /// </summary>
        public override bool Apply(PromoDataFlowModule.PromoDataFlowSimpleModel promo)
        {
            foreach (var baseLine in this.ChangedModels)
            {
                var baseLineStartDateBeetweenPromoDates = !(promo.StartDate > baseLine.StartDate.Value.AddDays(6) || promo.EndDate < baseLine.StartDate);
                if (baseLineStartDateBeetweenPromoDates)
                {
                    var promoProductContainsBaseLineProduct = this.DataFlowModuleCollection.PromoProductDataFlowModule.Collection
                        .Any(x => x.PromoId == promo.Id && x.ProductId == baseLine.ProductId && !x.Disabled);

                    if (promoProductContainsBaseLineProduct)
                    {
                        var clientTree = this.DataFlowModuleCollection.ClientTreeDataFlowModule.Collection
                            .FirstOrDefault(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue);

                        if (clientTree != null)
                        {
                            while (clientTree != null && clientTree.Type.ToLower() != "root")
                            {
                                if (clientTree.Id == baseLine.ClientTreeId)
                                {
                                    break;
                                }

                                clientTree = this.DataFlowModuleCollection.ClientTreeDataFlowModule.Collection
                                    .FirstOrDefault(x => x.ObjectId == clientTree.ParentId && !x.EndDate.HasValue);
                            }

                            var clientTreeHierarchyContainsBaseLineClient = clientTree.Id == baseLine.ClientTreeId;
                            if (clientTreeHierarchyContainsBaseLineClient)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
    }

    public class AssortmentMatrixDataFlowFilter : DataFlowFilter
    {
        private IEnumerable<AssortmentMatrixDataFlowModule.AssortmentMatrixDataFlowSimpleModel> ChangedModels { get; }

        public AssortmentMatrixDataFlowFilter(IEnumerable<ChangesIncident> changesIncidents, DataFlowModuleCollection dataFlowModuleCollection) 
            : base(dataFlowModuleCollection)
        {
            var currentChangesIncidents = changesIncidents.Where(x => x.DirectoryName == nameof(AssortmentMatrix));
            var currentChangesIncidentsIds = base.GetChangedModelsGuid(currentChangesIncidents);

            this.ChangedModels = this.DataFlowModuleCollection.AssortmentMatrixDataFlowModule.Collection
                .Where(x => currentChangesIncidentsIds.Contains(x.Id));
        }

        /// <summary>
        /// Клиент в промо такой же как и в ассортиментной матрице.
        /// Даты промо находятся в промежутке дат из ассортиментной матрицы.
        /// </summary>
        public override bool Apply(PromoDataFlowModule.PromoDataFlowSimpleModel promo)
        {
            foreach (var assortmentMatrix in this.ChangedModels)
            {
                var matrixClientEqualsPromoClient = assortmentMatrix.ClientTreeId == promo.ClientTreeKeyId;
                if (matrixClientEqualsPromoClient)
                {
                    var promoDatesBeetweenMatrixDates = promo.DispatchesStart >= assortmentMatrix.StartDate &&
                        promo.DispatchesEnd <= assortmentMatrix.EndDate;

                    if (promoDatesBeetweenMatrixDates)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    public class ClientTreeDataFlowFilter : DataFlowFilter
    {
        private IEnumerable<ClientTreeDataFlowModule.ClientTreeDataFlowSimpleModel> ChangedModels { get; }

        public ClientTreeDataFlowFilter(IEnumerable<ChangesIncident> changesIncidents, DataFlowModuleCollection dataFlowModuleCollection) 
            : base(dataFlowModuleCollection)
        {
            var currentChangesIncidents = changesIncidents.Where(x => x.DirectoryName == nameof(ClientTree));
            var currentChangesIncidentsIds = base.GetChangedModelsInt(currentChangesIncidents);

            this.ChangedModels = this.DataFlowModuleCollection.ClientTreeDataFlowModule.Collection
                .Where(x => currentChangesIncidentsIds.Contains(x.Id));
        }

        public override bool Apply(PromoDataFlowModule.PromoDataFlowSimpleModel promo)
        {
            return true;
        }
    }

    public class ProductTreeDataFlowFilter : DataFlowFilter
    {
        private IEnumerable<ProductTreeDataFlowModule.ProductTreeDataFlowSimpleModel> ChangedModels { get; }

        public ProductTreeDataFlowFilter(IEnumerable<ChangesIncident> changesIncidents, DataFlowModuleCollection dataFlowModuleCollection) 
            : base(dataFlowModuleCollection)
        {
            var currentChangesIncidents = changesIncidents.Where(x => x.DirectoryName == nameof(ProductTree));
            var currentChangesIncidentsIds = base.GetChangedModelsInt(currentChangesIncidents);

            this.ChangedModels = this.DataFlowModuleCollection.ProductTreeDataFlowModule.Collection
                .Where(x => currentChangesIncidentsIds.Contains(x.Id));
        }

        /// <summary>
        /// Промо не In-Out.
        /// Если изменился фильтр у любого выбранного узла.
        /// </summary>
        public override bool Apply(PromoDataFlowModule.PromoDataFlowSimpleModel promo)
        {
            if (!promo.InOut.HasValue || !promo.InOut.Value)
            {
                foreach (var productTree in this.ChangedModels)
                {
                    var productTreeFilterChanged = this.DataFlowModuleCollection.PromoProductTreeDataFlowModule.Collection
                        .Any(x => x.PromoId == promo.Id && x.ProductTreeObjectId == productTree.ObjectId && !x.Disabled);

                    if (productTreeFilterChanged)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    public class IncrementalPromoDataFlowFilter : DataFlowFilter
    {
        private IEnumerable<IncrementalPromoDataFlowModule.IncrementalPromoDataFlowSimpleModel> ChangedModels { get; }

        public IncrementalPromoDataFlowFilter(IEnumerable<ChangesIncident> changesIncidents, DataFlowModuleCollection dataFlowModuleCollection) 
            : base(dataFlowModuleCollection)
        {
            var currentChangesIncidents = changesIncidents.Where(x => x.DirectoryName == nameof(IncrementalPromo));
            var currentChangesIncidentsIds = base.GetChangedModelsGuid(currentChangesIncidents);

            this.ChangedModels = this.DataFlowModuleCollection.IncrementalPromoDataFlowModule.Collection
                .Where(x => currentChangesIncidentsIds.Contains(x.Id));
        }

        /// <summary>
        /// Промо не In Out
        /// Если обновилась запись в таблице IncrementalPromo, прикрепленная к какому-то промо, то это промо нужно пересчитать.
        /// </summary>
        public override bool Apply(PromoDataFlowModule.PromoDataFlowSimpleModel promo)
        {
            if (promo.InOut.HasValue && promo.InOut.Value)
            {
                foreach (var incrementalPromo in this.ChangedModels)
                {
                    if (promo.Id == incrementalPromo.PromoId)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
