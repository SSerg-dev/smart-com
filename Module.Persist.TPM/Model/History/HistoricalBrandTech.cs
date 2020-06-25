using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(BrandTech))]
    public class HistoricalBrandTech : BaseHistoricalEntity<System.Guid>
    {
        public string Name { get; set; }
        public string BrandName { get; set; }
        public string TechnologyName { get; set; }
        public string TechnologySubBrand { get; set; }
        public string BrandTech_code { get; set; }
        public string BrandsegTechsub_code { get; set; }
    }
}
