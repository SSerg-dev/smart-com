namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PromoProductCorrectionPriceIncreacseViews_AP : DbMigration
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
            INSERT INTO [DefaultSchemaSetting].[AccessPoint] ([Disabled],[Resource],[Action],[TPMmode]) VALUES
				(0, 'PromoProductCorrectionPriceIncreaseViews',	'GetPromoProductCorrectionViews', 1)
                GO
				DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'Administrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='GetPromoProductCorrectionViews'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CustomerMarketing');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='GetPromoProductCorrectionViews'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CMManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='GetPromoProductCorrectionViews'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SuperReader');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='GetPromoProductCorrectionViews'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'KeyAccountManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='GetPromoProductCorrectionViews'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandFinance');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='GetPromoProductCorrectionViews'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SupportAdministrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='GetPromoProductCorrectionViews'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'FunctionalExpert');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='GetPromoProductCorrectionViews'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandPlanning');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='GetPromoProductCorrectionViews'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='GetPromoProductCorrectionViews'))
                GO

            INSERT INTO [DefaultSchemaSetting].[AccessPoint] ([Disabled],[Resource],[Action],[TPMmode]) VALUES
				(0, 'PromoProductCorrectionPriceIncreaseViews',	'GetFilteredData', 1)
                GO
				DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'Administrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='GetFilteredData'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CustomerMarketing');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='GetFilteredData'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CMManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='GetFilteredData'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SuperReader');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='GetFilteredData'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'KeyAccountManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='GetFilteredData'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandFinance');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='GetFilteredData'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SupportAdministrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='GetFilteredData'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'FunctionalExpert');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='GetFilteredData'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandPlanning');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='GetFilteredData'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='GetFilteredData'))
                GO

            INSERT INTO [DefaultSchemaSetting].[AccessPoint] ([Disabled],[Resource],[Action],[TPMmode]) VALUES
				(0, 'PromoProductCorrectionPriceIncreaseViews',	'ExportXLSX', 1)
                GO
				DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'Administrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='ExportXLSX'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CustomerMarketing');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='ExportXLSX'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CMManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='ExportXLSX'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SuperReader');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='ExportXLSX'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'KeyAccountManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='ExportXLSX'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandFinance');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='ExportXLSX'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SupportAdministrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='ExportXLSX'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'FunctionalExpert');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='ExportXLSX'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandPlanning');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='ExportXLSX'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='ExportXLSX'))
                GO

            INSERT INTO [DefaultSchemaSetting].[AccessPoint] ([Disabled],[Resource],[Action],[TPMmode]) VALUES
				(0, 'PromoProductCorrectionPriceIncreaseViews',	'ExportCorrectionXLSX', 1)
                GO
				DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'Administrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='ExportCorrectionXLSX'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CustomerMarketing');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='ExportCorrectionXLSX'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CMManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='ExportCorrectionXLSX'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SuperReader');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='ExportCorrectionXLSX'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'KeyAccountManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='ExportCorrectionXLSX'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandFinance');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='ExportCorrectionXLSX'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SupportAdministrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='ExportCorrectionXLSX'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'FunctionalExpert');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='ExportCorrectionXLSX'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandPlanning');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='ExportCorrectionXLSX'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='ExportCorrectionXLSX'))
                GO

            INSERT INTO [DefaultSchemaSetting].[AccessPoint] ([Disabled],[Resource],[Action],[TPMmode]) VALUES
				(0, 'PromoProductCorrectionPriceIncreaseViews',	'Put', 1)
                GO
				DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'Administrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='Put'))
                GO                
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SupportAdministrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='Put'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'FunctionalExpert');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='Put'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandPlanning');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='Put'))
                GO

            INSERT INTO [DefaultSchemaSetting].[AccessPoint] ([Disabled],[Resource],[Action],[TPMmode]) VALUES
				(0, 'PromoProductCorrectionPriceIncreaseViews',	'Post', 1)
                GO
				DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'Administrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='Post'))
                GO                
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SupportAdministrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='Post'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'FunctionalExpert');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='Post'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandPlanning');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='Post'))
                GO

            INSERT INTO [DefaultSchemaSetting].[AccessPoint] ([Disabled],[Resource],[Action],[TPMmode]) VALUES
				(0, 'PromoProductCorrectionPriceIncreaseViews',	'Patch', 1)
                GO
				DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'Administrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='Patch'))
                GO                
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SupportAdministrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='Patch'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'FunctionalExpert');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='Patch'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandPlanning');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='Patch'))
                GO

            INSERT INTO [DefaultSchemaSetting].[AccessPoint] ([Disabled],[Resource],[Action],[TPMmode]) VALUES
				(0, 'PromoProductCorrectionPriceIncreaseViews',	'Delete', 1)
                GO
				DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'Administrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='Delete'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SupportAdministrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='Delete'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'FunctionalExpert');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='Delete'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandPlanning');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='Delete'))
                GO

            INSERT INTO [DefaultSchemaSetting].[AccessPoint] ([Disabled],[Resource],[Action],[TPMmode]) VALUES
				(0, 'PromoProductCorrectionPriceIncreaseViews',	'FullImportXLSX', 1)
                GO
				DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'Administrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='FullImportXLSX'))
                GO                
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SupportAdministrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='FullImportXLSX'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'FunctionalExpert');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='FullImportXLSX'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandPlanning');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='FullImportXLSX'))
                GO

            INSERT INTO [DefaultSchemaSetting].[AccessPoint] ([Disabled],[Resource],[Action],[TPMmode]) VALUES
				(0, 'PromoProductCorrectionPriceIncreaseViews',	'DownloadTemplateXLSX', 1)
                GO
				DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'Administrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='DownloadTemplateXLSX'))
                GO                
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SupportAdministrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='DownloadTemplateXLSX'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'FunctionalExpert');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='DownloadTemplateXLSX'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandPlanning');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreaseViews' and [Action]='DownloadTemplateXLSX'))
                GO
        ";
    }
}
