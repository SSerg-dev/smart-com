using Core.Data;
using Module.Persist.TPM.Utils;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class CompetitorPromo : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; } = Guid.NewGuid();

        [Index("Unique_CompetitorPromo", 1, IsUnique = true)]
        public bool Disabled { get; set; }

        [Index("Unique_CompetitorPromo", 2, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }

        [Required]
        public Guid? CompetitorId { get; set; }

        [Required]
        public int? ClientTreeObjectId { get; set; }

        [Required]
        public Guid CompetitorBrandTechId { get; set; }

        [StringLength(124)]
        [Required]
        public string Name { get; set; }

        [Index("Unique_CompetitorPromo", 0, IsUnique = true)]
        public int Number { get; set; }

        [Required]
        public DateTimeOffset? StartDate { get; set; }

        [Required]
        public DateTimeOffset? EndDate { get; set; }

        public DateTimeOffset? DispatchesStart { get; set; }
        public DateTimeOffset? DispatchesEnd { get; set; }

        public int? PromoDuration { get; set; }
        public int? DispatchDuration { get; set; }

        //MarsDates
        [StringLength(15)]
        public string MarsStartDate { get; set; }
        [StringLength(15)]
        public string MarsEndDate { get; set; }
        [StringLength(15)]
        public string MarsDispatchesStart { get; set; }
        [StringLength(15)]
        public string MarsDispatchesEnd { get; set; }

        public double? Discount { get; set; }

        public double? Price { get; set; }

        [StringLength(256)]
        public string Subrange { get; set; }

        [ForeignKey("CompetitorId")]
        public virtual Competitor Competitor { get; set; }
        [ForeignKey("ClientTreeObjectId")]
        [SpecialNotKeyProperty]
        public virtual ClientTree ClientTree { get; set; }
        [ForeignKey("CompetitorBrandTechId")]
        public virtual CompetitorBrandTech CompetitorBrandTech { get; set; }

    }
}
