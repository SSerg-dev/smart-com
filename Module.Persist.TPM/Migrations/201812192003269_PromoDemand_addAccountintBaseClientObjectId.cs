namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoDemand_addAccountintBaseClientObjectId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoDemand", "BaseClientObjectId", c => c.Int(nullable: false));
            AddColumn("dbo.PromoDemand", "Account", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoDemand", "Account");
            DropColumn("dbo.PromoDemand", "BaseClientObjectId");
        }
    }
}
