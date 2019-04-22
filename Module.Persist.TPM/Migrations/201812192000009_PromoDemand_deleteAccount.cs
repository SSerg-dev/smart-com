namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoDemand_deleteAccount : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PromoDemand", "Account");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PromoDemand", "Account", c => c.String());
        }
    }
}
