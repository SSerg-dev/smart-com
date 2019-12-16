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
    public class TradeInvestmentDataFlowFilter : DataFlowFilter
    {
        public IEnumerable<TradeInvestmentDataFlowModule.TradeInvestmentDataFlowSimpleModel> ChangedModels { get; }

        public TradeInvestmentDataFlowFilter(
            IEnumerable<ChangesIncidentDataFlowModule.ChangesIncidentDataFlowSimpleModel> changesIncidents, DataFlowModuleCollection dataFlowModuleCollection)
            : base(dataFlowModuleCollection)
        {
            var currentChangesIncidents = changesIncidents.Where(x => x.DirectoryName == nameof(TradeInvestment));
            var currentChangesIncidentsIds = base.GetChangedModelsGuid(currentChangesIncidents);

            this.ChangedModels = this.DataFlowModuleCollection.TradeInvestmentDataFlowModule.Collection
                .Where(x => currentChangesIncidentsIds.Contains(x.Id));
        }

        public Tuple<IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel>, string> Apply(TradeInvestmentDataFlowModule.TradeInvestmentDataFlowSimpleModel tradeInvestment, IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel> promoes)
        {
            var filteredPromoes = new List<PromoDataFlowModule.PromoDataFlowSimpleModel>();
            foreach (var promo in promoes)
            {
                if (this.InnerApply(promo, tradeInvestment))
                {
                    filteredPromoes.Add(promo);
                }
            }

            return new Tuple<IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel>, string>(filteredPromoes, this.ToString());
        }

        private bool InnerApply(PromoDataFlowModule.PromoDataFlowSimpleModel promo, TradeInvestmentDataFlowModule.TradeInvestmentDataFlowSimpleModel tradeInvestment)
        {
            var clientNode = this.DataFlowModuleCollection.ClientTreeDataFlowModule.Collection.Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue).FirstOrDefault();
            var isPromoFiltered = false;

            // проверка текущей пары Promo-TI, как если бы для данного промо подбирался TI
            // идем вверх по дереву клиентов промо, пока не дойдем до корневого узла или не найдется совпадение
            while (!isPromoFiltered && clientNode != null && clientNode.Type != "root")
            {
                isPromoFiltered = clientNode.ObjectId == tradeInvestment.ClientTreeObjectId
                                && ((tradeInvestment.BrandTechId != null && tradeInvestment.BrandTechId == promo.BrandTechId) || tradeInvestment.BrandTechId == null)
                                && tradeInvestment.StartDate <= promo.StartDate
                                && tradeInvestment.EndDate >= promo.StartDate;

                clientNode = this.DataFlowModuleCollection.ClientTreeDataFlowModule.Collection.Where(x => x.ParentId == clientNode.ObjectId && !x.EndDate.HasValue).FirstOrDefault();
            }

            return isPromoFiltered;
        }

        public override string ToString()
        {
            return nameof(TradeInvestmentDataFlowFilter);
        }
    }
}