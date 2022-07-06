using Core.Import;
using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportRPAEvent : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "Promo ID")]
        public int PromoNumber { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "Event")]
        public string EventName { get; set; }
    }
}
