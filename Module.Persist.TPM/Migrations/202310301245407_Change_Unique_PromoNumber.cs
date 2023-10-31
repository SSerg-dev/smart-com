namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_Unique_PromoNumber : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropIndex($"{defaultSchema}.Promo", "Unique_PromoNumber");
            CreateIndex($"{defaultSchema}.Promo", new[] { "Number", "TPMmode", "DeletedDate", "SavedScenarioId" }, unique: true, name: "Unique_PromoNumber");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropIndex($"{defaultSchema}.Promo", "Unique_PromoNumber");
            CreateIndex($"{defaultSchema}.Promo", new[] { "Number", "TPMmode", "DeletedDate" }, unique: true, name: "Unique_PromoNumber");
        }
    }
}
