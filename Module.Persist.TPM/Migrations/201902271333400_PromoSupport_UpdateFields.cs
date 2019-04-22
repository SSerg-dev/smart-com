namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoSupport_UpdateFields : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PromoSupport", "EventId", "dbo.Event");
            DropIndex("dbo.PromoSupport", new[] { "EventId" });
            AddColumn("dbo.PromoSupport", "PlanQuantity", c => c.Int());
            AddColumn("dbo.PromoSupport", "ActualQuantity", c => c.Int());
            AddColumn("dbo.PromoSupport", "PlanCostTE", c => c.Double());
            AddColumn("dbo.PromoSupport", "ActualCostTE", c => c.Double());
            AddColumn("dbo.PromoSupport", "PlanProdCostPer1Item", c => c.Double());
            AddColumn("dbo.PromoSupport", "ActualProdCostPer1Item", c => c.Double());
            AddColumn("dbo.PromoSupport", "PlanProdCost", c => c.Double());
            AddColumn("dbo.PromoSupport", "ActualProdCost", c => c.Double());
            DropColumn("dbo.PromoSupport", "EventId");
            DropColumn("dbo.PromoSupport", "Quantity");
            DropColumn("dbo.PromoSupport", "ProdCost");
            DropColumn("dbo.PromoSupport", "ProdCostTotal");
            DropColumn("dbo.PromoSupport", "CostTE");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PromoSupport", "CostTE", c => c.Int());
            AddColumn("dbo.PromoSupport", "ProdCostTotal", c => c.Int());
            AddColumn("dbo.PromoSupport", "ProdCost", c => c.Int());
            AddColumn("dbo.PromoSupport", "Quantity", c => c.Int());
            AddColumn("dbo.PromoSupport", "EventId", c => c.Guid());
            DropColumn("dbo.PromoSupport", "ActualProdCost");
            DropColumn("dbo.PromoSupport", "PlanProdCost");
            DropColumn("dbo.PromoSupport", "ActualProdCostPer1Item");
            DropColumn("dbo.PromoSupport", "PlanProdCostPer1Item");
            DropColumn("dbo.PromoSupport", "ActualCostTE");
            DropColumn("dbo.PromoSupport", "PlanCostTE");
            DropColumn("dbo.PromoSupport", "ActualQuantity");
            DropColumn("dbo.PromoSupport", "PlanQuantity");
            CreateIndex("dbo.PromoSupport", "EventId");
            AddForeignKey("dbo.PromoSupport", "EventId", "dbo.Event", "Id");
        }
    }
}
