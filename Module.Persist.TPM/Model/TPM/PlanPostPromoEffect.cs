using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class PlanPostPromoEffect : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Index]
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        [StringLength(255)]
        public string Size { get; set; }
        public double PlanPostPromoEffectW1 { get; set; }
        public double PlanPostPromoEffectW2 { get; set; }
        
        public Guid DiscountRangeId { get; set; }
        public DiscountRange DiscountRange { get; set; }
        public Guid DurationRangeId { get; set; }
        public DurationRange DurationRange { get; set; }
        public Guid BrandTechId { get; set; }
        public BrandTech BrandTech { get; set; }
        public int ClientTreeId { get; set; }
        public ClientTree ClientTree { get; set; }
    }
}