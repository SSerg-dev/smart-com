using Core.Data;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
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

        [Index("Unique_BrandTech", 1, IsUnique = true)]
        public Guid BrandId { get; set; }

        [Index("Unique_BrandTech", 2, IsUnique = true)]
        public Guid TechnologyId { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Technology Technology { get; set; }

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
    }
}
