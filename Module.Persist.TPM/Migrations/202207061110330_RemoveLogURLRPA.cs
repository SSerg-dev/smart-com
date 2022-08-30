namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveLogURLRPA : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropColumn($"{defaultSchema}.RPA", "LogURL");
        }

        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.RPA", "LogURL", c => c.String());

        }
    }
}
