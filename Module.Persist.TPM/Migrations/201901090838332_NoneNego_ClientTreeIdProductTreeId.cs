namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoneNego_ClientTreeIdProductTreeId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientTree", "FullPathName", c => c.String());
            AddColumn("dbo.ProductTree", "FullPathName", c => c.String());
            AddColumn("dbo.NoneNego", "ClientTreeId", c => c.Int(nullable: false));
            AddColumn("dbo.NoneNego", "ProductTreeId", c => c.Int(nullable: false));
            CreateIndex("dbo.NoneNego", "ClientTreeId");
            CreateIndex("dbo.NoneNego", "ProductTreeId");
            AddForeignKey("dbo.NoneNego", "ClientTreeId", "dbo.ClientTree", "Id");
            AddForeignKey("dbo.NoneNego", "ProductTreeId", "dbo.ProductTree", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NoneNego", "ProductTreeId", "dbo.ProductTree");
            DropForeignKey("dbo.NoneNego", "ClientTreeId", "dbo.ClientTree");
            DropIndex("dbo.NoneNego", new[] { "ProductTreeId" });
            DropIndex("dbo.NoneNego", new[] { "ClientTreeId" });
            DropColumn("dbo.NoneNego", "ProductTreeId");
            DropColumn("dbo.NoneNego", "ClientTreeId");
            DropColumn("dbo.ProductTree", "FullPathName");
            DropColumn("dbo.ClientTree", "FullPathName");
        }
    }
}
