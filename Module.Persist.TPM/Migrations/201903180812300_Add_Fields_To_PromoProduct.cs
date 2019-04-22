namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Fields_To_PromoProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoProduct", "ActualProductPostPromoEffectQty", c => c.Double());
            AddColumn("dbo.PromoProduct", "PlanProductPostPromoEffectQty", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoProduct", "PlanProductPostPromoEffectQty");
            DropColumn("dbo.PromoProduct", "ActualProductPostPromoEffectQty");
        }
    }
}
