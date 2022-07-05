using Core.Import;
using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportRPAPromoSupport: BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "PromoSupport Id")]
        public int PromoSupportNumber { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "External Code")]
        public string ExternalCode { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        public Guid PromoSupportId { get; set; }


    }
}
