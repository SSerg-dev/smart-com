namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_BTLPromo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BTLPromo",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        BTLId = c.Guid(nullable: false),
                        PromoId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BTL", t => t.BTLId)
                .ForeignKey("dbo.Promo", t => t.PromoId)
                .Index(t => t.BTLId)
                .Index(t => t.PromoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BTLPromo", "PromoId", "dbo.Promo");
            DropForeignKey("dbo.BTLPromo", "BTLId", "dbo.BTL");
            DropIndex("dbo.BTLPromo", new[] { "PromoId" });
            DropIndex("dbo.BTLPromo", new[] { "BTLId" });
            DropTable("dbo.BTLPromo");
        }
    }
}
