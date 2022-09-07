namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncrementalPromo_TPMmode_Index : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropIndex($"{defaultSchema}.IncrementalPromo", "Unique_IncrementalPromo");
            CreateIndex($"{defaultSchema}.IncrementalPromo", new[] { "DeletedDate", "TPMmode", "PromoId", "ProductId" }, unique: true, name: "Unique_IncrementalPromo");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropIndex($"{defaultSchema}.IncrementalPromo", "Unique_IncrementalPromo");
            CreateIndex($"{defaultSchema}.IncrementalPromo", new[] { "DeletedDate", "PromoId", "ProductId" }, unique: true, name: "Unique_IncrementalPromo");
        }
    }
}
