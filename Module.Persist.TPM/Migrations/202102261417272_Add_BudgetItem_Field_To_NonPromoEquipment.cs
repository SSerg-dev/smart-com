namespace Module.Persist.TPM.Migrations
{
    using System.Data.Entity.Migrations;
    using Core.Settings;

    public partial class Add_BudgetItem_Field_To_NonPromoEquipment : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.NonPromoEquipment", "BudgetItemId", c => c.Guid());
            CreateIndex($"{defaultSchema}.NonPromoEquipment", "BudgetItemId");
            AddForeignKey($"{defaultSchema}.NonPromoEquipment", "BudgetItemId", $"{defaultSchema}.BudgetItem", "Id");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropIndex($"{defaultSchema}.NonPromoEquipment", new[] { "BudgetItemId" });
            DropForeignKey($"{defaultSchema}.NonPromoEquipment", "BudgetItemId", $"{defaultSchema}.BudgetItem");
            DropColumn($"{defaultSchema}.NonPromoEquipment", "BudgetItemId");
        }
    }
}
