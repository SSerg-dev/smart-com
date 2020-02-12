using System;
using Core.History;
using Module.Persist.TPM.Model.TPM;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(COGS))]
    public class HistoricalCOGS : BaseHistoricalEntity<Guid>
    {
        public int? ClientTreeId { get; set; }
        public Guid? BrandTechId { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public float? LSVpercent { get; set; }

        public string BrandTechName { get; set; }
        public string ClientTreeFullPathName { get; set; }
        public int? ClientTreeObjectId { get; set; }
    }
}
