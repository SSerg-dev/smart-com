namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_AccessPoints_For_RATIShopper : DbMigration
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
				(0, 'RATIShoppers',	'DownloadTemplateXLSX'),
				(0, 'RATIShoppers',	'GetFilteredData'),
				(0, 'RATIShoppers',	'GetRATIShoppers'),
				(0, 'RATIShoppers',	'GetRATIShopper'),
				(0, 'RATIShoppers',	'Put'),
				(0, 'RATIShoppers',	'Post'),
				(0, 'RATIShoppers',	'Patch'),
				(0, 'RATIShoppers',	'Delete'),
				(0, 'RATIShoppers',	'ExportXLSX'),
				(0, 'RATIShoppers',	'FullImportXLSX'),
				(0, 'DeletedRATIShoppers',	'GetFilteredData'),
				(0, 'DeletedRATIShoppers',	'GetDeletedRATIShoppers'),
				(0, 'DeletedRATIShoppers',	'GetDeletedRATIShopper'),
				(0, 'HistoricalRATIShoppers',	'GetHistoricalRATIShoppers'),
				(0, 'HistoricalRATIShoppers',	'GetFilteredData')";
	}
}
