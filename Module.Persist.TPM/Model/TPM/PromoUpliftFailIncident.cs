using Core.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class PromoUpliftFailIncident : IEntity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        [Index]
        public DateTimeOffset? ProcessDate { get; set; }

        [Index]
        public Guid PromoId { get; set; }
        public virtual Promo Promo { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
