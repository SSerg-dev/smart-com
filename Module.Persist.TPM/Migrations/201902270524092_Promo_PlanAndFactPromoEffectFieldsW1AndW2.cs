namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_PlanAndFactPromoEffectFieldsW1AndW2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "PlanPostPromoEffectW1", c => c.Double());
            AddColumn("dbo.Promo", "PlanPostPromoEffectW2", c => c.Double());
            AddColumn("dbo.Promo", "FactPostPromoEffectW1", c => c.Double());
            AddColumn("dbo.Promo", "FactPostPromoEffectW2", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "FactPostPromoEffectW2");
            DropColumn("dbo.Promo", "FactPostPromoEffectW1");
            DropColumn("dbo.Promo", "PlanPostPromoEffectW2");
            DropColumn("dbo.Promo", "PlanPostPromoEffectW1");
        }
    }
}
