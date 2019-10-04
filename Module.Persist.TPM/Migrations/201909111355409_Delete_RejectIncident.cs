namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Delete_RejectIncident : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PromoRejectIncident", "PromoId", "dbo.Promo");
            DropIndex("dbo.PromoRejectIncident", new[] { "PromoId" });
            DropIndex("dbo.PromoRejectIncident", new[] { "ProcessDate" });
            DropTable("dbo.PromoRejectIncident");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PromoRejectIncident",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PromoId = c.Guid(nullable: false),
                        CreateDate = c.DateTimeOffset(nullable: false, precision: 7),
                        ProcessDate = c.DateTimeOffset(precision: 7),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.PromoRejectIncident", "ProcessDate");
            CreateIndex("dbo.PromoRejectIncident", "PromoId");
            AddForeignKey("dbo.PromoRejectIncident", "PromoId", "dbo.Promo", "Id");
        }
    }
}
