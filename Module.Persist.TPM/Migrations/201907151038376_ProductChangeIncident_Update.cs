namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductChangeIncident_Update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductChangeIncident", "NotificationProcessDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.ProductChangeIncident", "RecalculationProcessDate", c => c.DateTimeOffset(precision: 7));
            DropColumn("dbo.ProductChangeIncident", "ProcessDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductChangeIncident", "ProcessDate", c => c.DateTimeOffset(precision: 7));
            DropColumn("dbo.ProductChangeIncident", "RecalculationProcessDate");
            DropColumn("dbo.ProductChangeIncident", "NotificationProcessDate");
        }
    }
}
