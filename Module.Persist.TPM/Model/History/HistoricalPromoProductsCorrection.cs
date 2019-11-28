using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(PromoProductsCorrection))]
    public class HistoricalPromoProductsCorrection : BaseHistoricalEntity<System.Guid>
    { 
             
        public Guid PromoProductId { get; set; }
        public double? PlanProductUpliftPercentCorrected { get; set; }

        public Guid? UserId { get; set; }
        public string UserName { get; set; }

        public DateTimeOffset? CreateDate { get; set; }
        public DateTimeOffset? ChangeDate { get; set; }

         
    }
}
