namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_RollingScenario : DbMigration
    {
        public override void Up()
        {
            AddColumn("Jupiter.RollingScenario", "IsSendForApproval", c => c.Boolean(nullable: false));
            AddColumn("Jupiter.RollingScenario", "IsCMManagerApproved", c => c.Boolean(nullable: false));
            DropColumn("Jupiter.RollingScenario", "CreatorId");
            DropColumn("Jupiter.RollingScenario", "CreatorLogin");
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            SqlString = SqlString.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlString);
        }
        
        public override void Down()
        {
            AddColumn("Jupiter.RollingScenario", "CreatorLogin", c => c.String());
            AddColumn("Jupiter.RollingScenario", "CreatorId", c => c.Guid(nullable: false));
            DropColumn("Jupiter.RollingScenario", "IsCMManagerApproved");
            DropColumn("Jupiter.RollingScenario", "IsSendForApproval");
        }
        private string SqlString = @" 
            UPDATE [DefaultSchemaSetting].[AccessPoint]
               SET [TPMmode] = 1
             WHERE Resource = 'RollingScenarios'
            GO
        ";
    }
}
