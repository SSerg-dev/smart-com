namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSavedSettings : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            CreateTable(
                $"{defaultSchema}.SavedSettings",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Key = c.String(nullable: false, maxLength: 512),
                        Value = c.String(nullable: false),
                        UserRoleId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey($"{defaultSchema}.UserRole", t => t.UserRoleId)
                .Index(t => t.UserRoleId);
            SqlString = SqlString.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlString);
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.SavedSettings", "UserRoleId", $"{defaultSchema}.UserRole");
            DropIndex($"{defaultSchema}.SavedSettings", new[] { "UserRoleId" });
            DropTable($"{defaultSchema}.SavedSettings");
        }
        private string SqlString = @"
        INSERT INTO [DefaultSchemaSetting].[AccessPoint]
                   ([Id],[Disabled],[DeletedDate],[Resource],[Action],[Description],[TPMmode])
		          VALUES
                   (NEWID(),0,NULL,'SavedSettings','SaveSettings',NULL,0)
        GO
        INSERT INTO [DefaultSchemaSetting].[AccessPoint]
                   ([Id],[Disabled],[DeletedDate],[Resource],[Action],[Description],[TPMmode])
		          VALUES
                   (NEWID(),0,NULL,'SavedSettings','LoadSettings',NULL,0)
        GO

        DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='Administrator');

                    INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                        ([Id],[RoleId],[AccessPointId])
                    VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Resource = 'SavedSettings' AND Action = 'SaveSettings')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Resource = 'SavedSettings' AND Action = 'LoadSettings'))             
        GO

        DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='CMManager');

                    INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                        ([Id],[RoleId],[AccessPointId])
                    VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Resource = 'SavedSettings' AND Action = 'SaveSettings')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Resource = 'SavedSettings' AND Action = 'LoadSettings'))               
                    GO

        DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='CustomerMarketing');

                    INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                        ([Id],[RoleId],[AccessPointId])
                    VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Resource = 'SavedSettings' AND Action = 'SaveSettings')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Resource = 'SavedSettings' AND Action = 'LoadSettings'))          
                    GO

        DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='DemandFinance');

                    INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                        ([Id],[RoleId],[AccessPointId])
                    VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Resource = 'SavedSettings' AND Action = 'SaveSettings')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Resource = 'SavedSettings' AND Action = 'LoadSettings'))             
                    GO

        DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='DemandPlanning');

                    INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                        ([Id],[RoleId],[AccessPointId])
                    VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Resource = 'SavedSettings' AND Action = 'SaveSettings')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Resource = 'SavedSettings' AND Action = 'LoadSettings'))              
                    GO
        DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='FunctionalExpert');

                    INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                        ([Id],[RoleId],[AccessPointId])
                    VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Resource = 'SavedSettings' AND Action = 'SaveSettings')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Resource = 'SavedSettings' AND Action = 'LoadSettings'))              
                    GO

        DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='GAManager');

                    INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                        ([Id],[RoleId],[AccessPointId])
                    VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Resource = 'SavedSettings' AND Action = 'SaveSettings')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Resource = 'SavedSettings' AND Action = 'LoadSettings'))           
                    GO

        DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='KeyAccountManager');

                    INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                        ([Id],[RoleId],[AccessPointId])
                    VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Resource = 'SavedSettings' AND Action = 'SaveSettings')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Resource = 'SavedSettings' AND Action = 'LoadSettings'))             
                    GO

        DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='SuperReader');

                    INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                        ([Id],[RoleId],[AccessPointId])
                    VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Resource = 'SavedSettings' AND Action = 'SaveSettings')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Resource = 'SavedSettings' AND Action = 'LoadSettings'))              
                    GO

        DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='SupportAdministrator');

                    INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                        ([Id],[RoleId],[AccessPointId])
                    VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Resource = 'SavedSettings' AND Action = 'SaveSettings')),
			        (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Resource = 'SavedSettings' AND Action = 'LoadSettings'))               
                    GO
        ";
    }
}
