namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Actuals_Modification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Actual", "PromoId", c => c.Guid(nullable: false));
            AddColumn("dbo.Actual", "ProductId", c => c.Guid(nullable: false));
            AddColumn("dbo.Actual", "EAN", c => c.String(maxLength: 255));
            AddColumn("dbo.Actual", "PlanProductQty", c => c.Int());
            AddColumn("dbo.Actual", "PlanProductPCQty", c => c.Int());
            AddColumn("dbo.Actual", "PlanProductLSV", c => c.Double());
            AddColumn("dbo.Actual", "PlanProductPCLSV", c => c.Double());
            AddColumn("dbo.Actual", "ProductBaselinePrice", c => c.Double());
            AddColumn("dbo.Actual", "ProductBaselinePCPrice", c => c.Double());
            AddColumn("dbo.Actual", "PlanProductUplift", c => c.Double());
            AddColumn("dbo.Actual", "ActualProductPCQty", c => c.Int());
            AddColumn("dbo.Actual", "ActualProductQty", c => c.Int());
            AddColumn("dbo.Actual", "ActualProductUOM", c => c.String());
            AddColumn("dbo.Actual", "ActualProductSellInPrice", c => c.Double());
            AddColumn("dbo.Actual", "ActualProductSellInDiscount", c => c.Double());
            AddColumn("dbo.Actual", "ActualProductShelfPrice", c => c.Double());
            AddColumn("dbo.Actual", "ActualProductShelfDiscount", c => c.Double());
            AddColumn("dbo.Actual", "ActualProductPCLSV", c => c.Double());
            AddColumn("dbo.Actual", "ActualPromoShare", c => c.Double());
            AddColumn("dbo.Actual", "ActualProductUplift", c => c.Double());
            AddColumn("dbo.Actual", "ActualProductIncrementalPCQty", c => c.Double());
            AddColumn("dbo.Actual", "ActualProductIncrementalPCLSV", c => c.Double());
            AddColumn("dbo.Actual", "ActualProductIncrementalLSV", c => c.Double());
            CreateIndex("dbo.Actual", "PromoId");
            CreateIndex("dbo.Actual", "ProductId");
            AddForeignKey("dbo.Actual", "ProductId", "dbo.Product", "Id");
            AddForeignKey("dbo.Actual", "PromoId", "dbo.Promo", "Id");
            DropColumn("dbo.Actual", "DemandCode");
            DropColumn("dbo.Actual", "SalesOutCode");
            DropColumn("dbo.Actual", "SalesInCode");
            DropColumn("dbo.Actual", "Date");
            DropColumn("dbo.Actual", "Actuals");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Actual", "Actuals", c => c.Double(nullable: false));
            AddColumn("dbo.Actual", "Date", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.Actual", "SalesInCode", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Actual", "SalesOutCode", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Actual", "DemandCode", c => c.String(nullable: false, maxLength: 255));
            DropForeignKey("dbo.Actual", "PromoId", "dbo.Promo");
            DropForeignKey("dbo.Actual", "ProductId", "dbo.Product");
            DropIndex("dbo.Actual", new[] { "ProductId" });
            DropIndex("dbo.Actual", new[] { "PromoId" });
            DropColumn("dbo.Actual", "ActualProductIncrementalLSV");
            DropColumn("dbo.Actual", "ActualProductIncrementalPCLSV");
            DropColumn("dbo.Actual", "ActualProductIncrementalPCQty");
            DropColumn("dbo.Actual", "ActualProductUplift");
            DropColumn("dbo.Actual", "ActualPromoShare");
            DropColumn("dbo.Actual", "ActualProductPCLSV");
            DropColumn("dbo.Actual", "ActualProductShelfDiscount");
            DropColumn("dbo.Actual", "ActualProductShelfPrice");
            DropColumn("dbo.Actual", "ActualProductSellInDiscount");
            DropColumn("dbo.Actual", "ActualProductSellInPrice");
            DropColumn("dbo.Actual", "ActualProductUOM");
            DropColumn("dbo.Actual", "ActualProductQty");
            DropColumn("dbo.Actual", "ActualProductPCQty");
            DropColumn("dbo.Actual", "PlanProductUplift");
            DropColumn("dbo.Actual", "ProductBaselinePCPrice");
            DropColumn("dbo.Actual", "ProductBaselinePrice");
            DropColumn("dbo.Actual", "PlanProductPCLSV");
            DropColumn("dbo.Actual", "PlanProductLSV");
            DropColumn("dbo.Actual", "PlanProductPCQty");
            DropColumn("dbo.Actual", "PlanProductQty");
            DropColumn("dbo.Actual", "EAN");
            DropColumn("dbo.Actual", "ProductId");
            DropColumn("dbo.Actual", "PromoId");
        }
    }
}
