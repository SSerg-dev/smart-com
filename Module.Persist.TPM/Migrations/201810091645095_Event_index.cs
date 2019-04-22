namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Event_index : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Event", new[] { "Name" });
            CreateIndex("dbo.Event", new[] { "Disabled", "DeletedDate", "Name" }, unique: true, name: "Event_index");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Event", "Event_index");
            CreateIndex("dbo.Event", "Name", unique: true);
        }
    }
}
