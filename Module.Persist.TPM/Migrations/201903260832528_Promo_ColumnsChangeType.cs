namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_ColumnsChangeType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Promo", "ActualPromoTIShopper", c => c.Double());
            AlterColumn("dbo.Promo", "ActualPromoBranding", c => c.Double());
            AlterColumn("dbo.Promo", "ActualPromoBTL", c => c.Double());
            AlterColumn("dbo.Promo", "ActualPromoCost", c => c.Double());
            AlterColumn("dbo.Promo", "ActualPromoIncrementalLSV", c => c.Double());
            AlterColumn("dbo.Promo", "ActualPromoLSV", c => c.Double());
            AlterColumn("dbo.Promo", "ActualPromoIncrementalNSV", c => c.Double());
            AlterColumn("dbo.Promo", "ActualPromoNetIncrementalNSV", c => c.Double());
            AlterColumn("dbo.Promo", "ActualPromoIncrementalMAC", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Promo", "ActualPromoIncrementalMAC", c => c.Int());
            AlterColumn("dbo.Promo", "ActualPromoNetIncrementalNSV", c => c.Int());
            AlterColumn("dbo.Promo", "ActualPromoIncrementalNSV", c => c.Int());
            AlterColumn("dbo.Promo", "ActualPromoLSV", c => c.Int());
            AlterColumn("dbo.Promo", "ActualPromoIncrementalLSV", c => c.Int());
            AlterColumn("dbo.Promo", "ActualPromoCost", c => c.Int());
            AlterColumn("dbo.Promo", "ActualPromoBTL", c => c.Int());
            AlterColumn("dbo.Promo", "ActualPromoBranding", c => c.Int());
            AlterColumn("dbo.Promo", "ActualPromoTIShopper", c => c.Int());
        }
    }
}
