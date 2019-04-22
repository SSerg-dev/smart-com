namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoSupportPromo_AddCostProdFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoSupportPromo", "PlanCostProd", c => c.Double(nullable: false));
            AddColumn("dbo.PromoSupportPromo", "FactCostProd", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoSupportPromo", "FactCostProd");
            DropColumn("dbo.PromoSupportPromo", "PlanCostProd");
        }
    }
}
