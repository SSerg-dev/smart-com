using Core.Import;
using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportPromoProduct : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "EAN PC")]
        [StringLength(255)]
        [Required]
        public string EAN_PC { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "Actual Product PC Qty")]
        [Range(0, 1000000000)]
        public int? ActualProductPCQty { get; set; }
      
    }
}
