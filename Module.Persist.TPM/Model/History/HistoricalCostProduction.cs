using Core.History;
using System;

namespace Module.Persist.TPM.Model.History
{
    public class HistoricalCostProduction : BaseHistoricalEntity<Guid>
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
        public string AttachFileName { get; set; }
        public string BorderColor { get; set; }
        public string PONumber { get; set; }
        public string ClientTreeFullPathName { get; set; }
        public string BudgetItemName { get; set; }
        public string BudgetSubItemBudgetItemName { get; set; }
    }
}