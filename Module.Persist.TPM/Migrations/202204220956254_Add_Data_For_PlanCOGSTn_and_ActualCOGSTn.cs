namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Data_For_PlanCOGSTn_and_ActualCOGSTn : DbMigration
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
            (Resource, Action, Disabled, DeletedDate) VALUES 
            ('PlanCOGSTns', 'GetPlanCOGSTn', 0, NULL),
            ('PlanCOGSTns', 'GetPlanCOGSTns', 0, NULL),
            ('PlanCOGSTns', 'GetFilteredData', 0, NULL),
            ('PlanCOGSTns', 'Post', 0, NULL),
            ('PlanCOGSTns', 'Put', 0, NULL),
            ('PlanCOGSTns', 'Patch', 0, NULL),
            ('PlanCOGSTns', 'Delete', 0, NULL),
            ('PlanCOGSTns', 'ExportXLSX', 0, NULL),
            ('PlanCOGSTns', 'FullImportXLSX', 0, NULL),
            ('PlanCOGSTns', 'DownloadTemplateXLSX', 0, NULL)
            GO
            DECLARE @RoleIdPSA uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SupportAdministrator');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleIdPSA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetPlanCOGSTn')),
            (@RoleIdPSA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetPlanCOGSTns')),
            (@RoleIdPSA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdPSA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Post')),
            (@RoleIdPSA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Put')),
            (@RoleIdPSA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Patch')),
            (@RoleIdPSA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Delete')),
            (@RoleIdPSA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='ExportXLSX')),
            (@RoleIdPSA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='FullImportXLSX')),
            (@RoleIdPSA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='DownloadTemplateXLSX')),
            (@RoleIdPSA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdPSA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTns')),
            (@RoleIdPSA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTn')),
            (@RoleIdPSA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdPSA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetHistoricalPlanCOGSTns'))

            DECLARE @RoleIdPA uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'Administrator');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleIdPA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetPlanCOGSTn')),
            (@RoleIdPA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetPlanCOGSTns')),
            (@RoleIdPA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdPA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Post')),
            (@RoleIdPA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Put')),
            (@RoleIdPA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Patch')),
            (@RoleIdPA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Delete')),
            (@RoleIdPA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='ExportXLSX')),
            (@RoleIdPA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='FullImportXLSX')),
            (@RoleIdPA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='DownloadTemplateXLSX')),
            (@RoleIdPA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdPA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTns')),
            (@RoleIdPA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTn')),
            (@RoleIdPA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdPA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetHistoricalPlanCOGSTns'))

            DECLARE @RoleIdPDF uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandFinance');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleIdPDF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetPlanCOGSTn')),
            (@RoleIdPDF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetPlanCOGSTns')),
            (@RoleIdPDF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdPDF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Post')),
            (@RoleIdPDF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Put')),
            (@RoleIdPDF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Patch')),
            (@RoleIdPDF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Delete')),
            (@RoleIdPDF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='ExportXLSX')),
            (@RoleIdPDF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='FullImportXLSX')),
            (@RoleIdPDF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='DownloadTemplateXLSX')),
            (@RoleIdPDF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdPDF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTns')),
            (@RoleIdPDF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTn')),
            (@RoleIdPDF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdPDF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetHistoricalPlanCOGSTns'))

            DECLARE @RoleIdPSR uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SuperReader');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleIdPSR, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetPlanCOGSTn')),
            (@RoleIdPSR, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetPlanCOGSTns')),
            (@RoleIdPSR, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdPSR, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='ExportXLSX')),
            (@RoleIdPSR, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='FullImportXLSX')),
            (@RoleIdPSR, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdPSR, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTns')),
            (@RoleIdPSR, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTn')),
            (@RoleIdPSR, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdPSR, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetHistoricalPlanCOGSTns'))

            DECLARE @RoleIdPFE uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'FunctionalExpert');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleIdPFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetPlanCOGSTn')),
            (@RoleIdPFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetPlanCOGSTns')),
            (@RoleIdPFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdPFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Post')),
            (@RoleIdPFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Put')),
            (@RoleIdPFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Patch')),
            (@RoleIdPFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Delete')),
            (@RoleIdPFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='ExportXLSX')),
            (@RoleIdPFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='FullImportXLSX')),
            (@RoleIdPFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='DownloadTemplateXLSX')),
            (@RoleIdPFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdPFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTns')),
            (@RoleIdPFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTn')),
            (@RoleIdPFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdPFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetHistoricalPlanCOGSTns'))

            DECLARE @RoleIdPCM uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CustomerMarketing');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleIdPCM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetPlanCOGSTns')),
            (@RoleIdPCM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdPCM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='ExportXLSX')),
            (@RoleIdPCM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdPCM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTns')),
            (@RoleIdPCM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdPCM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetHistoricalPlanCOGSTns'))

            DECLARE @RoleIdPCMM uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CMManager');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleIdPCMM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetPlanCOGSTns')),
            (@RoleIdPCMM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdPCMM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='ExportXLSX')),
            (@RoleIdPCMM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdPCMM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTns')),
            (@RoleIdPCMM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdPCMM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetHistoricalPlanCOGSTns'))

            DECLARE @RoleIdPDP uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandPlanning');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleIdPDP, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdPDP, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdPDP, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetFilteredData'))

            DECLARE @RoleIdPKAM uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'KeyAccountManager');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleIdPKAM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdPKAM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdPKAM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetFilteredData'))
            GO
            INSERT INTO [DefaultSchemaSetting].[AccessPoint]
            (Resource, Action, Disabled, DeletedDate) VALUES 
            ('ActualCOGSTns', 'GetActualCOGSTn', 0, NULL),
            ('ActualCOGSTns', 'GetActualCOGSTns', 0, NULL),
            ('ActualCOGSTns', 'GetFilteredData', 0, NULL),
            ('ActualCOGSTns', 'Post', 0, NULL),
            ('ActualCOGSTns', 'Put', 0, NULL),
            ('ActualCOGSTns', 'Patch', 0, NULL),
            ('ActualCOGSTns', 'Delete', 0, NULL),
            ('ActualCOGSTns', 'ExportXLSX', 0, NULL),
            ('ActualCOGSTns', 'FullImportXLSX', 0, NULL),
            ('ActualCOGSTns', 'DownloadTemplateXLSX', 0, NULL)
            GO
            DECLARE @RoleIdASA uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SupportAdministrator');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleIdASA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetActualCOGSTn')),
            (@RoleIdASA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetActualCOGSTns')),
            (@RoleIdASA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdASA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Post')),
            (@RoleIdASA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Put')),
            (@RoleIdASA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Patch')),
            (@RoleIdASA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Delete')),
            (@RoleIdASA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='ExportXLSX')),
            (@RoleIdASA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='FullImportXLSX')),
            (@RoleIdASA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='DownloadTemplateXLSX')),
            (@RoleIdASA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdASA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTns')),
            (@RoleIdASA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTn')),
            (@RoleIdASA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdASA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSTns' and [Action]='GetHistoricalActualCOGSTns'))

            DECLARE @RoleIdAA uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'Administrator');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleIdAA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetActualCOGSTn')),
            (@RoleIdAA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetActualCOGSTns')),
            (@RoleIdAA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdAA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Post')),
            (@RoleIdAA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Put')),
            (@RoleIdAA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Patch')),
            (@RoleIdAA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Delete')),
            (@RoleIdAA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='ExportXLSX')),
            (@RoleIdAA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='FullImportXLSX')),
            (@RoleIdAA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='DownloadTemplateXLSX')),
            (@RoleIdAA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdAA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTns')),
            (@RoleIdAA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTn')),
            (@RoleIdAA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdAA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSTns' and [Action]='GetHistoricalActualCOGSTns'))

            DECLARE @RoleIdADF uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandFinance');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleIdADF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetActualCOGSTn')),
            (@RoleIdADF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetActualCOGSTns')),
            (@RoleIdADF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdADF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Post')),
            (@RoleIdADF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Put')),
            (@RoleIdADF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Patch')),
            (@RoleIdADF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Delete')),
            (@RoleIdADF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='ExportXLSX')),
            (@RoleIdADF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='FullImportXLSX')),
            (@RoleIdADF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='DownloadTemplateXLSX')),
            (@RoleIdADF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdADF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTns')),
            (@RoleIdADF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTn')),
            (@RoleIdADF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdADF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSTns' and [Action]='GetHistoricalActualCOGSTns'))

            DECLARE @RoleIdASR uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SuperReader');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleIdASR, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetActualCOGSTn')),
            (@RoleIdASR, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetActualCOGSTns')),
            (@RoleIdASR, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdASR, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='ExportXLSX')),
            (@RoleIdASR, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='FullImportXLSX')),
            (@RoleIdASR, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdASR, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTns')),
            (@RoleIdASR, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTn')),
            (@RoleIdASR, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdASR, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSTns' and [Action]='GetHistoricalActualCOGSTns'))

            DECLARE @RoleIdAFE uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'FunctionalExpert');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleIdAFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetActualCOGSTn')),
            (@RoleIdAFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetActualCOGSTns')),
            (@RoleIdAFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdAFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Post')),
            (@RoleIdAFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Put')),
            (@RoleIdAFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Patch')),
            (@RoleIdAFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Delete')),
            (@RoleIdAFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='ExportXLSX')),
            (@RoleIdAFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='FullImportXLSX')),
            (@RoleIdAFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='DownloadTemplateXLSX')),
            (@RoleIdAFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdAFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTns')),
            (@RoleIdAFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTn')),
            (@RoleIdAFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdAFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSTns' and [Action]='GetHistoricalActualCOGSTns'))

            DECLARE @RoleIdACM uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CustomerMarketing');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleIdACM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetActualCOGSTns')),
            (@RoleIdACM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdACM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='ExportXLSX')),
            (@RoleIdACM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdACM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTns')),
            (@RoleIdACM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTn')),
            (@RoleIdACM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdACM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSTns' and [Action]='GetHistoricalActualCOGSTns'))

            DECLARE @RoleIdACMM uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CMManager');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleIdACMM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetActualCOGSTns')),
            (@RoleIdACMM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdACMM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='ExportXLSX')),
            (@RoleIdACMM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdACMM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTns')),
            (@RoleIdACMM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTn')),
            (@RoleIdACMM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSTns' and [Action]='GetHistoricalActualCOGSTns'))

            DECLARE @RoleIdADP uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandPlanning');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleIdADP, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdADP, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdADP, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTns')),
            (@RoleIdADP, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTn')),
            (@RoleIdADP, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdADP, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSTns' and [Action]='GetHistoricalActualCOGSTns'))

            DECLARE @RoleIdAKAM uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'KeyAccountManager');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleIdAKAM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdAKAM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdAKAM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTns')),
            (@RoleIdAKAM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTn')),
            (@RoleIdAKAM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleIdAKAM, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSTns' and [Action]='GetHistoricalActualCOGSTns'))
            GO
        ";
    }
}
