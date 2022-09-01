namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Promo_CascadeDelete_Some2 : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.PromoCancelledIncident", "PromoId", $"{defaultSchema}.Promo");
            DropForeignKey($"{defaultSchema}.PromoOnRejectIncident", "PromoId", $"{defaultSchema}.Promo");
            DropForeignKey($"{defaultSchema}.PromoApprovedIncident", "PromoId", $"{defaultSchema}.Promo");
            AddForeignKey($"{defaultSchema}.PromoCancelledIncident", "PromoId", $"{defaultSchema}.Promo", "Id", cascadeDelete: true);
            AddForeignKey($"{defaultSchema}.PromoOnRejectIncident", "PromoId", $"{defaultSchema}.Promo", "Id", cascadeDelete: true);
            AddForeignKey($"{defaultSchema}.PromoApprovedIncident", "PromoId", $"{defaultSchema}.Promo", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.PromoApprovedIncident", "PromoId", $"{defaultSchema}.Promo");
            DropForeignKey($"{defaultSchema}.PromoOnRejectIncident", "PromoId", $"{defaultSchema}.Promo");
            DropForeignKey($"{defaultSchema}.PromoCancelledIncident", "PromoId", $"{defaultSchema}.Promo");
            AddForeignKey($"{defaultSchema}.PromoApprovedIncident", "PromoId", $"{defaultSchema}.Promo", "Id");
            AddForeignKey($"{defaultSchema}.PromoOnRejectIncident", "PromoId", $"{defaultSchema}.Promo", "Id");
            AddForeignKey($"{defaultSchema}.PromoCancelledIncident", "PromoId", $"{defaultSchema}.Promo", "Id");
        }
    }
}
