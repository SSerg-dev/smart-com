namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateActualCOGSTn : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.ActualCOGSTn", "IsCOGSIncidentCreated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropColumn($"{defaultSchema}.ActualCOGSTn", "IsCOGSIncidentCreated");
        }
    }
}
