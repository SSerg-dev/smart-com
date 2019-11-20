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
    public class BaseLineDataFlowFilter : DataFlowFilter
    {
        public IEnumerable<BaseLineDataFlowModule.BaseLineDataFlowSimpleModel> ChangedModels { get; }

        public BaseLineDataFlowFilter(
            IEnumerable<ChangesIncidentDataFlowModule.ChangesIncidentDataFlowSimpleModel> changesIncidents, DataFlowModuleCollection dataFlowModuleCollection) 
            : base(dataFlowModuleCollection)
        {
            var currentChangesIncidents = changesIncidents.Where(x => x.DirectoryName == nameof(BaseLine));
            var currentChangesIncidentsIds = base.GetChangedModelsGuid(currentChangesIncidents);

            this.ChangedModels = this.DataFlowModuleCollection.BaseLineDataFlowModule.Collection
                .Where(x => currentChangesIncidentsIds.Contains(x.Id));
        }

        public Tuple<IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel>, string> Apply(BaseLineDataFlowModule.BaseLineDataFlowSimpleModel baseLine, IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel> promoes)
        {
            var filteredPromoes = new List<PromoDataFlowModule.PromoDataFlowSimpleModel>();
            foreach (var promo in promoes)
            {
                if (this.InnerApply(promo, baseLine))
                {
                    filteredPromoes.Add(promo);
                }
            }

            return new Tuple<IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel>, string>(filteredPromoes, this.ToString());
        }

         /// <summary>
        /// StartDate в BaseLine лежит в промежутке дат промо (включительно). 
        /// В промо входит продукт с таким же ZREP как и в Baseline.
        /// Клиент в BaseLine cовпадает с клиентом в промо (смотрим по всей иерархии).
        /// </summary>
        private bool InnerApply(PromoDataFlowModule.PromoDataFlowSimpleModel promo, BaseLineDataFlowModule.BaseLineDataFlowSimpleModel baseLine)
        {
            var baseLineStartDateBeetweenPromoDates = !(promo.StartDate > baseLine.StartDate.Value.AddDays(6) || promo.EndDate < baseLine.StartDate);
            if (baseLineStartDateBeetweenPromoDates)
            {
                var promoProductContainsBaseLineProduct = this.DataFlowModuleCollection.PromoProductDataFlowModule.Collection
                    .Any(x => x.PromoId == promo.Id && x.ProductId == baseLine.ProductId && !x.Disabled);

                if (promoProductContainsBaseLineProduct)
                {
                    var clientTree = this.DataFlowModuleCollection.ClientTreeDataFlowModule.Collection
                        .FirstOrDefault(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue);

                    if (clientTree != null)
                    {
                        while (clientTree != null && clientTree.Type.ToLower() != "root")
                        {
                            if (clientTree.DemandCode == baseLine.DemandCode)
                            {
                                break;
                            }

                            clientTree = this.DataFlowModuleCollection.ClientTreeDataFlowModule.Collection
                                .FirstOrDefault(x => x.ObjectId == clientTree.ParentId && !x.EndDate.HasValue);
                        }

                        var clientTreeHierarchyContainsBaseLineClient = clientTree.DemandCode == baseLine.DemandCode;
                        if (clientTreeHierarchyContainsBaseLineClient)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public override string ToString()
        {
            return nameof(BaseLineDataFlowFilter);
        }
    }
}
