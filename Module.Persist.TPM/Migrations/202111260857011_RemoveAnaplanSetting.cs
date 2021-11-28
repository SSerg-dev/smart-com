namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveAnaplanSetting : DbMigration
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

		private string SqlString =
		@"
			DELETE FROM [DefaultSchemaSetting].[Setting] WHERE [Name] = 'ANAPLAN_BASELINE_CLIENTS'
		";
	}
}
