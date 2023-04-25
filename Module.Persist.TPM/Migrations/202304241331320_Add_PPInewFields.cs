namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PPInewFields : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoPostPromoEffectLSVW1", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoPostPromoEffectLSVW2", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoPostPromoEffectVolumeW1", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoPostPromoEffectVolumeW2", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoPostPromoEffectVolume", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoBaselineVolume", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoTIShopper", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoCost", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoIncrementalCOGS", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoBaseTI", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoIncrementalBaseTI", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoTotalCost", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetIncrementalLSV", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetLSV", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetIncrementalBaseTI", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetIncrementalCOGS", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetBaseTI", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoBaselineBaseTI", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNSV", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoIncrementalNSV", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetIncrementalNSV", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetIncrementalMAC", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoIncrementalVolume", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetIncrementalVolume", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetNSV", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoIncrementalCOGSTn", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetIncrementalCOGSTn", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoIncrementalMAC", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoIncrementalEarnings", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetIncrementalEarnings", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoROIPercent", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetROIPercent", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetIncrementalMACLSV", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoIncrementalMACLSV", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoIncrementalEarningsLSV", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetIncrementalEarningsLSV", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoROIPercentLSV", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetROIPercentLSV", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanAddTIShopperCalculated", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanAddTIShopperApproved", c => c.Double());
            AddColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetUpliftPercent", c => c.Double());
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetUpliftPercent");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanAddTIShopperApproved");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanAddTIShopperCalculated");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetROIPercentLSV");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoROIPercentLSV");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetIncrementalEarningsLSV");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoIncrementalEarningsLSV");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoIncrementalMACLSV");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetIncrementalMACLSV");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetROIPercent");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoROIPercent");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetIncrementalEarnings");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoIncrementalEarnings");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoIncrementalMAC");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetIncrementalCOGSTn");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoIncrementalCOGSTn");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetNSV");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetIncrementalVolume");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoIncrementalVolume");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetIncrementalMAC");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetIncrementalNSV");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoIncrementalNSV");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNSV");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoBaselineBaseTI");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetBaseTI");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetIncrementalCOGS");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetIncrementalBaseTI");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetLSV");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoNetIncrementalLSV");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoTotalCost");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoIncrementalBaseTI");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoBaseTI");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoIncrementalCOGS");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoCost");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoTIShopper");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoBaselineVolume");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoPostPromoEffectVolume");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoPostPromoEffectVolumeW2");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoPostPromoEffectVolumeW1");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoPostPromoEffectLSVW2");
            DropColumn($"{defaultSchema}.PromoPriceIncrease", "PlanPromoPostPromoEffectLSVW1");
        }
    }
}
