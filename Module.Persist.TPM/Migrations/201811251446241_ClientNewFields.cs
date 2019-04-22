namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientNewFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientTree", "Share", c => c.Short(nullable: false, defaultValue: 0));
            AddColumn("dbo.ClientTree", "ExecutionCode", c => c.String(maxLength: 255));
            AddColumn("dbo.ClientTree", "DemandCode", c => c.String(maxLength: 255));
            AddColumn("dbo.ClientTree", "IsBaseClient", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClientTree", "IsBaseClient");
            DropColumn("dbo.ClientTree", "DemandCode");
            DropColumn("dbo.ClientTree", "ExecutionCode");
            DropColumn("dbo.ClientTree", "Share");
        }
    }
}
