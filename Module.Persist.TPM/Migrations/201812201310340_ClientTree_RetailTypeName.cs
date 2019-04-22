namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientTree_RetailTypeName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientTree", "RetailTypeName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClientTree", "RetailTypeName");
        }
    }
}
