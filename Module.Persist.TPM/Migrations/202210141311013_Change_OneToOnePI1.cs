namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_OneToOnePI1 : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.PromoProductPriceIncrease", "PromoPriceIncreaseId", $"{defaultSchema}.PromoPriceIncrease");
            DropIndex($"{defaultSchema}.PromoPriceIncrease", new[] { "Id" });
            DropIndex($"{defaultSchema}.PromoProductCorrectionPriceIncrease", new[] { "Id" });
            DropPrimaryKey($"{defaultSchema}.PromoPriceIncrease");
            DropPrimaryKey($"{defaultSchema}.PromoProductCorrectionPriceIncrease");
            AlterColumn($"{defaultSchema}.PromoPriceIncrease", "Id", c => c.Guid(nullable: false));
            AlterColumn($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey($"{defaultSchema}.PromoPriceIncrease", "Id");
            AddPrimaryKey($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "Id");
            CreateIndex($"{defaultSchema}.PromoPriceIncrease", "Id");
            CreateIndex($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "Id");
            AddForeignKey($"{defaultSchema}.PromoProductPriceIncrease", "PromoPriceIncreaseId", $"{defaultSchema}.PromoPriceIncrease", "Id");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.PromoProductPriceIncrease", "PromoPriceIncreaseId", $"{defaultSchema}.PromoPriceIncrease");
            DropIndex($"{defaultSchema}.PromoProductCorrectionPriceIncrease", new[] { "Id" });
            DropIndex($"{defaultSchema}.PromoPriceIncrease", new[] { "Id" });
            DropPrimaryKey($"{defaultSchema}.PromoProductCorrectionPriceIncrease");
            DropPrimaryKey($"{defaultSchema}.PromoPriceIncrease");
            AlterColumn($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "Id", c => c.Guid(nullable: false, identity: true));
            AlterColumn($"{defaultSchema}.PromoPriceIncrease", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "Id");
            AddPrimaryKey($"{defaultSchema}.PromoPriceIncrease", "Id");
            CreateIndex($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "Id");
            CreateIndex($"{defaultSchema}.PromoPriceIncrease", "Id");
            AddForeignKey($"{defaultSchema}.PromoProductPriceIncrease", "PromoPriceIncreaseId", $"{defaultSchema}.PromoPriceIncrease", "Id");
        }
    }
}
