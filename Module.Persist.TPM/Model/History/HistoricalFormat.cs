using Core.History;
using Module.Persist.TPM.Model.TPM;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(Format))]
    public class HistoricalFormat : BaseHistoricalEntity<System.Guid>
    {
        public string Name { get; set; }
    }
}
