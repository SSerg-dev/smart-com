namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using Core.Settings;

    public partial class Update_RPA_Columns : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AlterColumn($"{defaultSchema}.RPA", "Constraint", c => c.String());
            AlterColumn($"{defaultSchema}.RPA", "Parametr", c => c.String());
            AlterColumn($"{defaultSchema}.RPA", "Status", c => c.String());
            AlterColumn($"{defaultSchema}.RPA", "FileURL", c => c.String());
            AlterColumn($"{defaultSchema}.RPA", "LogURL", c => c.String());
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AlterColumn($"{defaultSchema}.RPA", "LogURL", c => c.String(nullable: false));
            AlterColumn($"{defaultSchema}.RPA", "FileURL", c => c.String(nullable: false));
            AlterColumn($"{defaultSchema}.RPA", "Status", c => c.String(nullable: false));
            AlterColumn($"{defaultSchema}.RPA", "Parametr", c => c.String(nullable: false));
            AlterColumn($"{defaultSchema}.RPA", "Constraint", c => c.String(nullable: false));
        }
    }
}
