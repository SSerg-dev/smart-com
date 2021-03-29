namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_DMP : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            CreateTable(
                $"{defaultSchema}.NonPromoSupportDMP",
                c => new
                {
                    Id = c.Guid(nullable: false, identity: true),
                    NonPromoSupportId = c.Guid(nullable: false),
                    TT = c.Int(nullable: false),
                    Quantity = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id);

        }

        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropIndex($"{defaultSchema}.NonPromoSupportDMP", new[] { "Id" });
            DropTable($"{defaultSchema}.NonPromoSupportDMP");
        }
    }
}
