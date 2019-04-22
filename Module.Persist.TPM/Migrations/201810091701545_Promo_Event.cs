namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_Event : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "EventId", c => c.Guid());
            CreateIndex("dbo.Promo", "EventId");
            AddForeignKey("dbo.Promo", "EventId", "dbo.Event", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Promo", "EventId", "dbo.Event");
            DropIndex("dbo.Promo", new[] { "EventId" });
            DropColumn("dbo.Promo", "EventId");
        }
    }
}
