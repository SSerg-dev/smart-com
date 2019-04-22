namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoStatusChanges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PromoStatusChange",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PromoId = c.Guid(),
                        UserId = c.Guid(),
                        RoleId = c.Guid(),
                        StatusId = c.Guid(),
                        Date = c.DateTimeOffset(precision: 7),
                        Comment = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Promo", t => t.PromoId)
                .ForeignKey("dbo.PromoStatus", t => t.StatusId)
                .Index(t => t.PromoId)
                .Index(t => t.StatusId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PromoStatusChange", "StatusId", "dbo.PromoStatus");
            DropForeignKey("dbo.PromoStatusChange", "PromoId", "dbo.Promo");
            DropIndex("dbo.PromoStatusChange", new[] { "StatusId" });
            DropIndex("dbo.PromoStatusChange", new[] { "PromoId" });
            DropTable("dbo.PromoStatusChange");
        }
    }
}
