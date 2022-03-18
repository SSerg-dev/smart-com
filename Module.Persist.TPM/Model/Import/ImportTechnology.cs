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
    public class ImportTechnology : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "Technology RU")]
        public string Description_ru { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Tech Code")]
        public string Tech_code { get; set; }

        [ImportCSVColumn(ColumnNumber = 3)]
        [Display(Name = "Sub Brand Code")]
        public string SubBrand { get; set; }

        [ImportCSVColumn(ColumnNumber = 4)]
        [Display(Name = "Sub Brand Code")]
        public string SubBrand_code { get; set; }

        [ImportCSVColumn(ColumnNumber = 5)]
        [Display(Name = "Splittable")]
        public bool IsSplittable { get; set; }
    }
}
