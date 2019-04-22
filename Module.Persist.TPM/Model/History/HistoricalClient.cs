using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(Client))]
    public class HistoricalClient : BaseHistoricalEntity<System.Guid>
    {
        public Guid CommercialSubnetId { get; set; }
      
        public string CommercialSubnetCommercialNetName { get; set; }
        public string CommercialSubnetName { get; set; }
    }
}
