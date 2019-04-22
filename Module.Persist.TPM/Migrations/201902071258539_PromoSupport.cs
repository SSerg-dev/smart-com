namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoSupport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PromoSupport",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        ClientTreeId = c.Int(nullable: false),
                        BudgetItemId = c.Guid(),
                        BudgetSubItemId = c.Guid(),
                        EventId = c.Guid(),
                        PromoId = c.Guid(),
                        Quantity = c.Int(),
                        ProdCost = c.Int(),
                        ProdCostTotal = c.Int(),
                        CostTE = c.Int(),
                        StartDate = c.DateTimeOffset(precision: 7),
                        EndDate = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BudgetItem", t => t.BudgetItemId)
                .ForeignKey("dbo.BudgetSubItem", t => t.BudgetSubItemId)
                .ForeignKey("dbo.ClientTree", t => t.ClientTreeId)
                .ForeignKey("dbo.Event", t => t.EventId)
                .ForeignKey("dbo.Promo", t => t.PromoId)
                .Index(t => t.ClientTreeId)
                .Index(t => t.BudgetItemId)
                .Index(t => t.BudgetSubItemId)
                .Index(t => t.EventId)
                .Index(t => t.PromoId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PromoSupport", "PromoId", "dbo.Promo");
            DropForeignKey("dbo.PromoSupport", "EventId", "dbo.Event");
            DropForeignKey("dbo.PromoSupport", "ClientTreeId", "dbo.ClientTree");
            DropForeignKey("dbo.PromoSupport", "BudgetSubItemId", "dbo.BudgetSubItem");
            DropForeignKey("dbo.PromoSupport", "BudgetItemId", "dbo.BudgetItem");
            DropIndex("dbo.PromoSupport", new[] { "PromoId" });
            DropIndex("dbo.PromoSupport", new[] { "EventId" });
            DropIndex("dbo.PromoSupport", new[] { "BudgetSubItemId" });
            DropIndex("dbo.PromoSupport", new[] { "BudgetItemId" });
            DropIndex("dbo.PromoSupport", new[] { "ClientTreeId" });
            DropTable("dbo.PromoSupport");
        }
    }
}
