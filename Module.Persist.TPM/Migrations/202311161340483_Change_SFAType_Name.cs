namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_SFAType_Name : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            RenameTable(name: $"{defaultSchema}.SFATypes", newName: "SFAType");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            RenameTable(name: $"{defaultSchema}.SFAType", newName: "SFATypes");
        }
    }
}
