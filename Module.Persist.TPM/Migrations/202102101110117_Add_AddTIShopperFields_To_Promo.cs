namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_AddTIShopperFields_To_Promo : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.Promo", "AddTIShopperApproved", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "AddTIShopperCalculated", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "AddTIMarketingApproved", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "AddTIShopper", c => c.Double());
            AddColumn($"{defaultSchema}.Promo", "AddTIMarketing", c => c.Double());
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropColumn($"{defaultSchema}.Promo", "AddTIMarketing");
            DropColumn($"{defaultSchema}.Promo", "AddTIShopper");
            DropColumn($"{defaultSchema}.Promo", "AddTIMarketingApproved");
            DropColumn($"{defaultSchema}.Promo", "AddTIShopperCalculated");
            DropColumn($"{defaultSchema}.Promo", "AddTIShopperApproved");
        }
    }
}
