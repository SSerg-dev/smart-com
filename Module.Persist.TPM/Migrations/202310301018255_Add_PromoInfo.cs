namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PromoInfo : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            CreateTable(
                $"{defaultSchema}.PromoInfo",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreatedFrom = c.Int(nullable: false),
                        CreatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        Description = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey($"{defaultSchema}.Promo", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id);
            
            AlterColumn($"{defaultSchema}.Promo", "CreatorLogin", c => c.String(maxLength: 128));
            AlterColumn($"{defaultSchema}.Promo", "ClientHierarchy", c => c.String(maxLength: 128));
            AlterColumn($"{defaultSchema}.Promo", "ProductHierarchy", c => c.String(maxLength: 128));
            AlterColumn($"{defaultSchema}.Promo", "InvoiceNumber", c => c.String(maxLength: 256));
            AlterColumn($"{defaultSchema}.Promo", "DocumentNumber", c => c.String(maxLength: 128));
            AlterColumn($"{defaultSchema}.Promo", "EventName", c => c.String(maxLength: 128));
            AlterColumn($"{defaultSchema}.Promo", "BlockInformation", c => c.String(maxLength: 128));
            AlterColumn($"{defaultSchema}.Promo", "RegularExcludedProductIds", c => c.String(maxLength: 128));
            AlterColumn($"{defaultSchema}.Promo", "MLPromoId", c => c.String(maxLength: 255));
            AlterColumn($"{defaultSchema}.PromoProductPriceIncrease", "ActualProductUOM", c => c.String(maxLength: 128));
            AlterColumn($"{defaultSchema}.PromoProductPriceIncrease", "ProductEN", c => c.String(maxLength: 128));
            AlterColumn($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "UserName", c => c.String(maxLength: 128));
            AlterColumn($"{defaultSchema}.PromoProduct", "ActualProductUOM", c => c.String(maxLength: 128));
            AlterColumn($"{defaultSchema}.PromoProduct", "ProductEN", c => c.String(maxLength: 128));
            AlterColumn($"{defaultSchema}.PromoProductsCorrection", "UserName", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.PromoInfo", "Id", $"{defaultSchema}.Promo");
            DropIndex($"{defaultSchema}.PromoInfo", new[] { "Id" });
            AlterColumn($"{defaultSchema}.PromoProductsCorrection", "UserName", c => c.String());
            AlterColumn($"{defaultSchema}.PromoProduct", "ProductEN", c => c.String());
            AlterColumn($"{defaultSchema}.PromoProduct", "ActualProductUOM", c => c.String());
            AlterColumn($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "UserName", c => c.String());
            AlterColumn($"{defaultSchema}.PromoProductPriceIncrease", "ProductEN", c => c.String());
            AlterColumn($"{defaultSchema}.PromoProductPriceIncrease", "ActualProductUOM", c => c.String());
            AlterColumn($"{defaultSchema}.Promo", "MLPromoId", c => c.String());
            AlterColumn($"{defaultSchema}.Promo", "RegularExcludedProductIds", c => c.String());
            AlterColumn($"{defaultSchema}.Promo", "BlockInformation", c => c.String());
            AlterColumn($"{defaultSchema}.Promo", "EventName", c => c.String());
            AlterColumn($"{defaultSchema}.Promo", "DocumentNumber", c => c.String());
            AlterColumn($"{defaultSchema}.Promo", "InvoiceNumber", c => c.String());
            AlterColumn($"{defaultSchema}.Promo", "ProductHierarchy", c => c.String());
            AlterColumn($"{defaultSchema}.Promo", "ClientHierarchy", c => c.String());
            AlterColumn($"{defaultSchema}.Promo", "CreatorLogin", c => c.String());
            DropTable($"{defaultSchema}.PromoInfo");
        }
    }
}
