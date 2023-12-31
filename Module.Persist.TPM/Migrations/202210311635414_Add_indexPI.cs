namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_indexPI : DbMigration
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
                ([Disabled],[Resource],[Action],[TPMmode]) VALUES (0, 'PromoProductCorrectionPriceIncreases',	'Patch', 1)
                GO
                INSERT INTO [DefaultSchemaSetting].[AccessPoint] 
                ([Disabled],[Resource],[Action],[TPMmode]) VALUES (0, 'PromoProductCorrectionPriceIncreases',	'Post', 1)
                GO
                INSERT INTO [DefaultSchemaSetting].[AccessPoint] 
                ([Disabled],[Resource],[Action],[TPMmode]) VALUES (0, 'PromoProductCorrectionPriceIncreases',	'GetFilteredData', 1)
                GO
                INSERT INTO [DefaultSchemaSetting].[AccessPoint] 
                ([Disabled],[Resource],[Action],[TPMmode]) VALUES (0, 'PromoProductCorrectionPriceIncreases',	'GetPromoProductCorrectionPriceIncreases', 1)
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CMManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='GetPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='Patch' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='Post' and [Disabled] = 0))
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'Administrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='GetPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='Patch' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='Post' and [Disabled] = 0))
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'FunctionalExpert');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='GetPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='Patch' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='Post' and [Disabled] = 0))
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandPlanning');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='GetPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='Patch' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='Post' and [Disabled] = 0))
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'KeyAccountManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='GetPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='Patch' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='Post' and [Disabled] = 0))
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SuperReader');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='GetPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='Patch' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='Post' and [Disabled] = 0))
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandFinance');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='GetPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='Patch' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='Post' and [Disabled] = 0))
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='GetPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='Patch' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='Post' and [Disabled] = 0))
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CustomerMarketing');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='GetPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='Patch' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='Post' and [Disabled] = 0))
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SupportAdministrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='GetPromoProductCorrectionPriceIncreases' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='Patch' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionPriceIncreases' and [Action]='Post' and [Disabled] = 0))
                GO
        ";
    }
}
