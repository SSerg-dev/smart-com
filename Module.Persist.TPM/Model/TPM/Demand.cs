using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class Demand : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public Guid? ClientId { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? BrandTechId { get; set; }
        public int? Number { get; set; }
        public string Name { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset? DispatchesStart { get; set; }
        public DateTimeOffset? DispatchesEnd { get; set; }
        public int? PlanBaseline { get; set; }
        public int? PlanDuration { get; set; }
        public int? PlanUplift { get; set; }
        public int? PlanIncremental { get; set; }
        public int? PlanActivity { get; set; }
        public int? PlanSteal { get; set; }
        public int? FactBaseline { get; set; }
        public int? FactDuration { get; set; }
        public int? FactUplift { get; set; }
        public int? FactIncremental { get; set; }
        public int? FactActivity { get; set; }
        public int? FactSteal { get; set; }

        public virtual Client Client { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual BrandTech BrandTech { get; set; }
    }
}
