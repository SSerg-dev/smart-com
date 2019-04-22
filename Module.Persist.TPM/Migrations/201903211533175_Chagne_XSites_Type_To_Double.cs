namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Chagne_XSites_Type_To_Double : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Promo", "PlanPromoXSites", c => c.Double());
            AlterColumn("dbo.Promo", "PlanPromoCostProdXSites", c => c.Double());
            AlterColumn("dbo.Promo", "ActualPromoXSites", c => c.Double());
            AlterColumn("dbo.Promo", "ActualPromoCostProdXSites", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Promo", "ActualPromoCostProdXSites", c => c.Int());
            AlterColumn("dbo.Promo", "ActualPromoXSites", c => c.Int());
            AlterColumn("dbo.Promo", "PlanPromoCostProdXSites", c => c.Int());
            AlterColumn("dbo.Promo", "PlanPromoXSites", c => c.Int());
        }
    }
}
