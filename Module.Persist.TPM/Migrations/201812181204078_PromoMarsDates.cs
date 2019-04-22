namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoMarsDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "MarsStartDate", c => c.String(maxLength: 15));
            AddColumn("dbo.Promo", "MarsEndDate", c => c.String(maxLength: 15));
            AddColumn("dbo.Promo", "MarsDispatchesStart", c => c.String(maxLength: 15));
            AddColumn("dbo.Promo", "MarsDispatchesEnd", c => c.String(maxLength: 15));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "MarsDispatchesEnd");
            DropColumn("dbo.Promo", "MarsDispatchesStart");
            DropColumn("dbo.Promo", "MarsEndDate");
            DropColumn("dbo.Promo", "MarsStartDate");
        }
    }
}
