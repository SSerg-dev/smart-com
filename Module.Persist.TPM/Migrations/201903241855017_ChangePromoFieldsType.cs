namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePromoFieldsType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Promo", "PlanPromoTIShopper", c => c.Double());
            AlterColumn("dbo.Promo", "PlanPromoBranding", c => c.Double());
            AlterColumn("dbo.Promo", "PlanPromoCost", c => c.Double());
            AlterColumn("dbo.Promo", "PlanPromoBTL", c => c.Double());
            AlterColumn("dbo.Promo", "PlanPromoIncrementalLSV", c => c.Double());
            AlterColumn("dbo.Promo", "PlanPromoLSV", c => c.Double());
            AlterColumn("dbo.Promo", "PlanPostPromoEffect", c => c.Double());
            AlterColumn("dbo.Promo", "PlanPromoIncrementalNSV", c => c.Double());
            AlterColumn("dbo.Promo", "PlanPromoNetIncrementalNSV", c => c.Double());
            AlterColumn("dbo.Promo", "PlanPromoIncrementalMAC", c => c.Double());
            AlterColumn("dbo.PromoDemandChangeIncident", "OldPlanPromoIncrementalLSV", c => c.Double());
            AlterColumn("dbo.PromoDemandChangeIncident", "NewPlanPromoIncrementalLSV", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PromoDemandChangeIncident", "NewPlanPromoIncrementalLSV", c => c.Int());
            AlterColumn("dbo.PromoDemandChangeIncident", "OldPlanPromoIncrementalLSV", c => c.Int());
            AlterColumn("dbo.Promo", "PlanPromoIncrementalMAC", c => c.Int());
            AlterColumn("dbo.Promo", "PlanPromoNetIncrementalNSV", c => c.Int());
            AlterColumn("dbo.Promo", "PlanPromoIncrementalNSV", c => c.Int());
            AlterColumn("dbo.Promo", "PlanPostPromoEffect", c => c.Int());
            AlterColumn("dbo.Promo", "PlanPromoLSV", c => c.Int());
            AlterColumn("dbo.Promo", "PlanPromoIncrementalLSV", c => c.Int());
            AlterColumn("dbo.Promo", "PlanPromoBTL", c => c.Int());
            AlterColumn("dbo.Promo", "PlanPromoCost", c => c.Int());
            AlterColumn("dbo.Promo", "PlanPromoBranding", c => c.Int());
            AlterColumn("dbo.Promo", "PlanPromoTIShopper", c => c.Int());
        }
    }
}
