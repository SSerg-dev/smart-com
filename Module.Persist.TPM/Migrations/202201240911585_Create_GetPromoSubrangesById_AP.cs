namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_GetPromoSubrangesById_AP : DbMigration
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

        private string SqlString = @"
           CREATE OR ALTER FUNCTION [DefaultSchemaSetting].[GetPromoSubrangesById]
            (
	            @promoId uniqueidentifier
            )
            RETURNS NVARCHAR(255) AS 
            BEGIN
	            DECLARE @result NVARCHAR(255)
	            SELECT @result = STRING_AGG([Name], ',') 
                FROM [DefaultSchemaSetting].ProductTree 
                WHERE [Type] = 'Subrange' AND EndDate IS NULL AND ObjectId IN (
	                SELECT ProductTreeObjectId 
                    FROM [DefaultSchemaSetting].PromoProductTree 
                    WHERE PromoId = @promoId 
                )
	            RETURN @result
            END
        ";
    }
}
