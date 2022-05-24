namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Product_Weight_Fields : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.Product", "NetWeight", c => c.Double());
            AddColumn($"{defaultSchema}.Product", "UOM", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropColumn($"{defaultSchema}.Product", "UOM");
            DropColumn($"{defaultSchema}.Product", "NetWeight");
        }
    }
}
