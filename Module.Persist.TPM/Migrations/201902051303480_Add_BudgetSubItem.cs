namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_BudgetSubItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BudgetSubItem",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Name = c.String(nullable: false, maxLength: 255),
                        BudgetItemId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BudgetItem", t => t.BudgetItemId)
                .Index(t => new { t.Name, t.Disabled, t.DeletedDate }, unique: true, name: "Unique_BudgetSubItem")
                .Index(t => t.BudgetItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BudgetSubItem", "BudgetItemId", "dbo.BudgetItem");
            DropIndex("dbo.BudgetSubItem", new[] { "BudgetItemId" });
            DropIndex("dbo.BudgetSubItem", "Unique_BudgetSubItem");
            DropTable("dbo.BudgetSubItem");
        }
    }
}
