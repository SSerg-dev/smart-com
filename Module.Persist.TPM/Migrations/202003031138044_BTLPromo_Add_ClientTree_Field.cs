namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BTLPromo_Add_ClientTree_Field : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BTLPromo", "ClientTreeId", c => c.Int(nullable: false));
            CreateIndex("dbo.BTLPromo", "ClientTreeId");
            AddForeignKey("dbo.BTLPromo", "ClientTreeId", "dbo.ClientTree", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BTLPromo", "ClientTreeId", "dbo.ClientTree");
            DropIndex("dbo.BTLPromo", new[] { "ClientTreeId" });
            DropColumn("dbo.BTLPromo", "ClientTreeId");
        }
    }
}
