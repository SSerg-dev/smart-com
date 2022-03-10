namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_CompetitorBrandTechs_AP : DbMigration
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
				(0, 'CompetitorBrandTechs',	'DownloadTemplateXLSX'),
				(0, 'CompetitorBrandTechs',	'GetFilteredData'),
				(0, 'CompetitorBrandTechs',	'GetCompetitorBrandTechs'),
				(0, 'CompetitorBrandTechs',	'GetCompetitorBrandTech'),
				(0, 'CompetitorBrandTechs',	'Put'),
				(0, 'CompetitorBrandTechs',	'Post'),
				(0, 'CompetitorBrandTechs',	'Patch'),
				(0, 'CompetitorBrandTechs',	'Delete'),
				(0, 'CompetitorBrandTechs',	'ExportXLSX'),
				(0, 'CompetitorBrandTechs',	'FullImportXLSX'),
				(0, 'DeletedCompetitorBrandTechs',	'GetFilteredData'),
				(0, 'DeletedCompetitorBrandTechs',	'GetDeletedCompetitorBrandTechs'),
				(0, 'DeletedCompetitorBrandTechs',	'GetDeletedCompetitorBrandTech'),
				(0, 'HistoricalCompetitorBrandTechs',	'GetHistoricalCompetitorBrandTechs'),
				(0, 'HistoricalCompetitorBrandTechs',	'GetFilteredData')  
                GO
				DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'Administrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='DownloadTemplateXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='GetCompetitorBrandTechs')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='GetCompetitorBrandTech')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='Put')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='Post')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='Patch')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='Delete')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='ExportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='FullImportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorBrandTechs' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorBrandTechs' and [Action]='GetDeletedCompetitorBrandTechs')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorBrandTechs' and [Action]='GetDeletedCompetitorBrandTech')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorBrandTechs' and [Action]='GetHistoricalCompetitorBrandTechs')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorBrandTechs' and [Action]='GetFilteredData'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'FunctionalExpert');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='DownloadTemplateXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='GetCompetitorBrandTechs')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='GetCompetitorBrandTech')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='Put')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='Post')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='Patch')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='Delete')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='ExportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='FullImportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorBrandTechs' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorBrandTechs' and [Action]='GetDeletedCompetitorBrandTechs')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorBrandTechs' and [Action]='GetDeletedCompetitorBrandTech')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorBrandTechs' and [Action]='GetHistoricalCompetitorBrandTechs')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorBrandTechs' and [Action]='GetFilteredData'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SupportAdministrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='DownloadTemplateXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='GetCompetitorBrandTechs')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='GetCompetitorBrandTech')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='Put')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='Post')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='Patch')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='Delete')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='ExportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='FullImportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorBrandTechs' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorBrandTechs' and [Action]='GetDeletedCompetitorBrandTechs')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorBrandTechs' and [Action]='GetDeletedCompetitorBrandTech')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorBrandTechs' and [Action]='GetHistoricalCompetitorBrandTechs')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorBrandTechs' and [Action]='GetFilteredData'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CustomerMarketing');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='DownloadTemplateXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='GetCompetitorBrandTechs')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='GetCompetitorBrandTech')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='Put')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='Post')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='Patch')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='Delete')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='ExportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='FullImportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorBrandTechs' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorBrandTechs' and [Action]='GetDeletedCompetitorBrandTechs')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorBrandTechs' and [Action]='GetDeletedCompetitorBrandTech')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorBrandTechs' and [Action]='GetHistoricalCompetitorBrandTechs')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorBrandTechs' and [Action]='GetFilteredData'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'KeyAccountManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='DownloadTemplateXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='GetCompetitorBrandTechs')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='GetCompetitorBrandTech')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='Put')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='Post')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='Patch')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='Delete')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='ExportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='FullImportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorBrandTechs' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorBrandTechs' and [Action]='GetDeletedCompetitorBrandTechs')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorBrandTechs' and [Action]='GetDeletedCompetitorBrandTech')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorBrandTechs' and [Action]='GetHistoricalCompetitorBrandTechs')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorBrandTechs' and [Action]='GetFilteredData'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CMManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='DownloadTemplateXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='GetCompetitorBrandTechs')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='GetCompetitorBrandTech')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='Put')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='Post')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='Patch')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='Delete')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='ExportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='FullImportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorBrandTechs' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorBrandTechs' and [Action]='GetDeletedCompetitorBrandTechs')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorBrandTechs' and [Action]='GetDeletedCompetitorBrandTech')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorBrandTechs' and [Action]='GetHistoricalCompetitorBrandTechs')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorBrandTechs' and [Action]='GetFilteredData'))
        ";
    }
}
