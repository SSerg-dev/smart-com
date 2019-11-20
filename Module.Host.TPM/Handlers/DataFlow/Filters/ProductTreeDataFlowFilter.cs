using Model.Host.TPM.Handlers.DataFlow;
using Module.Host.TPM.Handlers.DataFlow.Modules;
using Module.Persist.TPM.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Handlers.DataFlow.Filters
{
    public class ProductTreeDataFlowFilter : DataFlowFilter
    {
        public IEnumerable<ProductTreeDataFlowModule.ProductTreeDataFlowSimpleModel> ChangedModels { get; }

        public ProductTreeDataFlowFilter(
            IEnumerable<ChangesIncidentDataFlowModule.ChangesIncidentDataFlowSimpleModel> changesIncidents, DataFlowModuleCollection dataFlowModuleCollection) 
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
        public Tuple<IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel>, string> Apply(ProductTreeDataFlowModule.ProductTreeDataFlowSimpleModel productTree, IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel> promoes)
        {
            var filteredPromoes = new List<PromoDataFlowModule.PromoDataFlowSimpleModel>();
            foreach (var promo in promoes)
            {
                if (this.InnerApply(promo, productTree))
                {
                    filteredPromoes.Add(promo);
                }
            }

            return new Tuple<IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel>, string>(filteredPromoes, this.ToString());
        }

        private bool InnerApply(PromoDataFlowModule.PromoDataFlowSimpleModel promo, ProductTreeDataFlowModule.ProductTreeDataFlowSimpleModel productTree)
        {
            if (!promo.InOut.HasValue || !promo.InOut.Value)
            {
                var productTreeFilterChanged = this.DataFlowModuleCollection.PromoProductTreeDataFlowModule.Collection
                    .Any(x => x.PromoId == promo.Id && x.ProductTreeObjectId == productTree.ObjectId && !x.Disabled);

                if (productTreeFilterChanged)
                {
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            return nameof(ProductTreeDataFlowFilter);
        }
    }
}
