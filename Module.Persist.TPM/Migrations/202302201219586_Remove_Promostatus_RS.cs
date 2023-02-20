namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_Promostatus_RS : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.RollingScenario", "PromoStatusId", $"{defaultSchema}.PromoStatus");
            DropIndex($"{defaultSchema}.RollingScenario", new[] { "PromoStatusId" });
            AlterColumn($"{defaultSchema}.RollingScenario", "RSstatus", c => c.String(maxLength: 100));
            DropColumn($"{defaultSchema}.RollingScenario", "PromoStatusId");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.RollingScenario", "PromoStatusId", c => c.Guid(nullable: false));
            AlterColumn($"{defaultSchema}.RollingScenario", "RSstatus", c => c.String());
            CreateIndex($"{defaultSchema}.RollingScenario", "PromoStatusId");
            AddForeignKey($"{defaultSchema}.RollingScenario", "PromoStatusId", $"{defaultSchema}.PromoStatus", "Id");
        }
    }
}
