namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NonPromo_DeleteUselessFields : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.NonPromoSupport", "PlanProdCost");
            DropColumn("dbo.NonPromoSupport", "ActualProdCost");
            DropColumn("dbo.NonPromoSupport", "PONumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NonPromoSupport", "PONumber", c => c.String());
            AddColumn("dbo.NonPromoSupport", "ActualProdCost", c => c.Double());
            AddColumn("dbo.NonPromoSupport", "PlanProdCost", c => c.Double());
        }
    }
}
