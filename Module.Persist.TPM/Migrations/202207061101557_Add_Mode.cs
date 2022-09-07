namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Mode : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.Promo", "TPMmode", c => c.Int(nullable: false));
            AddColumn($"{defaultSchema}.PromoProduct", "TPMmode", c => c.Int(nullable: false));
            AddColumn($"{defaultSchema}.PromoSupportPromo", "TPMmode", c => c.Int(nullable: false));
            AddColumn($"{defaultSchema}.PromoProductTree", "TPMmode", c => c.Int(nullable: false));
            AddColumn($"{defaultSchema}.PromoProductsCorrection", "TPMmode", c => c.Int(nullable: false));
            AddColumn($"{defaultSchema}.BTLPromo", "TPMmode", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropColumn($"{defaultSchema}.BTLPromo", "TPMmode");
            DropColumn($"{defaultSchema}.PromoProductsCorrection", "TPMmode");
            DropColumn($"{defaultSchema}.PromoProductTree", "TPMmode");
            DropColumn($"{defaultSchema}.PromoSupportPromo", "TPMmode");
            DropColumn($"{defaultSchema}.PromoProduct", "TPMmode");
            DropColumn($"{defaultSchema}.Promo", "TPMmode");
        }
    }
}
