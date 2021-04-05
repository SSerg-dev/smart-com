namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System.Data.Entity.Migrations;
    
    public partial class Add_BrandsegTechsubRU_To_BrandTech : DbMigration
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
            CREATE OR ALTER FUNCTION DefaultSchemaSetting.[GetBrandsegTechsubNameRU]
            (
	            @brandId uniqueidentifier,
	            @technologyId uniqueidentifier
            )
            RETURNS NVARCHAR(255) AS 
            BEGIN
	            Declare @result NVARCHAR(255)
	            Select @result = CONCAT(DefaultSchemaSetting.Brand.Name, ' ', DefaultSchemaSetting.Technology.Description_ru, ' ', DefaultSchemaSetting.Technology.SubBrand) From DefaultSchemaSetting.Brand, DefaultSchemaSetting.Technology
			            Where DefaultSchemaSetting.Brand.Id = @brandId And DefaultSchemaSetting.Technology.Id = @technologyId

	            RETURN @result
            END

            GO
            ALTER TABLE DefaultSchemaSetting.[BrandTech]
                ADD [BrandsegTechsubRU] AS (DefaultSchemaSetting.[GetBrandsegTechsubNameRU]([BrandId], [TechnologyId]));
            GO
        ";

        private string DropField =
        @"
            ALTER TABLE DefaultSchemaSetting.BrandTech DROP COLUMN [BrandsegTechsubRU];

            DROP FUNCTION DefaultSchemaSetting.[GetBrandsegTechsubNameRU]
        ";
    }
}
