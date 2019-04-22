namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_approval_fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "IsAutomaticallyApproved", c => c.Boolean());
            AddColumn("dbo.Promo", "IsCustomerMarketingApproved", c => c.Boolean());
            AddColumn("dbo.Promo", "IsDemandPlanningApproved", c => c.Boolean());
            AddColumn("dbo.Promo", "IsDemandFinanceApproved", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "IsDemandFinanceApproved");
            DropColumn("dbo.Promo", "IsDemandPlanningApproved");
            DropColumn("dbo.Promo", "IsCustomerMarketingApproved");
            DropColumn("dbo.Promo", "IsAutomaticallyApproved");
        }
    }
}
