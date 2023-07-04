namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPromoesToSavedScenario : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.Promo", "SavedScenarioId", c => c.Guid());
            CreateIndex($"{defaultSchema}.Promo", "SavedScenarioId");
            AddForeignKey($"{defaultSchema}.Promo", "SavedScenarioId", $"{defaultSchema}.SavedScenario", "Id");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.Promo", "SavedScenarioId", $"{defaultSchema}.SavedScenario");
            DropIndex($"{defaultSchema}.Promo", new[] { "SavedScenarioId" });
            DropColumn($"{defaultSchema}.Promo", "SavedScenarioId");
        }
    }
}
