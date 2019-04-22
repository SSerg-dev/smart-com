namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Promo_Fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "PlanPromoXSites", c => c.Int());
            AddColumn("dbo.Promo", "PlanPromoCatalogue", c => c.Double());
            AddColumn("dbo.Promo", "PlanPromoPOSMInClient", c => c.Double());
            AddColumn("dbo.Promo", "PlanPromoCostProdXSites", c => c.Int());
            AddColumn("dbo.Promo", "PlanPromoCostProdCatalogue", c => c.Double());
            AddColumn("dbo.Promo", "PlanPromoCostProdPOSMInClient", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoXSites", c => c.Int());
            AddColumn("dbo.Promo", "ActualPromoCatalogue", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoPOSMInClient", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoCostProdXSites", c => c.Int());
            AddColumn("dbo.Promo", "ActualPromoCostProdCatalogue", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoCostProdPOSMInClient", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "ActualPromoCostProdPOSMInClient");
            DropColumn("dbo.Promo", "ActualPromoCostProdCatalogue");
            DropColumn("dbo.Promo", "ActualPromoCostProdXSites");
            DropColumn("dbo.Promo", "ActualPromoPOSMInClient");
            DropColumn("dbo.Promo", "ActualPromoCatalogue");
            DropColumn("dbo.Promo", "ActualPromoXSites");
            DropColumn("dbo.Promo", "PlanPromoCostProdPOSMInClient");
            DropColumn("dbo.Promo", "PlanPromoCostProdCatalogue");
            DropColumn("dbo.Promo", "PlanPromoCostProdXSites");
            DropColumn("dbo.Promo", "PlanPromoPOSMInClient");
            DropColumn("dbo.Promo", "PlanPromoCatalogue");
            DropColumn("dbo.Promo", "PlanPromoXSites");
        }
    }
}
