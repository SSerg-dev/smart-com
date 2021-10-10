namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_CalendarCompetitor_Tables : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            CreateTable(
                $"{defaultSchema}.CalendarCompetitor",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Name = c.String(maxLength: 124),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.Disabled, t.Name, t.DeletedDate }, unique: true, name: "Unique_Name");
            
            CreateTable(
                $"{defaultSchema}.CalendarCompetitorCompany",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        CompanyName = c.String(nullable: false, maxLength: 124),
                        CalendarCompetitorId = c.Guid(nullable: true),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey($"{defaultSchema}.CalendarCompetitor", t => t.CalendarCompetitorId)
                .Index(t => new { t.CalendarCompetitorId, t.CompanyName, t.Disabled, t.DeletedDate }, unique: true, name: "Unique_CalendarCompetitorCompany");
            
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.CalendarCompetitorCompany", "CalendarCompetitorId", $"{defaultSchema}.CalendarCompetitor");
            DropIndex($"{defaultSchema}.CalendarCompetitorCompany", "Unique_CalendarCompetitorCompany");
            DropIndex($"{defaultSchema}.CalendarCompetitor", "Unique_Name");
            DropTable($"{defaultSchema}.CalendarCompetitorCompany");
            DropTable($"{defaultSchema}.CalendarCompetitor");
        }
    }
}
