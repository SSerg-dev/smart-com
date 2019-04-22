namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BudgetItem_BudgetSubItems_ChangeIndex : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.BudgetItem", "Unique_Name");
            DropIndex("dbo.BudgetItem", new[] { "BudgetId" });
            DropIndex("dbo.BudgetSubItem", "Unique_BudgetSubItem");
            DropIndex("dbo.BudgetSubItem", new[] { "BudgetItemId" });
            CreateIndex("dbo.BudgetItem", new[] { "Name", "BudgetId", "Disabled", "DeletedDate" }, unique: true, name: "Unique_BudgetItem");
            CreateIndex("dbo.BudgetSubItem", new[] { "Name", "BudgetItemId", "Disabled", "DeletedDate" }, unique: true, name: "Unique_BudgetSubItem");
        }
        
        public override void Down()
        {
            DropIndex("dbo.BudgetSubItem", "Unique_BudgetSubItem");
            DropIndex("dbo.BudgetItem", "Unique_BudgetItem");
            CreateIndex("dbo.BudgetSubItem", "BudgetItemId");
            CreateIndex("dbo.BudgetSubItem", new[] { "Name", "Disabled", "DeletedDate" }, unique: true, name: "Unique_BudgetSubItem");
            CreateIndex("dbo.BudgetItem", "BudgetId");
            CreateIndex("dbo.BudgetItem", new[] { "Name", "Disabled", "DeletedDate" }, unique: true, name: "Unique_Name");
        }
    }
}
