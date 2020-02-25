namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Delete_PromoProductsProperties : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PromoProduct", "ActualProductSellInDiscount");
            DropColumn("dbo.PromoProduct", "ActualProductShelfPrice");
            DropColumn("dbo.PromoProduct", "ActualPromoShare");
            DropColumn("dbo.PromoProduct", "ActualProductPostPromoEffectLSVW1");
            DropColumn("dbo.PromoProduct", "ActualProductPostPromoEffectLSVW2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PromoProduct", "ActualProductPostPromoEffectLSVW2", c => c.Double());
            AddColumn("dbo.PromoProduct", "ActualProductPostPromoEffectLSVW1", c => c.Double());
            AddColumn("dbo.PromoProduct", "ActualPromoShare", c => c.Double());
            AddColumn("dbo.PromoProduct", "ActualProductShelfPrice", c => c.Double());
            AddColumn("dbo.PromoProduct", "ActualProductSellInDiscount", c => c.Double());
        }
    }
}
