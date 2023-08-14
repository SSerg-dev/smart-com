using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class DiscountRange: IEntity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Index]
        public Guid Id { get; set; } = Guid.NewGuid();

        [StringLength(255)]
        public string Name { get; set; }

        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        
        public ICollection<PlanPostPromoEffect> PlanPostPromoEffects{ get; set; }
    }
}