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
    public class ImportPromoProductsUplift : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "ZREP")]
        public string ZREP { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "Plan Product Uplift, %")]
        public string PlanProductUpliftPercent { get; set; }
    }
}