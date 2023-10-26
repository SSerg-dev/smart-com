using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(SFAType))]
    public class HistoricalSFAType : BaseHistoricalEntity<System.Guid>
    {
        public string Name { get; set; }

    }
}
