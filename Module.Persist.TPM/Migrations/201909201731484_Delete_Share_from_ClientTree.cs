namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Delete_Share_from_ClientTree : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ClientTree", "Share");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ClientTree", "Share", c => c.Short(nullable: false));
        }
    }
}
