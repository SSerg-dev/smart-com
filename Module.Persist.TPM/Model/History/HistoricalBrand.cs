using Core.History;
using Module.Persist.TPM.Model.TPM;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(Brand))]
    public class HistoricalBrand : BaseHistoricalEntity<System.Guid>
    {
        public string Name { get; set; }
        public string Brand_code { get; set; }
        public string Segmen_code { get; set; }
    }
}
