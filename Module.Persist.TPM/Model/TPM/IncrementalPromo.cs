using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class IncrementalPromo : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        public Guid? PromoId { get; set; }

        public Guid? ProductId { get; set; }

        public int? IncrementalCaseAmount { get; set; }

        public double? IncrementalLSV { get; set; }

        public double? IncrementalPrice { get; set; }

        public DateTimeOffset? LastModifiedDate { get; set; }

        public virtual Promo Promo { get; set; }
        public virtual Product Product { get; set; }
    }
}
