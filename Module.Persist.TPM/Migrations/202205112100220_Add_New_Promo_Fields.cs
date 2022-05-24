namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_New_Promo_Fields : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.Promo", "PlanPromoIncrementalMACLSV", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "PlanPromoNetIncrementalMACLSV", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "ActualPromoIncrementalMACLSV", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "ActualPromoNetIncrementalMACLSV", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "PlanPromoIncrementalEarningsLSV", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "PlanPromoNetIncrementalEarningsLSV", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "ActualPromoIncrementalEarningsLSV", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "ActualPromoNetIncrementalEarningsLSV", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "PlanPromoROIPercentLSV", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "PlanPromoNetROIPercentLSV", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "ActualPromoROIPercentLSV", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "ActualPromoNetROIPercentLSV", c => c.Double());

        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropColumn($"{defaultSchema}.Promo", "PlanPromoIncrementalMACLSV");
            DropColumn($"{defaultSchema}.Promo", "PlanPromoNetIncrementalMACLSV");
            DropColumn($"{defaultSchema}.Promo", "ActualPromoIncrementalMACLSV");
            DropColumn($"{defaultSchema}.Promo", "ActualPromoNetIncrementalMACLSV");
            DropColumn($"{defaultSchema}.Promo", "PlanPromoIncrementalEarningsLSV");
            DropColumn($"{defaultSchema}.Promo", "PlanPromoNetIncrementalEarningsLSV");
            DropColumn($"{defaultSchema}.Promo", "ActualPromoIncrementalEarningsLSV");
            DropColumn($"{defaultSchema}.Promo", "ActualPromoNetIncrementalEarningsLSV");
            DropColumn($"{defaultSchema}.Promo", "PlanPromoROIPercentLSV");
            DropColumn($"{defaultSchema}.Promo", "PlanPromoNetROIPercentLSV");
            DropColumn($"{defaultSchema}.Promo", "ActualPromoROIPercentLSV");
            DropColumn($"{defaultSchema}.Promo", "ActualPromoNetROIPercentLSV");
        }
    }
}
