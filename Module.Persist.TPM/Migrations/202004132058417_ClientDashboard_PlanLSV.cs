namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientDashboard_PlanLSV : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientDashboard", "PlanLSV", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClientDashboard", "PlanLSV");
        }
    }
}
