namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BaseLine_Add_Product_And_ProductId_Fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BaseLine", "ProductId", c => c.Guid(nullable: false));
            CreateIndex("dbo.BaseLine", "ProductId");
            AddForeignKey("dbo.BaseLine", "ProductId", "dbo.Product", "Id");
            DropColumn("dbo.BaseLine", "ZREP");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BaseLine", "ZREP", c => c.String(nullable: false, maxLength: 255));
            DropForeignKey("dbo.BaseLine", "ProductId", "dbo.Product");
            DropIndex("dbo.BaseLine", new[] { "ProductId" });
            DropColumn("dbo.BaseLine", "ProductId");
        }
    }
}
