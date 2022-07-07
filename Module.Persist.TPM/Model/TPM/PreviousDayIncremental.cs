using Core.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class PreviousDayIncremental : IEntity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }

        public string Week { get; set; }
        public string DemandCode { get; set; }
        public string DMDGroup { get; set; }
        public double? IncrementalQty { get; set; }
        public DateTimeOffset? LastChangeDate { get; set; }

        public Guid? PromoId { get; set; }
        public virtual Promo Promo { get; set; }
        public Guid? ProductId { get; set; }
        public virtual Product Product { get; set; }

    }
}
