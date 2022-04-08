using Core.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.DTO
{
    [Table("ClientDashboardView")]
    public class ClientDashboardView : IEntity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid? HistoryId { get; set; }
        public int ObjectId { get; set; }
        public string ClientHierarchy { get; set; }
        public Guid? BrandTechId { get; set; }
        public string BrandsegTechsubName { get; set; }
        public string LogoFileName { get; set; }
        public int? Year { get; set; }

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

        public double? PromoTiCostPlanPercent { get; set; }
        public double? PromoTiCostPlan { get; set; }
        public double? PromoTiCostYTD { get; set; }
        public double? PromoTiCostYTDPercent { get; set; }
        public double? PromoTiCostYEE { get; set; }
        public double? PromoTiCostYEEPercent { get; set; }

        public double? NonPromoTiCostPlanPercent { get; set; }
        public double? NonPromoTiCostPlan { get; set; }
        public double? NonPromoTiCostYTD { get; set; }
        public double? NonPromoTiCostYTDPercent { get; set; }
        public double? NonPromoTiCostYEE { get; set; }
        public double? NonPromoTiCostYEEPercent { get; set; }

        public int? PromoWeeks { get; set; }
        public double? TotalPromoIncrementalEarnings { get; set; }
        public double? ActualPromoCost { get; set; }
        public double? ActualPromoIncrementalEarnings { get; set; }
        public double? TotalPromoCost { get; set; }
        public double? VodYTD { get; set; }
        public double? VodYEE { get; set; }
        public double? ActualPromoLSV { get; set; }
        public double? PlanPromoLSV { get; set; }

        public ClientDashboardView Clone()
        {
            return new ClientDashboardView()
            {
                Id = this.Id,
                HistoryId = this.HistoryId,
                ObjectId = this.ObjectId,
                ClientHierarchy = this.ClientHierarchy,
                BrandTechId = this.BrandTechId,
                BrandsegTechsubName = this.BrandsegTechsubName,
                LogoFileName = this.LogoFileName,
                Year = this.Year,

                ShopperTiPlanPercent = this.ShopperTiPlanPercent,
                ShopperTiPlan = this.ShopperTiPlan,
                ShopperTiYTD = this.ShopperTiYTD,
                ShopperTiYTDPercent = this.ShopperTiYTDPercent,
                ShopperTiYEE = this.ShopperTiYEE,
                ShopperTiYEEPercent = this.ShopperTiYEEPercent,

                MarketingTiPlanPercent = this.MarketingTiPlanPercent,
                MarketingTiPlan = this.MarketingTiPlan,
                MarketingTiYTD = this.MarketingTiYTD,
                MarketingTiYTDPercent = this.MarketingTiYTDPercent,
                MarketingTiYEE = this.MarketingTiYEE,
                MarketingTiYEEPercent = this.MarketingTiYEEPercent,

                PromoTiCostPlan = this.PromoTiCostPlan,
                PromoTiCostPlanPercent = this.PromoTiCostPlanPercent,
                PromoTiCostYEE = this.PromoTiCostYEE,
                PromoTiCostYEEPercent = this.PromoTiCostYEEPercent,
                PromoTiCostYTD = this.PromoTiCostYTD,
                PromoTiCostYTDPercent = this.PromoTiCostYTDPercent,

                NonPromoTiCostPlan = this.NonPromoTiCostPlan,
                NonPromoTiCostPlanPercent = this.NonPromoTiCostPlanPercent,
                NonPromoTiCostYEE = this.NonPromoTiCostYEE,
                NonPromoTiCostYEEPercent = this.NonPromoTiCostYEEPercent,
                NonPromoTiCostYTD = this.NonPromoTiCostYTD,
                NonPromoTiCostYTDPercent = this.NonPromoTiCostYTDPercent,

                ProductionPlanPercent = this.ProductionPlanPercent,
                ProductionPlan = this.ProductionPlan,
                ProductionYTD = this.ProductionYTD,
                ProductionYTDPercent = this.ProductionYTDPercent,
                ProductionYEE = this.ProductionYEE,
                ProductionYEEPercent = this.ProductionYEEPercent,

                BrandingPlanPercent = this.BrandingPlanPercent,
                BrandingPlan = this.BrandingPlan,
                BrandingYTD = this.BrandingYTD,
                BrandingYTDPercent = this.BrandingYTDPercent,
                BrandingYEE = this.BrandingYEE,
                BrandingYEEPercent = this.BrandingYEEPercent,

                BTLPlanPercent = this.BTLPlanPercent,
                BTLPlan = this.BTLPlan,
                BTLYTD = this.BTLYTD,
                BTLYTDPercent = this.BTLYTDPercent,
                BTLYEE = this.BTLYEE,
                BTLYEEPercent = this.BTLYEEPercent,

                ROIPlanPercent = this.ROIPlanPercent,
                ROIYTDPercent = this.ROIYTDPercent,
                ROIYEEPercent = this.ROIYEEPercent,

                LSVPlan = this.LSVPlan,
                LSVYTD = this.LSVYTD,
                LSVYEE = this.LSVYEE,

                IncrementalNSVPlan = this.IncrementalNSVPlan,
                IncrementalNSVYTD = this.IncrementalNSVYTD,
                IncrementalNSVYEE = this.IncrementalNSVYEE,

                PromoNSVPlan = this.PromoNSVPlan,
                PromoNSVYTD = this.PromoNSVYTD,
                PromoNSVYEE = this.PromoNSVYEE,

                PromoWeeks = this.PromoWeeks,
                VodYTD = this.VodYTD,
                VodYEE = this.VodYEE,

                ActualPromoCost = this.ActualPromoCost,
                ActualPromoIncrementalEarnings = this.ActualPromoIncrementalEarnings,
                ActualPromoLSV = this.ActualPromoLSV,
                PlanPromoLSV = this.PlanPromoLSV,
                TotalPromoCost = this.TotalPromoCost,
                TotalPromoIncrementalEarnings=this.TotalPromoIncrementalEarnings
            };
        }
    }
}
