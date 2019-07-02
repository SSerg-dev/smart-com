namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_AddNaturalLink_ToClientTree : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "ClientTreeKeyId", c => c.Int());
            CreateIndex("dbo.Promo", "ClientTreeKeyId");
            AddForeignKey("dbo.Promo", "ClientTreeKeyId", "dbo.ClientTree", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Promo", "ClientTreeKeyId", "dbo.ClientTree");
            DropIndex("dbo.Promo", new[] { "ClientTreeKeyId" });
            DropColumn("dbo.Promo", "ClientTreeKeyId");
        }
    }
}
