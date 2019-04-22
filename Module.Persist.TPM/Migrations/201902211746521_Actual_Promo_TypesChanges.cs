namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Actual_Promo_TypesChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Promo", "PlanUplift", c => c.Double());
            AlterColumn("dbo.Promo", "FactUplift", c => c.Double());
            AlterColumn("dbo.Actual", "ZREP", c => c.String(maxLength: 255));
            AlterColumn("dbo.Actual", "PlanProductQty", c => c.Double());
            AlterColumn("dbo.Actual", "ActualProductQty", c => c.Double());
            AlterColumn("dbo.PromoDemandChangeIncident", "OldPlanUplift", c => c.Double());
            AlterColumn("dbo.PromoDemandChangeIncident", "NewPlanUplift", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PromoDemandChangeIncident", "NewPlanUplift", c => c.Int());
            AlterColumn("dbo.PromoDemandChangeIncident", "OldPlanUplift", c => c.Int());
            AlterColumn("dbo.Actual", "ActualProductQty", c => c.Int());
            AlterColumn("dbo.Actual", "PlanProductQty", c => c.Int());
            AlterColumn("dbo.Actual", "ZREP", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Promo", "FactUplift", c => c.Int());
            AlterColumn("dbo.Promo", "PlanUplift", c => c.Int());
        }
    }
}
