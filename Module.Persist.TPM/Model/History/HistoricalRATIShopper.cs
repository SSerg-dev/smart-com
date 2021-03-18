using System;
using Core.History;
using Module.Persist.TPM.Model.TPM;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(RATIShopper))]
    public class HistoricalRATIShopper : BaseHistoricalEntity<Guid>
    {
        public int? ClientTreeId { get; set; }
        public Guid? BrandTechId { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string ClientTreeFullPathName { get; set; }
        public int? ClientTreeObjectId { get; set; }
        public float? RATIShopperPercent { get; set; }
    }
}
