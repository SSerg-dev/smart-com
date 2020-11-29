using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;
namespace Module.Persist.TPM.Model.History
{
  [AssociatedWith(typeof(BudgetItem ))]
  public class HistoricalBudgetItem : BaseHistoricalEntity<System.Guid>
{
        public Guid BudgetId { get; set; }
        public string Name {get; set; }
        public string BudgetName { get; set; }
        public string Description_ru { get; set; }
        public string ButtonColor { get; set; }
    }
}
