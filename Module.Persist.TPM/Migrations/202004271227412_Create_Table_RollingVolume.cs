namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_Table_RollingVolume : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RollingVolume",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        DemandGroup = c.String(),
                        Week = c.String(),
                        PlanProductInсrementalQTY = c.Double(),
                        Actuals = c.Double(),
                        OpenOrders = c.Double(),
                        Baseline = c.Double(),
                        ActualIncremental = c.Double(),
                        PreviousRollingVolumes = c.Double(),
                        RollingVolumes = c.Double(),
                        Lock = c.Boolean(nullable: false),
                        ProductId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.ProductId)
                .Index(t => t.ProductId);
            
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RollingVolume", "ProductId", "dbo.Product");
            DropIndex("dbo.RollingVolume", new[] { "ProductId" });
            DropTable("dbo.RollingVolume");
           
        }
    }
}
