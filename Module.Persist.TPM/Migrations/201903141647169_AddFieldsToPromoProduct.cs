namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldsToPromoProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoProduct", "PlanProductBaselineLSV", c => c.Double());
            AddColumn("dbo.PromoProduct", "PlanProductBaselineQty", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoProduct", "PlanProductBaselineQty");
            DropColumn("dbo.PromoProduct", "PlanProductBaselineLSV");
        }
    }
}
