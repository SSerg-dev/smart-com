namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_AP_CopyYearScenario : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            string SqlString = $@"  
				INSERT INTO [{defaultSchema}].[AccessPoint] ([Disabled],[Resource],[Action]) VALUES
				(0, 'ClientTrees',	'CopyYearScenario')

                DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [{defaultSchema}].[Role] where SystemName='Administrator');
				INSERT INTO [{defaultSchema}].[AccessPointRole] (RoleId, AccessPointId) values
				(@RoleId, (SELECT [Id] FROM [{defaultSchema}].[AccessPoint] where [Resource]='ClientTrees' and [Action]='CopyYearScenario'))

                SET @RoleId = (SELECT [Id] FROM [{defaultSchema}].[Role] where SystemName='SupportAdministrator');
				INSERT INTO [{defaultSchema}].[AccessPointRole] (RoleId, AccessPointId) values
				(@RoleId, (SELECT [Id] FROM [{defaultSchema}].[AccessPoint] where [Resource]='ClientTrees' and [Action]='CopyYearScenario'))

                SET @RoleId = (SELECT [Id] FROM [{defaultSchema}].[Role] where SystemName='CMManager');
				INSERT INTO [{defaultSchema}].[AccessPointRole] (RoleId, AccessPointId) values
				(@RoleId, (SELECT [Id] FROM [{defaultSchema}].[AccessPoint] where [Resource]='ClientTrees' and [Action]='CopyYearScenario'))

                SET @RoleId = (SELECT [Id] FROM [{defaultSchema}].[Role] where SystemName='KeyAccountManager');
				INSERT INTO [{defaultSchema}].[AccessPointRole] (RoleId, AccessPointId) values
				(@RoleId, (SELECT [Id] FROM [{defaultSchema}].[AccessPoint] where [Resource]='ClientTrees' and [Action]='CopyYearScenario'))

                SET @RoleId = (SELECT [Id] FROM [{defaultSchema}].[Role] where SystemName='FunctionalExpert');
				INSERT INTO [{defaultSchema}].[AccessPointRole] (RoleId, AccessPointId) values
				(@RoleId, (SELECT [Id] FROM [{defaultSchema}].[AccessPoint] where [Resource]='ClientTrees' and [Action]='CopyYearScenario'))
				";
            Sql(SqlString);
        }
        
        public override void Down()
        {
        }
    }
}
