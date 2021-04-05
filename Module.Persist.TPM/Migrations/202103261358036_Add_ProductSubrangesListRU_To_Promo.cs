namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System.Data.Entity.Migrations;

    public partial class Add_ProductSubrangesListRU_To_Promo : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.Promo", "ProductSubrangesListRU", c => c.String(maxLength: 500));
            CreateIndex($"{defaultSchema}.ProductTree", "TechnologyId");
            AddForeignKey($"{defaultSchema}.ProductTree", "TechnologyId", $"{defaultSchema}.Technology", "Id");
        }

        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.ProductTree", "TechnologyId", $"{defaultSchema}.Technology");
            DropIndex($"{defaultSchema}.ProductTree", new[] { "TechnologyId" });
            DropColumn($"{defaultSchema}.Promo", "ProductSubrangesListRU");
        }
    }
}
