using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(PromoSupportPromo))]
    class HistoricalPromoSupportPromo : BaseHistoricalEntity<System.Guid>
    {
        public Guid PromoId { get; set; }
        public Guid PromoSupportId { get; set; }
        public double PlanCalculation { get; set; }
        public double FactCalculation { get; set; }
        public double PlanCostProd { get; set; }
        public double FactCostProd { get; set; }
    }
}