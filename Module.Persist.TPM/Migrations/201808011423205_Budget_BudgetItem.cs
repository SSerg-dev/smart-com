namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Budget_BudgetItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Budget",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.BudgetItem",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Name = c.String(nullable: false, maxLength: 255),
                        BudgetId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Budget", t => t.BudgetId)
                .Index(t => t.Name, unique: true)
                .Index(t => t.BudgetId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BudgetItem", "BudgetId", "dbo.Budget");
            DropIndex("dbo.BudgetItem", new[] { "BudgetId" });
            DropIndex("dbo.BudgetItem", new[] { "Name" });
            DropIndex("dbo.Budget", new[] { "Name" });
            DropTable("dbo.BudgetItem");
            DropTable("dbo.Budget");
        }
    }
}
