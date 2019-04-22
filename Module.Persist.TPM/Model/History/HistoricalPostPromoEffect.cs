using System;
using Module.Persist.TPM.Model.TPM;
using Core.History;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(PostPromoEffect))]
    public class HistoricalPostPromoEffect : BaseHistoricalEntity<Guid>
    {
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public int ClientTreeId { get; set; }
        public int ProductTreeId { get; set; }
        public double EffectWeek1 { get; set; }
        public double EffectWeek2 { get; set; }
        public double TotalEffect { get; set; }

        public string ClientTreeFullPathName { get; set; }
        public string ProductTreeFullPathName { get; set; }
    }
}