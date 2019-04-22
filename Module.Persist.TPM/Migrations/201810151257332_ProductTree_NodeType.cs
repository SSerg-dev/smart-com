namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductTree_NodeType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductTree", "NodeTypeId", c => c.Guid());
            CreateIndex("dbo.ProductTree", "NodeTypeId");
            AddForeignKey("dbo.ProductTree", "NodeTypeId", "dbo.NodeType", "Id");
            DropColumn("dbo.ProductTree", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductTree", "Type", c => c.String());
            DropForeignKey("dbo.ProductTree", "NodeTypeId", "dbo.NodeType");
            DropIndex("dbo.ProductTree", new[] { "NodeTypeId" });
            DropColumn("dbo.ProductTree", "NodeTypeId");
        }
    }
}
