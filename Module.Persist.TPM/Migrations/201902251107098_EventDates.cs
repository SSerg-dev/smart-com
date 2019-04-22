namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Event", "StartDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.Event", "EndDate", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Event", "EndDate");
            DropColumn("dbo.Event", "StartDate");
        }
    }
}
