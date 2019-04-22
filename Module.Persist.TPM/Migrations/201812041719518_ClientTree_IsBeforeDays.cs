namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientTree_IsBeforeDays : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientTree", "IsBeforeStart", c => c.Boolean(nullable: false));
            AddColumn("dbo.ClientTree", "DaysStart", c => c.Boolean(nullable: false));
            AddColumn("dbo.ClientTree", "IsBeforeEnd", c => c.Boolean(nullable: false));
            AddColumn("dbo.ClientTree", "DaysEnd", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClientTree", "DaysEnd");
            DropColumn("dbo.ClientTree", "IsBeforeEnd");
            DropColumn("dbo.ClientTree", "DaysStart");
            DropColumn("dbo.ClientTree", "IsBeforeStart");
        }
    }
}
