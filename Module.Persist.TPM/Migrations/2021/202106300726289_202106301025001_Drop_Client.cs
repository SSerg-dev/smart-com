namespace Module.Persist.TPM.Migrations
{
	using Core.Settings;
	using System;
    using System.Data.Entity.Migrations;
    
    public partial class _202106301025001_Drop_Client : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            Sql($"DROP TABLE {defaultSchema}.Client");
        }
        
        public override void Down()
        {
        }
    }
}
