namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_AddFactBudgetCosts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "FactShopperTi", c => c.Int());
            AddColumn("dbo.Promo", "FactMarketingTi", c => c.Int());
            AddColumn("dbo.Promo", "FactBranding", c => c.Int());
            AddColumn("dbo.Promo", "FactBTL", c => c.Int());
            AddColumn("dbo.Promo", "FactCostProduction", c => c.Int());
            AddColumn("dbo.Promo", "FactTotalCost", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "FactTotalCost");
            DropColumn("dbo.Promo", "FactCostProduction");
            DropColumn("dbo.Promo", "FactBTL");
            DropColumn("dbo.Promo", "FactBranding");
            DropColumn("dbo.Promo", "FactMarketingTi");
            DropColumn("dbo.Promo", "FactShopperTi");
        }
    }
}
