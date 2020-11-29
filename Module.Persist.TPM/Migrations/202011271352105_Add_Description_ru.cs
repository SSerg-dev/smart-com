namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Description_ru : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NonPromoEquipment", "Description_ru", c => c.String());
            AddColumn("dbo.BudgetItem", "Description_ru", c => c.String());
            AddColumn("dbo.BudgetSubItem", "Description_ru", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BudgetSubItem", "Description_ru");
            DropColumn("dbo.BudgetItem", "Description_ru");
            DropColumn("dbo.NonPromoEquipment", "Description_ru");
        }
    }
}
