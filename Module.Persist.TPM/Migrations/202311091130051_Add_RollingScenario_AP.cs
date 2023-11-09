namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_RollingScenario_AP : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            SqlString = SqlString.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlString);
        }
        
        public override void Down()
        {
        }
		private string SqlString = @"
			DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SuperReader' and [Disabled] = 0);
			   INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
			   (RoleId, AccessPointId) values
			   (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='RollingScenarios' and [Action]='GetRollingScenarios' and [Disabled] = 0)),
			   (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='RollingScenarios' and [Action]='GetVisibleButton' and [Disabled] = 0))
			GO

			DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
			   INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
			   (RoleId, AccessPointId) values
			   (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='RollingScenarios' and [Action]='GetRollingScenarios' and [Disabled] = 0)),
			   (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='RollingScenarios' and [Action]='GetVisibleButton' and [Disabled] = 0))
			GO
			DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CustomerMarketing' and [Disabled] = 0);
			   INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
			   (RoleId, AccessPointId) values
			   (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='RollingScenarios' and [Action]='GetRollingScenarios' and [Disabled] = 0)),
			   (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='RollingScenarios' and [Action]='GetVisibleButton' and [Disabled] = 0))
			GO			
            ";
	}
}
