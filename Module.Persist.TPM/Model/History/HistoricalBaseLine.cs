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
        public string ClientTreeDemandCode { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public double? QTY { get; set; }
        public double? Price { get; set; }
        public double? BaselineLSV { get; set; }
        public int? Type { get; set; }
        public DateTimeOffset? LastModifiedDate { get; set; }
    }
}
