namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CHange_PromoProductsViews_AP2 : DbMigration
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
            UPDATE [DefaultSchemaSetting].[AccessPoint]
               SET [TPMmode] = 0
             WHERE Resource = 'PromoProductsViews' AND Action = 'FullImportXLSX'
            GO

            UPDATE [DefaultSchemaSetting].[AccessPoint]
               SET [TPMmode] = 0
             WHERE Resource = 'PromoProductsViews' AND Action = 'DownloadTemplateXLSX'
            GO
            ";
    }
}
