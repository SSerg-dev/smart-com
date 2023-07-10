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
            SqlString1 = SqlString1.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlString1);
        }
        
        public override void Down()
        {
        }
        private string SqlString1 = @"            
            INSERT INTO [DefaultSchemaSetting].[AccessPoint]
                    ([Id],[Disabled],[DeletedDate],[Resource],[Action],[Description],[TPMmode])
                VALUES
                    (NEWID(), 0, NULL, 'SavedScenarios', 'GetSavedScenarios', NULL, 1)                 
            GO

            INSERT INTO [DefaultSchemaSetting].[AccessPoint]
                    ([Id],[Disabled],[DeletedDate],[Resource],[Action],[Description],[TPMmode])
                VALUES
                    (NEWID(), 0, NULL, 'SavedScenarios', 'UploadSavedScenario', NULL, 1)                 
            GO

            INSERT INTO [DefaultSchemaSetting].[AccessPoint]
                    ([Id],[Disabled],[DeletedDate],[Resource],[Action],[Description],[TPMmode])
                VALUES
                    (NEWID(), 0, NULL, 'RollingScenarios', 'UploadScenario', NULL, 1)                 
            GO

            DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='Administrator');

            INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                ([Id],[RoleId],[AccessPointId])
            VALUES
                (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetSavedScenarios' AND Resource = 'SavedScenarios')),
                (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'UploadSavedScenario' AND Resource = 'SavedScenarios')),
                (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'UploadScenario' AND Resource = 'RollingScenarios'))
            GO

            DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='SupportAdministrator');

            INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                ([Id],[RoleId],[AccessPointId])
            VALUES
                (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetSavedScenarios' AND Resource = 'SavedScenarios')),
                (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'UploadSavedScenario' AND Resource = 'SavedScenarios')),
                (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'UploadScenario' AND Resource = 'RollingScenarios'))
            GO

            DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='CMManager');

            INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                ([Id],[RoleId],[AccessPointId])
            VALUES
                (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetSavedScenarios' AND Resource = 'SavedScenarios')),
                (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'UploadSavedScenario' AND Resource = 'SavedScenarios')),
                (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'UploadScenario' AND Resource = 'RollingScenarios'))
            GO

            DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='KeyAccountManager');

            INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                ([Id],[RoleId],[AccessPointId])
            VALUES
                (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetSavedScenarios' AND Resource = 'SavedScenarios')),
                (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'UploadSavedScenario' AND Resource = 'SavedScenarios')),
                (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'UploadScenario' AND Resource = 'RollingScenarios'))
            GO

                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Promoes' and [Action]='GetRSPeriod'))
                GO
        ";
    }
}
