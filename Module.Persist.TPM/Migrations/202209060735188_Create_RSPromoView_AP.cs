namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_RSPromoView_AP : DbMigration
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
				(0, 'PromoRSViews',	'GetPromoRSView', 1)
                GO
        INSERT INTO [DefaultSchemaSetting].[AccessPoint] ([Disabled],[Resource],[Action],[TPMmode]) VALUES
				(0, 'PromoRSViews',	'GetPromoRSViews', 1)
                GO
        INSERT INTO [DefaultSchemaSetting].[AccessPoint] ([Disabled],[Resource],[Action],[TPMmode]) VALUES
				(0, 'PromoRSViews',	'ExportSchedule', 1)
                GO
        INSERT INTO [DefaultSchemaSetting].[AccessPoint] ([Disabled],[Resource],[Action],[TPMmode]) VALUES
				(0, 'PromoRSViews',	'GetFilteredData', 1)
                GO
        
		INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
			SELECT newid(), RoleId, 
				(select id from [DefaultSchemaSetting].[AccessPoint] where [Resource] = 'PromoRSViews' and [Action] = 'GetPromoRSView' and [Disabled] = 0)
			FROM [DefaultSchemaSetting].[AccessPoint] ap 
			JOIN [DefaultSchemaSetting].[AccessPointRole] apr on apr.AccessPointId = ap.id
			WHERE ap.[Resource] = 'PromoViews' and ap.[Action] = 'GetPromoView'
			GO
		
		INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
			SELECT newid(), RoleId, 
				(select id from [DefaultSchemaSetting].[AccessPoint] where [Resource] = 'PromoRSViews' and [Action] = 'GetPromoRSViews' and [Disabled] = 0)
			FROM [DefaultSchemaSetting].[AccessPoint] ap 
			JOIN [DefaultSchemaSetting].[AccessPointRole] apr on apr.AccessPointId = ap.id
			WHERE ap.[Resource] = 'PromoViews' and ap.[Action] = 'GetPromoViews'
			GO

		INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
			SELECT newid(), RoleId, 
				(select id from [DefaultSchemaSetting].[AccessPoint] where [Resource] = 'PromoRSViews' and [Action] = 'ExportSchedule' and [Disabled] = 0)
			FROM [DefaultSchemaSetting].[AccessPoint] ap 
			JOIN [DefaultSchemaSetting].[AccessPointRole] apr on apr.AccessPointId = ap.id
			WHERE ap.[Resource] = 'PromoViews' and ap.[Action] = 'ExportSchedule'
			GO

		INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
			SELECT newid(), RoleId, 
				(select id from [DefaultSchemaSetting].[AccessPoint] where [Resource] = 'PromoRSViews' and [Action] = 'GetFilteredData' and [Disabled] = 0)
			FROM [DefaultSchemaSetting].[AccessPoint] ap 
			JOIN [DefaultSchemaSetting].[AccessPointRole] apr on apr.AccessPointId = ap.id
			WHERE ap.[Resource] = 'PromoViews' and ap.[Action] = 'GetFilteredData'
			GO				
        ";
    }
}
