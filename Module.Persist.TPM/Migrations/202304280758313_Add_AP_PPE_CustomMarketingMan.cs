namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_AP_PPE_CustomMarketingMan : DbMigration
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
            DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='CMManager');

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
                (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'FullImportXLSX' AND Resource = 'PlanPostPromoEffects')),
                (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetHistoricalPlanPostPromoEffects' AND Resource = 'HistoricalPlanPostPromoEffects')),
                (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetFilteredData' AND Resource = 'HistoricalPlanPostPromoEffects')),
                (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetDeletedPlanPostPromoEffects' AND Resource = 'DeletedPlanPostPromoEffects')),
                (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetDeletedPlanPostPromoEffect' AND Resource = 'DeletedPlanPostPromoEffects')),
                (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetFilteredData' AND Resource = 'DeletedPlanPostPromoEffects')),
                (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetDiscountRanges' AND Resource = 'DiscountRanges')),
                (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetDurationRanges' AND Resource = 'DurationRanges')),
                (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetBrandTechSizes' AND Resource = 'PlanPostPromoEffects'))
            GO
        ";
    }
}
