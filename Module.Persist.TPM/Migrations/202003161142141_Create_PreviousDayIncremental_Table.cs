namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_PreviousDayIncremental_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PreviousDayIncremental",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Week = c.String(),
                        DemandCode = c.String(),
                        DMDGroup = c.String(),
                        PromoId = c.Guid(nullable: false),
                        ProductId = c.Guid(nullable: false),
                        IncrementalQty = c.Double(nullable: false),
                        LastChangeDate = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.ProductId)
                .ForeignKey("dbo.Promo", t => t.PromoId)
                .Index(t => t.PromoId)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PreviousDayIncremental", "PromoId", "dbo.Promo");
            DropForeignKey("dbo.PreviousDayIncremental", "ProductId", "dbo.Product");
            DropIndex("dbo.PreviousDayIncremental", new[] { "ProductId" });
            DropIndex("dbo.PreviousDayIncremental", new[] { "PromoId" });
            DropTable("dbo.PreviousDayIncremental");
        }
    }
}
