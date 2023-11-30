using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.TPM
{
    public class PromoBlockedStatus : IEntity<Guid>
    {
        [Key]
        public System.Guid Id { get; set; }
        public bool Blocked { get; set; }
        public bool DraftToPublished { get; set; }

        public virtual Promo Promo { get; set; }
        public ICollection<BlockedPromo> BlockedPromoes { get; set; }
    }
}
