using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(PromoSupport))]
    public class HistoricalPromoSupport : BaseHistoricalEntity<System.Guid>
    {
        public int Number { get; set; }
        public int? PlanQuantity { get; set; }
        public int? ActualQuantity { get; set; }
        public double? PlanCostTE { get; set; }
        public double? ActualCostTE { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public double? PlanProdCostPer1Item { get; set; }
        public double? ActualProdCostPer1Item { get; set; }
        public double? PlanProdCost { get; set; }
        public double? ActualProdCost { get; set; }
        public string UserTimestamp { get; set; }
        public string AttachFileName { get; set; }
        public string BorderColor { get; set; }
        public string PONumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string ClientTreeFullPathName { get; set; }
        public string BudgetSubItemName { get; set; }
        public string BudgetSubItemBudgetItemName { get; set; }
        public bool OffAllocation { get; set; }
    }
}
