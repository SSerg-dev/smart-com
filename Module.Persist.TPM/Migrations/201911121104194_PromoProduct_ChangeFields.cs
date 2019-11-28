namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoProduct_ChangeFields : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.PromoProduct", "ActualProductUplift", "ActualProductUpliftPercent");
            DropColumn("dbo.PromoProduct", "PlanProductUplift");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PromoProduct", "PlanProductUplift", c => c.Double());
			RenameColumn("dbo.PromoProduct", "ActualProductUpliftPercent", "ActualProductUplift");
        }
    }
}
