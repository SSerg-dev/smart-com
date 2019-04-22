using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(BrandTech))]
    public class HistoricalBrandTech : BaseHistoricalEntity<System.Guid>
    {
        public Guid? BrandId { get; set; }
        public Guid? TechnologyId { get; set; }

        public string Name { get; set; }
        public string BrandName { get; set; }
        public string TechnologyName { get; set; }
    }
}
