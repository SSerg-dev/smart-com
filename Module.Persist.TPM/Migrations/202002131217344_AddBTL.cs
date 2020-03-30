namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBTL : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BTL",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Number = c.Int(nullable: false),
                        PlanBTLTotal = c.Double(),
                        ActualBTLTotal = c.Double(),
                        StartDate = c.DateTimeOffset(precision: 7),
                        EndDate = c.DateTimeOffset(precision: 7),
                        InvoiceNumber = c.String(),
                        EventId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Event", t => t.EventId)
                .Index(t => t.EventId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BTL", "EventId", "dbo.Event");
            DropIndex("dbo.BTL", new[] { "EventId" });
            DropTable("dbo.BTL");
        }
    }
}
