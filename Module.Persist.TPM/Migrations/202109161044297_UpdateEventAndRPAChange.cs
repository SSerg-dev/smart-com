namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateEventAndRPAChange : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.RPA", "Parametrs", c => c.String());
            DropColumn($"{defaultSchema}.RPA", "Parametr");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.RPA", "Parametr", c => c.String());
            DropColumn($"{defaultSchema}.RPA", "Parametrs");
        }
    }
}
