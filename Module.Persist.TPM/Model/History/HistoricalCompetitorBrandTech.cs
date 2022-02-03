using System;
using Core.History;
using Module.Persist.TPM.Model.TPM;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(CompetitorBrandTech))]
    public class HistoricalCompetitorBrandTech : BaseHistoricalEntity<Guid>
    {
        public string BrandTech { get; set; }
        public string Color { get; set; }
        public string CompetitorName { get; set; }
    }
}
