namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_TPMmode_Index : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropIndex($"{defaultSchema}.Promo", new[] { "Number" });
            CreateIndex($"{defaultSchema}.Promo", new[] { "Number", "TPMmode", "DeletedDate"}, unique: true, name: "Unique_PromoNumber");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropIndex($"{defaultSchema}.Promo", "Unique_PromoNumber");
            CreateIndex($"{defaultSchema}.Promo", "Number", unique: true);
        }
    }
}
