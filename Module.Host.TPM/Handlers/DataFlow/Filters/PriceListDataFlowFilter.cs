using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Host.TPM.Handlers.DataFlow;
using Module.Host.TPM.Handlers.DataFlow.Modules;
using Module.Persist.TPM.Model.TPM;
using Persist;

namespace Module.Host.TPM.Handlers.DataFlow.Filters
{
    public class PriceListDataFlowFilter : DataFlowFilter
    {
        public IEnumerable<PriceListDataFlowModule.PriceListDataFlowSimpleModel> ChangedModels { get; }

        public PriceListDataFlowFilter(
            IEnumerable<ChangesIncidentDataFlowModule.ChangesIncidentDataFlowSimpleModel> changesIncidents, DataFlowModuleCollection dataFlowModuleCollection) 
            : base(dataFlowModuleCollection)
        {
            var currentChangesIncidents = changesIncidents.Where(x => x.DirectoryName == nameof(PriceList));
            var currentChangesIncidentsIds = base.GetChangedModelsGuid(currentChangesIncidents);

            this.ChangedModels = this.DataFlowModuleCollection.PriceListDataFlowModule.Collection
                .Where(x => currentChangesIncidentsIds.Contains(x.Id));
        }

        public Tuple<IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel>, string> Apply(PriceListDataFlowModule.PriceListDataFlowSimpleModel priceList, IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel> promoes)
        {
            var filteredPromoes = new List<PromoDataFlowModule.PromoDataFlowSimpleModel>();
            foreach (var promo in promoes)
            {
                if (this.InnerApply(promo, priceList))
                {
                    filteredPromoes.Add(promo);
                }
            }

            return new Tuple<IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel>, string>(filteredPromoes, this.ToString());
        }

        private bool InnerApply(PromoDataFlowModule.PromoDataFlowSimpleModel promo, PriceListDataFlowModule.PriceListDataFlowSimpleModel priceList)
        {
            using (DatabaseContext databaseContext = new DatabaseContext())
            {
                var сlientForPrice = databaseContext.Set<PriceList>().Where(x => x.Id == priceList.Id && x.ClientTreeId == promo.ClientTreeKeyId && !x.Disabled).FirstOrDefault();

                if (priceList.StartDate <= promo.DispatchesStart && priceList.EndDate >= promo.DispatchesStart && сlientForPrice != null)
                {
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            return nameof(PriceListDataFlowFilter);
        }
    }
}
