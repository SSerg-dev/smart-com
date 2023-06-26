namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_SavedScenario : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            CreateTable(
                $"{defaultSchema}.SavedScenario",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        CreateDate = c.DateTimeOffset(nullable: false, precision: 7),
                        ScenarioName = c.String(maxLength: 512),
                        RollingScenario_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey($"{defaultSchema}.RollingScenario", t => t.RollingScenario_Id)
                .Index(t => t.RollingScenario_Id);
            
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.SavedScenario", "RollingScenario_Id", $"{defaultSchema}.RollingScenario");
            DropIndex($"{defaultSchema}.SavedScenario", new[] { "RollingScenario_Id" });
            DropTable($"{defaultSchema}.SavedScenario");
        }
    }
}
