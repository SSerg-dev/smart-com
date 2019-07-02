using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(PromoSupport))]
    public class HistoricalPromoSupport : BaseHistoricalEntity<System.Guid>
    {
        public int Number { get; set; }
        public string ClientTreeFullPathName { get; set; }
        public string BudgetItemName { get; set; }
        public string BudgetSubItemName { get; set; }
        public int? PlanQuantity { get; set; }
        public int? ActualQuantity { get; set; }
        public double? PlanCostTE { get; set; }
        public double? ActualCostTE { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public double? PlanProdCost { get; set; }
        public double? ActualProdCost { get; set; }
        public string AttachFileName { get; set; }
    }
}
