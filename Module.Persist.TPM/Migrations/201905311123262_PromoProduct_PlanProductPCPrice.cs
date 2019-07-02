namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoProduct_PlanProductPCPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoProduct", "PlanProductPCPrice", c => c.Double());
            DropColumn("dbo.PromoProduct", "ProductBaselinePCPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PromoProduct", "ProductBaselinePCPrice", c => c.Double());
            DropColumn("dbo.PromoProduct", "PlanProductPCPrice");
        }
    }
}
