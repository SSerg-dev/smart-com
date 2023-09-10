namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Promo_NSVtn : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.Promo", "PlanPromoVolume", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "PlanPromoNSVtn", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "ActualPromoNSVtn", c => c.Double());
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropColumn($"{defaultSchema}.Promo", "ActualPromoNSVtn");
            DropColumn($"{defaultSchema}.Promo", "PlanPromoNSVtn");
            DropColumn($"{defaultSchema}.Promo", "PlanPromoVolume");
        }
    }
}
