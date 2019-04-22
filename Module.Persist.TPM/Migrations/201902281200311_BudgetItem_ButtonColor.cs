namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BudgetItem_ButtonColor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BudgetItem", "ButtonColor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BudgetItem", "ButtonColor");
        }
    }
}
