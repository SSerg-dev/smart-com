namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductIncidentChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductChangeIncident", "IsCreate", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProductChangeIncident", "IsDelete", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductChangeIncident", "IsDelete");
            DropColumn("dbo.ProductChangeIncident", "IsCreate");
        }
    }
}
