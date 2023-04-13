namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Actions_PPE : DbMigration
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
                       (NEWID(), 0, NULL, 'PlanPostPromoEffects', 'GetPlanPostPromoEffects', NULL, 1),                 
                       (NEWID(), 0, NULL, 'PlanPostPromoEffects', 'GetFilteredData', NULL, 1),
                       (NEWID(), 0, NULL, 'PlanPostPromoEffects', 'Put', NULL, 1),
                       (NEWID(), 0, NULL, 'PlanPostPromoEffects', 'Post', NULL, 1),
                       (NEWID(), 0, NULL, 'PlanPostPromoEffects', 'Delete', NULL, 1),
                       (NEWID(), 0, NULL, 'PlanPostPromoEffects', 'Patch', NULL, 1),
                       (NEWID(), 0, NULL, 'PlanPostPromoEffects', 'ExportXLSX', NULL, 1),
                       (NEWID(), 0, NULL, 'PlanPostPromoEffects', 'FullImportXLSX', NULL, 1)
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='Administrator');

                INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                ([Id],[RoleId],[AccessPointId])
                VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetPlanPostPromoEffects' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetFilteredData' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'Put' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'Post' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'Delete' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'Patch' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'ExportXLSX' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'FullImportXLSX' AND Resource = 'PlanPostPromoEffects'))
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='SupportAdministrator');

                INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                ([Id],[RoleId],[AccessPointId])
                VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetPlanPostPromoEffects' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetFilteredData' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'Put' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'Post' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'Delete' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'Patch' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'ExportXLSX' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'FullImportXLSX' AND Resource = 'PlanPostPromoEffects'))
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='FunctionalExpert');

                INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                ([Id],[RoleId],[AccessPointId])
                VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetPlanPostPromoEffects' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetFilteredData' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'Put' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'Post' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'Delete' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'Patch' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'ExportXLSX' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'FullImportXLSX' AND Resource = 'PlanPostPromoEffects'))
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='DemandPlanning');

                INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                ([Id],[RoleId],[AccessPointId])
                VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetPlanPostPromoEffects' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetFilteredData' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'Put' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'Post' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'Delete' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'Patch' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'ExportXLSX' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'FullImportXLSX' AND Resource = 'PlanPostPromoEffects'))
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='KeyAccountManager');

                INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                ([Id],[RoleId],[AccessPointId])
                VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetPlanPostPromoEffects' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetFilteredData' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'ExportXLSX' AND Resource = 'PlanPostPromoEffects'))
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='CustomerMarketing');

                INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                ([Id],[RoleId],[AccessPointId])
                VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetPlanPostPromoEffects' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetFilteredData' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'ExportXLSX' AND Resource = 'PlanPostPromoEffects'))
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='DemandFinance');

                INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                ([Id],[RoleId],[AccessPointId])
                VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetPlanPostPromoEffects' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetFilteredData' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'ExportXLSX' AND Resource = 'PlanPostPromoEffects'))
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='GAManager');

                INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                ([Id],[RoleId],[AccessPointId])
                VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetPlanPostPromoEffects' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetFilteredData' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'ExportXLSX' AND Resource = 'PlanPostPromoEffects'))
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='SuperReader');

                INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                ([Id],[RoleId],[AccessPointId])
                VALUES
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetPlanPostPromoEffects' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetFilteredData' AND Resource = 'PlanPostPromoEffects')),
                    (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'ExportXLSX' AND Resource = 'PlanPostPromoEffects'))
                GO
            ";
    }
}
