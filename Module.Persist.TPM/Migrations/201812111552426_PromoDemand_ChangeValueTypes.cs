namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoDemand_ChangeValueTypes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PromoDemand", "Baseline", c => c.Double(nullable: false));
            AlterColumn("dbo.PromoDemand", "Uplift", c => c.Double(nullable: false));
            AlterColumn("dbo.PromoDemand", "Incremental", c => c.Double(nullable: false));
            AlterColumn("dbo.PromoDemand", "Activity", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PromoDemand", "Activity", c => c.Int(nullable: false));
            AlterColumn("dbo.PromoDemand", "Incremental", c => c.Int(nullable: false));
            AlterColumn("dbo.PromoDemand", "Uplift", c => c.Int(nullable: false));
            AlterColumn("dbo.PromoDemand", "Baseline", c => c.Int(nullable: false));
        }
    }
}
