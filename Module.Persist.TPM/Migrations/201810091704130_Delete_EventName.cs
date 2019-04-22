namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Delete_EventName : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Promo", "EventName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Promo", "EventName", c => c.String());
        }
    }
}
