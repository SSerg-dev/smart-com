namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Promo_MasterPromo_And_List : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.Promo", "MasterPromoId", c => c.Guid());
            CreateIndex($"{defaultSchema}.Promo", "MasterPromoId");
            AddForeignKey($"{defaultSchema}.Promo", "MasterPromoId", $"{defaultSchema}.Promo", "Id");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.Promo", "MasterPromoId", $"{defaultSchema}.Promo");
            DropIndex($"{defaultSchema}.Promo", new[] { "MasterPromoId" });
            DropColumn($"{defaultSchema}.Promo", "MasterPromoId");
        }
    }
}
