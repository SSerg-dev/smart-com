namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using Core.Settings;
    
    public partial class RPASetting : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            CreateTable(
                $"{defaultSchema}.RPASetting",
                c => new
                {
                    Id = c.Guid(nullable: false, identity: true),
                    Json = c.String(nullable: false),
                    Type = c.Int(nullable: false)
                })
                .PrimaryKey(t => t.Id);                
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropTable($"{defaultSchema}.RPASetting");
        }
    }
}
