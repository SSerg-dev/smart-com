namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using Module.Persist.TPM.Migrations.Views;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PI_PromoProductCorrectionPriceIncreaseView : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            Sql(ViewMigrations.UpdatePromoProductCorrectionPriceIncreaseViewString(defaultSchema));
        }
        
        public override void Down()
        {
        }
    }
}
