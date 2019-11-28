namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUniuePromoProductCorrection : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PromoProductsCorrection", new[] { "PromoProductId" });
            CreateIndex("dbo.PromoProductsCorrection", new[] { "Disabled", "PromoProductId" }, unique: true, name: "Unique_PromoProductCorrection");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PromoProductsCorrection", "Unique_PromoProductCorrection");
            CreateIndex("dbo.PromoProductsCorrection", "PromoProductId");
        }
    }
}
