namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoColor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "ColorId", c => c.Guid());
            CreateIndex("dbo.Promo", "ColorId");
            AddForeignKey("dbo.Promo", "ColorId", "dbo.Color", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Promo", "ColorId", "dbo.Color");
            DropIndex("dbo.Promo", new[] { "ColorId" });
            DropColumn("dbo.Promo", "ColorId");
        }
    }
}
