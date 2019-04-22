namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoDemand_Change_MarsDates : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PromoDemand", "MarsStartDate");
            DropColumn("dbo.PromoDemand", "MarsEndDate");
            AddColumn("dbo.PromoDemand", "MarsStartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.PromoDemand", "MarsEndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoDemand", "MarsStartDate");
            DropColumn("dbo.PromoDemand", "MarsEndDate");
            AddColumn("dbo.PromoDemand", "MarsEndDate", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.PromoDemand", "MarsStartDate", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
    }
}
