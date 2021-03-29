namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;

    public partial class Change_Type_DMP_TT : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AlterColumn($"{defaultSchema}.NonPromoSupportDMP", "TT", c => c.String(nullable: false, maxLength: 255));
            AlterColumn($"{defaultSchema}.PromoSupportDMP", "TT", c => c.String(nullable: false, maxLength: 255));
        }

        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AlterColumn($"{defaultSchema}.PromoSupportDMP", "TT", c => c.Int(nullable: false));
            AlterColumn($"{defaultSchema}.NonPromoSupportDMP", "TT", c => c.Int(nullable: false));
        }
    }
}
