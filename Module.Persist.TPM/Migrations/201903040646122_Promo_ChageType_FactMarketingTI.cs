namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_ChageType_FactMarketingTI : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Promo", "FactMarketingTi", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Promo", "FactMarketingTi", c => c.Int());
        }
    }
}
