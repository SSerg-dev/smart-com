using Core.Import;
using Module.Persist.TPM.Model.TPM;
using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportPPE : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "Client hierarchy code")]
        public int ClientTreeObjectId { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [NavigationPropertyMap(LookupEntityType = typeof(BrandTech), LookupPropertyName = "Name")]
        [Display(Name = "Brand Tech")]
        public string BrandTechName { get; set; }

        [ImportCSVColumn(ColumnNumber = 3)]
        [Display(Name = "Size")]
        public string Size { get; set; }

        [ImportCSVColumn(ColumnNumber = 4)]
        [Display(Name = "Discount")]
        public double? Discount { get; set; }

        [ImportCSVColumn(ColumnNumber = 5)]
        [Display(Name = "Promo Duration")]
        public string PromoDuration { get; set; }

        [ImportCSVColumn(ColumnNumber = 6)]
        [Display(Name = "Plan Post Promo Effect W1")]
        public double? PlanPostPromoEffectW1 { get; set; }

        [ImportCSVColumn(ColumnNumber = 7)]
        [Display(Name = "Plan Post Promo Effect W2")]
        public double? PlanPostPromoEffectW2 { get; set; }

        public Guid BrandTechId { get; set; }
        public virtual BrandTech BrandTech { get; set; }
        public int ClientTreeId { get; set; }
    }
}
