using Model.Host.TPM.Handlers.DataFlow;
using Module.Host.TPM.Handlers.DataFlow.Modules;
using Module.Persist.TPM.Model.TPM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Module.Host.TPM.Handlers.DataFlow.Modules.COGSDataFlowModule;

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

        public Tuple<IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel>, string> Apply(COGSDataFlowSimpleModel cogs, IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel> promoes)
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

        private bool InnerApply(PromoDataFlowModule.PromoDataFlowSimpleModel promo, COGSDataFlowSimpleModel cogs)
        {
            //выполняется стандартный подбор COGS для проверяемого промо, если в результирующем наборе окажется 
            //отобранный по инциденту COGS, то промо отберется на пересчет

            var clientNode = this.DataFlowModuleCollection.ClientTreeDataFlowModule.Collection
                .Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue)
                .FirstOrDefault();
            var isPromoFiltered = false;
            List<COGSDataFlowSimpleModel> cogsList = new List<COGSDataFlowSimpleModel>();

            while ((cogsList == null || cogsList.Count() == 0) && clientNode != null && clientNode.Type != "root")
            {
                cogsList = this.DataFlowModuleCollection.COGSDataFlowModule.Collection
                    .Where(x => x.ClientTreeId == clientNode.Id && x.BrandTechId == promo.BrandTechId && !x.Disabled)
                    //promo DispatchesStart date должна лежать в интервале между COGS start date и COGS end date
                    .Where(x => x.StartDate.HasValue && x.EndDate.HasValue && promo.DispatchesStart.HasValue
                           && DateTimeOffset.Compare(x.StartDate.Value, promo.DispatchesStart.Value) <= 0
                           && DateTimeOffset.Compare(x.EndDate.Value, promo.DispatchesStart.Value) >= 0).ToList();

                clientNode = this.DataFlowModuleCollection.ClientTreeDataFlowModule.Collection
                    .Where(x => x.ObjectId == clientNode.ParentId && !x.EndDate.HasValue)
                    .FirstOrDefault();
            }

            //если не найдено COGS для конкретного BranTech, ищем COGS с пустым BrandTech(пустое=любое)
            if (cogsList.Count == 0)
            {
                clientNode = this.DataFlowModuleCollection.ClientTreeDataFlowModule.Collection
                    .Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue)
                    .FirstOrDefault();

                while ((cogsList == null || cogsList.Count() == 0) && clientNode != null && clientNode.Type != "root")
                {
                    cogsList = this.DataFlowModuleCollection.COGSDataFlowModule.Collection
                        .Where(x => x.ClientTreeId == clientNode.Id && x.BrandTechId == null && !x.Disabled)
                        //promo DispatchesStart date должна лежать в интервале между COGS start date и COGS end date
                        .Where(x => x.StartDate.HasValue && x.EndDate.HasValue && promo.DispatchesStart.HasValue
                               && DateTimeOffset.Compare(x.StartDate.Value, promo.DispatchesStart.Value) <= 0
                               && DateTimeOffset.Compare(x.EndDate.Value, promo.DispatchesStart.Value) >= 0).ToList();

                    clientNode = this.DataFlowModuleCollection.ClientTreeDataFlowModule.Collection
                        .Where(x => x.ObjectId == clientNode.ParentId && !x.EndDate.HasValue)
                        .FirstOrDefault();
                }
            }

            if (cogsList.Contains(cogs))
            {
                isPromoFiltered = true;
            }

            return isPromoFiltered;
        }

        public override string ToString()
        {
            return nameof(COGSDataFlowFilter);
        }
    }
}