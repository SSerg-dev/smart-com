namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductTree_Type : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductTree", "NodeTypeId", "dbo.NodeType");
            DropIndex("dbo.ProductTree", new[] { "NodeTypeId" });
            AddColumn("dbo.ProductTree", "Type", c => c.String());
            DropColumn("dbo.ProductTree", "NodeTypeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductTree", "NodeTypeId", c => c.Guid());
            DropColumn("dbo.ProductTree", "Type");
            CreateIndex("dbo.ProductTree", "NodeTypeId");
            AddForeignKey("dbo.ProductTree", "NodeTypeId", "dbo.NodeType", "Id");
        }
    }
}
