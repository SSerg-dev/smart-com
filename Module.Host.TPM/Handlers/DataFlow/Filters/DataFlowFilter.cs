using Model.Host.TPM.Handlers.DataFlow;
using Module.Host.TPM.Handlers.DataFlow.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Handlers.DataFlow.Filters
{
    public class DataFlowFilter
    {
        protected DataFlowModuleCollection DataFlowModuleCollection { get; }
        public DataFlowFilter(DataFlowModuleCollection dataFlowModuleCollection)
        {
            this.DataFlowModuleCollection = dataFlowModuleCollection;
        }

        protected virtual IEnumerable<int> GetChangedModelsInt(
            IEnumerable<ChangesIncidentDataFlowModule.ChangesIncidentDataFlowSimpleModel> changesIncidents)
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

        protected virtual IEnumerable<Guid> GetChangedModelsGuid(
            IEnumerable<ChangesIncidentDataFlowModule.ChangesIncidentDataFlowSimpleModel> changesIncidents)
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

        public override string ToString()
        {
            return nameof(DataFlowFilter);
        }
    }
}
