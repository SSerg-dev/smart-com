namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using Module.Persist.TPM.Migrations.Views;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PromoProductCorrectionPriceIncreacseView : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.PromoProductPriceIncrease", "Id", $"{defaultSchema}.PromoProduct");
            AddColumn($"{defaultSchema}.PromoProductPriceIncrease", "PromoProductId", c => c.Guid(nullable: false));
            CreateIndex($"{defaultSchema}.PromoProductPriceIncrease", "PromoProductId");
            AddForeignKey($"{defaultSchema}.PromoProductPriceIncrease", "PromoProductId", $"{defaultSchema}.PromoProduct", "Id");
            Sql(ViewMigrations.UpdatePromoProductCorrectionPriceIncreaseViewString(defaultSchema));
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.PromoProductPriceIncrease", "PromoProductId", $"{defaultSchema}.PromoProduct");
            DropIndex($"{defaultSchema}.PromoProductPriceIncrease", new[] { "PromoProductId" });
            DropColumn($"{defaultSchema}.PromoProductPriceIncrease", "PromoProductId");
            AddForeignKey($"{defaultSchema}.PromoProductPriceIncrease", "Id", $"{defaultSchema}.PromoProduct", "Id");
        }
    }
}
