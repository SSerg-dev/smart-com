namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoDemand_change_WeekField : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PromoDemand", "Week");
            AddColumn("dbo.PromoDemand", "Week", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoDemand", "Week");
            AddColumn("dbo.PromoDemand", "Week", c => c.Int(nullable: false));
        }
    }
}
