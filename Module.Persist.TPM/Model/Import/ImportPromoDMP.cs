using Core.Import;
using System.ComponentModel.DataAnnotations;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportPromoDMP : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "External Code")]
        public string ExternalCode { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
    }
}
