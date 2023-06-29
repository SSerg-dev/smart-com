namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System.Data.Entity.Migrations;
    
    public partial class RA_mode_AccessPoints : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            string SqlString = $@"  
				INSERT INTO [{defaultSchema}].[AccessPoint] ([Disabled],[Resource],[Action]) VALUES
				(0, 'ClientTrees',	'SaveScenario')

                DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [{defaultSchema}].[Role] where SystemName='Administrator');
				INSERT INTO [{defaultSchema}].[AccessPointRole] (RoleId, AccessPointId) values
				(@RoleId, (SELECT [Id] FROM [{defaultSchema}].[AccessPoint] where [Resource]='ClientTrees' and [Action]='SaveScenario'))

                SET @RoleId = (SELECT [Id] FROM [{defaultSchema}].[Role] where SystemName='SupportAdministrator');
				INSERT INTO [{defaultSchema}].[AccessPointRole] (RoleId, AccessPointId) values
				(@RoleId, (SELECT [Id] FROM [{defaultSchema}].[AccessPoint] where [Resource]='ClientTrees' and [Action]='SaveScenario'))

                SET @RoleId = (SELECT [Id] FROM [{defaultSchema}].[Role] where SystemName='CMManager');
				INSERT INTO [{defaultSchema}].[AccessPointRole] (RoleId, AccessPointId) values
				(@RoleId, (SELECT [Id] FROM [{defaultSchema}].[AccessPoint] where [Resource]='ClientTrees' and [Action]='SaveScenario'))

                SET @RoleId = (SELECT [Id] FROM [{defaultSchema}].[Role] where SystemName='KeyAccountManager');
				INSERT INTO [{defaultSchema}].[AccessPointRole] (RoleId, AccessPointId) values
				(@RoleId, (SELECT [Id] FROM [{defaultSchema}].[AccessPoint] where [Resource]='ClientTrees' and [Action]='SaveScenario'))
				";
            Sql(SqlString);
        }
        
        public override void Down()
        {
        }
    }
}
