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
     public class COGSDataFlowFilter : DataFlowFilter
     {
        public IEnumerable<COGSDataFlowModule.COGSDataFlowSimpleModel> ChangedModels { get; }

        public COGSDataFlowFilter(
            IEnumerable<ChangesIncidentDataFlowModule.ChangesIncidentDataFlowSimpleModel> changesIncidents, DataFlowModuleCollection dataFlowModuleCollection) 
            : base(dataFlowModuleCollection)
        {
            var currentChangesIncidents = changesIncidents.Where(x => x.DirectoryName == nameof(COGS));
            var currentChangesIncidentsIds = base.GetChangedModelsGuid(currentChangesIncidents);

            this.ChangedModels = this.DataFlowModuleCollection.COGSDataFlowModule.Collection
                .Where(x => currentChangesIncidentsIds.Contains(x.Id));
        }

        public Tuple<IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel>, string> Apply(COGSDataFlowModule.COGSDataFlowSimpleModel cogs, IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel> promoes)
        {
            var filteredPromoes = new List<PromoDataFlowModule.PromoDataFlowSimpleModel>();
            foreach (var promo in promoes)
            {
                if (this.InnerApply(promo, cogs))
                {
                    filteredPromoes.Add(promo);
                }
            }

            return new Tuple<IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel>, string>(filteredPromoes, this.ToString());
        }

        private bool InnerApply(PromoDataFlowModule.PromoDataFlowSimpleModel promo, COGSDataFlowModule.COGSDataFlowSimpleModel cogs)
        {
            var clientNode = this.DataFlowModuleCollection.ClientTreeDataFlowModule.Collection.Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue).FirstOrDefault();
            var isPromoFiltered = false;

            // проверка текущей пары Promo-COGS, как если бы для данного промо подбирался COGS
            // идем вверх по дереву клиентов промо, пока не дойдем до корневого узла или не найдется совпадение
            while (!isPromoFiltered && clientNode != null && clientNode.Type != "root")
            {
                isPromoFiltered = clientNode.ObjectId == cogs.ClientTreeObjectId
                                && ((cogs.BrandTechId != null && cogs.BrandTechId == promo.BrandTechId) || cogs.BrandTechId == null)
                                && cogs.StartDate <= promo.DispatchesStart
                                && cogs.EndDate >= promo.DispatchesStart;

                clientNode = this.DataFlowModuleCollection.ClientTreeDataFlowModule.Collection.Where(x => x.ObjectId == clientNode.ParentId && !x.EndDate.HasValue).FirstOrDefault();
            }

            return isPromoFiltered;
        }

        public override string ToString()
        {
            return nameof(COGSDataFlowFilter);
        }
    }
}