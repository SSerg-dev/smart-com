namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoSupportProcedure : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");

            Sql($@"
                CREATE   PROCEDURE {defaultSchema}.[RpaPipeSupport_UpdateRPAStatus]
                    (       
	                    @RPAId nvarchar(max),
					    @Status nvarchar(max)
                    )
                        AS
                        BEGIN

                            SET NOCOUNT ON

                            UPDATE 
		                        {defaultSchema}.[RPA]
	                        SET 
		                        Status = @Status
	                        WHERE 
		                        Id = @RPAId
                        END

                CREATE   PROCEDURE {defaultSchema}.[RpaPipeSupport_UpdateRPA]
                    (   
	                    @RPAId nvarchar(max),
	                    @LogFileURL nvarchar(max)
                    )
                        AS
                        BEGIN
	                        DECLARE @dropTableQuery nvarchar(max)

                            UPDATE 
		                       {defaultSchema}.[RPA]
	                        SET 		                
		                        LogURL = @LogFileURL
	                        WHERE 
		                        Id = @RPAId
				
					        SET @dropTableQuery = N'DROP TABLE {defaultSchema}.' + QUOTENAME('TEMP_RPA_SUPPORT'+@RPAId)
					        EXEC sp_executesql @dropTableQuery
					        SET @dropTableQuery = N'DROP TABLE {defaultSchema}.' + QUOTENAME('TEMP_RPA_SUPPORTDMP'+@RPAId)
					        EXEC sp_executesql @dropTableQuery

                        END

                ALTER PROCEDURE {defaultSchema}.[UpdateSupport]
                (@RPAId nvarchar(max),@SupportType nvarchar(max))
                AS
                BEGIN
				DECLARE @query nvarchar(max)
                SET @query = N'
				DELETE FROM {defaultSchema}.[' + @SupportType + 'DMP] WHERE Id IN (SELECT Id FROM Scenario.[TEMP_RPA_SUPPORTDMP' + @RPAId + '])
				INSERT INTO {defaultSchema}.[' + @SupportType + 'DMP] SELECT * FROM Scenario.[TEMP_RPA_SUPPORTDMP' + @RPAId + ']
				
				UPDATE {defaultSchema}.[' + @SupportType + ']
                SET
                    [ActualQuantity] = trp.[ActualQuantity],
                    [AttachFileName] = trp.[AttachFileName]
                FROM
					(SELECT * FROM {defaultSchema}..[TEMP_RPA_SUPPORT' + @RPAId + ']) AS trp WHERE {defaultSchema}.[' + @SupportType + '].[Id] = trp.Id'
				EXEC sp_executesql @query
                END
                    "
                );
        }
        public override void Down()
        {
        }
    }

}
