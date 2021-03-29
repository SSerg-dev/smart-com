namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameTTtoExternalCode : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            RenameColumn($"{defaultSchema}.NonPromoSupportDMP", "TT", "ExternalCode");
            RenameColumn($"{defaultSchema}.PromoSupportDMP", "TT", "ExternalCode");
        }

        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            RenameColumn($"{defaultSchema}.NonPromoSupportDMP", "ExternalCode", "TT");
            RenameColumn($"{defaultSchema}.PromoSupportDMP", "ExternalCode", "TT");
        }
    }
}
