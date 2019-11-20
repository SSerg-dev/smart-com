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
    public class ClientTreeDataFlowFilter : DataFlowFilter
    {
        public IEnumerable<ClientTreeDataFlowModule.ClientTreeDataFlowSimpleModel> ChangedModels { get; }

        public ClientTreeDataFlowFilter(
            IEnumerable<ChangesIncidentDataFlowModule.ChangesIncidentDataFlowSimpleModel> changesIncidents, DataFlowModuleCollection dataFlowModuleCollection) 
            : base(dataFlowModuleCollection)
        {
            var currentChangesIncidents = changesIncidents.Where(x => x.DirectoryName == nameof(ClientTree));
            var currentChangesIncidentsIds = base.GetChangedModelsInt(currentChangesIncidents);

            this.ChangedModels = this.DataFlowModuleCollection.ClientTreeDataFlowModule.Collection
                .Where(x => currentChangesIncidentsIds.Contains(x.Id));
        }

        public Tuple<IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel>, string> Apply(ClientTreeDataFlowModule.ClientTreeDataFlowSimpleModel clientTree, IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel> promoes)
        {
            var filteredPromoes = new List<PromoDataFlowModule.PromoDataFlowSimpleModel>();
            foreach (var promo in promoes)
            {
                if (this.InnerApply(promo, clientTree))
                {
                    filteredPromoes.Add(promo);
                }
            }

            return new Tuple<IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel>, string>(filteredPromoes, this.ToString());
        }

        private bool InnerApply(PromoDataFlowModule.PromoDataFlowSimpleModel promo, ClientTreeDataFlowModule.ClientTreeDataFlowSimpleModel clientTree)
        {
             var clientTreeChanged = this.DataFlowModuleCollection.ClientTreeDataFlowModule.Collection
                .Any(x => x.Id == promo.ClientTreeKeyId && x.Id == clientTree.Id && !x.EndDate.HasValue);

            return clientTreeChanged;
        }

        public override string ToString()
        {
            return nameof(ClientTreeDataFlowFilter);
        }
    }
}
