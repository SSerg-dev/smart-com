using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(CommercialSubnet))]
    public class HistoricalCommercialSubnet : BaseHistoricalEntity<System.Guid>
    {
        public string Name { get; set; }
        public Guid CommercialNetId { get; set; }
        public string CommercialNetName { get; set; }
    }
}
