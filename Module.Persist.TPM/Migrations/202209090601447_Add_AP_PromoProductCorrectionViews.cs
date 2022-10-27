namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_AP_PromoProductCorrectionViews : DbMigration
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
				(0, 'PromoProductCorrectionViews',	'PromoProductCorrectionDelete', 1)
                GO
				DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'Administrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionViews' and [Action]='PromoProductCorrectionDelete'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SupportAdministrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionViews' and [Action]='PromoProductCorrectionDelete'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'FunctionalExpert');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionViews' and [Action]='PromoProductCorrectionDelete'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandPlanning');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductCorrectionViews' and [Action]='PromoProductCorrectionDelete'))
                GO
        ";
    }
}
