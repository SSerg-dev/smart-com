using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(IncrementalPromo))]
    public class HistoricalIncrementalPromo : BaseHistoricalEntity<System.Guid>
    {

        public int PromoNumber { get; set; }
        public string PromoName { get; set; }
        public string PromoBrandTechName { get; set; }
        public DateTimeOffset? PromoStartDate { get; set; }
        public DateTimeOffset? PromoEndDate { get; set; }
        public DateTimeOffset? PromoDispatchesStart { get; set; }
        public DateTimeOffset? PromoDispatchesEnd { get; set; }
        public string ProductZREP { get; set; }
        public double IncrementalCaseAmount { get; set; }
        public double IncrementalLSV { get; set; }
        public double IncrementalPrice { get; set; }

        public DateTimeOffset? LastModifiedDate { get; set; }
    }
}
