namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_Add_Budgets : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "BTL", c => c.Int());
            AddColumn("dbo.Promo", "CostProduction", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "CostProduction");
            DropColumn("dbo.Promo", "BTL");
        }
    }
}
