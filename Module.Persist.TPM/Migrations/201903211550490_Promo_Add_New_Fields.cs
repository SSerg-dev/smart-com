namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_Add_New_Fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "PlanPromoBaselineBaseTI", c => c.Double());
            AddColumn("dbo.Promo", "PlanPromoBaseTI", c => c.Double());
            AddColumn("dbo.Promo", "PlanPromoNetNSV", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoBaselineBaseTI", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoBaseTI", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoNetNSV", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "ActualPromoNetNSV");
            DropColumn("dbo.Promo", "ActualPromoBaseTI");
            DropColumn("dbo.Promo", "ActualPromoBaselineBaseTI");
            DropColumn("dbo.Promo", "PlanPromoNetNSV");
            DropColumn("dbo.Promo", "PlanPromoBaseTI");
            DropColumn("dbo.Promo", "PlanPromoBaselineBaseTI");
        }
    }
}
