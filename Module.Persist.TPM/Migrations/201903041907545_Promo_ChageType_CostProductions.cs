namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_ChageType_CostProductions : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Promo", "CostProduction", c => c.Double());
            AlterColumn("dbo.Promo", "FactCostProduction", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Promo", "FactCostProduction", c => c.Int());
            AlterColumn("dbo.Promo", "CostProduction", c => c.Int());
        }
    }
}
