namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_CalendarCompetitor_Tables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Jupiter.CalendarСompetitor",
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
                "Jupiter.CalendarСompetitorCompany",
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
                .ForeignKey("Jupiter.CalendarСompetitor", t => t.CalendarСompetitor_Id)
                .Index(t => new { t.CompanyName, t.Disabled, t.DeletedDate }, unique: true, name: "Unique_CompanyName")
                .Index(t => t.CalendarCompetitorId, unique: true, name: "Unique_CalndarId")
                .Index(t => t.CalendarСompetitor_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Jupiter.CalendarСompetitorCompany", "CalendarСompetitor_Id", "Jupiter.CalendarСompetitor");
            DropIndex("Jupiter.CalendarСompetitorCompany", new[] { "CalendarСompetitor_Id" });
            DropIndex("Jupiter.CalendarСompetitorCompany", "Unique_CalndarId");
            DropIndex("Jupiter.CalendarСompetitorCompany", "Unique_CompanyName");
            DropIndex("Jupiter.CalendarСompetitor", "Unique_Name");
            DropTable("Jupiter.CalendarСompetitorCompany");
            DropTable("Jupiter.CalendarСompetitor");
        }
    }
}
