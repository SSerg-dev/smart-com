namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Fields_To_Promo_PromoProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "ActualPromoLSVSI", c => c.Double());
            AddColumn("dbo.Promo", "ActualPromoLSVSO", c => c.Double());
            AddColumn("dbo.PromoProduct", "ActualProductBaselineCaseQty", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoProduct", "ActualProductBaselineCaseQty");
            DropColumn("dbo.Promo", "ActualPromoLSVSO");
            DropColumn("dbo.Promo", "ActualPromoLSVSI");
        }
    }
}
