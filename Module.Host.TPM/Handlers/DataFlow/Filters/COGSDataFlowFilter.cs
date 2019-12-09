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
            var stack = new Stack<ClientTreeDataFlowModule.ClientTreeDataFlowSimpleModel>();

            var currentClientTreeForCurrentCOGS = this.DataFlowModuleCollection.ClientTreeDataFlowModule.Collection
                .FirstOrDefault(x => x.Id == cogs.ClientTreeId && !x.EndDate.HasValue);

            if (currentClientTreeForCurrentCOGS != null)
            {
                stack.Push(currentClientTreeForCurrentCOGS);
                while (stack.Any())
                {
                    var currentClientTree = stack.Pop();

                    if (promo.ClientTreeId == currentClientTree.ObjectId)
                    {
                        var cogsesForCurrentPromo = this.DataFlowModuleCollection.COGSDataFlowModule.Collection
                            .Where(x =>
                                x.ClientTreeId == promo.ClientTreeId &&
                                (x.BrandTechId == null || x.BrandTechId == cogs.BrandTechId) &&
                                x.StartDate <= promo.DispatchesStart &&
                                x.EndDate >= promo.DispatchesStart);

                        if (!cogsesForCurrentPromo.Any())
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
            return nameof(COGSDataFlowFilter);
        }
    }
}