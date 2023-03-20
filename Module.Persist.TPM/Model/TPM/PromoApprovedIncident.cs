using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Data;

namespace Module.Persist.TPM.Model.TPM
{
    public class PromoApprovedIncident : IEntity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Index]
        public Guid PromoId { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        [Index]
        public DateTimeOffset? ProcessDate { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedDate { get; set; }
        public virtual Promo Promo { get; set; }
    }
}
