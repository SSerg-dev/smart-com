namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoProduct_AddFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoProduct", "PlanProductIncrementaLSV", c => c.Double());
            AddColumn("dbo.PromoProduct", "PlanProductLSV", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoProduct", "PlanProductLSV");
            DropColumn("dbo.PromoProduct", "PlanProductIncrementaLSV");
        }
    }
}
