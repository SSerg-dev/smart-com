namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_NeedRecountUplift_PPI : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.Promo", "NeedRecountUpliftPI", c => c.Boolean(nullable: false));
            AddColumn($"{defaultSchema}.Promo", "PlanPromoUpliftPercentPI", c => c.Double());
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropColumn($"{defaultSchema}.Promo", "PlanPromoUpliftPercentPI");
            DropColumn($"{defaultSchema}.Promo", "NeedRecountUpliftPI");
        }
    }
}
