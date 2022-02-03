namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExportQueryTable : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "Jupiter");
            CreateTable(
                $"[{defaultSchema}].ExportQuery",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Type = c.String(),
                        Text = c.String(),
                        Parameters = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "Jupiter");
            DropTable($"[{defaultSchema}].ExportQuery");
        }
    }
}
