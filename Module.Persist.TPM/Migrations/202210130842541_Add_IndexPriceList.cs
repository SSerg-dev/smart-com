namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_IndexPriceList : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropIndex($"{defaultSchema}.PriceList", "Unique_PriceList");
            CreateIndex($"{defaultSchema}.PriceList", new[] { "DeletedDate", "StartDate", "EndDate", "ClientTreeId", "ProductId", "FuturePriceMarker" }, unique: true, name: "Unique_PriceList");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropIndex($"{defaultSchema}.PriceList", "Unique_PriceList");
            CreateIndex($"{defaultSchema}.PriceList", new[] { "DeletedDate", "StartDate", "EndDate", "ClientTreeId", "ProductId" }, unique: true, name: "Unique_PriceList");
        }
    }
}
