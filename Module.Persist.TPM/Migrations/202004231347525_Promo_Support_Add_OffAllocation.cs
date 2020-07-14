namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_Support_Add_OffAllocation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoSupport", "OffAllocation", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoSupport", "OffAllocation");
        }
    }
}
