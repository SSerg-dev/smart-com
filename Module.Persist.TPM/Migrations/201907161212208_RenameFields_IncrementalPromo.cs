namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameFields_IncrementalPromo : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.IncrementalPromo", "IncrementalCaseAmount", c => c.Double(nullable: true));
            RenameColumn("dbo.IncrementalPromo", "IncrementalCaseAmount", "PlanPromoIncrementalCases");
            RenameColumn("dbo.IncrementalPromo", "IncrementalLSV", "PlanPromoIncrementalLSV");
            RenameColumn("dbo.IncrementalPromo", "IncrementalPrice", "CasePrice");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.IncrementalPromo", "PlanPromoIncrementalCases", c => c.Int(nullable: true));
            RenameColumn("dbo.IncrementalPromo", "PlanPromoIncrementalCases", "IncrementalCaseAmount");
            RenameColumn("dbo.IncrementalPromo", "PlanPromoIncrementalLSV", "IncrementalLSV");
            RenameColumn("dbo.IncrementalPromo", "CasePrice", "IncrementalPrice");
        }
    }
}
