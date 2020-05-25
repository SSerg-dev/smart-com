namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PriceList_UpdateFields : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PriceList", "Unique_PriceList");
            AddColumn("dbo.PriceList", "ClientTreeId", c => c.Guid(nullable: false));
            AddColumn("dbo.PriceList", "ClientTree_Id", c => c.Int());
            CreateIndex("dbo.PriceList", new[] { "DeletedDate", "StartDate", "EndDate", "ClientTreeId", "ProductId" }, unique: true, name: "Unique_PriceList");
            CreateIndex("dbo.PriceList", "ClientTree_Id");
            AddForeignKey("dbo.PriceList", "ClientTree_Id", "dbo.ClientTree", "Id");
            DropColumn("dbo.PriceList", "GHierarchyCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PriceList", "GHierarchyCode", c => c.String(maxLength: 255));
            DropForeignKey("dbo.PriceList", "ClientTree_Id", "dbo.ClientTree");
            DropIndex("dbo.PriceList", new[] { "ClientTree_Id" });
            DropIndex("dbo.PriceList", "Unique_PriceList");
            DropColumn("dbo.PriceList", "ClientTree_Id");
            DropColumn("dbo.PriceList", "ClientTreeId");
            CreateIndex("dbo.PriceList", new[] { "DeletedDate", "GHierarchyCode", "StartDate", "EndDate", "ProductId" }, unique: true, name: "Unique_PriceList");
        }
    }
}
