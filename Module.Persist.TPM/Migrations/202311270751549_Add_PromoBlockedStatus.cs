namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PromoBlockedStatus : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            CreateTable(
                $"{defaultSchema}.PromoBlockedStatus",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Blocked = c.Boolean(nullable: false),
                        DraftToPublished = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey($"{defaultSchema}.Promo", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id);
            SqlString = SqlString.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlString);
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.PromoBlockedStatus", "Id", $"{defaultSchema}.Promo");
            DropIndex($"{defaultSchema}.PromoBlockedStatus", new[] { "Id" });
            DropTable($"{defaultSchema}.PromoBlockedStatus");
        }

        private string SqlString = $@"
            DELETE FROM [DefaultSchemaSetting].[BlockedPromo]
                  WHERE NOT EXISTS(SELECT * FROM [DefaultSchemaSetting].[Promo] p WHERE  p.Id = [DefaultSchemaSetting].[BlockedPromo].PromoId)
            GO

            INSERT INTO [DefaultSchemaSetting].[PromoBlockedStatus]
                       ([Id]
                       ,[Blocked]
                       ,[DraftToPublished])
                 Select Id, 0, 0 From DefaultSchemaSetting.Promo
            GO
        ";
    }
}
