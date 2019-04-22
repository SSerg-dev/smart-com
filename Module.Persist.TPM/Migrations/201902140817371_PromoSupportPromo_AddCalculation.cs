namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoSupportPromo_AddCalculation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoSupportPromo", "Calculation", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoSupportPromo", "Calculation");
        }
    }
}
