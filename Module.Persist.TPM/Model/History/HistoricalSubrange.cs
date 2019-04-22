using Core.History;
using Module.Persist.TPM.Model.TPM;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(Subrange))]
    public class HistoricalSubrange : BaseHistoricalEntity<System.Guid>
    {
        public string Name { get; set; }
    }
}
