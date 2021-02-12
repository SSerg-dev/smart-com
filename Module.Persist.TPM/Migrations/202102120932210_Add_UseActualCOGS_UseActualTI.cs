namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_UseActualCOGS_UseActualTI : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.Promo", "UseActualTI", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn($"{defaultSchema}.Promo", "UseActualCOGS", c => c.Boolean(nullable: false, defaultValue: false));
            DropColumn($"{defaultSchema}.Promo", "UseActualCOGSTI");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.Promo", "UseActualCOGSTI", c => c.Boolean(nullable: false, defaultValue: false));
            DropColumn($"{defaultSchema}.Promo", "UseActualCOGS");
            DropColumn($"{defaultSchema}.Promo", "UseActualTI");
        }
    }
}
