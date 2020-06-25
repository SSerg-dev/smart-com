using Core.History;
using Module.Persist.TPM.Model.TPM;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(Technology))]
    public class HistoricalTechnology : BaseHistoricalEntity<System.Guid>
    {
        public string Name { get; set; }
        public string Tech_code { get; set; }
        public string SubBrand { get; set; }
        public string SubBrand_code { get; set; }
    }
}
