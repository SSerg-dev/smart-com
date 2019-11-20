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
    public class ClientTreeBrandTechDataFlowFilter : DataFlowFilter
    {
        public IEnumerable<ClientTreeBrandTechDataFlowModule.ClientTreeBrandTechDataFlowSimpleModel> ChangedModels { get; }

        public ClientTreeBrandTechDataFlowFilter(
            IEnumerable<ChangesIncidentDataFlowModule.ChangesIncidentDataFlowSimpleModel> changesIncidents, DataFlowModuleCollection dataFlowModuleCollection) 
            : base(dataFlowModuleCollection)
        {
            var currentChangesIncidents = changesIncidents.Where(x => x.DirectoryName == nameof(ClientTreeBrandTech));
            var currentChangesIncidentsIds = base.GetChangedModelsGuid(currentChangesIncidents);

            this.ChangedModels = this.DataFlowModuleCollection.ClientTreeBrandTechDataFlowModule.Collection
                .Where(x => currentChangesIncidentsIds.Contains(x.Id));
        }

        public Tuple<IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel>, string> Apply(ClientTreeBrandTechDataFlowModule.ClientTreeBrandTechDataFlowSimpleModel clientTreeBrandTech, IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel> promoes)
        {
            var filteredPromoes = new List<PromoDataFlowModule.PromoDataFlowSimpleModel>();
            foreach (var promo in promoes)
            {
                if (this.InnerApply(promo, clientTreeBrandTech))
                {
                    filteredPromoes.Add(promo);
                }
            }

            return new Tuple<IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel>, string>(filteredPromoes, this.ToString());
        }

        private bool InnerApply(PromoDataFlowModule.PromoDataFlowSimpleModel promo, ClientTreeBrandTechDataFlowModule.ClientTreeBrandTechDataFlowSimpleModel clientTreeBrandTech)
        {
            var clientTreeBrachTechShareChanged = this.DataFlowModuleCollection.ClientTreeBrandTechDataFlowModule.Collection
                .Any(x => x.ClientTreeId == promo.ClientTreeKeyId && x.BrandTechId == promo.BrandTechId && x.Id == clientTreeBrandTech.Id);

            return clientTreeBrachTechShareChanged;
        }

        public override string ToString()
        {
            return nameof(ClientTreeBrandTechDataFlowFilter);
        }
    }
}
