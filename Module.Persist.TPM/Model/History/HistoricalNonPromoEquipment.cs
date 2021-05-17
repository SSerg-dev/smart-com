using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(NonPromoEquipment))]
    public class HistoricalNonPromoEquipment : BaseHistoricalEntity<System.Guid>
    {
		public string EquipmentType { get; set; }
        public string Description_ru { get; set; }
        public Guid? BudgetItemId { get; set; }
        public string BudgetItemName { get; set; }
    }
}
