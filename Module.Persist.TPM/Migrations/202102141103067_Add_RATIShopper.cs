namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_RATIShopper : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            CreateTable(
                $"{defaultSchema}.RATIShopper",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Year = c.Int(nullable: false),
                        ClientTreeId = c.Int(nullable: false),
                        RATIShopperPercent = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.ClientTreeId);
            AddForeignKey($"{defaultSchema}.RATIShopper", "ClientTreeId", $"{defaultSchema}.ClientTree", "Id");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.RATIShopper", "ClientTreeId", $"{defaultSchema}.ClientTree");
            DropIndex($"{defaultSchema}.RATIShopper", new[] { "ClientTreeId" });
            DropIndex($"{defaultSchema}.RATIShopper", new[] { "Id" });
            DropTable($"{defaultSchema}.RATIShopper");
        }
    }
}
