namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventYearNumber : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Event", "Year");
            AddColumn("dbo.Event", "Year", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Event", "Year");
            AddColumn("dbo.Event", "Year", c => c.String());
        }
    }
}
