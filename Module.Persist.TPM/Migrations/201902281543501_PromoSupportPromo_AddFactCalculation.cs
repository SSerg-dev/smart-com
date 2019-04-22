namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoSupportPromo_AddFactCalculation : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.PromoSupportPromo", "Calculation", "PlanCalculation");
            AddColumn("dbo.PromoSupportPromo", "FactCalculation", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            RenameColumn("dbo.PromoSupportPromo", "PlanCalculation", "Calculation");
            DropColumn("dbo.PromoSupportPromo", "FactCalculation");
        }
    }
}
