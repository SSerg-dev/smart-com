namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoFilterDisplay : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "ClientFilterDisplay", c => c.String());
            AddColumn("dbo.Promo", "ProductFilterDisplay", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "ProductFilterDisplay");
            DropColumn("dbo.Promo", "ClientFilterDisplay");
        }
    }
}
