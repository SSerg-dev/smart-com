namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostPromoEffect_Fields_Modification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "PlanPromoPostPromoEffectLSVW1", c => c.Double());
            AddColumn("dbo.Promo", "PlanPromoPostPromoEffectLSVW2", c => c.Double());
            AddColumn("dbo.Promo", "PlanPromoPostPromoEffectLSV", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoPostPromoEffectLSVW1", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoPostPromoEffectLSVW2", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoPostPromoEffectLSV", c => c.Int());
            DropColumn("dbo.Promo", "PlanPromoPostPromoEffect");
            DropColumn("dbo.Promo", "PlanPromoPostPromoEffectW1");
            DropColumn("dbo.Promo", "PlanPromoPostPromoEffectW2");
            DropColumn("dbo.Promo", "ActualPromoPostPromoEffect");
            DropColumn("dbo.Promo", "ActualPromoPostPromoEffectW1");
            DropColumn("dbo.Promo", "ActualPromoPostPromoEffectW2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Promo", "ActualPromoPostPromoEffectW2", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoPostPromoEffectW1", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoPostPromoEffect", c => c.Int());
            AddColumn("dbo.Promo", "PlanPromoPostPromoEffectW2", c => c.Double());
            AddColumn("dbo.Promo", "PlanPromoPostPromoEffectW1", c => c.Double());
            AddColumn("dbo.Promo", "PlanPromoPostPromoEffect", c => c.Double());
            DropColumn("dbo.Promo", "ActualPromoPostPromoEffectLSV");
            DropColumn("dbo.Promo", "ActualPromoPostPromoEffectLSVW2");
            DropColumn("dbo.Promo", "ActualPromoPostPromoEffectLSVW1");
            DropColumn("dbo.Promo", "PlanPromoPostPromoEffectLSV");
            DropColumn("dbo.Promo", "PlanPromoPostPromoEffectLSVW2");
            DropColumn("dbo.Promo", "PlanPromoPostPromoEffectLSVW1");
        }
    }
}
