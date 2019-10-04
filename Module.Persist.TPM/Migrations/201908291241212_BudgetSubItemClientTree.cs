namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BudgetSubItemClientTree : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BudgetSubItemClientTree",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ClientTreeId = c.Int(nullable: false),
                        BudgetSubItemId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BudgetSubItem", t => t.BudgetSubItemId)
                .ForeignKey("dbo.ClientTree", t => t.ClientTreeId)
                .Index(t => t.ClientTreeId)
                .Index(t => t.BudgetSubItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BudgetSubItemClientTree", "ClientTreeId", "dbo.ClientTree");
            DropForeignKey("dbo.BudgetSubItemClientTree", "BudgetSubItemId", "dbo.BudgetSubItem");
            DropIndex("dbo.BudgetSubItemClientTree", new[] { "BudgetSubItemId" });
            DropIndex("dbo.BudgetSubItemClientTree", new[] { "ClientTreeId" });
            DropTable("dbo.BudgetSubItemClientTree");
        }
    }
}
