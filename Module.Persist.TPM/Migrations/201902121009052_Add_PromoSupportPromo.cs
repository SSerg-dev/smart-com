namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PromoSupportPromo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PromoSupportPromo",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        PromoId = c.Guid(nullable: false),
                        PromoSupportId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Promo", t => t.PromoId)
                .ForeignKey("dbo.PromoSupport", t => t.PromoSupportId)
                .Index(t => new { t.PromoId, t.PromoSupportId, t.Disabled, t.DeletedDate }, unique: true, name: "Unique_PromoSupportPromo");           
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PromoSupportPromo", "PromoSupportId", "dbo.PromoSupport");
            DropForeignKey("dbo.PromoSupportPromo", "PromoId", "dbo.Promo");
            DropIndex("dbo.PromoSupportPromo", "Unique_PromoSupportPromo");
            DropTable("dbo.PromoSupportPromo");
        }
    }
}
