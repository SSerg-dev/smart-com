namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Increase_Promo_NSVtn : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoVolume", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNSVtn", c => c.Double());
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNSVtn");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoVolume");
        }
    }
}
