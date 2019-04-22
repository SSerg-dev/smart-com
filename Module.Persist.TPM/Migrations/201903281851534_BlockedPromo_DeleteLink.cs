namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BlockedPromo_DeleteLink : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BlockedPromo", "PromoId", "dbo.Promo");
            DropIndex("dbo.BlockedPromo", new[] { "PromoId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.BlockedPromo", "PromoId");
            AddForeignKey("dbo.BlockedPromo", "PromoId", "dbo.Promo", "Id");
        }
    }
}
