namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_ActualPostPromoEffectLSV : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Promo", "ActualPostPromoEffectLSV");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Promo", "ActualPostPromoEffectLSV", c => c.Double());
        }
    }
}
