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
                $"{defaultSchema}.CalendarСompetitor",
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
                $"{defaultSchema}.CalendarСompetitorCompany",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        CompanyName = c.String(maxLength: 124),
                        CalendarCompetitorId = c.Guid(nullable: false),
                        CalendarСompetitor_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey($"{defaultSchema}.CalendarСompetitor", t => t.CalendarСompetitor_Id)
                .Index(t => new { t.CompanyName, t.Disabled, t.DeletedDate }, unique: true, name: "Unique_CompanyName")
                .Index(t => t.CalendarCompetitorId, unique: true, name: "Unique_CalndarId")
                .Index(t => t.CalendarСompetitor_Id);
            
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.CalendarСompetitorCompany", "CalendarСompetitor_Id", $"{defaultSchema}.CalendarСompetitor");
            DropIndex($"{defaultSchema}.CalendarСompetitorCompany", new[] { "CalendarСompetitor_Id" });
            DropIndex($"{defaultSchema}.CalendarСompetitorCompany", "Unique_CalndarId");
            DropIndex($"{defaultSchema}.CalendarСompetitorCompany", "Unique_CompanyName");
            DropIndex($"{defaultSchema}.CalendarСompetitor", "Unique_Name");
            DropTable($"{defaultSchema}.CalendarСompetitorCompany");
            DropTable($"{defaultSchema}.CalendarСompetitor");
        }
    }
}
