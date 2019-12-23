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
    public class ImportBrandTech : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [NavigationPropertyMap(LookupEntityType = typeof(Brand), LookupPropertyName = "Name")]
        [Display(Name = "Brand")]
        public String BrandName { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [NavigationPropertyMap(LookupEntityType = typeof(Technology), LookupPropertyName = "Name")]
        [Display(Name = "Technology")]
        public String TechnologyName { get; set; }
        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Brand Tech Code")]
        public String BrandTech_code { get; set; }

      


    }
}
