namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateEventAndRPA : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            Sql($@"
                    {UpdateRPAStatus}
                    go
					{UpdateEventByPromo}
                    go
                    {UpdateRPA}
                    go
                ");

        }
        
        public override void Down()
        {
        }

        private string UpdateRPAStatus =
            @"
                CREATE OR ALTER PROCEDURE [Jupiter].[RpaPipeEvent_UpdateRPAStatus]
                (   
	                @RPAId nvarchar(max)
                )
                AS
                BEGIN

                    SET NOCOUNT ON

                    UPDATE 
		                [Jupiter].[RPA]
	                SET 
		                Status = 'In progress' 
	                WHERE 
		                Id = @RPAId
                END  
            ";

        private string UpdateEventByPromo =
			@"
                CREATE OR ALTER PROCEDURE [Jupiter].[RpaPipeEvent_UpdateEventByPromo]
                (
                   @RunPipeId nvarchar(max),
				   @Shema nvarchar(max)
                )
                AS
                BEGIN
					DECLARE @insertQuery nvarchar(max)
	                DECLARE @updateQuery nvarchar(max)
	                DECLARE @selectQuery nvarchar(max)
					DECLARE @PromoStatus TABLE (Id uniqueidentifier)

                    SET NOCOUNT ON
					
					SET @insertQuery = N'
						INSERT @PromoStatus (Id) 
							SELECT Id FROM ['+@Shema+'].PromoStatus 
							WHERE SystemName = ''Draft''

						INSERT @PromoStatus (Id) 
							SELECT Id FROM ['+@Shema+'].PromoStatus 
							WHERE SystemName = ''OnApproval''	
							
						INSERT @PromoStatus (Id) 
							SELECT Id FROM ['+@Shema+'].PromoStatus 
							WHERE SystemName = ''Planned''

						INSERT @PromoStatus (Id) 
							SELECT Id FROM ['+@Shema+'].PromoStatus 
							WHERE SystemName = ''Approved''
							
						INSERT @PromoStatus (Id) 
							SELECT Id FROM ['+@Shema+'].PromoStatus 
							WHERE SystemName = ''DraftPpublished'''

					EXEC sp_executesql @insertQuery

	                SET @updateQuery = N'  
	                UPDATE p 
	                SET p.EventName = temp.EventName, p.EventId = e.Id
	                FROM ['+@Shema+'].' + QUOTENAME('TempEventTestStage'+@RunPipeId) + ' temp
	                INNER JOIN ['+@Shema+'].Promo p ON p.Number = temp.PromoNumber
					INNER JOIN @PromoStatus ps ON ps.Id = p.StatusId
	                INNER JOIN ['+@Shema+'].[Event] e ON e.Name = temp.EventName'
	
	                EXEC sp_executesql @updateQuery
  

                  SET @selectQuery = N'
	                SELECT temp.PromoNumber, 
						temp.EventName,
		                CASE WHEN e.Name IS NULL OR p.Id IS NULL OR ps.Id IS NULL THEN ''Error'' ELSE ''Success'' END AS [Status],
		                CASE WHEN p.Id IS NULL  THEN ''Promo not found'' 
		                WHEN e.Id IS NULL THEN ''Event not found''
						WHEN ps.Id IS NULL THEN ''Promo status is not valid''
		                ELSE '''' END AS [Description]
  		                FROM ['+@Shema+'].' + QUOTENAME('TempEventTestStage'+@RunPipeId) + ' temp
	                LEFT JOIN ['+@Shema+'].Promo p ON p.Number = temp.PromoNumber
					LEFT JOIN @PromoStatus ps ON ps.Id = p.StatusId
	                LEFT JOIN ['+@Shema+'].[Event] e ON e.Name = temp.EventName'
	                EXEC sp_executesql @selectQuery
                END
            ";

        private string UpdateRPA =
            @"
                CREATE OR ALTER PROCEDURE [Jupiter].[RpaPipeEvent_UpdateRPA]
                (
                    @Status nvarchar(max),
	                @RPAId nvarchar(max),
	                @RunPipeId nvarchar(max),
	                @UploadFileName nvarchar(max)
                )
                AS
                BEGIN
	                DECLARE @dropTableQuery nvarchar(max)

                    SET NOCOUNT ON

                    UPDATE 
		                [Jupiter].[RPA]
	                SET 
		                Status = @Status,
		                LogURL = '<a href=https://tpmuiuxsa.blob.core.windows.net/jupiteruiuxcontainer/RPAFiles/OutputLogFile_'+@RPAId+'.xlsx download>Log file</a>'
	                WHERE 
		                Id = @RPAId

	                SET @dropTableQuery = N'DROP TABLE [Jupiter].' + QUOTENAME('TempEventTestStage'+@RunPipeId)
	                EXEC sp_executesql @dropTableQuery

                END
            ";

    }
}
