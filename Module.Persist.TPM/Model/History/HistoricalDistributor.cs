using Core.History;
using Module.Persist.TPM.Model.TPM;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(Distributor))]
    public class HistoricalDistributor : BaseHistoricalEntity<System.Guid>
    {
        public string Name { get; set; }
    }
}
