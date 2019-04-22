namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_AddFieldsForCalculation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "Calculating", c => c.Boolean());
            AddColumn("dbo.Promo", "BlockInformation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "BlockInformation");
            DropColumn("dbo.Promo", "Calculating");
        }
    }
}
