namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using Module.Persist.TPM.Migrations.Views;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ROI_Reports_NSVtn : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            Sql(ViewMigrations.UpdatePromoPriceIncreaseROIReportViewString(defaultSchema));
            Sql(ViewMigrations.UpdatePromoROIReportViewString(defaultSchema));
        }
        
        public override void Down()
        {
        }
    }
}
