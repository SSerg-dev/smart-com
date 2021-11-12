using System;
using Core.History;
using Module.Persist.TPM.Model.TPM;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(CompetitorPromo))]
    public class HistoricalCompetitorPromo : BaseHistoricalEntity<Guid>
    {
        public string CompetitorName { get; set; }
        public string ClientTreeFullPathName { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public double? Price { get; set; }
        public double? Discount { get; set; }
        public string Status { get; set; }
        public string CompetitorBrandTechName { get; set; }
        public string Subrange { get; set; }
    }
}
