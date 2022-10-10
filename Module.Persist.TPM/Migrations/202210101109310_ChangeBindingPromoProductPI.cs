namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeBindingPromoProductPI : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.PromoProductPriceIncrease", "Id", $"{defaultSchema}.PromoPriceIncrease");
            DropIndex($"{defaultSchema}.PromoProductPriceIncrease", new[] { "Id" });
            AddColumn($"{defaultSchema}.PromoProductPriceIncrease", "PromoPriceIncreaseId", c => c.Guid(nullable: false));
            CreateIndex($"{defaultSchema}.PromoProductPriceIncrease", "PromoPriceIncreaseId");
            AddForeignKey($"{defaultSchema}.PromoProductPriceIncrease", "PromoPriceIncreaseId", $"{defaultSchema}.PromoPriceIncrease", "Id");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.PromoProductPriceIncrease", "PromoPriceIncreaseId", $"{defaultSchema}.PromoPriceIncrease");
            DropIndex($"{defaultSchema}.PromoProductPriceIncrease", new[] { "PromoPriceIncreaseId" });
            DropColumn($"{defaultSchema}.PromoProductPriceIncrease", "PromoPriceIncreaseId");
            CreateIndex($"{defaultSchema}.PromoProductPriceIncrease", "Id");
            AddForeignKey($"{defaultSchema}.PromoProductPriceIncrease", "Id", $"{defaultSchema}.PromoPriceIncrease", "Id");
        }
    }
}
