namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_Client1LevelName_Client2LevelName_ClientName_Promo : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Promo", "Client1LevelName");
            DropColumn("dbo.Promo", "Client2LevelName");
            DropColumn("dbo.Promo", "ClientName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Promo", "ClientName", c => c.String(maxLength: 255));
            AddColumn("dbo.Promo", "Client2LevelName", c => c.String(maxLength: 255));
            AddColumn("dbo.Promo", "Client1LevelName", c => c.String(maxLength: 255));
        }
    }
}
