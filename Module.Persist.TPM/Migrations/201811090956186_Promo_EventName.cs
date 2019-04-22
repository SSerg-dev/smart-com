namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_EventName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "OtherEventName", c => c.String());
            AddColumn("dbo.Promo", "EventName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "EventName");
            DropColumn("dbo.Promo", "OtherEventName");
        }
    }
}
