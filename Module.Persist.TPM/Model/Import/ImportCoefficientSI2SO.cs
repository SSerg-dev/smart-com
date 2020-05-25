using Core.Import;
using Module.Persist.TPM.Model.TPM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportCoefficientSI2SO : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "Demand Code")]
        public string DemandCode { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [NavigationPropertyMap(LookupEntityType = typeof(BrandTech), LookupPropertyName = "BrandTech_code")]
        [Display(Name = "Brand Tech Code")]
        public String BrandTechBrandTech_code { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [NavigationPropertyMap(LookupEntityType = typeof(BrandTech), LookupPropertyName = "Name")]
        [Display(Name = "Brand Tech Name")]
        public String BrandTechName { get; set; }

        [ImportCSVColumn(ColumnNumber = 3)]
        [Display(Name = "Coefficient Value")]
        public double? CoefficientValue { get; set; }
        
        [ImportCSVColumn(ColumnNumber = 4)]
        [Display(Name = "Lock")]
        public bool? Lock { get; set; }
    }
}
