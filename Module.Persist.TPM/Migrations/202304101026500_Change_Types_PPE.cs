namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using Module.Persist.TPM.Migrations.Views;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_Types_PPE : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.PlanPostPromoEffect", "ModifiedDate", c => c.DateTimeOffset(precision: 7));
            AlterColumn($"{defaultSchema}.PlanPostPromoEffect", "PlanPostPromoEffectW1", c => c.Double(nullable: false));
            AlterColumn($"{defaultSchema}.PlanPostPromoEffect", "PlanPostPromoEffectW2", c => c.Double(nullable: false));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "PlanPostPromoEffect"));
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AlterColumn($"{defaultSchema}.PlanPostPromoEffect", "PlanPostPromoEffectW2", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn($"{defaultSchema}.PlanPostPromoEffect", "PlanPostPromoEffectW1", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn($"{defaultSchema}.PlanPostPromoEffect", "ModifiedDate");
        }
    }
}
