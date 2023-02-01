namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class After_Create_developPriceIncrease2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("Jupiter.PromoProductsCorrection", "Unique_PromoProductsCorrection");
            AlterColumn("Jupiter.PromoProductsCorrection", "TempId", c => c.String(maxLength: 128));
            CreateIndex("Jupiter.PromoProductsCorrection", new[] { "DeletedDate", "TPMmode", "PromoProductId", "TempId" }, unique: true, name: "Unique_PromoProductsCorrection");
        }
        
        public override void Down()
        {
            DropIndex("Jupiter.PromoProductsCorrection", "Unique_PromoProductsCorrection");
            AlterColumn("Jupiter.PromoProductsCorrection", "TempId", c => c.String(maxLength: 256));
            CreateIndex("Jupiter.PromoProductsCorrection", new[] { "DeletedDate", "TPMmode", "PromoProductId", "TempId" }, unique: true, name: "Unique_PromoProductsCorrection");
        }
    }
}
