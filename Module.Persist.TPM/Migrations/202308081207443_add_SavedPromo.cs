namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using Module.Persist.TPM.Migrations.Views;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_SavedPromo : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            CreateTable(
                $"{defaultSchema}.SavedPromo",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        CreateDate = c.DateTimeOffset(nullable: false, precision: 7),
                        StartDate = c.DateTimeOffset(nullable: false, precision: 7),
                        EndDate = c.DateTimeOffset(nullable: false, precision: 7),
                        ClientTreeId = c.Int(),
                        SavedPromoType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey($"{defaultSchema}.ClientTree", t => t.ClientTreeId)
                .Index(t => t.ClientTreeId);
            
            AddColumn($"{defaultSchema}.Promo", "SavedPromoId", c => c.Guid());
            CreateIndex($"{defaultSchema}.Promo", "SavedPromoId");
            AddForeignKey($"{defaultSchema}.Promo", "SavedPromoId", $"{defaultSchema}.SavedPromo", "Id", cascadeDelete: true);

            Sql(ViewMigrations.UpdateClientDashboardRSViewString(defaultSchema));
            Sql(ViewMigrations.GetPromoRSViewString(defaultSchema));
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.Promo", "SavedPromoId", $"{defaultSchema}.SavedPromo");
            DropForeignKey($"{defaultSchema}.SavedPromo", "ClientTreeId", $"{defaultSchema}.ClientTree");
            DropIndex($"{defaultSchema}.SavedPromo", new[] { "ClientTreeId" });
            DropIndex($"{defaultSchema}.Promo", new[] { "SavedPromoId" });
            DropColumn($"{defaultSchema}.Promo", "SavedPromoId");
            DropTable($"{defaultSchema}.SavedPromo");
        }
    }
}
