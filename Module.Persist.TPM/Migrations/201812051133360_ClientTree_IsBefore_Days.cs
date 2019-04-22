namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientTree_IsBefore_Days : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientTree", "IsBeforeStart", c => c.Boolean());
            AddColumn("dbo.ClientTree", "DaysStart", c => c.Int());
            AddColumn("dbo.ClientTree", "IsBeforeEnd", c => c.Boolean());
            AddColumn("dbo.ClientTree", "DaysEnd", c => c.Int());
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
