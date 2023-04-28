namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ActualProductPostPromoEffectLSVW12 : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.PromoProduct", "ActualProductPostPromoEffectLSVW1", c => c.Double());
            AddColumn($"{defaultSchema}.PromoProduct", "ActualProductPostPromoEffectLSVW2", c => c.Double());
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropColumn($"{defaultSchema}.PromoProduct", "ActualProductPostPromoEffectLSVW2");
            DropColumn($"{defaultSchema}.PromoProduct", "ActualProductPostPromoEffectLSVW1");
        }
    }
}
