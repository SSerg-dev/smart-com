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
    public class ImportCompetitorPromo : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [NavigationPropertyMap(LookupEntityType = typeof(Competitor), LookupPropertyName = "Name")]
        [Display(Name = "Competitor")]
        public string CompetitorName { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "Promo name")]
        public String Name { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Client hierarchy code")]
        public int ClientTreeObjectId { get; set; }

        [ImportCSVColumn(ColumnNumber = 3)]
        [NavigationPropertyMap(LookupEntityType = typeof(CompetitorBrandTech), LookupPropertyName = "BrandTech")]
        [Display(Name = "BrandTech")]
        public String CompetitorBrandTechName { get; set; }

        [ImportCSVColumn(ColumnNumber = 4)]
        [Display(Name = "StartDate")]
        public DateTimeOffset? StartDate { get; set; }

        [ImportCSVColumn(ColumnNumber = 5)]
        [Display(Name = "EndDate")]
        public DateTimeOffset? EndDate { get; set; }

        [ImportCSVColumn(ColumnNumber = 6)]
        [Display(Name = "Discount")]
        public float Discount { get; set; }

        [ImportCSVColumn(ColumnNumber = 7)]
        [Display(Name = "Shelf price")]
        public int Price { get; set; }

        [ImportCSVColumn(ColumnNumber = 8)]
        [Display(Name = "Subranges")]
        public String Subrange { get; set; }

        [ImportCSVColumn(ColumnNumber = 9)]
        [NavigationPropertyMap(LookupEntityType = typeof(PromoStatus), LookupPropertyName = "Name")]
        [Display(Name = "PromoStatus")]
        public String PromoStatusName { get; set; }

        public Guid CompetitorId { get; set; }
        public virtual Competitor Competitor { get; set; }
        public Guid CompetitorBrandTechId { get; set; }
        public virtual CompetitorBrandTech CompetitorBrandTech { get; set; }
        public Guid PromoStatusId { get; set; }
        public virtual PromoStatus PromoStatus { get; set; }
    }
}
