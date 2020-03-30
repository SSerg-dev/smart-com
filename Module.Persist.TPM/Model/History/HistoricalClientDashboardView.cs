using Core.History;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using System;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(ClientDashboardView))]
    public class HistoricalClientDashboardView : BaseHistoricalEntity<Guid>
    {
        public int ObjectId { get; set; }
        public string ClientHierarchy { get; set; }
        public string ClientName { get; set; }
        public Guid? BrandTechId { get; set; }
        public string BrandTechName { get; set; }
        public string LogoFileName { get; set; }
        public int Year { get; set; }

        public double? ShopperTiPlanPercent { get; set; }
        public double? ShopperTiPlan { get; set; }
        public double? ShopperTiYTD { get; set; }
        public double? ShopperTiYTDPercent { get; set; }
        public double? ShopperTiYEE { get; set; }
        public double? ShopperTiYEEPercent { get; set; }

        public double? MarketingTiPlanPercent { get; set; }
        public double? MarketingTiPlan { get; set; }
        public double? MarketingTiYTD { get; set; }
        public double? MarketingTiYTDPercent { get; set; }
        public double? MarketingTiYEE { get; set; }
        public double? MarketingTiYEEPercent { get; set; }

        public double? ProductionPlanPercent { get; set; }
        public double? ProductionPlan { get; set; }
        public double? ProductionYTD { get; set; }
        public double? ProductionYTDPercent { get; set; }
        public double? ProductionYEE { get; set; }
        public double? ProductionYEEPercent { get; set; }

        public double? BrandingPlanPercent { get; set; }
        public double? BrandingPlan { get; set; }
        public double? BrandingYTD { get; set; }
        public double? BrandingYTDPercent { get; set; }
        public double? BrandingYEE { get; set; }
        public double? BrandingYEEPercent { get; set; }

        public double? BTLPlanPercent { get; set; }
        public double? BTLPlan { get; set; }
        public double? BTLYTD { get; set; }
        public double? BTLYTDPercent { get; set; }
        public double? BTLYEE { get; set; }
        public double? BTLYEEPercent { get; set; }

        public double? ROIPlanPercent { get; set; }
        public double? ROIYTDPercent { get; set; }
        public double? ROIYEEPercent { get; set; }

        public double? LSVPlan { get; set; }
        public double? LSVYTD { get; set; }
        public double? LSVYEE { get; set; }

        public double? IncrementalNSVPlan { get; set; }
        public double? IncrementalNSVYTD { get; set; }
        public double? IncrementalNSVYEE { get; set; }

        public double? PromoNSVPlan { get; set; }
        public double? PromoNSVYTD { get; set; }
        public double? PromoNSVYEE { get; set; }

        public int? PromoWeeks { get; set; }
        public double? VodYTD { get; set; }
        public double? VodYEE { get; set; }
        public double? ActualPromoLSV { get; set; }
        public double? PlanPromoLSV { get; set; }
    }
}