namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using Module.Persist.TPM.Migrations.Views;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeOneToManyPPCPI : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            SqlString = SqlString.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlString);
            DropForeignKey($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "Id", $"{defaultSchema}.PromoProductPriceIncrease");
            DropIndex($"{defaultSchema}.PromoProductCorrectionPriceIncrease", new[] { "Id" });
            DropPrimaryKey($"{defaultSchema}.PromoProductCorrectionPriceIncrease");
            AddColumn($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "PromoProductPriceIncreaseId", c => c.Guid(nullable: false));
            AlterColumn($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "Id");
            CreateIndex($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "PromoProductPriceIncreaseId");
            AddForeignKey($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "PromoProductPriceIncreaseId", $"{defaultSchema}.PromoProductPriceIncrease", "Id");
            //Sql(ViewMigrations.UpdatePromoProductCorrectionPriceIncreaseViewString(defaultSchema));
            Sql(ViewMigrations.UpdatePromoProductPriceIncreasesViewString(defaultSchema));
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "PromoProductPriceIncreaseId", $"{defaultSchema}.PromoProductPriceIncrease");
            DropIndex($"{defaultSchema}.PromoProductCorrectionPriceIncrease", new[] { "PromoProductPriceIncreaseId" });
            DropPrimaryKey($"{defaultSchema}.PromoProductCorrectionPriceIncrease");
            AlterColumn($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "Id", c => c.Guid(nullable: false));
            DropColumn($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "PromoProductPriceIncreaseId");
            AddPrimaryKey($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "Id");
            CreateIndex($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "Id");
            AddForeignKey($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "Id", $"{defaultSchema}.PromoProductPriceIncrease", "Id");
        }
        private string SqlString = @"                
                DELETE FROM [DefaultSchemaSetting].[PromoProductCorrectionPriceIncrease]
                GO
                DELETE FROM [DefaultSchemaSetting].[PromoProductPriceIncrease]
                GO
                DELETE FROM [DefaultSchemaSetting].[PromoPriceIncrease]
                GO
        ";
    }
}
