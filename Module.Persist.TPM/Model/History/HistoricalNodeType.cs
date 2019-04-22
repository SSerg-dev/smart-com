using Core.History;
using Module.Persist.TPM.Model.TPM;

namespace Module.Persist.TPM.Model.History {
    [AssociatedWith(typeof(NodeType))]
    public class HistoricalNodeType : BaseHistoricalEntity<System.Guid> {
        public string Type { get; set; }
        public string Name { get; set; }
    }
}
