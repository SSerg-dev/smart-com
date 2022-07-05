using Core.Import;
using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportRpaActualEanPc: BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "Promo Id")]
        public int PromoNumberImport { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "EAN PC")]
        [StringLength(255)]
        [Required]
        public string EanPcImport { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Actual Product PC Quantity")]
        [Range(0, 1000000000)]
        public int ActualProductPcQuantityImport { get; set; }

        public Guid PromoId { get; set; }
    }
}
