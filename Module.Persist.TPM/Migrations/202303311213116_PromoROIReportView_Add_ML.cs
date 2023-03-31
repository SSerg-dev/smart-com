namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using Module.Persist.TPM.Migrations.Views;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoROIReportView_Add_ML : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            Sql(ViewMigrations.UpdatePromoROIReportViewString(defaultSchema));
            Sql(ViewMigrations.UpdatePromoPriceIncreaseROIReportViewString(defaultSchema));
        }
        
        public override void Down()
        {
        }
    }
}
