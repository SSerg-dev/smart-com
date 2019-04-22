namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoSupport_DeletePromoId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PromoSupport", "PromoId", "dbo.Promo");
            DropIndex("dbo.PromoSupport", new[] { "PromoId" });
            DropColumn("dbo.PromoSupport", "PromoId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PromoSupport", "PromoId", c => c.Guid());
            CreateIndex("dbo.PromoSupport", "PromoId");
            AddForeignKey("dbo.PromoSupport", "PromoId", "dbo.Promo", "Id");
        }
    }
}
