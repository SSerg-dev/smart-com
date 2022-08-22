namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System.Data.Entity.Migrations;
    
    public partial class Update_PromoProduct_and_Promo : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.PromoProduct", "PlanProductBaselineVolume", c => c.Double());
            AddColumn($"{defaultSchema}.PromoProduct", "PlanProductPostPromoEffectVolumeW1", c => c.Double());
            AddColumn($"{defaultSchema}.PromoProduct", "PlanProductPostPromoEffectVolumeW2", c => c.Double());
            AddColumn($"{defaultSchema}.PromoProduct", "PlanProductPostPromoEffectVolume", c => c.Double());
            AddColumn($"{defaultSchema}.PromoProduct", "ActualProductQtySO", c => c.Double());
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropColumn($"{defaultSchema}.PromoProduct", "ActualProductQtySO");
            DropColumn($"{defaultSchema}.PromoProduct", "PlanProductPostPromoEffectVolume");
            DropColumn($"{defaultSchema}.PromoProduct", "PlanProductPostPromoEffectVolumeW2");
            DropColumn($"{defaultSchema}.PromoProduct", "PlanProductPostPromoEffectVolumeW1");
            DropColumn($"{defaultSchema}.PromoProduct", "PlanProductBaselineVolume");
        }
    }
}
