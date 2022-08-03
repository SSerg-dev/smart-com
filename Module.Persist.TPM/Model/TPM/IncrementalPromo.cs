using Core.Data;
using Module.Persist.TPM.Model.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class IncrementalPromo : IEntity<Guid>, IDeactivatable, IMode
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        public bool Disabled { get; set; }
        [Index("Unique_IncrementalPromo", 1, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }
        public TPMmode TPMmode { get; set; }

        public double? PlanPromoIncrementalCases { get; set; }

        public double? PlanPromoIncrementalLSV { get; set; }

        public DateTimeOffset? LastModifiedDate { get; set; }

        public double? CasePrice { get; set; }

        [Index("Unique_IncrementalPromo", 2, IsUnique = true)]
        public Guid PromoId { get; set; }
        public virtual Promo Promo { get; set; }
        [Index("Unique_IncrementalPromo", 3, IsUnique = true)]
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
