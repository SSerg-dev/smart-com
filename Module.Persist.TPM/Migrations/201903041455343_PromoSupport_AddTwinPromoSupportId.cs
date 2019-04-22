namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoSupport_AddTwinPromoSupportId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoSupport", "TwinPromoSupportId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoSupport", "TwinPromoSupportId");
        }
    }
}
