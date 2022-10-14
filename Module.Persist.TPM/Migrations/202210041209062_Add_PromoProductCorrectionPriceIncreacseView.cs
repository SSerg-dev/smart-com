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
            DropForeignKey($"{defaultSchema}.PromoProductPriceIncrease", "Id", $"{defaultSchema}.PromoPriceIncrease");
            DropIndex($"{defaultSchema}.PromoProductPriceIncrease", new[] { "Id" });
            AddColumn($"{defaultSchema}.PromoProductPriceIncrease", "PromoPriceIncreaseId", c => c.Guid(nullable: false));
            CreateIndex($"{defaultSchema}.PromoProductPriceIncrease", "PromoPriceIncreaseId");
            AddForeignKey($"{defaultSchema}.PromoProductPriceIncrease", "PromoPriceIncreaseId", $"{defaultSchema}.PromoPriceIncrease", "Id");
            Sql(ViewMigrations.UpdatePromoProductCorrectionPriceIncreaseViewString(defaultSchema));
            DropIndex($"{defaultSchema}.PriceList", "Unique_PriceList");
            CreateIndex($"{defaultSchema}.PriceList", new[] { "DeletedDate", "StartDate", "EndDate", "ClientTreeId", "ProductId", "FuturePriceMarker" }, unique: true, name: "Unique_PriceList");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropIndex($"{defaultSchema}.PriceList", "Unique_PriceList");
            CreateIndex($"{defaultSchema}.PriceList", new[] { "DeletedDate", "StartDate", "EndDate", "ClientTreeId", "ProductId" }, unique: true, name: "Unique_PriceList");
            DropForeignKey($"{defaultSchema}.PromoProductPriceIncrease", "PromoPriceIncreaseId", $"{defaultSchema}.PromoPriceIncrease");
            DropIndex($"{defaultSchema}.PromoProductPriceIncrease", new[] { "PromoPriceIncreaseId" });
            DropColumn($"{defaultSchema}.PromoProductPriceIncrease", "PromoPriceIncreaseId");
            CreateIndex($"{defaultSchema}.PromoProductPriceIncrease", "Id");
            AddForeignKey($"{defaultSchema}.PromoProductPriceIncrease", "Id", $"{defaultSchema}.PromoPriceIncrease", "Id");
            DropForeignKey($"{defaultSchema}.PromoProductPriceIncrease", "PromoProductId", $"{defaultSchema}.PromoProduct");
            DropIndex($"{defaultSchema}.PromoProductPriceIncrease", new[] { "PromoProductId" });
            DropColumn($"{defaultSchema}.PromoProductPriceIncrease", "PromoProductId");
            AddForeignKey($"{defaultSchema}.PromoProductPriceIncrease", "Id", $"{defaultSchema}.PromoProduct", "Id");
        }
    }
}
