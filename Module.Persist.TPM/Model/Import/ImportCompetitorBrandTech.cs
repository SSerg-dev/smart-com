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
    public class ImportCompetitorBrandTech : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [NavigationPropertyMap(LookupEntityType = typeof(Competitor), LookupPropertyName = "Name")]
        [Display(Name = "Competitor")]
        public string CompetitorName { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "BrandTech")]
        public string BrandTech { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Color")]
        public string Color { get; set; }

        public Guid CompetitorId { get; set; }
        public virtual Competitor Competitor { get; set; }
    }
}
