namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTLCImport : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            CreateTable(
                $"{defaultSchema}.TLCImports",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PromoId = c.Guid(nullable: false),
                        LoadDate = c.DateTimeOffset(nullable: false, precision: 7),
                        HandlerId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            SqlString = SqlString.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlString);
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropTable($"{defaultSchema}.TLCImports");
        }
        private string SqlString = @"
            INSERT [DefaultSchemaSetting].[TLCImports]
           ([PromoId]
           ,[LoadDate])
                 SELECT PromoId, PromoDate FROM [DefaultSchemaSetting].[TLCImport]
            GO

            IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DefaultSchemaSetting].[TLCImport]') AND type in (N'U'))
            DROP TABLE [DefaultSchemaSetting].[TLCImport]
            GO
        ";
    }
}
