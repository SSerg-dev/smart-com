using System;
using Core.History;
using Module.Persist.TPM.Model.TPM;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(CompetitorPromo))]
    public class HistoricalCompetitorPromo : BaseHistoricalEntity<Guid>
    {
        public string BrandTech { get; set; }
        public string Color { get; set; }
        public Guid? CompetitorId { get; set; }
        public int? ClientTreeId { get; set; }
        //доделать
    }
}
