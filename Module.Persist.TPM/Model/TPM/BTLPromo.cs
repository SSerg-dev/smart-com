using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.TPM
{
    public class BTLPromo : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public Guid BTLId { get; set; }
        public Guid PromoId { get; set; }
        public int ClientTreeId { get; set; }

        public virtual BTL BTL { get; set; }
        public virtual Promo Promo { get; set; }
        public virtual ClientTree ClientTree { get; set; }

        public BTLPromo() { }

        public BTLPromo(Guid btlId, Guid promoId, int clientTreeId)
        {
            BTLId = btlId;
            PromoId = promoId;
            ClientTreeId = clientTreeId;
        }
    }
}
