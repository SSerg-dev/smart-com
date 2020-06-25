using System;
using Core.History;
using Module.Persist.TPM.Model.TPM;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(ActualTradeInvestment))]
    public class HistoricalActualTradeInvestment : BaseHistoricalEntity<Guid>
    {
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
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

        public string TIType { get; set; }
        public string TISubType { get; set; }
        public float? SizePercent { get; set; }

        public bool? MarcCalcROI { get; set; }
        public bool? MarcCalcBudgets { get; set; }
    }
}
