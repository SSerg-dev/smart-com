namespace Module.Persist.TPM.Migrations
{
	using Core.Settings;
	using System;
    using System.Data.Entity.Migrations;
    
    public partial class PLUHistoryId : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AlterColumn($"{defaultSchema}.Plu", "Id", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AlterColumn($"{defaultSchema}.Plu", "Id", c => c.Guid(nullable: false, identity: true));
        }
    }
}
