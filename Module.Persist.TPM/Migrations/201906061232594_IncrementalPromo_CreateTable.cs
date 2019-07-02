namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncrementalPromo_CreateTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IncrementalPromo",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        PromoId = c.Guid(),
                        ProductId = c.Guid(),
                        IncrementalCaseAmount = c.Int(),
                        IncrementalLSV = c.Double(),
                        IncrementalPrice = c.Double(),
                        LastModifiedDate = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.ProductId)
                .ForeignKey("dbo.Promo", t => t.PromoId)
                .Index(t => t.PromoId)
                .Index(t => t.ProductId);
            
            //AddColumn("dbo.PlanIncrementalReport", "PlanProductCaseQty", c => c.Double());
            //DropColumn("dbo.PlanIncrementalReport", "PlanProductQty");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.PlanIncrementalReport", "PlanProductQty", c => c.Double());
            DropForeignKey("dbo.IncrementalPromo", "PromoId", "dbo.Promo");
            DropForeignKey("dbo.IncrementalPromo", "ProductId", "dbo.Product");
            DropIndex("dbo.IncrementalPromo", new[] { "ProductId" });
            DropIndex("dbo.IncrementalPromo", new[] { "PromoId" });
            //DropColumn("dbo.PlanIncrementalReport", "PlanProductCaseQty");
            DropTable("dbo.IncrementalPromo");
        }
    }
}
