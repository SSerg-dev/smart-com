namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fix_Unique_PromoProductsCorrection : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropIndex($"{defaultSchema}.PromoProductsCorrection", "Unique_PromoProductsCorrection");            
            AlterColumn($"{defaultSchema}.PromoProductsCorrection", "TempId", c => c.String(maxLength: 256));
            CreateIndex($"{defaultSchema}.PromoProductsCorrection", new[] { "DeletedDate", "TPMmode", "PromoProductId", "TempId" }, unique: true, name: "Unique_PromoProductsCorrection");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropIndex($"{defaultSchema}.PromoProductsCorrection", "Unique_PromoProductsCorrection");
            AlterColumn($"{defaultSchema}.PromoProductsCorrection", "TempId", c => c.String());
            CreateIndex($"{defaultSchema}.PromoProductsCorrection", new[] { "DeletedDate", "TPMmode", "PromoProductId" }, unique: true, name: "Unique_PromoProductsCorrection");
        }
    }
}
