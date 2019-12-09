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
    public class ImportMechanic : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "SystemName")]
        [Required]
        public string SystemName { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [NavigationPropertyMap(LookupEntityType = typeof(PromoTypes), LookupPropertyName = "Name")]
        [Display(Name = "PromoTypes.Name")]
        [Required]
        public String PromoTypeName { get; set; }
        public System.Guid PromoTypesId { get; set; } 
        public virtual PromoTypes PromoTypes { get; set; }
    }
}
