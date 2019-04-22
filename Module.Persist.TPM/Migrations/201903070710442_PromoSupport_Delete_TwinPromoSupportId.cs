namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoSupport_Delete_TwinPromoSupportId : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PromoSupport", "TwinPromoSupportId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PromoSupport", "TwinPromoSupportId", c => c.Guid());
        }
    }
}
