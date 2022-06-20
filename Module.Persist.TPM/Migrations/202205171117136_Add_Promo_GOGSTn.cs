namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Promo_GOGSTn : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.Promo", "PlanCOGSTn", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "ActualCOGSTn", c => c.Double());
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropColumn($"{defaultSchema}.Promo", "ActualCOGSTn");
            DropColumn($"{defaultSchema}.Promo", "PlanCOGSTn");
        }
    }
}
