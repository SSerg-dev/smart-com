using Core.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class NonPromoSupportBrandTech : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        public bool Disabled { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedDate { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }


        public Guid NonPromoSupportId { get; set; }
        public NonPromoSupport NonPromoSupport { get; set; }
        public Guid BrandTechId { get; set; }
        public virtual BrandTech BrandTech { get; set; }
    }
}
