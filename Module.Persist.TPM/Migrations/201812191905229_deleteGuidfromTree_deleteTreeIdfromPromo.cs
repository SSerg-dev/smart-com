namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteGuidfromTree_deleteTreeIdfromPromo : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ClientTree", "CX_ObjDate");
            DropIndex("dbo.ProductTree", "CX_ObjDate");
            DropColumn("dbo.Promo", "ClientTreeId");
            DropColumn("dbo.Promo", "ProductTreeId");
            DropColumn("dbo.Promo", "BaseClientTreeId");
            DropColumn("dbo.ClientTree", "ObjectId");
            DropColumn("dbo.ClientTree", "parentId");
            DropColumn("dbo.ProductTree", "ObjectId");
            DropColumn("dbo.ProductTree", "parentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductTree", "parentId", c => c.Guid(nullable: false));
            AddColumn("dbo.ProductTree", "ObjectId", c => c.Guid(nullable: false));
            AddColumn("dbo.ClientTree", "parentId", c => c.Guid(nullable: false));
            AddColumn("dbo.ClientTree", "ObjectId", c => c.Guid(nullable: false));
            AddColumn("dbo.Promo", "BaseClientTreeId", c => c.Guid());
            AddColumn("dbo.Promo", "ProductTreeId", c => c.Guid());
            AddColumn("dbo.Promo", "ClientTreeId", c => c.Guid());
            CreateIndex("dbo.ProductTree", new[] { "ObjectId", "EndDate" }, unique: true, name: "CX_ObjDate");
            CreateIndex("dbo.ClientTree", new[] { "ObjectId", "EndDate" }, unique: true, name: "CX_ObjDate");
        }
    }
}
