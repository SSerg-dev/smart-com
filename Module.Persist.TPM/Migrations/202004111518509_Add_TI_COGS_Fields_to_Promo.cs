namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_TI_COGS_Fields_to_Promo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "PlanTIBasePercent", c => c.Double());
            AddColumn("dbo.Promo", "PlanCOGSPercent", c => c.Double());
            AddColumn("dbo.Promo", "ActualTIBasePercent", c => c.Double());
            AddColumn("dbo.Promo", "ActualCOGSPercent", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "ActualCOGSPercent");
            DropColumn("dbo.Promo", "ActualTIBasePercent");
            DropColumn("dbo.Promo", "PlanCOGSPercent");
            DropColumn("dbo.Promo", "PlanTIBasePercent");
        }
    }
}
