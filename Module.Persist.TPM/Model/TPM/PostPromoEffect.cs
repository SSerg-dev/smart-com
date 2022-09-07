using Core.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class PostPromoEffect : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public int ClientTreeId { get; set; }
        public int ProductTreeId { get; set; }
        public double EffectWeek1 { get; set; }
        public double EffectWeek2 { get; set; }
        public double TotalEffect { get; set; }

        public virtual ClientTree ClientTree { get; set; }
        public virtual ProductTree ProductTree { get; set; }
    }
}
