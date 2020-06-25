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
        [Display(Name = "Brand Seg Tech Sub")]
        public string BrandsegTechsubName { get; set; }

        [ImportCSVColumn(ColumnNumber = 3)]
        [Display(Name = "Year")]
        public int Year { get; set; }

        [ImportCSVColumn(ColumnNumber = 4)]
        [Display(Name = "Shopper TI Plan, %")]
        public double ShopperTiPlanPercent { get; set; }

        [ImportCSVColumn(ColumnNumber = 10)]
        [Display(Name = "Marketing TI Plan, %")] 
        public double MarketingTiPlanPercent { get; set; }

        [ImportCSVColumn(ColumnNumber = 20)]
        [Display(Name = "Production Plan, mln")]
        public double ProductionPlan { get; set; }

        [ImportCSVColumn(ColumnNumber = 26)]
        [Display(Name = "Branding Plan, mln")]
        public double BrandingPlan { get; set; }

        [ImportCSVColumn(ColumnNumber = 12)]
        [Display(Name = "Promo Ti Cost Plan, %")]
        public double PromoTiCostPlanPercent { get; set; }

        [ImportCSVColumn(ColumnNumber = 18)]
        [Display(Name = "Non Promo Ti Cost Plan, %")]
        public double NonPromoTiCostPlanPercent { get; set; }

        [ImportCSVColumn(ColumnNumber = 32)]
        [Display(Name = "BTL Plan, mln")]
        public double BTLPlan { get; set; }

        [ImportCSVColumn(ColumnNumber = 37)]
        [Display(Name = "ROI Plan, %")]
        public double ROIPlanPercent { get; set; }

        [ImportCSVColumn(ColumnNumber = 40)]
        [Display(Name = "LSV Plan")]
        public double PlanLSV { get; set; }

        [ImportCSVColumn(ColumnNumber = 43)]
        [Display(Name = "Incremental NSV Plan, mln")]
        public double IncrementalNSVPlan { get; set; }

        [ImportCSVColumn(ColumnNumber = 46)]
        [Display(Name = "Promo NSV Plan, mln")]
        public double PromoNSVPlan { get; set; }

        public int ClientTreeObjectId { get; set; }
    }
}
