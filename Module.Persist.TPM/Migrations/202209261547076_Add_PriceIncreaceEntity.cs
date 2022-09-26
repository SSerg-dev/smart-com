namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PriceIncreaceEntity : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            CreateTable(
                $"{defaultSchema}.PromoPriceIncrease",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        PlanPromoUpliftPercent = c.Double(),
                        PlanPromoBaselineLSV = c.Double(),
                        PlanPromoIncrementalLSV = c.Double(),
                        PlanPromoLSV = c.Double(),
                        PlanPromoPostPromoEffectLSV = c.Double(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey($"{defaultSchema}.Promo", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                $"{defaultSchema}.PromoProductPriceIncrease",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        CreateDate = c.DateTimeOffset(precision: 7),
                        ZREP = c.String(maxLength: 255),
                        EAN_Case = c.String(maxLength: 255),
                        EAN_PC = c.String(maxLength: 255),
                        PlanProductCaseQty = c.Double(),
                        PlanProductPCQty = c.Int(),
                        PlanProductCaseLSV = c.Double(),
                        PlanProductPCLSV = c.Double(),
                        PlanProductBaselineLSV = c.Double(),
                        ActualProductBaselineLSV = c.Double(),
                        PlanProductIncrementalLSV = c.Double(),
                        PlanProductLSV = c.Double(),
                        PlanProductBaselineCaseQty = c.Double(),
                        Price = c.Double(),
                        PlanProductPCPrice = c.Double(),
                        ActualProductPCQty = c.Int(),
                        ActualProductCaseQty = c.Double(),
                        ActualProductUOM = c.String(),
                        ActualProductSellInPrice = c.Double(),
                        ActualProductShelfDiscount = c.Double(),
                        ActualProductPCLSV = c.Double(),
                        ActualProductUpliftPercent = c.Double(),
                        ActualProductIncrementalPCQty = c.Double(),
                        ActualProductIncrementalPCLSV = c.Double(),
                        ActualProductIncrementalLSV = c.Double(),
                        PlanProductPostPromoEffectLSVW1 = c.Double(),
                        PlanProductPostPromoEffectLSVW2 = c.Double(),
                        PlanProductPostPromoEffectLSV = c.Double(),
                        ActualProductPostPromoEffectLSV = c.Double(),
                        PlanProductIncrementalCaseQty = c.Double(),
                        PlanProductUpliftPercent = c.Double(),
                        AverageMarker = c.Boolean(nullable: false),
                        ActualProductLSV = c.Double(),
                        ActualProductPostPromoEffectQtyW1 = c.Double(),
                        ActualProductPostPromoEffectQtyW2 = c.Double(),
                        ActualProductPostPromoEffectQty = c.Double(),
                        PlanProductPostPromoEffectQtyW1 = c.Double(),
                        PlanProductPostPromoEffectQtyW2 = c.Double(),
                        PlanProductPostPromoEffectQty = c.Double(),
                        ProductEN = c.String(),
                        ActualProductLSVByCompensation = c.Double(),
                        ActualProductBaselineCaseQty = c.Double(),
                        SumInvoiceProduct = c.Double(),
                        PlanProductBaselineVolume = c.Double(),
                        PlanProductPostPromoEffectVolumeW1 = c.Double(),
                        PlanProductPostPromoEffectVolumeW2 = c.Double(),
                        PlanProductPostPromoEffectVolume = c.Double(),
                        ActualProductQtySO = c.Double(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey($"{defaultSchema}.PromoPriceIncrease", t => t.Id)
                .ForeignKey($"{defaultSchema}.PromoProduct", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                $"{defaultSchema}.PromoProductCorrectionPriceIncrease",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        PlanProductUpliftPercentCorrected = c.Double(),
                        TempId = c.String(),
                        UserId = c.Guid(),
                        UserName = c.String(),
                        CreateDate = c.DateTimeOffset(precision: 7),
                        ChangeDate = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey($"{defaultSchema}.PromoProductPriceIncrease", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.PromoProductPriceIncrease", "Id", $"{defaultSchema}.PromoProduct");
            DropForeignKey($"{defaultSchema}.PromoProductPriceIncrease", "Id", $"{defaultSchema}.PromoPriceIncrease");
            DropForeignKey($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "Id", $"{defaultSchema}.PromoProductPriceIncrease");
            DropForeignKey($"{defaultSchema}.PromoPriceIncrease", "Id", $"{defaultSchema}.Promo");
            DropIndex($"{defaultSchema}.PromoProductCorrectionPriceIncrease", new[] { "Id" });
            DropIndex($"{defaultSchema}.PromoProductPriceIncrease", new[] { "Id" });
            DropIndex($"{defaultSchema}.PromoPriceIncrease", new[] { "Id" });
            DropTable($"{defaultSchema}.PromoProductCorrectionPriceIncrease");
            DropTable($"{defaultSchema}.PromoProductPriceIncrease");
            DropTable($"{defaultSchema}.PromoPriceIncrease");
        }
    }
}
