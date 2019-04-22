namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoneNego_HierarchyFields : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.NoneNego", "ProductId", "dbo.Product");
            DropIndex("dbo.NoneNego", new[] { "ProductId" });
            AddColumn("dbo.NoneNego", "ClientHierarchy", c => c.String());
            AddColumn("dbo.NoneNego", "ProductHierarchy", c => c.String());
            DropColumn("dbo.NoneNego", "ProductId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NoneNego", "ProductId", c => c.Guid());
            DropColumn("dbo.NoneNego", "ProductHierarchy");
            DropColumn("dbo.NoneNego", "ClientHierarchy");
            CreateIndex("dbo.NoneNego", "ProductId");
            AddForeignKey("dbo.NoneNego", "ProductId", "dbo.Product", "Id");
        }
    }
}
