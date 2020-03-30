namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_ClientDashboard : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientDashboard",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ClientTreeId = c.Int(),
                        ClientHierarchy = c.String(),
                        BrandTechId = c.Guid(),
                        BrandTechName = c.String(),
                        Year = c.String(maxLength: 50),
                        ShopperTiPlanPercent = c.Double(),
                        MarketingTiPlanPercent = c.Double(),
                        ProductionPlan = c.Double(),
                        BrandingPlanPercent = c.Double(),
                        BrandingPlan = c.Double(),
                        BTLPlan = c.Double(),
                        ROIPlanPercent = c.Double(),
                        IncrementalNSVPlan = c.Double(),
                        PromoNSVPlan = c.Double(),
                    })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.ClientDashboard");
        }
    }
}
