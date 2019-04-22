namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoTreeFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "Client1LevelName", c => c.String(maxLength: 255));
            AddColumn("dbo.Promo", "Client2LevelName", c => c.String(maxLength: 255));
            AddColumn("dbo.Promo", "ClientName", c => c.String(maxLength: 255));
            AddColumn("dbo.Promo", "ProductSubrangesList", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "ProductSubrangesList");
            DropColumn("dbo.Promo", "ClientName");
            DropColumn("dbo.Promo", "Client2LevelName");
            DropColumn("dbo.Promo", "Client1LevelName");
        }
    }
}
