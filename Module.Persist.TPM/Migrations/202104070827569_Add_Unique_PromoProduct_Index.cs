namespace Module.Persist.TPM.Migrations
{
    using System.Data.Entity.Migrations;
    using Core.Settings;

    public partial class Add_Unique_PromoProduct_Index : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropIndex($"{defaultSchema}.PromoProduct", new[] { "PromoId" });
            CreateIndex($"{defaultSchema}.PromoProduct", new[] { "Disabled", "DeletedDate", "PromoId", "ZREP" }, unique: true, name: "Unique_PromoProduct");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropIndex($"{defaultSchema}.PromoProduct", "Unique_PromoProduct");
            CreateIndex($"{defaultSchema}.PromoProduct", "PromoId");
        }
    }
}
