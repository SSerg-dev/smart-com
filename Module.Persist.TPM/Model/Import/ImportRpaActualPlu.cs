using Core.Import;
using Module.Persist.TPM.Model.TPM;
using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportRpaActualPlu: BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "Promo Id")]
        public int PromoNumberImport { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "PLU")]
        [StringLength(255)]
        [Required]
        public string PluImport { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Actual Product PC Quantity")]
        [Range(0, 1000000000)]
        public int ActualProductPcQuantityImport { get; set; }

        public Guid PromoId { get; set; }

        public PromoProduct2Plu Plu { get; set; }

        public string EAN_PC { get; set; }

        public string StatusName { get; set; }
    }
}
