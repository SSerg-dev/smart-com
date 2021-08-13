namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using Core.Settings;
    public partial class RPASetting_Update_Column_Type : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.RPASetting", "Name", c => c.String(nullable: false));
            DropColumn($"{defaultSchema}.RPASetting", "Type");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.RPASetting", "Type", c => c.Int(nullable: false));
            DropColumn($"{defaultSchema}.RPASetting", "Name");
            DropTable($"{defaultSchema}.RPA");
        }
    }
}
