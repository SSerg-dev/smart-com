namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoProduct_PlanProductCaseLSV : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoProduct", "PlanProductCaseLSV", c => c.Double());
            DropColumn("dbo.PromoProduct", "PlanProductLSV");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PromoProduct", "PlanProductLSV", c => c.Double());
            DropColumn("dbo.PromoProduct", "PlanProductCaseLSV");
        }
    }
}
