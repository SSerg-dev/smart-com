namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsBeforeIsDaysTree : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientTree", "IsDaysStart", c => c.Boolean());
            AddColumn("dbo.ClientTree", "IsDaysEnd", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClientTree", "IsDaysEnd");
            DropColumn("dbo.ClientTree", "IsDaysStart");
        }
    }
}
