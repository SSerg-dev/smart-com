namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Delete_RSstatus : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropColumn($"{defaultSchema}.IncrementalPromo", "RSstatus");
            DropColumn($"{defaultSchema}.Promo", "RSstatus");
            DropColumn($"{defaultSchema}.BTLPromo", "RSstatus");
            DropColumn($"{defaultSchema}.PromoProduct", "RSstatus");
            DropColumn($"{defaultSchema}.PromoProductsCorrection", "RSstatus");
            DropColumn($"{defaultSchema}.PromoProductTree", "RSstatus");
            DropColumn($"{defaultSchema}.PromoSupportPromo", "RSstatus");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.PromoSupportPromo", "RSstatus", c => c.Int(nullable: false));
            AddColumn($"{defaultSchema}.PromoProductTree", "RSstatus", c => c.Int(nullable: false));
            AddColumn($"{defaultSchema}.PromoProductsCorrection", "RSstatus", c => c.Int(nullable: false));
            AddColumn($"{defaultSchema}.PromoProduct", "RSstatus", c => c.Int(nullable: false));
            AddColumn($"{defaultSchema}.BTLPromo", "RSstatus", c => c.Int(nullable: false));
            AddColumn($"{defaultSchema}.Promo", "RSstatus", c => c.Int(nullable: false));
            AddColumn($"{defaultSchema}.IncrementalPromo", "RSstatus", c => c.Int(nullable: false));
        }
    }
}
