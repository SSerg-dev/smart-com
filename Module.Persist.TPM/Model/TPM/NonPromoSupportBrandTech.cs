using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.TPM
{
    public class NonPromoSupportBrandTech : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public Guid NonPromoSupportId { get; set; }
        public Guid BrandTechId { get; set; }

        public virtual NonPromoSupport NonPromoSupport { get; set; }
        public virtual BrandTech BrandTech { get; set; }
    }
}
