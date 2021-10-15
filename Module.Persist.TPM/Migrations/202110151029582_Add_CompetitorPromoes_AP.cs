namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_CompetitorPromoes_AP : DbMigration
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
                INSERT INTO [DefaultSchemaSetting].[AccessPoint] ([Disabled],[Resource],[Action]) VALUES
				(0, 'CompetitorPromoes',	'DownloadTemplateXLSX'),
				(0, 'CompetitorPromoes',	'GetFilteredData'),
				(0, 'CompetitorPromoes',	'GetCompetitorPromoes'),
				(0, 'CompetitorPromoes',	'GetCompetitorPromo'),
				(0, 'CompetitorPromoes',	'Put'),
				(0, 'CompetitorPromoes',	'Post'),
				(0, 'CompetitorPromoes',	'Patch'),
				(0, 'CompetitorPromoes',	'Delete'),
				(0, 'CompetitorPromoes',	'ExportXLSX'),
				(0, 'CompetitorPromoes',	'FullImportXLSX'),
				(0, 'DeletedCompetitorPromoes',	'GetFilteredData'),
				(0, 'DeletedCompetitorPromoes',	'GetDeletedCompetitorPromoes'),
				(0, 'DeletedCompetitorPromoes',	'GetDeletedCompetitorPromo'),
				(0, 'HistoricalCompetitorPromoes',	'GetHistoricalCompetitorPromoes'),
				(0, 'HistoricalCompetitorPromoes',	'GetFilteredData')  
                GO
				DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'Administrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='DownloadTemplateXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='GetCompetitorPromoes')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='GetCompetitorPromo')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='Put')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='Post')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='Patch')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='Delete')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='ExportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='FullImportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorPromoes' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorPromoes' and [Action]='GetDeletedCompetitorPromoes')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorPromoes' and [Action]='GetDeletedCompetitorPromo')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorPromoes' and [Action]='GetHistoricalCompetitorPromoes')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorPromoes' and [Action]='GetFilteredData'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SupportAdministrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='DownloadTemplateXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='GetCompetitorPromoes')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='GetCompetitorPromo')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='Put')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='Post')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='Patch')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='Delete')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='ExportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='FullImportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorPromoes' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorPromoes' and [Action]='GetDeletedCompetitorPromoes')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorPromoes' and [Action]='GetDeletedCompetitorPromo')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorPromoes' and [Action]='GetHistoricalCompetitorPromoes')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorPromoes' and [Action]='GetFilteredData'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CustomerMarketing');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='DownloadTemplateXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='GetCompetitorPromoes')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='GetCompetitorPromo')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='Put')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='Post')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='Patch')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='Delete')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='ExportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='FullImportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorPromoes' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorPromoes' and [Action]='GetDeletedCompetitorPromoes')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorPromoes' and [Action]='GetDeletedCompetitorPromo')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorPromoes' and [Action]='GetHistoricalCompetitorPromoes')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorPromoes' and [Action]='GetFilteredData'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'KeyAccountManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='DownloadTemplateXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='GetCompetitorPromoes')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='GetCompetitorPromo')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='Put')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='Post')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='Patch')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='Delete')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='ExportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='FullImportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorPromoes' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorPromoes' and [Action]='GetDeletedCompetitorPromoes')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorPromoes' and [Action]='GetDeletedCompetitorPromo')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorPromoes' and [Action]='GetHistoricalCompetitorPromoes')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorPromoes' and [Action]='GetFilteredData'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CMManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='DownloadTemplateXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='GetCompetitorPromoes')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='GetCompetitorPromo')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='Put')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='Post')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='Patch')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='Delete')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='ExportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='FullImportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorPromoes' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorPromoes' and [Action]='GetDeletedCompetitorPromoes')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorPromoes' and [Action]='GetDeletedCompetitorPromo')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorPromoes' and [Action]='GetHistoricalCompetitorPromoes')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorPromoes' and [Action]='GetFilteredData'))
        ";
    }
}
