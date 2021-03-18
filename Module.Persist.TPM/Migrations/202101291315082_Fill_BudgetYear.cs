namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;

    public partial class Fill_BudgetYear : DbMigration
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
				UPDATE [DefaultSchemaSetting].[Promo] SET [DefaultSchemaSetting].[Promo].[BudgetYear]=YEAR([DefaultSchemaSetting].[Promo].[DispatchesStart]) WHERE [DefaultSchemaSetting].[Promo].[IsOnInvoice]=1
				UPDATE [DefaultSchemaSetting].[Promo] SET [DefaultSchemaSetting].[Promo].[BudgetYear]=YEAR([DefaultSchemaSetting].[Promo].[StartDate]) WHERE [DefaultSchemaSetting].[Promo].[IsOnInvoice]=0";
	}
}
