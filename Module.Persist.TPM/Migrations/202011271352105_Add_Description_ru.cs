namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;

    public partial class Add_Description_ru : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.NonPromoEquipment", "Description_ru", c => c.String());
            AddColumn($"{defaultSchema}.BudgetItem", "Description_ru", c => c.String());
            AddColumn($"{defaultSchema}.BudgetSubItem", "Description_ru", c => c.String());
        }

        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropColumn($"{defaultSchema}.BudgetSubItem", "Description_ru");
            DropColumn($"{defaultSchema}.BudgetItem", "Description_ru");
            DropColumn($"{defaultSchema}.NonPromoEquipment", "Description_ru");
        }
    }
}
