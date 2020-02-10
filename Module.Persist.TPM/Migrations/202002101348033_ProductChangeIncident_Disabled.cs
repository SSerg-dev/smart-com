namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductChangeIncident_Disabled : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductChangeIncident", "Disabled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductChangeIncident", "Disabled");
        }
    }
}
