using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class BrandTech : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }

        [Index("Unique_BrandTech", 3, IsUnique = true)]
        public bool Disabled { get; set; }

        [Index("Unique_BrandTech", 4, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Name { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string BrandTech_code { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string BrandsegTechsub_code { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string BrandsegTechsub { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string TechSubName { get; set; }

        [Index("Unique_BrandTech", 1, IsUnique = true)]
        public Guid BrandId { get; set; }
        public virtual Brand Brand { get; set; }
        [Index("Unique_BrandTech", 2, IsUnique = true)]
        public Guid TechnologyId { get; set; }
        public virtual Technology Technology { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedDate { get; set; }

        public ICollection<ClientTreeBrandTech> ClientTreeBrandTeches { get; set; }
        public ICollection<CoefficientSI2SO> CoefficientSI2SOs { get; set; }
        public ICollection<NonPromoSupportBrandTech> NonPromoSupportBrandTeches { get; set; }
        public ICollection<TradeInvestment> TradeInvestments { get; set; }
    }
}
