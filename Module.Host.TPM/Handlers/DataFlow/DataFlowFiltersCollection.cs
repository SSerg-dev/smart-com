using Model.Host.TPM.Handlers.DataFlow;
using Module.Host.TPM.Handlers.DataFlow.Filters;
using Module.Host.TPM.Handlers.DataFlow.Modules;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.LogWriter;

namespace Module.Host.TPM.Handlers.DataFlow
{
    public class DataFlowFilterCollection
    {
        public ClientTreeDataFlowFilter ClientTreeDataFlowFilter { get; }
        public ClientTreeBrandTechDataFlowFilter ClientTreeBrandTechDataFlowFilter { get; }
        public ProductTreeDataFlowFilter ProductTreeDataFlowFilter { get; }
        public IncrementalPromoDataFlowFilter IncrementalPromoDataFlowFilter { get; }
        public AssortmentMatrixDataFlowFilter AssortmentMatrixDataFlowFilter { get; }
        public BaseLineDataFlowFilter BaseLineDataFlowFilter { get; }

        public DataFlowFilterCollection(DataFlowModuleCollection dataFlowModuleCollection)
        {
            var changesIncidents = dataFlowModuleCollection.ChangesIncidentDataFlowModule.Collection
                .Where(x => x.ProcessDate == null);

            this.ClientTreeDataFlowFilter = new ClientTreeDataFlowFilter(changesIncidents, dataFlowModuleCollection);
            this.ClientTreeBrandTechDataFlowFilter = new ClientTreeBrandTechDataFlowFilter(changesIncidents, dataFlowModuleCollection);
            this.ProductTreeDataFlowFilter = new ProductTreeDataFlowFilter(changesIncidents, dataFlowModuleCollection);
            this.IncrementalPromoDataFlowFilter = new IncrementalPromoDataFlowFilter(changesIncidents, dataFlowModuleCollection);
            this.AssortmentMatrixDataFlowFilter = new AssortmentMatrixDataFlowFilter(changesIncidents, dataFlowModuleCollection);
            this.BaseLineDataFlowFilter = new BaseLineDataFlowFilter(changesIncidents, dataFlowModuleCollection);
        }
    }
}
