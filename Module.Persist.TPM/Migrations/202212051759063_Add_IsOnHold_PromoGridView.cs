namespace Module.Persist.TPM.Migrations
{
    using System.Data.Entity.Migrations;
    using Core.Settings;
    using Module.Persist.TPM.Migrations.Views;
    
    public partial class Add_IsOnHold_PromoGridView : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            //Sql(ViewMigrations.GetPromoGridViewString(defaultSchema));
        }
        
        public override void Down()
        {
        }
    }
}
