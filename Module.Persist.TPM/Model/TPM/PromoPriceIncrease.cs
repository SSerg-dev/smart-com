﻿using Core.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class PromoPriceIncrease : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        public double? PlanPromoUpliftPercent { get; set; }
        public double? PlanPromoBaselineLSV { get; set; }
        public double? PlanPromoIncrementalLSV { get; set; }
        public double? PlanPromoLSV { get; set; }
        public double? PlanPromoPostPromoEffectLSV { get; set; }

        public Promo Promo { get; set; }
        public PromoProductPriceIncrease PromoProductPriceIncrease { get; set; }
    }
}
