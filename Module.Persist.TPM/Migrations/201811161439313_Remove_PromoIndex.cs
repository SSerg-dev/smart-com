namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_PromoIndex : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Promo", "Promo_index");
            AlterColumn("dbo.Promo", "Name", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Promo", "Name", c => c.String(nullable: false, maxLength: 255));
            CreateIndex("dbo.Promo", new[] { "Disabled", "DeletedDate", "Name" }, unique: true, name: "Promo_index");
        }
    }
}
