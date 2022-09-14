using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(IncreaseBaseLine))]
    class HistoricalIncreaseBaseLine
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
