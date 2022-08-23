using Core.Data;
using Module.Persist.TPM.Model.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class PromoSupportPromo : IEntity<Guid>, IDeactivatable, IMode
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        [Index("Unique_PromoSupportPromo", 3, IsUnique = true)]
        public bool Disabled { get; set; }
        [Index("Unique_PromoSupportPromo", 4, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }
        public TPMmode TPMmode { get; set; }
        public RSstatus RSstatus { get; set; }

        // Cost TE
        public double PlanCalculation { get; set; }
        public double FactCalculation { get; set; }

        // Cost Production
        public double PlanCostProd { get; set; }
        public double FactCostProd { get; set; }

        [Index("Unique_PromoSupportPromo", 1, IsUnique = true)]
        public Guid PromoId { get; set; }
        public virtual Promo Promo { get; set; }
        [Index("Unique_PromoSupportPromo", 2, IsUnique = true)]
        public Guid PromoSupportId { get; set; }
        public virtual PromoSupport PromoSupport { get; set; }

    }
}
