using Core.Import;
using System.ComponentModel.DataAnnotations;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportNonPromoEquipment : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "EquipmentType")]
        public string EquipmentType { get; set; }
    }
}
