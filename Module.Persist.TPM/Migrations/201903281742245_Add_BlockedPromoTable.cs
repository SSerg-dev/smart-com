namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_BlockedPromoTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlockedPromo",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        PromoId = c.Guid(nullable: false),
                        HandlerId = c.Guid(nullable: false),
                        CreateDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Promo", t => t.PromoId)
                .Index(t => t.PromoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BlockedPromo", "PromoId", "dbo.Promo");
            DropIndex("dbo.BlockedPromo", new[] { "PromoId" });
            DropTable("dbo.BlockedPromo");
        }
    }
}
