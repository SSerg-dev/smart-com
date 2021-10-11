namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Calendar_BrandTech_Color_Table : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            CreateTable(
                $"{defaultSchema}.CalendarCompetitorBrandTechColor",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        CalendarCompetitorId = c.Guid(nullable: true),
                        CalendarCompetitorCompanyId = c.Guid(nullable: false),
                        BrandTech = c.String(nullable: false, maxLength: 124),
                        Color = c.String(nullable: false, maxLength: 24),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey($"{defaultSchema}.CalendarCompetitor", t => t.CalendarCompetitorId)
                .ForeignKey($"{defaultSchema}.CalendarCompetitorCompany", t => t.CalendarCompetitorCompanyId)
                .Index(t => new { t.CalendarCompetitorId, t.CalendarCompetitorCompanyId, t.BrandTech, t.Disabled, t.DeletedDate }, unique: true, name: "Unique_CalendarCompetitorBrandTechColor");
            
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.CalendarCompetitorBrandTechColor", "CalendarCompetitorCompanyId", $"{defaultSchema}.CalendarCompetitorCompany");
            DropForeignKey($"{defaultSchema}.CalendarCompetitorBrandTechColor", "CalendarCompetitorId", $"{defaultSchema}.CalendarCompetitor");
            DropIndex($"{defaultSchema}.CalendarCompetitorBrandTechColor", "Unique_CalendarCompetitorBrandTechColor");
            DropTable($"{defaultSchema}.CalendarCompetitorBrandTechColor");
        }
    }
}
