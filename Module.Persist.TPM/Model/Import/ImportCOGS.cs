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
    public class ImportCOGS : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "StartDate")]
        public DateTimeOffset? StartDate { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "EndDate")]
        public DateTimeOffset? EndDate { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Client hierarchy")]
        public String ClientTreeFullPath { get; set; }

        [ImportCSVColumn(ColumnNumber = 3)]
        [Display(Name = "Client hierarchy code")]
        public int ClientTreeObjectId { get; set; }

        [ImportCSVColumn(ColumnNumber = 4)]
        [NavigationPropertyMap(LookupEntityType = typeof(BrandTech), LookupPropertyName = "Name")]
        [Display(Name = "BrandTech")]
        public String BrandTechName { get; set; }

        [ImportCSVColumn(ColumnNumber = 5)]
        [Display(Name = "LVS percent")]
        public float LVSpercent { get; set; }


        public int ClientTreeId { get; set; }
        public System.Guid? BrandTechId { get; set; }
        public virtual BrandTech BrandTech { get; set; }
    }
}
