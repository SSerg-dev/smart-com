namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_PromoProduct_PromoROIReport_ADD_RENAME_FIELDS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "InOut", c => c.Boolean());
            AddColumn("dbo.Promo", "PlanPromoPostPromoEffect", c => c.Double());
            AddColumn("dbo.Promo", "PlanPromoPostPromoEffectW1", c => c.Double());
            AddColumn("dbo.Promo", "PlanPromoPostPromoEffectW2", c => c.Double());
            AddColumn("dbo.Promo", "PlanPromoNetIncrementalBaseTI", c => c.Double());
            AddColumn("dbo.Promo", "PlanPromoNetIncrementalCOGS", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoNetIncrementalBaseTI", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoNetIncrementalCOGS", c => c.Double());
            AddColumn("dbo.Promo", "PlanPromoNetBaseTI", c => c.Double());
            AddColumn("dbo.Promo", "PlanPromoNSV", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoNetBaseTI", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoNSV", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoLSVByCompensation", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoPostPromoEffect", c => c.Int());
            AddColumn("dbo.Promo", "ActualPromoPostPromoEffectW1", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoPostPromoEffectW2", c => c.Double());
            AddColumn("dbo.PromoProduct", "PlanProductCaseQty", c => c.Double());
            AddColumn("dbo.PromoProduct", "PlanProductBaselineCaseQty", c => c.Double());
            AddColumn("dbo.PromoProduct", "ActualProductCaseQty", c => c.Double());
            AddColumn("dbo.PromoProduct", "PlanProductPostPromoEffectLSVW1", c => c.Double());
            AddColumn("dbo.PromoProduct", "PlanProductPostPromoEffectLSVW2", c => c.Double());
            AddColumn("dbo.PromoProduct", "PlanProductPostPromoEffectLSV", c => c.Double());
            AddColumn("dbo.PromoProduct", "ActualProductPostPromoEffectLSVW1", c => c.Double());
            AddColumn("dbo.PromoProduct", "ActualProductPostPromoEffectLSVW2", c => c.Double());
            AddColumn("dbo.PromoProduct", "ActualProductPostPromoEffectLSV", c => c.Double());
            AddColumn("dbo.PromoProduct", "PlanProductIncrementalCaseQty", c => c.Double());
            AddColumn("dbo.PromoProduct", "ActualProductPostPromoEffectQtyW1", c => c.Double());
            AddColumn("dbo.PromoProduct", "ActualProductPostPromoEffectQtyW2", c => c.Double());
            AddColumn("dbo.PromoProduct", "PlanProductPostPromoEffectQtyW1", c => c.Double());
            AddColumn("dbo.PromoProduct", "PlanProductPostPromoEffectQtyW2", c => c.Double());
            //AddColumn("dbo.PlanIncrementalReport", "PlanProductCaseQty", c => c.Double());
            DropColumn("dbo.Promo", "PlanPostPromoEffect");
            DropColumn("dbo.Promo", "PlanPostPromoEffectW1");
            DropColumn("dbo.Promo", "PlanPostPromoEffectW2");
            DropColumn("dbo.Promo", "FactPostPromoEffect");
            DropColumn("dbo.Promo", "FactPostPromoEffectW1");
            DropColumn("dbo.Promo", "FactPostPromoEffectW2");
            DropColumn("dbo.PromoProduct", "PlanProductQty");
            DropColumn("dbo.PromoProduct", "PlanProductBaselineQty");
            DropColumn("dbo.PromoProduct", "ActualProductQty");
            DropColumn("dbo.PromoProduct", "PlanPostPromoEffectLSVW1");
            DropColumn("dbo.PromoProduct", "PlanPostPromoEffectLSVW2");
            DropColumn("dbo.PromoProduct", "PlanPostPromoEffectLSV");
            DropColumn("dbo.PromoProduct", "ActualPostPromoEffectLSVW1");
            DropColumn("dbo.PromoProduct", "ActualPostPromoEffectLSVW2");
            DropColumn("dbo.PromoProduct", "ActualPostPromoEffectLSV");
            DropColumn("dbo.PromoProduct", "PlanProductIncrementalQty");
            //DropColumn("dbo.PlanIncrementalReport", "PlanProductQty");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.PlanIncrementalReport", "PlanProductQty", c => c.Double());
            AddColumn("dbo.PromoProduct", "PlanProductIncrementalQty", c => c.Double());
            AddColumn("dbo.PromoProduct", "ActualPostPromoEffectLSV", c => c.Double());
            AddColumn("dbo.PromoProduct", "ActualPostPromoEffectLSVW2", c => c.Double());
            AddColumn("dbo.PromoProduct", "ActualPostPromoEffectLSVW1", c => c.Double());
            AddColumn("dbo.PromoProduct", "PlanPostPromoEffectLSV", c => c.Double());
            AddColumn("dbo.PromoProduct", "PlanPostPromoEffectLSVW2", c => c.Double());
            AddColumn("dbo.PromoProduct", "PlanPostPromoEffectLSVW1", c => c.Double());
            AddColumn("dbo.PromoProduct", "ActualProductQty", c => c.Double());
            AddColumn("dbo.PromoProduct", "PlanProductBaselineQty", c => c.Double());
            AddColumn("dbo.PromoProduct", "PlanProductQty", c => c.Double());
            AddColumn("dbo.Promo", "FactPostPromoEffectW2", c => c.Double());
            AddColumn("dbo.Promo", "FactPostPromoEffectW1", c => c.Double());
            AddColumn("dbo.Promo", "FactPostPromoEffect", c => c.Int());
            AddColumn("dbo.Promo", "PlanPostPromoEffectW2", c => c.Double());
            AddColumn("dbo.Promo", "PlanPostPromoEffectW1", c => c.Double());
            AddColumn("dbo.Promo", "PlanPostPromoEffect", c => c.Double());
            //DropColumn("dbo.PlanIncrementalReport", "PlanProductCaseQty");
            DropColumn("dbo.PromoProduct", "PlanProductPostPromoEffectQtyW2");
            DropColumn("dbo.PromoProduct", "PlanProductPostPromoEffectQtyW1");
            DropColumn("dbo.PromoProduct", "ActualProductPostPromoEffectQtyW2");
            DropColumn("dbo.PromoProduct", "ActualProductPostPromoEffectQtyW1");
            DropColumn("dbo.PromoProduct", "PlanProductIncrementalCaseQty");
            DropColumn("dbo.PromoProduct", "ActualProductPostPromoEffectLSV");
            DropColumn("dbo.PromoProduct", "ActualProductPostPromoEffectLSVW2");
            DropColumn("dbo.PromoProduct", "ActualProductPostPromoEffectLSVW1");
            DropColumn("dbo.PromoProduct", "PlanProductPostPromoEffectLSV");
            DropColumn("dbo.PromoProduct", "PlanProductPostPromoEffectLSVW2");
            DropColumn("dbo.PromoProduct", "PlanProductPostPromoEffectLSVW1");
            DropColumn("dbo.PromoProduct", "ActualProductCaseQty");
            DropColumn("dbo.PromoProduct", "PlanProductBaselineCaseQty");
            DropColumn("dbo.PromoProduct", "PlanProductCaseQty");
            DropColumn("dbo.Promo", "ActualPromoPostPromoEffectW2");
            DropColumn("dbo.Promo", "ActualPromoPostPromoEffectW1");
            DropColumn("dbo.Promo", "ActualPromoPostPromoEffect");
            DropColumn("dbo.Promo", "ActualPromoLSVByCompensation");
            DropColumn("dbo.Promo", "ActualPromoNSV");
            DropColumn("dbo.Promo", "ActualPromoNetBaseTI");
            DropColumn("dbo.Promo", "PlanPromoNSV");
            DropColumn("dbo.Promo", "PlanPromoNetBaseTI");
            DropColumn("dbo.Promo", "ActualPromoNetIncrementalCOGS");
            DropColumn("dbo.Promo", "ActualPromoNetIncrementalBaseTI");
            DropColumn("dbo.Promo", "PlanPromoNetIncrementalCOGS");
            DropColumn("dbo.Promo", "PlanPromoNetIncrementalBaseTI");
            DropColumn("dbo.Promo", "PlanPromoPostPromoEffectW2");
            DropColumn("dbo.Promo", "PlanPromoPostPromoEffectW1");
            DropColumn("dbo.Promo", "PlanPromoPostPromoEffect");
            DropColumn("dbo.Promo", "InOut");
        }
    }
}
