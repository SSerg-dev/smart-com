namespace Module.Persist.TPM.Migrations
{
    using System.Data.Entity.Migrations;
    using Core.Settings;

    public partial class Add_TechSubNameRU_To_BrandTech : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddField = AddField.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(AddField);
        }

        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropField = DropField.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(DropField);
        }

        private string AddField =
        @"
            CREATE OR ALTER FUNCTION DefaultSchemaSetting.[GetTechSubNameRU]
            (
	            @technologyId uniqueidentifier
            )
            RETURNS NVARCHAR(255) AS 
            BEGIN
	            Declare @result NVARCHAR(255)
	            Select @result = CONCAT(DefaultSchemaSetting.Technology.Description_ru, ' ', DefaultSchemaSetting.Technology.SubBrand) From DefaultSchemaSetting.Technology
			            Where DefaultSchemaSetting.Technology.Id = @technologyId

	            RETURN @result
            END

            GO
            ALTER TABLE DefaultSchemaSetting.[BrandTech]
                ADD [TechSubNameRU] AS (DefaultSchemaSetting.[GetTechSubNameRU]([TechnologyId]));
            GO
        ";

        private string DropField =
        @"
            ALTER TABLE DefaultSchemaSetting.BrandTech DROP COLUMN [TechSubNameRU];

            DROP FUNCTION DefaultSchemaSetting.[GetTechSubNameRU]
        ";
    }
}
