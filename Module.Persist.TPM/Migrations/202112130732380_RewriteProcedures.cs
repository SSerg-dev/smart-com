namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RewriteProcedures : DbMigration
    {
		public override void Up()
		{
			var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
			RpaPipeActual_UpdateRPA =RpaPipeActual_UpdateRPA.Replace("DefaultSchemaSetting", defaultSchema);
			RpaPipeActual_UpdateRPAStatus = RpaPipeActual_UpdateRPAStatus.Replace("DefaultSchemaSetting", defaultSchema);
			RpaPipeEvent_UpdateEventByPromo = RpaPipeEvent_UpdateEventByPromo.Replace("DefaultSchemaSetting", defaultSchema);
			RpaPipeEvent_UpdateRPA = RpaPipeEvent_UpdateRPA.Replace("DefaultSchemaSetting", defaultSchema);
			RpaPipeEvent_UpdateRPAStatus = RpaPipeEvent_UpdateRPAStatus.Replace("DefaultSchemaSetting", defaultSchema);
			RpaPipeSupport_UpdateRPA = RpaPipeSupport_UpdateRPA.Replace("DefaultSchemaSetting", defaultSchema);
			RpaPipeSupport_UpdateRPAStatus = RpaPipeSupport_UpdateRPAStatus.Replace("DefaultSchemaSetting", defaultSchema);

			Sql($@"
                    {RpaPipeActual_UpdateRPA}
                    go
					{RpaPipeActual_UpdateRPAStatus}
                    go
					{RpaPipeEvent_UpdateEventByPromo}
                    go
					{RpaPipeEvent_UpdateRPA}
                    go
					{RpaPipeEvent_UpdateRPAStatus}
                    go
                    {RpaPipeSupport_UpdateRPA}
                    go
                    {RpaPipeSupport_UpdateRPAStatus}
                    go
                ");

		}

		public override void Down()
		{
		}

		private string RpaPipeActual_UpdateRPA =
			$@"
                CREATE OR ALTER PROCEDURE [DefaultSchemaSetting].[RpaPipeActual_UpdateRPA]
                (   
	                @RPAId nvarchar(max),
	                @LogFileURL nvarchar(max)
                )
                AS
                BEGIN
	                DECLARE @dropTableQuery nvarchar(max)
                    -- SET NOCOUNT ON added to prevent extra result sets from
                    -- interfering with SELECT statements.
                    SET NOCOUNT ON

                    UPDATE 
		                [DefaultSchemaSetting].[RPA]
	                SET 		                
		                LogURL = @LogFileURL
	                WHERE 
		                Id = @RPAId
				
					SET @dropTableQuery = N'DROP TABLE [DefaultSchemaSetting].' + QUOTENAME('TEMP_RPA_PROMOPRODUCT'+@RPAId)
					EXEC sp_executesql @dropTableQuery

                END
            ";

		private string RpaPipeActual_UpdateRPAStatus =
			$@"
                CREATE OR ALTER PROCEDURE [DefaultSchemaSetting].[RpaPipeActual_UpdateRPAStatus]
                (   
	                @RPAId nvarchar(max),
					@Status nvarchar(max)
                )
                AS
                BEGIN

                    SET NOCOUNT ON

                    UPDATE 
		                [DefaultSchemaSetting].[RPA]
	                SET 
		                Status = @Status
	                WHERE 
		                Id = @RPAId
                END
            ";

		private string RpaPipeEvent_UpdateEventByPromo =
			$@"
                CREATE OR ALTER PROCEDURE [DefaultSchemaSetting].[RpaPipeEvent_UpdateEventByPromo]
                 (
                   @RunPipeId nvarchar(max),
				   @Shema nvarchar(max),
				   @Constraints nvarchar(max)
                )
                AS
                BEGIN
					SET NOCOUNT ON
	                
					DECLARE @query nvarchar(max)
					
					SET @query = N'					
					DECLARE @PromoStatus TABLE (Id uniqueidentifier)
					DECLARE @PromoAllowClient TABLE (Id int)	
					DECLARE @Duplicates TABLE (Number int)

					

					INSERT @PromoStatus (Id) VALUES (''EFCA0CEC-4554-E911-8BC8-08606E18DF3F'')
					INSERT @PromoStatus (Id) VALUES (''D6F20200-4654-E911-8BC8-08606E18DF3F'')
					INSERT @PromoStatus (Id) VALUES (''2305DC07-4654-E911-8BC8-08606E18DF3F'')
					INSERT @PromoStatus (Id) VALUES (''DA5DA702-4754-E911-8BC8-08606E18DF3F'')
					INSERT @PromoStatus (Id) VALUES (''FE7FFE19-4754-E911-8BC8-08606E18DF3F'')

					INSERT @PromoAllowClient(Id) SELECT CAST(value AS INT) FROM STRING_SPLIT('''+@Constraints+''','','')
					
					INSERT @Duplicates(Number)	SELECT PromoNumber
												FROM ['+@Shema+'].' + QUOTENAME('TempEventTestStage'+@RunPipeId) + '
												GROUP BY PromoNumber
												HAVING COUNT(*) > 1

					DECLARE @ExistConstraint int

					SELECT @ExistConstraint = COUNT(*) FROM @PromoAllowClient

	                UPDATE p 
	                SET p.EventName = temp.EventName, p.EventId = e.Id
	                FROM ['+@Shema+'].' + QUOTENAME('TempEventTestStage'+@RunPipeId) + ' temp
	                INNER JOIN ['+@Shema+'].Promo p ON p.Number = temp.PromoNumber
					INNER JOIN @PromoStatus ps ON ps.Id = p.PromoStatusId
					LEFT JOIN @Duplicates dp ON dp.Number = p.Number
	                INNER JOIN ['+@Shema+'].[Event] e ON e.Name = temp.EventName
	                LEFT JOIN @PromoAllowClient pfc ON pfc.Id = p.ClientTreeId
					WHERE ((pfc.Id IS NOT NULL AND @ExistConstraint > 0) OR (pfc.Id IS NULL AND @ExistConstraint = 0)) AND dp.Number IS NULL
					
					
	                SELECT p.Id, 
						temp.PromoNumber, 
						temp.EventName,
		                CASE WHEN e.Name IS NULL OR p.Id IS NULL OR ps.Id IS NULL OR dp.Number IS NOT NULL OR (pfc.Id IS NULL AND @ExistConstraint > 0) THEN ''Error'' ELSE ''Success'' END AS [Status],
		                CASE WHEN p.Id IS NULL  THEN ''Promo not found''						
		                WHEN e.Id IS NULL THEN ''Event not found''
						WHEN ps.Id IS NULL THEN ''Promo status is not valid''
						WHEN dp.Number IS NOT NULL THEN ''The record with this PromoID occurs more than once''
						WHEN pfc.Id IS NULL AND @ExistConstraint > 0 THEN ''No access to the client''
		                ELSE '''' END AS [ValidationMessage]
  		                FROM ['+@Shema+'].' + QUOTENAME('TempEventTestStage'+@RunPipeId) + ' temp
	                LEFT JOIN ['+@Shema+'].Promo p ON p.Number = temp.PromoNumber
					LEFT JOIN @PromoStatus ps ON ps.Id = p.PromoStatusId
					LEFT JOIN @Duplicates dp on dp.Number = p.Number
	                LEFT JOIN ['+@Shema+'].[Event] e ON e.Name = temp.EventName
					LEFT JOIN @PromoAllowClient pfc ON pfc.Id = p.ClientTreeId'
	                
					EXEC sp_executesql @query
                END
            ";

		private string RpaPipeEvent_UpdateRPA =
			@"
				CREATE OR ALTER PROCEDURE [DefaultSchemaSetting].[RpaPipeEvent_UpdateRPA]
                (   
	                @RPAId nvarchar(max),
	                @RunPipeId nvarchar(max),
					@LogFileURL nvarchar(max)
                )
                AS
                BEGIN
	                DECLARE @dropTableQuery nvarchar(max)
                    -- SET NOCOUNT ON added to prevent extra result sets from
                    -- interfering with SELECT statements.
                    SET NOCOUNT ON

                    UPDATE 
		                [DefaultSchemaSetting].[RPA]
	                SET 		                
		                LogURL = @LogFileURL
	                WHERE 
		                Id = @RPAId
				
					SET @dropTableQuery = N'DROP TABLE [DefaultSchemaSetting].' + QUOTENAME('TempEventTestStage'+@RunPipeId)
					EXEC sp_executesql @dropTableQuery

                END
			";

		private string RpaPipeEvent_UpdateRPAStatus =
			@"
				CREATE OR ALTER PROCEDURE [DefaultSchemaSetting].[RpaPipeEvent_UpdateRPAStatus]
                (   
	                @RPAId nvarchar(max),
					@Status nvarchar(max)
                )
                AS
                BEGIN

                    SET NOCOUNT ON

                    UPDATE 
		                [DefaultSchemaSetting].[RPA]
	                SET 
		                Status = @Status
	                WHERE 
		                Id = @RPAId
                END
			";

		private string RpaPipeSupport_UpdateRPA =
			@"
               CREATE OR ALTER PROCEDURE [DefaultSchemaSetting].[RpaPipeSupport_UpdateRPA]
                    (   
	                    @RPAId nvarchar(max),
	                    @LogFileURL nvarchar(max)
                    )
                        AS
                        BEGIN
	                        DECLARE @dropTableQuery nvarchar(max)

                            UPDATE 
		                       [DefaultSchemaSetting].[RPA]
	                        SET 		                
		                        LogURL = @LogFileURL
	                        WHERE 
		                        Id = @RPAId
				
					        SET @dropTableQuery = N'DROP TABLE [DefaultSchemaSetting].' + QUOTENAME('TEMP_RPA_SUPPORT'+@RPAId)
					        EXEC sp_executesql @dropTableQuery
					        SET @dropTableQuery = N'DROP TABLE [DefaultSchemaSetting].' + QUOTENAME('TEMP_RPA_SUPPORTDMP'+@RPAId)
					        EXEC sp_executesql @dropTableQuery

                        END
            ";

		private string RpaPipeSupport_UpdateRPAStatus =
			@"
                CREATE OR ALTER PROCEDURE [DefaultSchemaSetting].[RpaPipeSupport_UpdateRPAStatus]
                    (       
	                    @RPAId nvarchar(max),
					    @Status nvarchar(max)
                    )
                        AS
                        BEGIN

                            SET NOCOUNT ON

                            UPDATE 
		                        [DefaultSchemaSetting].[RPA]
	                        SET 
		                        Status = @Status
	                        WHERE 
		                        Id = @RPAId
                        END
            ";

	}
}
