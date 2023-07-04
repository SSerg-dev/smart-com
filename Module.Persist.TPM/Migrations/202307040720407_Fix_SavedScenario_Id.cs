namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fix_SavedScenario_Id : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            RenameColumn(table: $"{defaultSchema}.SavedScenario", name: "RollingScenario_Id", newName: "RollingScenarioId");
            RenameIndex(table: $"{defaultSchema}.SavedScenario", name: "IX_RollingScenario_Id", newName: "IX_RollingScenarioId");
        }
        
        public override void Down() 
        { 
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            RenameIndex(table: $"{defaultSchema}.SavedScenario", name: "IX_RollingScenarioId", newName: "IX_RollingScenario_Id");
            RenameColumn(table: $"{defaultSchema}.SavedScenario", name: "RollingScenarioId", newName: "RollingScenario_Id");
        }
    }
}
