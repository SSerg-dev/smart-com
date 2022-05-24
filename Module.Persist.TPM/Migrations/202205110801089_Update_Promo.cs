namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Promo : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.Promo", "PlanPromoBaselineVolume", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "PlanPromoPostPromoEffectVolume", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "PlanPromoPostPromoEffectVolumeW1", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "PlanPromoPostPromoEffectVolumeW2", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "PlanPromoIncrementalVolume", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "PlanPromoNetIncrementalVolume", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "ActualPromoBaselineVolume", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "ActualPromoPostPromoEffectVolume", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "ActualPromoVolumeByCompensation", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "ActualPromoVolumeSI", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "ActualPromoVolume", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "ActualPromoIncrementalVolume", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "ActualPromoNetIncrementalVolume", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "PlanPromoIncrementalCOGSTn", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "PlanPromoNetIncrementalCOGSTn", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "ActualPromoIncrementalCOGSTn", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "ActualPromoNetIncrementalCOGSTn", c => c.Double());
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropColumn($"{defaultSchema}.Promo", "ActualPromoNetIncrementalCOGSTn");
            DropColumn($"{defaultSchema}.Promo", "ActualPromoIncrementalCOGSTn");
            DropColumn($"{defaultSchema}.Promo", "PlanPromoNetIncrementalCOGSTn");
            DropColumn($"{defaultSchema}.Promo", "PlanPromoIncrementalCOGSTn");
            DropColumn($"{defaultSchema}.Promo", "ActualPromoNetIncrementalVolume");
            DropColumn($"{defaultSchema}.Promo", "ActualPromoIncrementalVolume");
            DropColumn($"{defaultSchema}.Promo", "ActualPromoVolume");
            DropColumn($"{defaultSchema}.Promo", "ActualPromoVolumeSI");
            DropColumn($"{defaultSchema}.Promo", "ActualPromoVolumeByCompensation");
            DropColumn($"{defaultSchema}.Promo", "ActualPromoPostPromoEffectVolume");
            DropColumn($"{defaultSchema}.Promo", "ActualPromoBaselineVolume");
            DropColumn($"{defaultSchema}.Promo", "PlanPromoNetIncrementalVolume");
            DropColumn($"{defaultSchema}.Promo", "PlanPromoIncrementalVolume");
            DropColumn($"{defaultSchema}.Promo", "PlanPromoPostPromoEffectVolumeW2");
            DropColumn($"{defaultSchema}.Promo", "PlanPromoPostPromoEffectVolumeW1");
            DropColumn($"{defaultSchema}.Promo", "PlanPromoPostPromoEffectVolume");
            DropColumn($"{defaultSchema}.Promo", "PlanPromoBaselineVolume");
        }
    }
}
