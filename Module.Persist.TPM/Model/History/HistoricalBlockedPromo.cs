using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(BlockedPromo))]
    public class HistoricalBlockedPromo : BaseHistoricalEntity<System.Guid>
    {
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        public Guid PromoId { get; set; }
        public Guid HandlerId { get; set; }
        public DateTimeOffset CreateDate { get; set; }
    }
}
