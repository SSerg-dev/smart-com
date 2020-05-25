namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Edit_ClientDashboard : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientDashboard", "PromoTiCostPlanPercent", c => c.Double());
            AddColumn("dbo.ClientDashboard", "NonPromoTiCostPlanPercent", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClientDashboard", "NonPromoTiCostPlanPercent");
            DropColumn("dbo.ClientDashboard", "PromoTiCostPlanPercent");
        }
    }
}
