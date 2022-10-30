using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(PromoProductCorrectionPriceIncrease))]
    public class HistoricalPromoProductCorrectionPriceIncrease : BaseHistoricalEntity<System.Guid>
    {

        public Guid PromoProductId { get; set; }
        public double? PlanProductUpliftPercentCorrected { get; set; }

        public Guid? UserId { get; set; }
        public string UserName { get; set; }

        public DateTimeOffset? CreateDate { get; set; }
        public DateTimeOffset? ChangeDate { get; set; }
    }
}
