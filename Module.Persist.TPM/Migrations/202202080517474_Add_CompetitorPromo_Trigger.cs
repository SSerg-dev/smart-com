namespace Module.Persist.TPM.Migrations
{
    using System.Data.Entity.Migrations;
    using Core.Settings;

    public partial class Add_CompetitorPromo_Trigger : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            SqlString = SqlString.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlString);
        }

        public override void Down()
        {
        }

        string SqlString = $@"
        IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DefaultSchemaSetting].[CompetitorPromoSequence]') AND type = 'SO')
        CREATE SEQUENCE [DefaultSchemaSetting].[CompetitorPromoSequence] 
            AS [bigint]
            START WITH 1
            INCREMENT BY 1
            MINVALUE 1
            MAXVALUE 9223372036854775807
            CACHE  
        GO

        
        CREATE OR ALTER TRIGGER [DefaultSchemaSetting].[CompetitorPromo_increment_number] ON [DefaultSchemaSetting].[CompetitorPromo] AFTER INSERT AS

        BEGIN
	        UPDATE cp
		        SET 
			        cp.Number = NEXT VALUE FOR DefaultSchemaSetting.CompetitorPromoSequence
		        FROM
			        DefaultSchemaSetting.CompetitorPromo cp
			        INNER JOIN Inserted i ON i.Id = cp.Id
        END;
        ";
    }
}
