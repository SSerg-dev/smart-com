namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Delete_Old_PromoProduct : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PromoProduct", "ProductId", "dbo.Product");
            DropForeignKey("dbo.PromoProduct", "PromoId", "dbo.Promo");
            DropIndex("dbo.PromoProduct", new[] { "PromoId" });
            DropIndex("dbo.PromoProduct", new[] { "ProductId" });
            DropTable("dbo.PromoProduct");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PromoProduct",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PromoId = c.Guid(nullable: false),
                        ProductId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.PromoProduct", "ProductId");
            CreateIndex("dbo.PromoProduct", "PromoId");
            AddForeignKey("dbo.PromoProduct", "PromoId", "dbo.Promo", "Id");
            AddForeignKey("dbo.PromoProduct", "ProductId", "dbo.Product", "Id");
        }
    }
}
