namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PPPI_PromoProduct_Optional : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropIndex($"{defaultSchema}.PromoProductPriceIncrease", new[] { "PromoProductId" });
            AlterColumn($"{defaultSchema}.PromoProductPriceIncrease", "PromoProductId", c => c.Guid());
            CreateIndex($"{defaultSchema}.PromoProductPriceIncrease", "PromoProductId");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropIndex($"{defaultSchema}.PromoProductPriceIncrease", new[] { "PromoProductId" });
            AlterColumn($"{defaultSchema}.PromoProductPriceIncrease", "PromoProductId", c => c.Guid(nullable: false));
            CreateIndex($"{defaultSchema}.PromoProductPriceIncrease", "PromoProductId");
        }
    }
}
