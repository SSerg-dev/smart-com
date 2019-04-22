namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_ChangeType_MarketingTI : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Promo", "MarketingTi", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Promo", "MarketingTi", c => c.Int());
        }
    }
}
