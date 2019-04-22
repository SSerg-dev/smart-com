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
    public class ImportColor : BaseImportEntity
    {

        [ImportCSVColumn(ColumnNumber = 0)]
        [NavigationPropertyMap(LookupEntityType = typeof(BrandTech), LookupPropertyName = "Name")]
        [Display(Name = "BrandTech.Name")]
        public string BrandTechName { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "SystemName")]
        public string SystemName { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "DisplayName")]
        public string DisplayName { get; set; }
    }
}
