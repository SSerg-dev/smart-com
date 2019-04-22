using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;
namespace Module.Persist.TPM.Model.History
{
  [AssociatedWith(typeof(Sale ))]
  public class HistoricalSale : BaseHistoricalEntity<System.Guid>
{
        public Guid? BudgetId {get; set;}
        public Guid? BudgetItemId {get; set;}
        public Guid? PromoId {get; set;}
        public int? Plan {get; set;}
        public int? Fact {get; set; }
        public string BudgetItemBudgetName { get; set; }
        public string BudgetItemName { get; set; }
    }
}
