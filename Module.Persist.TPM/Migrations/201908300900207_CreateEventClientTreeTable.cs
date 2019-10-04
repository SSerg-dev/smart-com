namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateEventClientTreeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventClientTree",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        EventId = c.Guid(nullable: false),
                        ClientTreeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClientTree", t => t.ClientTreeId)
                .ForeignKey("dbo.Event", t => t.EventId)
                .Index(t => new { t.EventId, t.ClientTreeId }, unique: true, name: "Unique_EventClientTree");
            
            DropColumn("dbo.Event", "Year");
            DropColumn("dbo.Event", "Period");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Event", "Period", c => c.String());
            AddColumn("dbo.Event", "Year", c => c.Int(nullable: false));
            DropForeignKey("dbo.EventClientTree", "EventId", "dbo.Event");
            DropForeignKey("dbo.EventClientTree", "ClientTreeId", "dbo.ClientTree");
            DropIndex("dbo.EventClientTree", "Unique_EventClientTree");
            DropTable("dbo.EventClientTree");
        }
    }
}
