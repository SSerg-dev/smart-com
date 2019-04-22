namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeProductIncidentDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductChangeIncident", "CreateDate", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.ProductChangeIncident", "ProcessDate", c => c.DateTimeOffset(precision: 7));
            DropColumn("dbo.ProductChangeIncident", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductChangeIncident", "Date", c => c.DateTimeOffset(nullable: false, precision: 7));
            DropColumn("dbo.ProductChangeIncident", "ProcessDate");
            DropColumn("dbo.ProductChangeIncident", "CreateDate");
        }
    }
}
