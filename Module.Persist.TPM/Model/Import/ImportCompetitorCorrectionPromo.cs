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
    public class ImportCompetitorCorrectionPromo : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "Number")]
        public int? Number { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [NavigationPropertyMap(LookupEntityType = typeof(Competitor), LookupPropertyName = "Name")]
        [Display(Name = "Competitor")]
        public string CompetitorName { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Promo name")]
        public String Name { get; set; }

        [ImportCSVColumn(ColumnNumber = 4)]
        [Display(Name = "Client hierarchy code")]
        public int ClientTreeObjectId { get; set; }    

        [ImportCSVColumn(ColumnNumber = 5)]
        [NavigationPropertyMap(LookupEntityType = typeof(CompetitorBrandTech), LookupPropertyName = "BrandTech")]
        [Display(Name = "BrandTech")]
        public String CompetitorBrandTechName { get; set; }

        [ImportCSVColumn(ColumnNumber = 6)]
        [Display(Name = "Start Date")]
        public DateTimeOffset? StartDate { get; set; }

        [ImportCSVColumn(ColumnNumber = 7)]
        [Display(Name = "End Date")]
        public DateTimeOffset? EndDate { get; set; }

        [ImportCSVColumn(ColumnNumber = 8)]
        [Display(Name = "MechanicType")]
        public string MechanicType { get; set; }

        [ImportCSVColumn(ColumnNumber = 9)]
        [Display(Name = "Discount")]
        public double? Discount { get; set; }

        [ImportCSVColumn(ColumnNumber = 10)]
        [Display(Name = "Shelf price")]
        public double? Price { get; set; }

        public Guid CompetitorId { get; set; }
        public virtual Competitor Competitor { get; set; }
        public Guid CompetitorBrandTechId { get; set; }
        public virtual CompetitorBrandTech CompetitorBrandTech { get; set; }
    }
}
