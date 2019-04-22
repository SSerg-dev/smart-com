namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoIndex : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Promo", new[] { "Name" });
            CreateIndex("dbo.Promo", new[] { "Disabled", "DeletedDate", "Name" }, unique: true, name: "Promo_index");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Promo", "Promo_index");
            CreateIndex("dbo.Promo", "Name", unique: true);
        }
    }
}
