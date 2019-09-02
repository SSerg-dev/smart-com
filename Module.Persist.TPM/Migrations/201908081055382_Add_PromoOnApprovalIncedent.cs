namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PromoOnApprovalIncedent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PromoOnApprovalIncident",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PromoId = c.Guid(nullable: false),
                        CreateDate = c.DateTimeOffset(nullable: false, precision: 7),
                        ProcessDate = c.DateTimeOffset(precision: 7),
                        ApprovingRole = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Promo", t => t.PromoId)
                .Index(t => t.PromoId)
                .Index(t => t.ProcessDate);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PromoOnApprovalIncident", "PromoId", "dbo.Promo");
            DropIndex("dbo.PromoOnApprovalIncident", new[] { "ProcessDate" });
            DropIndex("dbo.PromoOnApprovalIncident", new[] { "PromoId" });
            DropTable("dbo.PromoOnApprovalIncident");
        }
    }
}
