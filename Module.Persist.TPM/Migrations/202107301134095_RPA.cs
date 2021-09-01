namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using Core.Settings;
    public partial class RPA : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            CreateTable(
                $"{defaultSchema}.RPA",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        HandlerName = c.String(nullable: false),
                        Constraint = c.String(nullable: false),
                        Parametr = c.String(nullable: false),
                        Status = c.String(nullable: false),
                        FileURL = c.String(nullable: false),
                        LogURL = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropTable($"{defaultSchema}.RPA");
        }
    }
}
