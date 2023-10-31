using Core.Data;
using Module.Persist.TPM.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.TPM
{
    public class PromoInfo : IEntity<Guid>
    {
        [Key]
        public System.Guid Id { get; set; }

        public CreatedFrom CreatedFrom { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        [StringLength(255)]
        public string Description { get; set; }

        public virtual Promo Promo { get; set; }
    }
}
