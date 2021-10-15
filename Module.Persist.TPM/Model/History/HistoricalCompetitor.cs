using System;
using Core.History;
using Module.Persist.TPM.Model.TPM;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(Competitor))]
    public class HistoricalCompetitor : BaseHistoricalEntity<Guid>
    {
        public string Name { get; set; }
    }
}
