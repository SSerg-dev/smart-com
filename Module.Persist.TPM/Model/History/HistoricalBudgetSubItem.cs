using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(BudgetSubItem))]
    public class HistoricalBudgetSubItem : BaseHistoricalEntity<System.Guid>
    {
        public Guid BudgetItemId { get; set; }
        public string Name { get; set; }
        public string BudgetItemName { get; set; }
        public string BudgetItemBudgetName { get; set; }
    }
}
