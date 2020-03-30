using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(BTL))]
    public class HistoricalBTL : BaseHistoricalEntity<System.Guid>
    {
        public int Number { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public double? PlanBTLTotal { get; set; }
        public double? ActualBTLTotal { get; set; }
        public string InvoiceNumber { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
    }
}
