namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PromoOnRejectIncident : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PromoOnRejectIncident",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PromoId = c.Guid(nullable: false),
                        CreateDate = c.DateTimeOffset(nullable: false, precision: 7),
                        ProcessDate = c.DateTimeOffset(precision: 7),
                        UserLogin = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Promo", t => t.PromoId)
                .Index(t => t.PromoId)
                .Index(t => t.ProcessDate);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PromoOnRejectIncident", "PromoId", "dbo.Promo");
            DropIndex("dbo.PromoOnRejectIncident", new[] { "ProcessDate" });
            DropIndex("dbo.PromoOnRejectIncident", new[] { "PromoId" });
            DropTable("dbo.PromoOnRejectIncident");
        }
    }
}
