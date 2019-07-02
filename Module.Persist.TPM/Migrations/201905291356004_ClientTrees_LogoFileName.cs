namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientTrees_LogoFileName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientTree", "LogoFileName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClientTree", "LogoFileName");
        }
    }
}
