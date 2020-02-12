using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(ActualTradeInvestment))]
    public class HistoricalActualTradeInvestment : BaseHistoricalEntity<System.Guid>
    {
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }       
        public string BrandTechName { get; set; }
        public string ClientTreeFullPathName { get; set; }
        public int ClientTreeObjectId { get; set; }

        public string TIType { get; set; }
        public string TISubType { get; set; }
        public float SizePercent { get; set; }

        public bool MarcCalcROI { get; set; }
        public bool MarcCalcBudgets { get; set; }
    }
}
