namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;

    public partial class Add_AP_Deleted_ClientTreeBranchTech : DbMigration
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
        INSERT INTO [DefaultSchemaSetting].[AccessPoint]
                   ([Id],[Disabled],[DeletedDate],[Resource],[Action],[Description],[TPMmode])
		          VALUES
                   (NEWID(),0,NULL,'DeletedClientTreeBrandTeches','GetDeletedClientTreeBrandTeches',NULL,0)
        GO
        INSERT INTO [DefaultSchemaSetting].[AccessPoint]
                   ([Id],[Disabled],[DeletedDate],[Resource],[Action],[Description],[TPMmode])
		           VALUES
                   (NEWID(),0,NULL,'DeletedClientTreeBrandTeches','GetDeletedClientTreeBrandTech',NULL,0)
        GO
        INSERT INTO [DefaultSchemaSetting].[AccessPoint]
                   ([Id],[Disabled],[DeletedDate],[Resource],[Action],[Description],[TPMmode])
		            VALUES
                   (NEWID(),0,NULL,'DeletedClientTreeBrandTeches','GetFilteredData',NULL,0)
        GO

        DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='FunctionalExpert');

                    INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                        ([Id],[RoleId],[AccessPointId])
                    VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetDeletedClientTreeBrandTeches' AND Resource = 'DeletedClientTreeBrandTeches')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetDeletedClientTreeBrandTech' AND Resource = 'DeletedClientTreeBrandTeches')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetFilteredData' AND Resource = 'DeletedClientTreeBrandTeches'))             
        GO

        DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='Administrator');

                    INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                        ([Id],[RoleId],[AccessPointId])
                    VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetDeletedClientTreeBrandTeches' AND Resource = 'DeletedClientTreeBrandTeches')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetDeletedClientTreeBrandTech' AND Resource = 'DeletedClientTreeBrandTeches')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetFilteredData' AND Resource = 'DeletedClientTreeBrandTeches'))             
                    GO

        DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='GAManager');

                    INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                        ([Id],[RoleId],[AccessPointId])
                    VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetDeletedClientTreeBrandTeches' AND Resource = 'DeletedClientTreeBrandTeches')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetDeletedClientTreeBrandTech' AND Resource = 'DeletedClientTreeBrandTeches')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetFilteredData' AND Resource = 'DeletedClientTreeBrandTeches'))             
                    GO

        DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='KeyAccountManager');

                    INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                        ([Id],[RoleId],[AccessPointId])
                    VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetDeletedClientTreeBrandTeches' AND Resource = 'DeletedClientTreeBrandTeches')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetDeletedClientTreeBrandTech' AND Resource = 'DeletedClientTreeBrandTeches')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetFilteredData' AND Resource = 'DeletedClientTreeBrandTeches'))             
                    GO

        DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='SuperReader');

                    INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                        ([Id],[RoleId],[AccessPointId])
                    VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetDeletedClientTreeBrandTeches' AND Resource = 'DeletedClientTreeBrandTeches')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetDeletedClientTreeBrandTech' AND Resource = 'DeletedClientTreeBrandTeches')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetFilteredData' AND Resource = 'DeletedClientTreeBrandTeches'))             
                    GO
        DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='DemandPlanning');

                    INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                        ([Id],[RoleId],[AccessPointId])
                    VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetDeletedClientTreeBrandTeches' AND Resource = 'DeletedClientTreeBrandTeches')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetDeletedClientTreeBrandTech' AND Resource = 'DeletedClientTreeBrandTeches')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetFilteredData' AND Resource = 'DeletedClientTreeBrandTeches'))             
                    GO

        DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='CustomerMarketing');

                    INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                        ([Id],[RoleId],[AccessPointId])
                    VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetDeletedClientTreeBrandTeches' AND Resource = 'DeletedClientTreeBrandTeches')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetDeletedClientTreeBrandTech' AND Resource = 'DeletedClientTreeBrandTeches')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetFilteredData' AND Resource = 'DeletedClientTreeBrandTeches'))             
                    GO

        DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='DemandFinance');

                    INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                        ([Id],[RoleId],[AccessPointId])
                    VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetDeletedClientTreeBrandTeches' AND Resource = 'DeletedClientTreeBrandTeches')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetDeletedClientTreeBrandTech' AND Resource = 'DeletedClientTreeBrandTeches')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetFilteredData' AND Resource = 'DeletedClientTreeBrandTeches'))             
                    GO

        DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='SupportAdministrator');

                    INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                        ([Id],[RoleId],[AccessPointId])
                    VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetDeletedClientTreeBrandTeches' AND Resource = 'DeletedClientTreeBrandTeches')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetDeletedClientTreeBrandTech' AND Resource = 'DeletedClientTreeBrandTeches')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetFilteredData' AND Resource = 'DeletedClientTreeBrandTeches'))             
                    GO
        ";

    }
}
