namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_Calculation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "ShopperTi", c => c.Int());
            AddColumn("dbo.Promo", "MarketingTi", c => c.Int());
            AddColumn("dbo.Promo", "Branding", c => c.Int());
            AddColumn("dbo.Promo", "TotalCost", c => c.Int());
            AddColumn("dbo.Promo", "PlanIncrementalLsv", c => c.Int());
            AddColumn("dbo.Promo", "PlanTotalPromoLsv", c => c.Int());
            AddColumn("dbo.Promo", "PlanPostPromoEffect", c => c.Int());
            AddColumn("dbo.Promo", "PlanRoi", c => c.Int());
            AddColumn("dbo.Promo", "PlanIncrementalNsv", c => c.Int());
            AddColumn("dbo.Promo", "PlanTotalPromoNsv", c => c.Int());
            AddColumn("dbo.Promo", "PlanIncrementalMac", c => c.Int());
            AlterColumn("dbo.Promo", "OtherEventName", c => c.String(maxLength: 255));
            DropColumn("dbo.Promo", "RoiPlan");
            DropColumn("dbo.Promo", "RoiFact");
            DropColumn("dbo.Promo", "PlanBaseline");
            DropColumn("dbo.Promo", "PlanDuration");
            DropColumn("dbo.Promo", "PlanIncremental");
            DropColumn("dbo.Promo", "PlanActivity");
            DropColumn("dbo.Promo", "PlanSteal");
            DropColumn("dbo.Promo", "FactBaseline");
            DropColumn("dbo.Promo", "FactDuration");
            DropColumn("dbo.Promo", "FactUplift");
            DropColumn("dbo.Promo", "FactIncremental");
            DropColumn("dbo.Promo", "FactActivity");
            DropColumn("dbo.Promo", "FactSteal");
            DropColumn("dbo.Promo", "ProductFilter");
            DropColumn("dbo.Promo", "ProductFilterDisplay");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Promo", "ProductFilterDisplay", c => c.String());
            AddColumn("dbo.Promo", "ProductFilter", c => c.String());
            AddColumn("dbo.Promo", "FactSteal", c => c.Int());
            AddColumn("dbo.Promo", "FactActivity", c => c.Int());
            AddColumn("dbo.Promo", "FactIncremental", c => c.Int());
            AddColumn("dbo.Promo", "FactUplift", c => c.Int());
            AddColumn("dbo.Promo", "FactDuration", c => c.Int());
            AddColumn("dbo.Promo", "FactBaseline", c => c.Int());
            AddColumn("dbo.Promo", "PlanSteal", c => c.Int());
            AddColumn("dbo.Promo", "PlanActivity", c => c.Int());
            AddColumn("dbo.Promo", "PlanIncremental", c => c.Int());
            AddColumn("dbo.Promo", "PlanDuration", c => c.Int());
            AddColumn("dbo.Promo", "PlanBaseline", c => c.Int());
            AddColumn("dbo.Promo", "RoiFact", c => c.Int());
            AddColumn("dbo.Promo", "RoiPlan", c => c.Int());
            AlterColumn("dbo.Promo", "OtherEventName", c => c.String());
            DropColumn("dbo.Promo", "PlanIncrementalMac");
            DropColumn("dbo.Promo", "PlanTotalPromoNsv");
            DropColumn("dbo.Promo", "PlanIncrementalNsv");
            DropColumn("dbo.Promo", "PlanRoi");
            DropColumn("dbo.Promo", "PlanPostPromoEffect");
            DropColumn("dbo.Promo", "PlanTotalPromoLsv");
            DropColumn("dbo.Promo", "PlanIncrementalLsv");
            DropColumn("dbo.Promo", "TotalCost");
            DropColumn("dbo.Promo", "Branding");
            DropColumn("dbo.Promo", "MarketingTi");
            DropColumn("dbo.Promo", "ShopperTi");
        }
    }
}
