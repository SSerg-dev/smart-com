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
    public class IncrementalPromoDataFlowFilter : DataFlowFilter
    {
        public IEnumerable<IncrementalPromoDataFlowModule.IncrementalPromoDataFlowSimpleModel> ChangedModels { get; }

        public IncrementalPromoDataFlowFilter(
            IEnumerable<ChangesIncidentDataFlowModule.ChangesIncidentDataFlowSimpleModel> changesIncidents, DataFlowModuleCollection dataFlowModuleCollection) 
            : base(dataFlowModuleCollection)
        {
            var currentChangesIncidents = changesIncidents.Where(x => x.DirectoryName == nameof(IncrementalPromo));
            var currentChangesIncidentsIds = base.GetChangedModelsGuid(currentChangesIncidents);

            this.ChangedModels = this.DataFlowModuleCollection.IncrementalPromoDataFlowModule.Collection
                .Where(x => currentChangesIncidentsIds.Contains(x.Id));
        }

        public Tuple<IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel>, string> Apply(IncrementalPromoDataFlowModule.IncrementalPromoDataFlowSimpleModel incrementalPromo, IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel> promoes)
        {
            var filteredPromoes = new List<PromoDataFlowModule.PromoDataFlowSimpleModel>();
            foreach (var promo in promoes)
            {
                if (this.InnerApply(promo, incrementalPromo))
                {
                    filteredPromoes.Add(promo);
                }
            }

            return new Tuple<IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel>, string>(filteredPromoes, this.ToString());
        }

        /// <summary>
        /// Промо не In Out
        /// Если обновилась запись в таблице IncrementalPromo, прикрепленная к какому-то промо, то это промо нужно пересчитать.
        /// </summary>
        private bool InnerApply(PromoDataFlowModule.PromoDataFlowSimpleModel promo, IncrementalPromoDataFlowModule.IncrementalPromoDataFlowSimpleModel incrementalPromo)
        {
            if (promo.InOut.HasValue && promo.InOut.Value)
            {
                if (promo.Id == incrementalPromo.PromoId)
                {
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            return nameof(IncrementalPromoDataFlowFilter);
        }
    }
}
