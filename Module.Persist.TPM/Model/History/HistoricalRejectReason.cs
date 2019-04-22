using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;

namespace Module.Persist.TPM.Model.History {
    [AssociatedWith(typeof(RejectReason))]
    public class HistoricalRejectReason : BaseHistoricalEntity<System.Guid> {
        public string Name { get; set; }
        public string SystemName { get; set; }
    }
}
