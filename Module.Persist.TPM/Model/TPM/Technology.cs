using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class Technology : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Index("Unique_Tech", 3, IsUnique = true)]
        public bool Disabled { get; set; }

        [Index("Unique_Tech", 4, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }

        [Index("Unique_Tech", 5, IsUnique = true)]
        [StringLength(255)]
        [Required]
        public string Name { get; set; }


        [StringLength(255)]
        public string Description_ru { get; set; }

        [Index("Unique_Tech", 1, IsUnique = true)]
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string Tech_code { get; set; }

        [StringLength(255)]
        public string SubBrand { get; set; }

        [Index("Unique_Tech", 2, IsUnique = true)]
        [StringLength(255)]
        public string SubBrand_code { get; set; }
        public bool IsSplittable { get; set; }

        public ICollection<BrandTech> BrandTeches { get; set; }
        public ICollection<ProductTree> ProductTrees { get; set; }
    }
}
