namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_NonPromoSupport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NonPromoSupport",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Number = c.Int(nullable: false),
                        ClientTreeId = c.Int(nullable: false),
                        BrandTechId = c.Guid(),
                        NonPromoEquipmentId = c.Guid(),
                        PlanQuantity = c.Int(),
                        ActualQuantity = c.Int(),
                        PlanCostTE = c.Double(),
                        ActualCostTE = c.Double(),
                        StartDate = c.DateTimeOffset(precision: 7),
                        EndDate = c.DateTimeOffset(precision: 7),
                        PlanProdCost = c.Double(),
                        ActualProdCost = c.Double(),
                        UserTimestamp = c.String(),
                        AttachFileName = c.String(),
                        BorderColor = c.String(),
                        PONumber = c.String(),
                        InvoiceNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BrandTech", t => t.BrandTechId)
                .ForeignKey("dbo.ClientTree", t => t.ClientTreeId)
                .ForeignKey("dbo.NonPromoEquipment", t => t.NonPromoEquipmentId)
                .Index(t => t.ClientTreeId)
                .Index(t => t.BrandTechId)
                .Index(t => t.NonPromoEquipmentId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NonPromoSupport", "NonPromoEquipmentId", "dbo.NonPromoEquipment");
            DropForeignKey("dbo.NonPromoSupport", "ClientTreeId", "dbo.ClientTree");
            DropForeignKey("dbo.NonPromoSupport", "BrandTechId", "dbo.BrandTech");
            DropIndex("dbo.NonPromoSupport", new[] { "NonPromoEquipmentId" });
            DropIndex("dbo.NonPromoSupport", new[] { "BrandTechId" });
            DropIndex("dbo.NonPromoSupport", new[] { "ClientTreeId" });
            DropTable("dbo.NonPromoSupport");
        }
    }
}
