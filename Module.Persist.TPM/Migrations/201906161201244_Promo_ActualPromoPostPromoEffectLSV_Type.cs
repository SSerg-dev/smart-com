namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_ActualPromoPostPromoEffectLSV_Type : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Promo", "ActualPromoPostPromoEffectLSV", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Promo", "ActualPromoPostPromoEffectLSV", c => c.Int());
        }
    }
}
