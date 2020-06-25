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
    public class ImportRollingVolume : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [NavigationPropertyMap(LookupEntityType = typeof(Product), LookupPropertyName = "ZREP")]
        [Display(Name = "ZREP")]
        public String ZREP { get; set; }

        [ImportCSVColumn(ColumnNumber = 3)]
        [Display(Name = "DemandGroup")]
        public String DemandGroup { get; set; }

        [ImportCSVColumn(ColumnNumber = 4)]
        [Display(Name = "Week")]
        public string Week { get; set; }


        [ImportCSVColumn(ColumnNumber = 16)]
        [Display(Name = "ManualRollingTotalVolumes")]
        public double ManualRollingTotalVolumes { get; set; }
    }
}
