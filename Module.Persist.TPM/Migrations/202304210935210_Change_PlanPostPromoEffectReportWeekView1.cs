namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using Module.Persist.TPM.Migrations.Views;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_PlanPostPromoEffectReportWeekView1 : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            Sql(ViewMigrations.UpdatePlanPostPromoEffectReportWeekViewString(defaultSchema));
        }
        
        public override void Down()
        {
        }
    }
}
