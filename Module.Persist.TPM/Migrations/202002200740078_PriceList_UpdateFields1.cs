namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PriceList_UpdateFields1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PriceList", "Unique_PriceList");
            DropIndex("dbo.PriceList", new[] { "ClientTree_Id" });
            DropColumn("dbo.PriceList", "ClientTreeId");
            RenameColumn(table: "dbo.PriceList", name: "ClientTree_Id", newName: "ClientTreeId");
            AlterColumn("dbo.PriceList", "ClientTreeId", c => c.Int(nullable: false));
            AlterColumn("dbo.PriceList", "ClientTreeId", c => c.Int(nullable: false));
            CreateIndex("dbo.PriceList", new[] { "DeletedDate", "StartDate", "EndDate", "ClientTreeId", "ProductId" }, unique: true, name: "Unique_PriceList");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PriceList", "Unique_PriceList");
            AlterColumn("dbo.PriceList", "ClientTreeId", c => c.Int());
            AlterColumn("dbo.PriceList", "ClientTreeId", c => c.Guid(nullable: false));
            RenameColumn(table: "dbo.PriceList", name: "ClientTreeId", newName: "ClientTree_Id");
            AddColumn("dbo.PriceList", "ClientTreeId", c => c.Guid(nullable: false));
            CreateIndex("dbo.PriceList", "ClientTree_Id");
            CreateIndex("dbo.PriceList", new[] { "DeletedDate", "StartDate", "EndDate", "ClientTreeId", "ProductId" }, unique: true, name: "Unique_PriceList");
        }
    }
}
