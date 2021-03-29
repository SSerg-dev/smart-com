using System;
using System.ComponentModel.DataAnnotations;
using Core.Import;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportNonPromoEquipment : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "EquipmentType")]
        public string EquipmentType { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "EquipmentType")]
        public string Description_ru { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Budget item")]
        public string BudgetItem { get; set; }

        public Guid? BudgetItemId { get; set; }
    }
}
