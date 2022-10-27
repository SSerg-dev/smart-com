namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fix_PromoProductsViews_Patch_AP : DbMigration
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
               SET [TPMmode] = 1
             WHERE Resource = 'PromoProductsViews' AND Action = 'Patch'
            GO
            ";
    }
}
