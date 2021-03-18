namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Renaming_AddTI_Fields : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.Promo", "PlanAddTIShopperApproved", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "PlanAddTIShopperCalculated", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "PlanAddTIMarketingApproved", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "ActualAddTIShopper", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "ActualAddTIMarketing", c => c.Double());
            DropColumn($"{defaultSchema}.Promo", "AddTIShopperApproved");
            DropColumn($"{defaultSchema}.Promo", "AddTIShopperCalculated");
            DropColumn($"{defaultSchema}.Promo", "AddTIMarketingApproved");
            DropColumn($"{defaultSchema}.Promo", "AddTIShopper");
            DropColumn($"{defaultSchema}.Promo", "AddTIMarketing");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.Promo", "AddTIMarketing", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "AddTIShopper", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "AddTIMarketingApproved", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "AddTIShopperCalculated", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "AddTIShopperApproved", c => c.Double());
            DropColumn($"{defaultSchema}.Promo", "ActualAddTIMarketing");
            DropColumn($"{defaultSchema}.Promo", "ActualAddTIShopper");
            DropColumn($"{defaultSchema}.Promo", "PlanAddTIMarketingApproved");
            DropColumn($"{defaultSchema}.Promo", "PlanAddTIShopperCalculated");
            DropColumn($"{defaultSchema}.Promo", "PlanAddTIShopperApproved");
        }
    }
}
