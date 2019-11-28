using Model.Host.TPM.Handlers.DataFlow;
using Module.Host.TPM.Handlers.DataFlow.Modules;
using Module.Persist.TPM.Model.TPM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Handlers.DataFlow.Filters
{
     public class PromoProductsCorrectionDataFlowFilter : DataFlowFilter
    {
        public IEnumerable<PromoProductsCorrectionDataFlowModule.PromoProductsCorrectionDataFlowSimpleModel> ChangedModels { get; }

        public PromoProductsCorrectionDataFlowFilter(
            IEnumerable<ChangesIncidentDataFlowModule.ChangesIncidentDataFlowSimpleModel> changesIncidents, DataFlowModuleCollection dataFlowModuleCollection) 
            : base(dataFlowModuleCollection)
        {
            var currentChangesIncidents = changesIncidents.Where(x => x.DirectoryName == nameof(PromoProductsCorrection));
            var currentChangesIncidentsIds = base.GetChangedModelsGuid(currentChangesIncidents);

            this.ChangedModels = this.DataFlowModuleCollection.PromoProductsCorrectionDataFlowModule.Collection
                .Where(x => currentChangesIncidentsIds.Contains(x.Id));
        }

        public Tuple<IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel>, string> Apply(PromoProductsCorrectionDataFlowModule.PromoProductsCorrectionDataFlowSimpleModel promoProductsCorrection, IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel> promoes)
        {
            var filteredPromoes = new List<PromoDataFlowModule.PromoDataFlowSimpleModel>();
            foreach (var promo in promoes)
            {
                if (this.InnerApply(promo, promoProductsCorrection))
                {
                    filteredPromoes.Add(promo);
                }
            }

            return new Tuple<IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel>, string>(filteredPromoes, this.ToString());
        }

        private bool InnerApply(PromoDataFlowModule.PromoDataFlowSimpleModel promo, PromoProductsCorrectionDataFlowModule.PromoProductsCorrectionDataFlowSimpleModel promoProductsCorrection)
        {
            var promoProduct = this.DataFlowModuleCollection.PromoProductDataFlowModule.Collection
                .FirstOrDefault(x => x.Id == promoProductsCorrection.PromoProductId);

            if (promoProduct != null && promoProduct.PromoId == promo.Id)
            {
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return nameof(PromoProductsCorrectionDataFlowFilter);
        }
    }
}