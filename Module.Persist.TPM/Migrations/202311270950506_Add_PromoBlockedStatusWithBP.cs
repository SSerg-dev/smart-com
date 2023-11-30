namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PromoBlockedStatusWithBP : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            RenameColumn($"{defaultSchema}.BlockedPromo", "PromoId", "PromoBlockedStatusId");
            CreateIndex($"{defaultSchema}.BlockedPromo", "PromoBlockedStatusId");
            AddForeignKey($"{defaultSchema}.BlockedPromo", "PromoBlockedStatusId", $"{defaultSchema}.PromoBlockedStatus", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.BlockedPromo", "PromoBlockedStatusId", $"{defaultSchema}.PromoBlockedStatus");
            DropIndex($"{defaultSchema}.BlockedPromo", new[] { "PromoBlockedStatusId" });
            RenameColumn($"{defaultSchema}.BlockedPromo", "PromoBlockedStatusId", "PromoId");
        }
    }
}
