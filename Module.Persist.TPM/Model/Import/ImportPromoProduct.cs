using Core.Import;
using Module.Persist.TPM.Model.TPM;
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

    public class ImportPromoProductPlu : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "PLU")]
        [StringLength(255)]
        [Required]
        public string PluCode  { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "Actual Product PC Qty")]
        [Range(0, 1000000000)]
        public int? ActualProductPCQty { get; set; }

        public PromoProduct2Plu Plu { get; set; }

        public string EAN_PC { get; set; }
    }
}
