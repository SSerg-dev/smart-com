namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Promo_Product_Fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoProduct", "PlanPostPromoEffectLSVW1", c => c.Double());
            AddColumn("dbo.PromoProduct", "PlanPostPromoEffectLSVW2", c => c.Double());
            AddColumn("dbo.PromoProduct", "PlanPostPromoEffectLSV", c => c.Double());
            AddColumn("dbo.PromoProduct", "ActualPostPromoEffectLSVW1", c => c.Double());
            AddColumn("dbo.PromoProduct", "ActualPostPromoEffectLSVW2", c => c.Double());
            AddColumn("dbo.PromoProduct", "ActualPostPromoEffectLSV", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoProduct", "ActualPostPromoEffectLSV");
            DropColumn("dbo.PromoProduct", "ActualPostPromoEffectLSVW2");
            DropColumn("dbo.PromoProduct", "ActualPostPromoEffectLSVW1");
            DropColumn("dbo.PromoProduct", "PlanPostPromoEffectLSV");
            DropColumn("dbo.PromoProduct", "PlanPostPromoEffectLSVW2");
            DropColumn("dbo.PromoProduct", "PlanPostPromoEffectLSVW1");
        }
    }
}
