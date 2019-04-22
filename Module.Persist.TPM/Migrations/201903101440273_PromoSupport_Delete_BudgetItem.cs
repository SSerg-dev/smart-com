namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoSupport_Delete_BudgetItem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PromoSupport", "BudgetItemId", "dbo.BudgetItem");
            DropIndex("dbo.PromoSupport", new[] { "BudgetItemId" });
            DropColumn("dbo.PromoSupport", "BudgetItemId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PromoSupport", "BudgetItemId", c => c.Guid());
            CreateIndex("dbo.PromoSupport", "BudgetItemId");
            AddForeignKey("dbo.PromoSupport", "BudgetItemId", "dbo.BudgetItem", "Id");
        }
    }
}
