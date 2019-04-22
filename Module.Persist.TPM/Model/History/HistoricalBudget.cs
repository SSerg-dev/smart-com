using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(Budget))]
    public class HistoricalBudget : BaseHistoricalEntity<System.Guid>
    {
        public string Name { get; set; }
    }
}
