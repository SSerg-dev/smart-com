namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using Module.Persist.TPM.Migrations.Views;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_MLPromoId_TaskStatus_IsMLmodel : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.Promo", "MLPromoId", c => c.String());
            AddColumn($"{defaultSchema}.RollingScenario", "IsMLmodel", c => c.Boolean(nullable: false));
            AddColumn($"{defaultSchema}.RollingScenario", "TaskStatus", c => c.String(maxLength: 100));
            //Sql(ViewMigrations.GetPromoGridViewString(defaultSchema));
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropColumn($"{defaultSchema}.RollingScenario", "TaskStatus");
            DropColumn($"{defaultSchema}.RollingScenario", "IsMLmodel");
            DropColumn($"{defaultSchema}.Promo", "MLPromoId");
        }
    }
}
