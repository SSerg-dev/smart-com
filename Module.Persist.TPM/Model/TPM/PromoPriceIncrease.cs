using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class PromoPriceIncrease : IEntity<Guid>, IDeactivatable
    {
        [Key]
        public System.Guid Id { get; set; }
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        public double? PlanPromoUpliftPercent { get; set; }
        public double? PlanPromoBaselineLSV { get; set; }
        public double? PlanPromoIncrementalLSV { get; set; }
        public double? PlanPromoLSV { get; set; }
        public double? PlanPromoPostPromoEffectLSVW1 { get; set; }
        public double? PlanPromoPostPromoEffectLSVW2 { get; set; }
        public double? PlanPromoPostPromoEffectLSV { get; set; }
        public double? PlanPromoPostPromoEffectVolumeW1 { get; set; }
        public double? PlanPromoPostPromoEffectVolumeW2 { get; set; }
        public double? PlanPromoPostPromoEffectVolume { get; set; }
        public double? PlanPromoBaselineVolume { get; set; }
        public double? PlanPromoTIShopper { get; set; }
        public double? PlanPromoCost { get; set; }
        public double? PlanPromoIncrementalCOGS { get; set; }
        public double? PlanPromoBaseTI { get; set; }
        public double? PlanPromoIncrementalBaseTI { get; set; }
        public double? PlanPromoTotalCost { get; set; }
        public double? PlanPromoNetIncrementalLSV { get; set; }
        public double? PlanPromoNetLSV { get; set; }
        public double? PlanPromoNetIncrementalBaseTI { get; set; }
        public double? PlanPromoNetIncrementalCOGS { get; set; }
        public double? PlanPromoNetBaseTI { get; set; }
        public double? PlanPromoBaselineBaseTI { get; set; }
        public double? PlanPromoNSV { get; set; }
        public double? PlanPromoIncrementalNSV { get; set; }
        public double? PlanPromoNetIncrementalNSV { get; set; }
        public double? PlanPromoNetIncrementalMAC { get; set; }
        public double? PlanPromoIncrementalVolume { get; set; }
        public double? PlanPromoNetIncrementalVolume { get; set; }
        public double? PlanPromoNetNSV { get; set; }
        public double? PlanPromoIncrementalCOGSTn { get; set; }
        public double? PlanPromoNetIncrementalCOGSTn { get; set; }
        public double? PlanPromoIncrementalMAC { get; set; }
        public double? PlanPromoIncrementalEarnings { get; set; }
        public double? PlanPromoNetIncrementalEarnings { get; set; }
        public double? PlanPromoROIPercent { get; set; }
        public double? PlanPromoNetROIPercent { get; set; }
        public double? PlanPromoNetIncrementalMACLSV { get; set; }
        public double? PlanPromoIncrementalMACLSV { get; set; }
        public double? PlanPromoIncrementalEarningsLSV { get; set; }
        public double? PlanPromoNetIncrementalEarningsLSV { get; set; }
        public double? PlanPromoROIPercentLSV { get; set; }
        public double? PlanPromoNetROIPercentLSV { get; set; }
        public double? PlanAddTIShopperCalculated { get; set; }
        public double? PlanAddTIShopperApproved { get; set; }
        public double? PlanPromoNetUpliftPercent { get; set; }

        public virtual Promo Promo { get; set; }
        public ICollection<PromoProductPriceIncrease> PromoProductPriceIncreases { get; set; }
    }
}
