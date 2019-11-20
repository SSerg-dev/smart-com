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
     public class AssortmentMatrixDataFlowFilter : DataFlowFilter
    {
        public IEnumerable<AssortmentMatrixDataFlowModule.AssortmentMatrixDataFlowSimpleModel> ChangedModels { get; }

        public AssortmentMatrixDataFlowFilter(
            IEnumerable<ChangesIncidentDataFlowModule.ChangesIncidentDataFlowSimpleModel> changesIncidents, DataFlowModuleCollection dataFlowModuleCollection) 
            : base(dataFlowModuleCollection)
        {
            var currentChangesIncidents = changesIncidents.Where(x => x.DirectoryName == nameof(AssortmentMatrix));
            var currentChangesIncidentsIds = base.GetChangedModelsGuid(currentChangesIncidents);

            this.ChangedModels = this.DataFlowModuleCollection.AssortmentMatrixDataFlowModule.Collection
                .Where(x => currentChangesIncidentsIds.Contains(x.Id));
        }

        public Tuple<IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel>, string> Apply(AssortmentMatrixDataFlowModule.AssortmentMatrixDataFlowSimpleModel assortmentMatrix, IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel> promoes)
        {
            var filteredPromoes = new List<PromoDataFlowModule.PromoDataFlowSimpleModel>();
            foreach (var promo in promoes)
            {
                if (this.InnerApply(promo, assortmentMatrix))
                {
                    filteredPromoes.Add(promo);
                }
            }

            return new Tuple<IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel>, string>(filteredPromoes, this.ToString());
        }

        /// <summary>
        /// Клиент в промо такой же как и в ассортиментной матрице.
        /// Даты промо находятся в промежутке дат из ассортиментной матрицы.
        /// </summary>
        private bool InnerApply(PromoDataFlowModule.PromoDataFlowSimpleModel promo, AssortmentMatrixDataFlowModule.AssortmentMatrixDataFlowSimpleModel assortmentMatrix)
        {
            var matrixClientEqualsPromoClient = assortmentMatrix.ClientTreeId == promo.ClientTreeKeyId;
            if (matrixClientEqualsPromoClient)
            {
                var promoDatesBeetweenMatrixDates = promo.DispatchesStart >= assortmentMatrix.StartDate &&
                promo.DispatchesEnd <= assortmentMatrix.EndDate;

                if (promoDatesBeetweenMatrixDates /* || assortmentMatrix.Disabled */)
                {
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            return nameof(AssortmentMatrixDataFlowFilter);
        }
    }
}
