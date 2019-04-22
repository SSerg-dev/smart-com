namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addInttoTree_addInttoPromo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "ClientTreeId", c => c.Int());
            AddColumn("dbo.Promo", "ProductTreeId", c => c.Int());
            AddColumn("dbo.Promo", "BaseClientTreeId", c => c.Int());
            AddColumn("dbo.ClientTree", "ObjectId", c => c.Int(nullable: false));
            AddColumn("dbo.ClientTree", "parentId", c => c.Int(nullable: false));
            AddColumn("dbo.ProductTree", "ObjectId", c => c.Int(nullable: false));
            AddColumn("dbo.ProductTree", "parentId", c => c.Int(nullable: false));
            CreateIndex("dbo.ClientTree", new[] { "ObjectId", "EndDate" }, unique: true, name: "CX_ObjDate");
            CreateIndex("dbo.ProductTree", new[] { "ObjectId", "EndDate" }, unique: true, name: "CX_ObjDate");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ProductTree", "CX_ObjDate");
            DropIndex("dbo.ClientTree", "CX_ObjDate");
            DropColumn("dbo.ProductTree", "parentId");
            DropColumn("dbo.ProductTree", "ObjectId");
            DropColumn("dbo.ClientTree", "parentId");
            DropColumn("dbo.ClientTree", "ObjectId");
            DropColumn("dbo.Promo", "BaseClientTreeId");
            DropColumn("dbo.Promo", "ProductTreeId");
            DropColumn("dbo.Promo", "ClientTreeId");
        }
    }
}
