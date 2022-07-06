using Core.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class BaseCOGSTn : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Index]
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public int Year { get; set; }
        public double TonCost { get; set; }

        public Guid? BrandTechId { get; set; }
        public virtual BrandTech BrandTech { get; set; }
        public int ClientTreeId { get; set; }
        public virtual ClientTree ClientTree { get; set; }
    }

    public class PlanCOGSTn : BaseCOGSTn { }

    public class ActualCOGSTn : BaseCOGSTn
    {
        public bool IsCOGSIncidentCreated { get; set; }
    }
}
