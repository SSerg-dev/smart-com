namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPromoProductsCorrection : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PromoProductsCorrection",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        PromoProductId = c.Guid(nullable: false),
                        PlanProductUpliftPercentCorrected = c.Double(),
                        UserId = c.Guid(),
                        CreateDate = c.DateTimeOffset(nullable: false, precision: 7),
                        ChangeDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PromoProduct", t => t.PromoProductId)
                .Index(t => t.PromoProductId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PromoProductsCorrection", "PromoProductId", "dbo.PromoProduct");
            DropIndex("dbo.PromoProductsCorrection", new[] { "PromoProductId" });
            DropTable("dbo.PromoProductsCorrection");
        }
    }
}
