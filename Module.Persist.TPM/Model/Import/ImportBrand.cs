using Core.Import;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module.Persist.TPM.Model.TPM;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportBrand : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "Brand Code")]
        public string  Brand_code { get; set; }
        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Segment Code")]
        public string Segmen_code { get; set; }
    }
}
