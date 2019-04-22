namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Event", "Year", c => c.String());
            AddColumn("dbo.Event", "Period", c => c.String());
            AddColumn("dbo.Event", "Description", c => c.String());
            DropColumn("dbo.Event", "StartDate");
            DropColumn("dbo.Event", "EndDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Event", "EndDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.Event", "StartDate", c => c.DateTimeOffset(precision: 7));
            DropColumn("dbo.Event", "Description");
            DropColumn("dbo.Event", "Period");
            DropColumn("dbo.Event", "Year");
        }
    }
}
