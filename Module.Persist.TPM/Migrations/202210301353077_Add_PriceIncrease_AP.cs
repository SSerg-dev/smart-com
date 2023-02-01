namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using Module.Persist.TPM.Migrations.Views;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PriceIncrease_AP : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            SqlString = SqlString.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlString);
            //Sql(ViewMigrations.UpdatePromoProductPriceIncreasesViewString(defaultSchema));
        }
        
        public override void Down()
        {
        }
        private string SqlString = @"
                INSERT INTO [DefaultSchemaSetting].[AccessPoint] 
                ([Disabled],[Resource],[Action],[TPMmode]) VALUES (0, 'HistoricalPromoProductCorrectionPriceIncreases',	'GetHistoricalPromoProductCorrectionPriceIncreases', 1)
                GO
                INSERT INTO [DefaultSchemaSetting].[AccessPoint] 
                ([Disabled],[Resource],[Action],[TPMmode]) VALUES (0, 'HistoricalPromoProductCorrectionPriceIncreases',	'GetFilteredData', 1)
                GO
                INSERT INTO [DefaultSchemaSetting].[AccessPoint] 
                ([Disabled],[Resource],[Action],[TPMmode]) VALUES (0, 'DeletedPromoProductCorrectionPriceIncreases',	'GetDeletedPromoProductCorrectionPriceIncreases', 1)
                GO
                INSERT INTO [DefaultSchemaSetting].[AccessPoint] 
                ([Disabled],[Resource],[Action],[TPMmode]) VALUES (0, 'DeletedPromoProductCorrectionPriceIncreases',	'GetFilteredData', 1)
                GO
                INSERT INTO [DefaultSchemaSetting].[AccessPoint] 
                ([Disabled],[Resource],[Action],[TPMmode]) VALUES (0, 'PromoProductPriceIncreases',	'GetPromoProductPriceIncreases', 1)
                GO
                INSERT INTO [DefaultSchemaSetting].[AccessPoint] 
                ([Disabled],[Resource],[Action],[TPMmode]) VALUES (0, 'PromoProductPriceIncreases',	'GetFilteredData', 1)
                GO
                INSERT INTO [DefaultSchemaSetting].[AccessPoint] 
                ([Disabled],[Resource],[Action],[TPMmode]) VALUES (0, 'PromoProductPriceIncreases',	'ExportXLSX', 1)
                GO
                INSERT INTO [DefaultSchemaSetting].[AccessPoint] 
                ([Disabled],[Resource],[Action],[TPMmode]) VALUES (0, 'PromoProductPriceIncreaseViews',	'GetPromoProductPriceIncreaseViews', 1)
                GO
                INSERT INTO [DefaultSchemaSetting].[AccessPoint] 
                ([Disabled],[Resource],[Action],[TPMmode]) VALUES (0, 'PromoProductPriceIncreaseViews',	'GetFilteredData', 1)
                GO
                INSERT INTO [DefaultSchemaSetting].[AccessPoint] 
                ([Disabled],[Resource],[Action],[TPMmode]) VALUES (0, 'PromoProductPriceIncreaseViews',	'Patch', 1)
                GO
                INSERT INTO [DefaultSchemaSetting].[AccessPoint] 
                ([Disabled],[Resource],[Action],[TPMmode]) VALUES (0, 'PromoProductPriceIncreaseViews',	'ExportXLSX', 1)
                GO
                INSERT INTO [DefaultSchemaSetting].[AccessPoint] 
                ([Disabled],[Resource],[Action],[TPMmode]) VALUES (0, 'PromoProductPriceIncreaseViews',	'FullImportXLSX', 1)
                GO
                INSERT INTO [DefaultSchemaSetting].[AccessPoint] 
                ([Disabled],[Resource],[Action],[TPMmode]) VALUES (0, 'PromoProductPriceIncreaseViews',	'DownloadTemplateXLSX', 1)
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CMManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoProductCorrectionPriceIncreases' and [Action]='GetHistoricalPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoProductCorrectionPriceIncreases' and [Action]='GetDeletedPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='GetPromoProductPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='GetPromoProductPriceIncreaseViews' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='Patch' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='FullImportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='DownloadTemplateXLSX' and [Disabled] = 0))
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'Administrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoProductCorrectionPriceIncreases' and [Action]='GetHistoricalPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoProductCorrectionPriceIncreases' and [Action]='GetDeletedPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='GetPromoProductPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='GetPromoProductPriceIncreaseViews' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='Patch' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='FullImportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='DownloadTemplateXLSX' and [Disabled] = 0))
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'FunctionalExpert');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoProductCorrectionPriceIncreases' and [Action]='GetHistoricalPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoProductCorrectionPriceIncreases' and [Action]='GetDeletedPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='GetPromoProductPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='GetPromoProductPriceIncreaseViews' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='Patch' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='FullImportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='DownloadTemplateXLSX' and [Disabled] = 0))
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandPlanning');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoProductCorrectionPriceIncreases' and [Action]='GetHistoricalPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoProductCorrectionPriceIncreases' and [Action]='GetDeletedPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='GetPromoProductPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='GetPromoProductPriceIncreaseViews' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='Patch' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='FullImportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='DownloadTemplateXLSX' and [Disabled] = 0))
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'KeyAccountManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoProductCorrectionPriceIncreases' and [Action]='GetHistoricalPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoProductCorrectionPriceIncreases' and [Action]='GetDeletedPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='GetPromoProductPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='GetPromoProductPriceIncreaseViews' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='Patch' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='FullImportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='DownloadTemplateXLSX' and [Disabled] = 0))
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SuperReader');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoProductCorrectionPriceIncreases' and [Action]='GetHistoricalPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoProductCorrectionPriceIncreases' and [Action]='GetDeletedPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='GetPromoProductPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='GetPromoProductPriceIncreaseViews' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='Patch' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='FullImportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='DownloadTemplateXLSX' and [Disabled] = 0))
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandFinance');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoProductCorrectionPriceIncreases' and [Action]='GetHistoricalPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoProductCorrectionPriceIncreases' and [Action]='GetDeletedPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='GetPromoProductPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='GetPromoProductPriceIncreaseViews' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='Patch' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='FullImportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='DownloadTemplateXLSX' and [Disabled] = 0))
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoProductCorrectionPriceIncreases' and [Action]='GetHistoricalPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoProductCorrectionPriceIncreases' and [Action]='GetDeletedPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='GetPromoProductPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='GetPromoProductPriceIncreaseViews' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='Patch' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='FullImportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='DownloadTemplateXLSX' and [Disabled] = 0))
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CustomerMarketing');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoProductCorrectionPriceIncreases' and [Action]='GetHistoricalPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoProductCorrectionPriceIncreases' and [Action]='GetDeletedPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='GetPromoProductPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='GetPromoProductPriceIncreaseViews' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='Patch' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='FullImportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='DownloadTemplateXLSX' and [Disabled] = 0))
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SupportAdministrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoProductCorrectionPriceIncreases' and [Action]='GetHistoricalPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoProductCorrectionPriceIncreases' and [Action]='GetDeletedPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='GetPromoProductPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreases' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='GetPromoProductPriceIncreaseViews' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='Patch' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='FullImportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductPriceIncreaseViews' and [Action]='DownloadTemplateXLSX' and [Disabled] = 0))
                GO
        ";
    }
}
