namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveProductFromPromo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Promo", "ProductId", "dbo.Product");
            DropIndex("dbo.Promo", new[] { "ProductId" });
            DropColumn("dbo.Promo", "ProductId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Promo", "ProductId", c => c.Guid());
            CreateIndex("dbo.Promo", "ProductId");
            AddForeignKey("dbo.Promo", "ProductId", "dbo.Product", "Id");
        }
    }
}
