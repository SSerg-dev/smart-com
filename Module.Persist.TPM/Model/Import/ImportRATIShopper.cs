using Core.Import;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportRATIShopper : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "Client hierarchy")]
        public String ClientTreeFullPath { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "Client hierarchy code")]
        public int ClientTreeObjectId { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Year")]
        public int Year { get; set; }

        [ImportCSVColumn(ColumnNumber = 3)]
        [Display(Name = "RA TI Shopper percent")]
        public float RATIShopperPercent { get; set; }


        public int ClientTreeId { get; set; }
    }
}
