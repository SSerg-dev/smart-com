namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Data_For_COGSTn : DbMigration
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
            DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SupportAdministrator');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetPlanCOGSTn')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetPlanCOGSTns')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Post')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Put')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Patch')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Delete')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='ExportXLSX')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='FullImportXLSX')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='DownloadTemplateXLSX')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTns')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTn')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTns'))

            DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'Administrator');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetPlanCOGSTn')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetPlanCOGSTns')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Post')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Put')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Patch')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Delete')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='ExportXLSX')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='FullImportXLSX')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='DownloadTemplateXLSX')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTns')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTn')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTns'))

            DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandFinance');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetPlanCOGSTn')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetPlanCOGSTns')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Post')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Put')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Patch')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Delete')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='ExportXLSX')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='FullImportXLSX')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='DownloadTemplateXLSX')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTns')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTn')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTns'))

            DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SuperReader');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetPlanCOGSTn')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetPlanCOGSTns')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='ExportXLSX')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='FullImportXLSX')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTns')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTn')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTns'))

            DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'FunctionalExpert');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetPlanCOGSTn')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetPlanCOGSTns')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Post')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Put')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Patch')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='Delete')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='ExportXLSX')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='FullImportXLSX')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='DownloadTemplateXLSX'))

            DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CustomerMarketing');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetPlanCOGSTns')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='ExportXLSX'))

            DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CMManager');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetPlanCOGSTns')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='ExportXLSX'))

            DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandPlanning');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTns')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTn')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTns'))

            DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'KeyAccountManager');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetFilteredData'))
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
            DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SupportAdministrator');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetActualCOGSTn')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetActualCOGSTns')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Post')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Put')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Patch')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Delete')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='ExportXLSX')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='FullImportXLSX')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='DownloadTemplateXLSX')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTns')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTn')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSTns' and [Action]='GetDeletedActualCOGSTns'))

            DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'Administrator');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetActualCOGSTn')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetActualCOGSTns')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Post')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Put')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Patch')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Delete')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='ExportXLSX')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='FullImportXLSX')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='DownloadTemplateXLSX')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTns')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTn')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSTns' and [Action]='GetDeletedActualCOGSTns'))

            DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandFinance');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetActualCOGSTn')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetActualCOGSTns')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Post')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Put')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Patch')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Delete')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='ExportXLSX')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='FullImportXLSX')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='DownloadTemplateXLSX')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTns')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTn')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSTns' and [Action]='GetDeletedActualCOGSTns'))

            DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SuperReader');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetActualCOGSTn')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetActualCOGSTns')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='ExportXLSX')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='FullImportXLSX')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTns')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTn')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSTns' and [Action]='GetDeletedActualCOGSTns'))

            DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'FunctionalExpert');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetActualCOGSTn')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetActualCOGSTns')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Post')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Put')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Patch')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='Delete')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='ExportXLSX')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='FullImportXLSX')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='DownloadTemplateXLSX'))

            DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CustomerMarketing');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetActualCOGSTns')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='ExportXLSX'))

            DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CMManager');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetActualCOGSTns')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetFilteredData')),
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='ExportXLSX'))

            DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandPlanning');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetFilteredData'))

            DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'KeyAccountManager');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetFilteredData'))
            GO
        ";
    }
}
