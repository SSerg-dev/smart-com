namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using Module.Persist.TPM.Migrations.Views;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PromoPriceIncreaseROIReportView : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            //Sql(ViewMigrations.UpdatePromoPriceIncreaseROIReportViewString(defaultSchema));
            SqlString = SqlString.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlString);
        }
        
        public override void Down()
        {

        }
        private string SqlString = @"
                INSERT INTO [DefaultSchemaSetting].[AccessPoint]
                     VALUES
                           (NEWID(), 0, NULL, 'PromoPriceIncreaseROIReports', 'GetPromoPriceIncreaseROIReports', NULL, 1)
                GO
                INSERT INTO [DefaultSchemaSetting].[AccessPoint]
                     VALUES
                           (NEWID(), 0, NULL, 'PromoPriceIncreaseROIReports', 'ExportXLSX', NULL, 1)
                GO
                INSERT INTO [DefaultSchemaSetting].[AccessPoint]
                     VALUES
                           (NEWID(), 0, NULL, 'PromoPriceIncreaseROIReports', 'GetFilteredData', NULL, 1)
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='GetPromoPriceIncreaseROIReports' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='GetFilteredData' and [Disabled] = 0))
            GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'Administrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='GetPromoPriceIncreaseROIReports' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='GetFilteredData' and [Disabled] = 0))
            GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandFinance');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='GetPromoPriceIncreaseROIReports' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='GetFilteredData' and [Disabled] = 0))
            GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandPlanning');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='GetPromoPriceIncreaseROIReports' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='GetFilteredData' and [Disabled] = 0))
            GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CMManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='GetPromoPriceIncreaseROIReports' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='GetFilteredData' and [Disabled] = 0))
            GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'FunctionalExpert');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='GetPromoPriceIncreaseROIReports' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='GetFilteredData' and [Disabled] = 0))
            GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CustomerMarketing');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='GetPromoPriceIncreaseROIReports' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='GetFilteredData' and [Disabled] = 0))
            GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SuperReader');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='GetPromoPriceIncreaseROIReports' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='GetFilteredData' and [Disabled] = 0))
            GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SupportAdministrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='GetPromoPriceIncreaseROIReports' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='GetFilteredData' and [Disabled] = 0))
            GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'KeyAccountManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='GetPromoPriceIncreaseROIReports' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoPriceIncreaseROIReports' and [Action]='GetFilteredData' and [Disabled] = 0))
            GO
            ";
    }
}
