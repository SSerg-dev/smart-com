namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_Int_To_Double : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Promo", "MarsMechanicDiscount", c => c.Double());
            AlterColumn("dbo.Promo", "PlanInstoreMechanicDiscount", c => c.Double());
            AlterColumn("dbo.Promo", "PlanPromoROIPercent", c => c.Double());
            AlterColumn("dbo.Promo", "PlanPromoNetROIPercent", c => c.Double());
            AlterColumn("dbo.Promo", "PlanPromoNetUpliftPercent", c => c.Double());
            AlterColumn("dbo.Promo", "ActualInStoreDiscount", c => c.Double());
            AlterColumn("dbo.Promo", "ActualPromoNetROIPercent", c => c.Double());
            AlterColumn("dbo.Promo", "ActualPromoNetUpliftPercent", c => c.Double());
            AlterColumn("dbo.Promo", "ActualPromoROIPercent", c => c.Double());
            AlterColumn("dbo.PromoDemandChangeIncident", "OldMarsMechanicDiscount", c => c.Double());
            AlterColumn("dbo.PromoDemandChangeIncident", "NewMarsMechanicDiscount", c => c.Double());
            AlterColumn("dbo.PromoDemandChangeIncident", "OldPlanInstoreMechanicDiscount", c => c.Double());
            AlterColumn("dbo.PromoDemandChangeIncident", "NewPlanInstoreMechanicDiscount", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PromoDemandChangeIncident", "NewPlanInstoreMechanicDiscount", c => c.Int());
            AlterColumn("dbo.PromoDemandChangeIncident", "OldPlanInstoreMechanicDiscount", c => c.Int());
            AlterColumn("dbo.PromoDemandChangeIncident", "NewMarsMechanicDiscount", c => c.Int());
            AlterColumn("dbo.PromoDemandChangeIncident", "OldMarsMechanicDiscount", c => c.Int());
            AlterColumn("dbo.Promo", "ActualPromoROIPercent", c => c.Int());
            AlterColumn("dbo.Promo", "ActualPromoNetUpliftPercent", c => c.Int());
            AlterColumn("dbo.Promo", "ActualPromoNetROIPercent", c => c.Int());
            AlterColumn("dbo.Promo", "ActualInStoreDiscount", c => c.Int());
            AlterColumn("dbo.Promo", "PlanPromoNetUpliftPercent", c => c.Int());
            AlterColumn("dbo.Promo", "PlanPromoNetROIPercent", c => c.Int());
            AlterColumn("dbo.Promo", "PlanPromoROIPercent", c => c.Int());
            AlterColumn("dbo.Promo", "PlanInstoreMechanicDiscount", c => c.Int());
            AlterColumn("dbo.Promo", "MarsMechanicDiscount", c => c.Int());
        }
    }
}
