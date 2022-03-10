namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Competitors_AP : DbMigration
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
				(0, 'Competitors',	'DownloadTemplateXLSX'),
				(0, 'Competitors',	'GetFilteredData'),
				(0, 'Competitors',	'GetCompetitors'),
				(0, 'Competitors',	'GetCompetitor'),
				(0, 'Competitors',	'Put'),
				(0, 'Competitors',	'Post'),
				(0, 'Competitors',	'Patch'),
				(0, 'Competitors',	'Delete'),
				(0, 'Competitors',	'ExportXLSX'),
				(0, 'Competitors',	'FullImportXLSX'),
				(0, 'DeletedCompetitors',	'GetFilteredData'),
				(0, 'DeletedCompetitors',	'GetDeletedCompetitors'),
				(0, 'DeletedCompetitors',	'GetDeletedCompetitor'),
				(0, 'HistoricalCompetitors',	'GetHistoricalCompetitors'),
				(0, 'HistoricalCompetitors',	'GetFilteredData')  
                GO
				DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'Administrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='DownloadTemplateXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='GetCompetitors')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='GetCompetitor')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='Put')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='Post')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='Patch')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='Delete')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='ExportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='FullImportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitors' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitors' and [Action]='GetDeletedCompetitors')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitors' and [Action]='GetDeletedCompetitor')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitors' and [Action]='GetHistoricalCompetitors')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitors' and [Action]='GetFilteredData'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'FunctionalExpert');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='DownloadTemplateXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='GetCompetitors')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='GetCompetitor')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='Put')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='Post')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='Patch')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='Delete')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='ExportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='FullImportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitors' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitors' and [Action]='GetDeletedCompetitors')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitors' and [Action]='GetDeletedCompetitor')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitors' and [Action]='GetHistoricalCompetitors')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitors' and [Action]='GetFilteredData'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SupportAdministrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='DownloadTemplateXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='GetCompetitors')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='GetCompetitor')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='Put')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='Post')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='Patch')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='Delete')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='ExportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='FullImportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitors' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitors' and [Action]='GetDeletedCompetitors')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitors' and [Action]='GetDeletedCompetitor')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitors' and [Action]='GetHistoricalCompetitors')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitors' and [Action]='GetFilteredData'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CustomerMarketing');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='DownloadTemplateXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='GetCompetitors')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='GetCompetitor')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='Put')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='Post')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='Patch')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='Delete')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='ExportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='FullImportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitors' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitors' and [Action]='GetDeletedCompetitors')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitors' and [Action]='GetDeletedCompetitor')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitors' and [Action]='GetHistoricalCompetitors')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitors' and [Action]='GetFilteredData'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'KeyAccountManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='DownloadTemplateXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='GetCompetitors')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='GetCompetitor')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='Put')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='Post')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='Patch')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='Delete')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='ExportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='FullImportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitors' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitors' and [Action]='GetDeletedCompetitors')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitors' and [Action]='GetDeletedCompetitor')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitors' and [Action]='GetHistoricalCompetitors')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitors' and [Action]='GetFilteredData'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CMManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='DownloadTemplateXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='GetCompetitors')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='GetCompetitor')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='Put')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='Post')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='Patch')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='Delete')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='ExportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='FullImportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitors' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitors' and [Action]='GetDeletedCompetitors')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitors' and [Action]='GetDeletedCompetitor')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitors' and [Action]='GetHistoricalCompetitors')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitors' and [Action]='GetFilteredData'))
        ";
    }
}
