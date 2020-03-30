using System.ComponentModel.DataAnnotations;

using Core.Import;
using Module.Persist.TPM.Model.TPM;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportClientDashboard : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "Client ID")]
        public int ClientTreeId { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Brand Tech")]
        public string BrandTechName { get; set; }

        [ImportCSVColumn(ColumnNumber = 3)]
        [Display(Name = "Year")]
        public int Year { get; set; }

        [ImportCSVColumn(ColumnNumber = 4)]
        [Display(Name = "Shopper TI Plan, %")]
        public double ShopperTiPlanPercent { get; set; }

        [ImportCSVColumn(ColumnNumber = 10)]
        [Display(Name = "Marketing TI Plan, %")] 
        public double MarketingTiPlanPercent { get; set; }

        [ImportCSVColumn(ColumnNumber = 17)]
        [Display(Name = "Production Plan, mln")]
        public double ProductionPlan { get; set; }

        [ImportCSVColumn(ColumnNumber = 23)]
        [Display(Name = "Branding Plan, mln")]
        public double BrandingPlan { get; set; }

        [ImportCSVColumn(ColumnNumber = 29)]
        [Display(Name = "BTL Plan, mln")]
        public double BTLPlan { get; set; }

        [ImportCSVColumn(ColumnNumber = 34)]
        [Display(Name = "ROI Plan, %")]
        public double ROIPlanPercent { get; set; }

        [ImportCSVColumn(ColumnNumber = 40)]
        [Display(Name = "Incremental NSV Plan, mln")]
        public double IncrementalNSVPlan { get; set; }

        [ImportCSVColumn(ColumnNumber = 43)]
        [Display(Name = "Promo NSV Plan, mln")]
        public double PromoNSVPlan { get; set; }

        public int ClientTreeObjectId { get; set; }
    }
}
