namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateActualCOGSTn : DbMigration
    {
        public override void Up()
        {
            AddColumn("jupiter_test.ActualCOGSTn", "IsCOGSIncidentCreated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("jupiter_test.ActualCOGSTn", "IsCOGSIncidentCreated");
        }
    }
}
