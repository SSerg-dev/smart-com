using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class CompetitorBrandTech : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; } = Guid.NewGuid();
        [Index("Unique_CompetitorBrandTech", 2, IsUnique = true)]
        public bool Disabled { get; set; }
        [Index("Unique_CompetitorBrandTech", 3, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }

        [StringLength(124)]
        [Index("Unique_CompetitorBrandTech", 1, IsUnique = true)]
        [Required]
        public string BrandTech { get; set; }
        [StringLength(24)]
        [Required]
        public string Color { get; set; }

        [Index("Unique_CompetitorBrandTech", 0, IsUnique = true)]
        public Guid? CompetitorId { get; set; }
        [ForeignKey("CompetitorId")]
        public virtual Competitor Competitor { get; set; }

        public ICollection<CompetitorPromo> CompetitorPromoes { get; set; }
    }
}
