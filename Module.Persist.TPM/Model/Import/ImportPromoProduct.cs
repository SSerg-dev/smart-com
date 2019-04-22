using Core.Import;
using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportPromoProduct : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "EAN")]
        [StringLength(255)]
        [Required]
        public string EAN { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "Actual Product PC Qty")]
        [Range(0, 1000000000)]
        public int? ActualProductPCQty { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Actual Product Qty")]
        [Range(0, 10000000000)]
        public double? ActualProductQty { get; set; }

        [ImportCSVColumn(ColumnNumber = 3)]
        [Display(Name = "Actual Product UOM")]
        [MaxLength(255)]
        public string ActualProductUOM { get; set; }

        [ImportCSVColumn(ColumnNumber = 4)]
        [Display(Name = "Actual Product Shelf Price")]
        [Range(0, 1000000000)]
        public double? ActualProductShelfPrice { get; set; }

        [ImportCSVColumn(ColumnNumber = 5)]
        [Display(Name = "Actual Product PC LSV")]
        [Range(0, 1000000000)]
        public double? ActualProductPCLSV { get; set; }        
    }
}
