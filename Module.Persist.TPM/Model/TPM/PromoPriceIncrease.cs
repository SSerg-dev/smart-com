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
        public double? PlanPromoPostPromoEffectLSV { get; set; }

        public Promo Promo { get; set; }
        public ICollection<PromoProductPriceIncrease> PromoProductPriceIncreases { get; set; }
    }
}
