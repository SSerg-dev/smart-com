namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Promo_CascadeDelete_PriceIncrease : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.PromoPriceIncrease", "Id", $"{defaultSchema}.Promo");
            DropForeignKey($"{defaultSchema}.PromoProductPriceIncrease", "PromoPriceIncreaseId", $"{defaultSchema}.PromoPriceIncrease");
            DropForeignKey($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "PromoProductPriceIncreaseId", $"{defaultSchema}.PromoProductPriceIncrease");
            AddForeignKey($"{defaultSchema}.PromoPriceIncrease", "Id", $"{defaultSchema}.Promo", "Id", cascadeDelete: true);
            AddForeignKey($"{defaultSchema}.PromoProductPriceIncrease", "PromoPriceIncreaseId", $"{defaultSchema}.PromoPriceIncrease", "Id", cascadeDelete: true);
            AddForeignKey($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "PromoProductPriceIncreaseId", $"{defaultSchema}.PromoProductPriceIncrease", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "PromoProductPriceIncreaseId", $"{defaultSchema}.PromoProductPriceIncrease");
            DropForeignKey($"{defaultSchema}.PromoProductPriceIncrease", "PromoPriceIncreaseId", $"{defaultSchema}.PromoPriceIncrease");
            DropForeignKey($"{defaultSchema}.PromoPriceIncrease", "Id", $"{defaultSchema}.Promo");
            AddForeignKey($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "PromoProductPriceIncreaseId", $"{defaultSchema}.PromoProductPriceIncrease", "Id");
            AddForeignKey($"{defaultSchema}.PromoProductPriceIncrease", "PromoPriceIncreaseId", $"{defaultSchema}.PromoPriceIncrease", "Id");
            AddForeignKey($"{defaultSchema}.PromoPriceIncrease", "Id", $"{defaultSchema}.Promo", "Id");
        }
    }
}
