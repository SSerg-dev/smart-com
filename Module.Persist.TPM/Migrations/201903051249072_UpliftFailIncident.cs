namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpliftFailIncident : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PromoUpliftFailIncident",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PromoId = c.Guid(nullable: false),
                        CreateDate = c.DateTimeOffset(nullable: false, precision: 7),
                        ProcessDate = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Promo", t => t.PromoId)
                .Index(t => t.PromoId)
                .Index(t => t.ProcessDate);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PromoUpliftFailIncident", "PromoId", "dbo.Promo");
            DropIndex("dbo.PromoUpliftFailIncident", new[] { "ProcessDate" });
            DropIndex("dbo.PromoUpliftFailIncident", new[] { "PromoId" });
            DropTable("dbo.PromoUpliftFailIncident");
        }
    }
}
