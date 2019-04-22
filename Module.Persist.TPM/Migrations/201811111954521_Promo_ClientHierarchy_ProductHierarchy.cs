namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_ClientHierarchy_ProductHierarchy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "ClientHierarchy", c => c.String());
            AddColumn("dbo.Promo", "ProductHierarchy", c => c.String());
            AlterColumn("dbo.Promo", "EventName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Promo", "EventName", c => c.String());
            DropColumn("dbo.Promo", "ProductHierarchy");
            DropColumn("dbo.Promo", "ClientHierarchy");
        }
    }
}
