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
    public class ImportIncrementalPromo : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 3)]
        [NavigationPropertyMap(LookupEntityType = typeof(Promo), LookupPropertyName = "Number")]
        [Display(Name = "Promo ID")]
        public int? PromoNumber { get; set; }

        //TODO: уточнить порядок импорта полей
        [ImportCSVColumn(ColumnNumber = 0)]
        [NavigationPropertyMap(LookupEntityType = typeof(Product), LookupPropertyName = "ZREP")]
        [Display(Name = "ZREP")]
        public string ProductZREP { get; set; }

        [Display(Name = "Case Amount")]
        [ImportCSVColumn(ColumnNumber = 5)]
        public double? PlanPromoIncrementalCases { get; set; }

        [Display(Name = "LSV")]
        [ImportCSVColumn(ColumnNumber = 7)]
        public double? PlanPromoIncrementalLSV { get; set; }

        [Display(Name = "Price")]
        [ImportCSVColumn(ColumnNumber = 6)]
        public double? CasePrice { get; set; }
    }
}
