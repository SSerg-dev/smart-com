using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(BaseLine))]
    public class HistoricalBaseLine : BaseHistoricalEntity<System.Guid>
    {
        public string ProductZREP { get; set; }
        public string DemandCode { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public double? InputBaselineQTY { get; set; }
        public double? SellInBaselineQTY { get; set; }
        public double? SellOutBaselineQTY { get; set; }
        public int? Type { get; set; }
        public DateTimeOffset? LastModifiedDate { get; set; }
    }
}
