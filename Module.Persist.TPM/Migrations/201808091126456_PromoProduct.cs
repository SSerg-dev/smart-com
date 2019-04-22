namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoProduct : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PromoProduct",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PromoId = c.Guid(nullable: false),
                        ProductId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.ProductId)
                .ForeignKey("dbo.Promo", t => t.PromoId)
                .Index(t => t.PromoId)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PromoProduct", "PromoId", "dbo.Promo");
            DropForeignKey("dbo.PromoProduct", "ProductId", "dbo.Product");
            DropIndex("dbo.PromoProduct", new[] { "ProductId" });
            DropIndex("dbo.PromoProduct", new[] { "PromoId" });
            DropTable("dbo.PromoProduct");
        }
    }
}
