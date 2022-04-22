using Core.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class BaseCOGS : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Index]
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        public int ClientTreeId { get; set; }
        public Guid? BrandTechId { get; set; }

        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public float LSVpercent { get; set; }
        public int Year { get; set; }

        public virtual BrandTech BrandTech { get; set; }
        public virtual ClientTree ClientTree { get; set; }
    }

    public class COGS : BaseCOGS { }

    public class ActualCOGS : BaseCOGS
    {
        public bool IsCOGSIncidentCreated { get; set; }
    }

    public class PlanCOGSTn : BaseCOGS
    {
        public int Volume { get; set; }
    }

    public class ActualCOGSTn : BaseCOGS 
    {
        public int Volume { get; set; }
        public bool IsCOGSIncidentCreated { get; set; }
    }
}
