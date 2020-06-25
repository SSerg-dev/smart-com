using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;
namespace Module.Persist.TPM.Model.History {
    [AssociatedWith(typeof(Color))]
    public class HistoricalColor : BaseHistoricalEntity<System.Guid> {
        public Guid? BrandTechId { get; set; }

        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public string BrandTechBrandName { get; set; }
        public string BrandTechTechnologyName { get; set; }
        public string BrandTechTechnologySubBrand { get; set; }
    }
}
