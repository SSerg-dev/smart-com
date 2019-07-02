namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoProduct_PlanProductIncrementalLSV_ChangeName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoProduct", "PlanProductIncrementalLSV", c => c.Double());
            DropColumn("dbo.PromoProduct", "PlanProductIncrementaLSV");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PromoProduct", "PlanProductIncrementaLSV", c => c.Double());
            DropColumn("dbo.PromoProduct", "PlanProductIncrementalLSV");
        }
    }
}
