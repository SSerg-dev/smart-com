namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System.Data.Entity.Migrations;
    
    public partial class Rename_Volume_to_TonCost : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.PlanCOGSTn", "TonCost", c => c.Double(nullable: false));
            AddColumn($"{defaultSchema}.ActualCOGSTn", "TonCost", c => c.Double(nullable: false));
            DropColumn($"{defaultSchema}.PlanCOGSTn", "Volume");
            DropColumn($"{defaultSchema}.ActualCOGSTn", "Volume");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.ActualCOGSTn", "Volume", c => c.Double(nullable: false));
            AddColumn($"{defaultSchema}.PlanCOGSTn", "Volume", c => c.Double(nullable: false));
            DropColumn($"{defaultSchema}.ActualCOGSTn", "TonCost");
            DropColumn($"{defaultSchema}.PlanCOGSTn", "TonCost");
        }
    }
}
