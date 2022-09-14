namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncreaseBaseLine_Create : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            CreateTable(
                 $"{defaultSchema}.IncreaseBaseLine",
                 c => new
                 {
                     Id = c.Guid(nullable: false, identity: true),
                     Disabled = c.Boolean(nullable: false),
                     DeletedDate = c.DateTimeOffset(precision: 7),
                     ProductId = c.Guid(nullable: false),
                     DemandCode = c.String(nullable: false, maxLength: 255),
                     StartDate = c.DateTimeOffset(precision: 7),
                     InputBaseLineQty = c.Double(nullable: false),
                     SellInBaseLineQty = c.Double(nullable: false),
                     SellOutBaseLineQty = c.Double(nullable: false),
                     Type = c.Int(nullable: false),
                     LastModifiedDate = c.DateTimeOffset(precision: 7)
                 })
                 .PrimaryKey(t => t.Id)
                 .Index(t => new { t.DemandCode, t.StartDate, t.DeletedDate, t.ProductId }, unique: true, name: "Unique_IncreaseBaseLine");

                  AddForeignKey($"{defaultSchema}.IncreaseBaseLine", "ProductId", $"{defaultSchema}.Product", "Id");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.IncreaseBaseLine", "ProductId", $"{defaultSchema}.Product");
            DropIndex($"{defaultSchema}.IncreaseBaseLine", "Unique_IncreaseBaseLine");
            DropTable($"{ defaultSchema}.IncreaseBaseLine");
        }
        
    }
}
