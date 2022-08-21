namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_RollingScenario : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.RollingScenario", "IsSendForApproval", c => c.Boolean(nullable: false));
            AddColumn($"{ defaultSchema}.RollingScenario", "IsCMManagerApproved", c => c.Boolean(nullable: false));
            DropColumn($"{defaultSchema}.RollingScenario", "CreatorId");
            DropColumn($"{defaultSchema}.RollingScenario", "CreatorLogin");
            
            SqlString = SqlString.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlString);
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.RollingScenario", "CreatorLogin", c => c.String());
            AddColumn($"{defaultSchema}.RollingScenario", "CreatorId", c => c.Guid(nullable: false));
            DropColumn($"{defaultSchema}.RollingScenario", "IsCMManagerApproved");
            DropColumn($"{defaultSchema}.RollingScenario", "IsSendForApproval");
        }
        private string SqlString = @" 
            UPDATE [DefaultSchemaSetting].[AccessPoint]
               SET [TPMmode] = 1
             WHERE Resource = 'RollingScenarios'
            GO

			DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'Administrator' and [Disabled] = 0);
			   INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
			   (RoleId, AccessPointId) values
			   (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='RollingScenarios' and [Action]='Decline' and [Disabled] = 0)),
			   (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='RollingScenarios' and [Action]='Approve' and [Disabled] = 0))
			GO
        ";
    }
}
