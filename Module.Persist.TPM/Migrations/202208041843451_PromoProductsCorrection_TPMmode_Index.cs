namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoProductsCorrection_TPMmode_Index : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropIndex($"{defaultSchema}.PromoProductsCorrection", "Unique_PromoProductsCorrection");
            CreateIndex($"{defaultSchema}.PromoProductsCorrection", new[] { "DeletedDate", "TPMmode", "PromoProductId" }, unique: true, name: "Unique_PromoProductsCorrection");
        }

        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropIndex($"{defaultSchema}.PromoProductsCorrection", "Unique_PromoProductsCorrection");
            CreateIndex($"{defaultSchema}.PromoProductsCorrection", new[] { "DeletedDate", "TPMmode", "PromoProductId" }, unique: true, name: "Unique_PromoProductsCorrection");
        }
    }
}
