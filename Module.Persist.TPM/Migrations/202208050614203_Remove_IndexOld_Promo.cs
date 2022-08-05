namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using Module.Persist.TPM.Migrations.Views;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_IndexOld_Promo : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropIndex($"{defaultSchema}.Promo", new[] { "PromoNumberUQ" });
            Sql(ViewMigrations.GetPromoInsertTriggerString(defaultSchema));
        }
        
        public override void Down()
        {
        }
    }
}
