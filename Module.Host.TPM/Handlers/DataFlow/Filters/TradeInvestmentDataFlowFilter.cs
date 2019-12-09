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
            var stack = new Stack<ClientTreeDataFlowModule.ClientTreeDataFlowSimpleModel>();

            var currentClientTreeForCurrentTradeInvestment = this.DataFlowModuleCollection.ClientTreeDataFlowModule.Collection
                .FirstOrDefault(x => x.Id == tradeInvestment.ClientTreeId && !x.EndDate.HasValue);

            if (currentClientTreeForCurrentTradeInvestment != null)
            {
                stack.Push(currentClientTreeForCurrentTradeInvestment);
                while (stack.Any())
                {
                    var currentClientTree = stack.Pop();

                    if (promo.ClientTreeId == currentClientTree.ObjectId)
                    {
                        var tradeInvestmentesForCurrentPromo = this.DataFlowModuleCollection.TradeInvestmentDataFlowModule.Collection
                            .Where(x =>
                                x.ClientTreeId == promo.ClientTreeId &&
                                (x.BrandTechId == null || x.BrandTechId == tradeInvestment.BrandTechId) &&
                                x.StartDate <= promo.StartDate &&
                                x.EndDate >= promo.StartDate);

                        if (!tradeInvestmentesForCurrentPromo.Any())
                        {
                            return true;
                        }
                    }

                    var currentClientTreeChildren = this.DataFlowModuleCollection.ClientTreeDataFlowModule.Collection
                        .Where(x => x.ParentId == currentClientTree.ObjectId && !x.EndDate.HasValue);

                    foreach (var children in currentClientTreeChildren)
                    {
                        stack.Push(children);
                    }
                }
            }

            return false;
        }

        public override string ToString()
        {
            return nameof(TradeInvestmentDataFlowFilter);
        }
    }
}