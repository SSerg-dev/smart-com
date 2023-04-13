using System;
using Core.History;
using Module.Persist.TPM.Model.TPM;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(PlanPostPromoEffect))]
    public class HistoricalPlanPostPromoEffect: BaseHistoricalEntity<Guid>
    {
        public int? ClientTreeId { get; set; }
        public Guid? BrandTechId { get; set; }
        public string Size { get; set; }
        public double PlanPostPromoEffectW1 { get; set; }
        public double PlanPostPromoEffectW2 { get; set; }
        public string DiscountRangeName { get; set; }
        public string DurationRangeName { get; set; }

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