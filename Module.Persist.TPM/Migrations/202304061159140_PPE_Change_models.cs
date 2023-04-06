namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PPE_Change_models : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.PromoProduct", "PlanProductPostPromoEffectW1", c => c.Double(nullable: false));
            AddColumn($"{defaultSchema}.PromoProduct", "PlanProductPostPromoEffectW2", c => c.Double(nullable: false));
            DropColumn($"{defaultSchema}.ClientTree", "PostPromoEffectW1");
            DropColumn($"{defaultSchema}.ClientTree", "PostPromoEffectW2");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.ClientTree", "PostPromoEffectW2", c => c.Double());
            AddColumn($"{defaultSchema}.ClientTree", "PostPromoEffectW1", c => c.Double());
            DropColumn($"{defaultSchema}.PromoProduct", "PlanProductPostPromoEffectW2");
            DropColumn($"{defaultSchema}.PromoProduct", "PlanProductPostPromoEffectW1");
        }
    }
}
