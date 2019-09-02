using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Data;

namespace Module.Persist.TPM.Model.TPM
{
    public class PromoCancelledIncident : IEntity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Index]
        public Guid PromoId { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        [Index]
        public DateTimeOffset? ProcessDate { get; set; }

		public virtual Promo Promo { get; set; }
    }
}
