using Core.Data;
using Module.Persist.TPM.Model.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class BTLPromo : IEntity<Guid>, IDeactivatable, IMode
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public TPMmode TPMmode { get; set; }

        public Guid BTLId { get; set; }
        public virtual BTL BTL { get; set; }
        public Guid PromoId { get; set; }
        public virtual Promo Promo { get; set; }
        public int ClientTreeId { get; set; }
        public virtual ClientTree ClientTree { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
