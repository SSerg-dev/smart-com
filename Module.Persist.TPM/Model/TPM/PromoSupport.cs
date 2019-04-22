using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class PromoSupport : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public int ClientTreeId { get; set; }
        public Guid? BudgetSubItemId { get; set; }
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

        public virtual ClientTree ClientTree { get; set; }
        public virtual BudgetSubItem BudgetSubItem { get; set; }
    }
}
