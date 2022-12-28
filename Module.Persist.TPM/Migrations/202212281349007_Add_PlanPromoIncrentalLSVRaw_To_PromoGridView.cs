namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using Module.Persist.TPM.Migrations.Views;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PlanPromoIncrentalLSVRaw_To_PromoGridView : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            Sql(ViewMigrations.GetPromoGridViewString(defaultSchema));
        }
        
        public override void Down()
        {
        }
    }
}
