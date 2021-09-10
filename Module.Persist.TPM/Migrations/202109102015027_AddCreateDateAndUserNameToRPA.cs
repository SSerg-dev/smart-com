namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreateDateAndUserNameToRPA : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.RPA", "CreateDate", c => c.DateTimeOffset(precision: 7));
            AddColumn($"{defaultSchema}.RPA", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropColumn($"{defaultSchema}.RPA", "UserName");
            DropColumn($"{defaultSchema}.RPA", "CreateDate");
        }
    }
}
