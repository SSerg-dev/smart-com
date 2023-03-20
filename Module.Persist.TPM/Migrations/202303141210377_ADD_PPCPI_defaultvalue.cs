namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ADD_PPCPI_defaultvalue : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropPrimaryKey($"{defaultSchema}.PromoProductCorrectionPriceIncrease");
            AlterColumn($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "Id");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropPrimaryKey($"{defaultSchema}.PromoProductCorrectionPriceIncrease");
            AlterColumn($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey($"{defaultSchema}.PromoProductCorrectionPriceIncrease", "Id");
        }
    }
}
