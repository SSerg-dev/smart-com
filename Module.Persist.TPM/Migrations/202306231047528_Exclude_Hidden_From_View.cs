namespace Module.Persist.TPM.Migrations
{
    using System.Data.Entity.Migrations;
    using Core.Settings;
    using Module.Persist.TPM.Migrations.Views;
    
    public partial class Exclude_Hidden_From_View : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            //Sql(ViewMigrations.GetPromoRSViewString(defaultSchema));
            //Sql(ViewMigrations.UpdateClientDashboardRSViewString(defaultSchema));
            Sql(ViewMigrations.UpdatePromoProductCorrectionViewString(defaultSchema));
        }
        
        public override void Down()
        {
        }
    }
}
