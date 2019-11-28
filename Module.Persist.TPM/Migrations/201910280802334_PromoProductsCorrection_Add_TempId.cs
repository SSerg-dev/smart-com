namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoProductsCorrection_Add_TempId : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PromoProductsCorrection", "Unique_PromoProductCorrection");
            AddColumn("dbo.PromoProductsCorrection", "TempId", c => c.String());
            CreateIndex("dbo.PromoProductsCorrection", "PromoProductId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PromoProductsCorrection", new[] { "PromoProductId" });
            DropColumn("dbo.PromoProductsCorrection", "TempId");
            CreateIndex("dbo.PromoProductsCorrection", new[] { "Disabled", "PromoProductId" }, unique: true, name: "Unique_PromoProductCorrection");
        }
    }
}
