namespace Module.Persist.TPM.Migrations
{
    using System.Data.Entity.Migrations;
    using Core.Settings;

    public partial class Add_AddPromoSupportDMP : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            CreateTable(
                $"{defaultSchema}.PromoSupportDMP",
                c => new
                {
                    Id = c.Guid(nullable: false, identity: true),
                    PromoSupportId = c.Guid(nullable: false),
                    TT = c.Int(nullable: false),
                    Quantity = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id);

        }

        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropIndex($"{defaultSchema}.PromoSupportDMP", new[] { "Id" });
            DropTable($"{defaultSchema}.PromoSupportDMP");
        }
    }
}
