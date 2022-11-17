namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using Module.Persist.TPM.Migrations.Views;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePromoROIReportView_IsApolloExport : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            Sql(ViewMigrations.UpdatePromoROIReportViewString(defaultSchema));
            Sql(SqlString);
        }
        
        public override void Down()
        {
        }
        private string SqlString = @"
            UPDATE [DefaultSchemaSetting].[AccessPoint]
               SET [TPMmode] = 1
             WHERE Resource = 'PromoProductsViews' AND Action = 'FullImportXLSX'
            GO

            UPDATE [DefaultSchemaSetting].[AccessPoint]
               SET [TPMmode] = 1
             WHERE Resource = 'PromoProductsViews' AND Action = 'DownloadTemplateXLSX'
            GO
            ";
    }
}
