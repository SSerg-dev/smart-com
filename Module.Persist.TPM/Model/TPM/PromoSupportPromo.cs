using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.TPM
{
    public class PromoSupportPromo : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        [Index("Unique_PromoSupportPromo", 3, IsUnique = true)]
        public bool Disabled { get; set; }
        [Index("Unique_PromoSupportPromo", 4, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }

        [Index("Unique_PromoSupportPromo", 1, IsUnique = true)]
        public Guid PromoId { get; set; }
        [Index("Unique_PromoSupportPromo", 2, IsUnique = true)]
        public Guid PromoSupportId { get; set; }

        // Cost TE
        public double PlanCalculation { get; set; }
        public double FactCalculation { get; set; }

        // Cost Production
        public double PlanCostProd { get; set; }
        public double FactCostProd { get; set; }

        public virtual Promo Promo { get; set; }
        public virtual PromoSupport PromoSupport { get; set; }

        public PromoSupportPromo() { }

        public PromoSupportPromo(Guid promoSupportId, Guid promoId)
        {
            PromoId = promoId;
            PromoSupportId = promoSupportId;
        }
    }
}
