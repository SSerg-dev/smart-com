namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Promo_CascadeDelete_Some : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.PromoOnApprovalIncident", "PromoId", $"{defaultSchema}.Promo");
            AddForeignKey($"{defaultSchema}.PromoOnApprovalIncident", "PromoId", $"{defaultSchema}.Promo", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.PromoOnApprovalIncident", "PromoId", $"{defaultSchema}.Promo");
            AddForeignKey($"{defaultSchema}.PromoOnApprovalIncident", "PromoId", $"{defaultSchema}.Promo", "Id");
        }
    }
}
