namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoDemand_AddDatesRange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoDemand", "MarsStartDate", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.PromoDemand", "MarsEndDate", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoDemand", "MarsEndDate");
            DropColumn("dbo.PromoDemand", "MarsStartDate");
        }
    }
}
