using System;
using Core.History;
using Module.Persist.TPM.Model.TPM;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(ActualCOGS))]
    public class HistoricalActualCOGS : BaseHistoricalEntity<Guid>
    {
        public int ClientTreeId { get; set; }
        public Guid? BrandTechId { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public float? LSVpercent { get; set; }

        private string brandTechName;
        public string BrandTechName
        {
            get
            {
                return string.IsNullOrEmpty(BrandTechBrandsegTechsub)
              ? brandTechName
              : BrandTechBrandsegTechsub;
            }
            set
            {
                brandTechName = value;
            }
        }
        public string BrandTechBrandsegTechsub { get; set; }
        public string ClientTreeFullPathName { get; set; }
        public int? ClientTreeObjectId { get; set; }
    }
}
