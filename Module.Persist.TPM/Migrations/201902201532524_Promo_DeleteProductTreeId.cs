namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_DeleteProductTreeId : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.PromoProductTree", "PromoId", "dbo.Promo", "Id");
            DropColumn("dbo.Promo", "ProductTreeId");
        }
        
        public override void Down()
        {            
            AddColumn("dbo.Promo", "ProductTreeId", c => c.Int());
            DropForeignKey("dbo.PromoProductTree", "PromoId", "dbo.Promo");
        }
    }
}
