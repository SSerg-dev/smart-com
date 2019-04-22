namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoDemand_deleteBaseClientObjectId : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PromoDemand", "BaseClientObjectId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PromoDemand", "BaseClientObjectId", c => c.Guid(nullable: false));
        }
    }
}
