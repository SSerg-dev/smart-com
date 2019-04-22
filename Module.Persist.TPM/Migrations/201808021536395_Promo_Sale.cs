namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_Sale : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Promo",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        ClientId = c.Guid(),
                        BrandId = c.Guid(),
                        BrandTechId = c.Guid(),
                        ProductId = c.Guid(),
                        PromoStatusId = c.Guid(),
                        MechanicId = c.Guid(),
                        Name = c.String(nullable: false, maxLength: 255),
                        StartDate = c.DateTimeOffset(precision: 7),
                        EndDate = c.DateTimeOffset(precision: 7),
                        DispatchesStart = c.DateTimeOffset(precision: 7),
                        DispatchesEnd = c.DateTimeOffset(precision: 7),
                        EventName = c.String(),
                        PlanBaseline = c.Int(),
                        PlanDuration = c.Int(),
                        PlanUplift = c.Int(),
                        PlanIncremental = c.Int(),
                        PlanActivity = c.Int(),
                        PlanSteal = c.Int(),
                        FactBaseline = c.Int(),
                        FactDuration = c.Int(),
                        FactUplift = c.Int(),
                        FactIncremental = c.Int(),
                        FactActivity = c.Int(),
                        FactSteal = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Brand", t => t.BrandId)
                .ForeignKey("dbo.BrandTech", t => t.BrandTechId)
                .ForeignKey("dbo.Client", t => t.ClientId)
                .ForeignKey("dbo.Mechanic", t => t.MechanicId)
                .ForeignKey("dbo.Product", t => t.ProductId)
                .ForeignKey("dbo.PromoStatus", t => t.PromoStatusId)
                .Index(t => t.ClientId)
                .Index(t => t.BrandId)
                .Index(t => t.BrandTechId)
                .Index(t => t.ProductId)
                .Index(t => t.PromoStatusId)
                .Index(t => t.MechanicId)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Sale",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        BudgetId = c.Guid(),
                        BudgetItemId = c.Guid(),
                        PromoId = c.Guid(),
                        Plan = c.Int(),
                        Fact = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Budget", t => t.BudgetId)
                .ForeignKey("dbo.BudgetItem", t => t.BudgetItemId)
                .ForeignKey("dbo.Promo", t => t.PromoId)
                .Index(t => t.BudgetId)
                .Index(t => t.BudgetItemId)
                .Index(t => t.PromoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sale", "PromoId", "dbo.Promo");
            DropForeignKey("dbo.Sale", "BudgetItemId", "dbo.BudgetItem");
            DropForeignKey("dbo.Sale", "BudgetId", "dbo.Budget");
            DropForeignKey("dbo.Promo", "PromoStatusId", "dbo.PromoStatus");
            DropForeignKey("dbo.Promo", "ProductId", "dbo.Product");
            DropForeignKey("dbo.Promo", "MechanicId", "dbo.Mechanic");
            DropForeignKey("dbo.Promo", "ClientId", "dbo.Client");
            DropForeignKey("dbo.Promo", "BrandTechId", "dbo.BrandTech");
            DropForeignKey("dbo.Promo", "BrandId", "dbo.Brand");
            DropIndex("dbo.Sale", new[] { "PromoId" });
            DropIndex("dbo.Sale", new[] { "BudgetItemId" });
            DropIndex("dbo.Sale", new[] { "BudgetId" });
            DropIndex("dbo.Promo", new[] { "Name" });
            DropIndex("dbo.Promo", new[] { "MechanicId" });
            DropIndex("dbo.Promo", new[] { "PromoStatusId" });
            DropIndex("dbo.Promo", new[] { "ProductId" });
            DropIndex("dbo.Promo", new[] { "BrandTechId" });
            DropIndex("dbo.Promo", new[] { "BrandId" });
            DropIndex("dbo.Promo", new[] { "ClientId" });
            DropTable("dbo.Sale");
            DropTable("dbo.Promo");
        }
    }
}
