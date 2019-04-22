namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_ClientFilter : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Promo", "ClientFilter");
            DropColumn("dbo.Promo", "ClientFilterDisplay");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Promo", "ClientFilterDisplay", c => c.String());
            AddColumn("dbo.Promo", "ClientFilter", c => c.String());
        }
    }
}
