namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BudgetReasonsUnique : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Budget", new[] { "Name" });
            DropIndex("dbo.BudgetItem", new[] { "Name" });
            DropIndex("dbo.RejectReason", new[] { "Name" });
            CreateIndex("dbo.Budget", new[] { "Name", "Disabled", "DeletedDate" }, unique: true, name: "Unique_Name");
            CreateIndex("dbo.BudgetItem", new[] { "Name", "Disabled", "DeletedDate" }, unique: true, name: "Unique_Name");
            CreateIndex("dbo.RejectReason", new[] { "Name", "Disabled", "DeletedDate" }, unique: true, name: "Unique_Name");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RejectReason", "Unique_Name");
            DropIndex("dbo.BudgetItem", "Unique_Name");
            DropIndex("dbo.Budget", "Unique_Name");
            CreateIndex("dbo.RejectReason", "Name", unique: true);
            CreateIndex("dbo.BudgetItem", "Name", unique: true);
            CreateIndex("dbo.Budget", "Name", unique: true);
        }
    }
}
