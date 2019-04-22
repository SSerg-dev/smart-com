namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Promo_And_PromoProduct_Fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "ActualInStoreMechanicsId", c => c.Guid());
            AddColumn("dbo.Promo", "ActualInStoreMechanicTypeId", c => c.Guid());
            AddColumn("dbo.Promo", "PromoDuration", c => c.Int());
            AddColumn("dbo.Promo", "DispatchDuration", c => c.Int());
            AddColumn("dbo.Promo", "InvoiceNumber", c => c.String());
            AddColumn("dbo.Promo", "PlanPromoBaselineLSV", c => c.Double());
            AddColumn("dbo.Promo", "PlanPromoIncrementalBaseTI", c => c.Double());
            AddColumn("dbo.Promo", "PlanPromoIncrementalCOGS", c => c.Double());
            AddColumn("dbo.Promo", "PlanPromoTotalCost", c => c.Double());
            AddColumn("dbo.Promo", "PlanPromoNetIncrementalLSV", c => c.Double());
            AddColumn("dbo.Promo", "PlanPromoNetLSV", c => c.Double());
            AddColumn("dbo.Promo", "PlanPromoNetIncrementalMAC", c => c.Double());
            AddColumn("dbo.Promo", "PlanPromoIncrementalEarnings", c => c.Double());
            AddColumn("dbo.Promo", "PlanPromoNetIncrementalEarnings", c => c.Double());
            AddColumn("dbo.Promo", "PlanPromoNetROIPercent", c => c.Int());
            AddColumn("dbo.Promo", "PlanPromoNetUpliftPercent", c => c.Int());
            AddColumn("dbo.Promo", "ActualPromoBaselineLSV", c => c.Double());
            AddColumn("dbo.Promo", "ActualInStoreDiscount", c => c.Int());
            AddColumn("dbo.Promo", "ActualInStoreShelfPrice", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoIncrementalBaseTI", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoIncrementalCOGS", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoTotalCost", c => c.Double());
            AddColumn("dbo.Promo", "ActualPostPromoEffectLSV", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoNetIncrementalLSV", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoNetLSV", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoNetIncrementalMAC", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoIncrementalEarnings", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoNetIncrementalEarnings", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoNetROIPercent", c => c.Int());
            AddColumn("dbo.Promo", "ActualPromoNetUpliftPercent", c => c.Int());
            AddColumn("dbo.PromoProduct", "PlanProductIncrementalQty", c => c.Double());
            AddColumn("dbo.PromoProduct", "PlanProductUpliftPercent", c => c.Double());
            AddColumn("dbo.PromoProduct", "ActualProductLSV", c => c.Double());
            CreateIndex("dbo.Promo", "ActualInStoreMechanicsId");
            CreateIndex("dbo.Promo", "ActualInStoreMechanicTypeId");
            AddForeignKey("dbo.Promo", "ActualInStoreMechanicsId", "dbo.Mechanic", "Id");
            AddForeignKey("dbo.Promo", "ActualInStoreMechanicTypeId", "dbo.MechanicType", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Promo", "ActualInStoreMechanicTypeId", "dbo.MechanicType");
            DropForeignKey("dbo.Promo", "ActualInStoreMechanicsId", "dbo.Mechanic");
            DropIndex("dbo.Promo", new[] { "ActualInStoreMechanicTypeId" });
            DropIndex("dbo.Promo", new[] { "ActualInStoreMechanicsId" });
            DropColumn("dbo.PromoProduct", "ActualProductLSV");
            DropColumn("dbo.PromoProduct", "PlanProductUpliftPercent");
            DropColumn("dbo.PromoProduct", "PlanProductIncrementalQty");
            DropColumn("dbo.Promo", "ActualPromoNetUpliftPercent");
            DropColumn("dbo.Promo", "ActualPromoNetROIPercent");
            DropColumn("dbo.Promo", "ActualPromoNetIncrementalEarnings");
            DropColumn("dbo.Promo", "ActualPromoIncrementalEarnings");
            DropColumn("dbo.Promo", "ActualPromoNetIncrementalMAC");
            DropColumn("dbo.Promo", "ActualPromoNetLSV");
            DropColumn("dbo.Promo", "ActualPromoNetIncrementalLSV");
            DropColumn("dbo.Promo", "ActualPostPromoEffectLSV");
            DropColumn("dbo.Promo", "ActualPromoTotalCost");
            DropColumn("dbo.Promo", "ActualPromoIncrementalCOGS");
            DropColumn("dbo.Promo", "ActualPromoIncrementalBaseTI");
            DropColumn("dbo.Promo", "ActualInStoreShelfPrice");
            DropColumn("dbo.Promo", "ActualInStoreDiscount");
            DropColumn("dbo.Promo", "ActualPromoBaselineLSV");
            DropColumn("dbo.Promo", "PlanPromoNetUpliftPercent");
            DropColumn("dbo.Promo", "PlanPromoNetROIPercent");
            DropColumn("dbo.Promo", "PlanPromoNetIncrementalEarnings");
            DropColumn("dbo.Promo", "PlanPromoIncrementalEarnings");
            DropColumn("dbo.Promo", "PlanPromoNetIncrementalMAC");
            DropColumn("dbo.Promo", "PlanPromoNetLSV");
            DropColumn("dbo.Promo", "PlanPromoNetIncrementalLSV");
            DropColumn("dbo.Promo", "PlanPromoTotalCost");
            DropColumn("dbo.Promo", "PlanPromoIncrementalCOGS");
            DropColumn("dbo.Promo", "PlanPromoIncrementalBaseTI");
            DropColumn("dbo.Promo", "PlanPromoBaselineLSV");
            DropColumn("dbo.Promo", "InvoiceNumber");
            DropColumn("dbo.Promo", "DispatchDuration");
            DropColumn("dbo.Promo", "PromoDuration");
            DropColumn("dbo.Promo", "ActualInStoreMechanicTypeId");
            DropColumn("dbo.Promo", "ActualInStoreMechanicsId");
        }
    }
}
