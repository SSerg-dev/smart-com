namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoDemand_AddLinkTo_BaseClient : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoDemand", "BaseClientObjectId", c => c.Guid(nullable: false));
            DropColumn("dbo.PromoDemand", "Account");
            AddColumn("dbo.PromoDemand", "Account", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoDemand", "Account");
            AddColumn("dbo.PromoDemand", "Account", c => c.String(nullable: false, maxLength: 255));
            DropColumn("dbo.PromoDemand", "BaseClientObjectId");
        }
    }
}
