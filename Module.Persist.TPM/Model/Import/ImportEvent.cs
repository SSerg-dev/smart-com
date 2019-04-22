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
    public class ImportEvent : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "Year")]
        public int Year { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Period")]
        public string Period { get; set; }

        [ImportCSVColumn(ColumnNumber = 3)]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}
