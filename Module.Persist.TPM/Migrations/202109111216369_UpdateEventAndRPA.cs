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
                   @RunPipeId nvarchar(max)
                )
                AS
                BEGIN
	                DECLARE @updateQuery nvarchar(max)
	                DECLARE @selectQuery nvarchar(max)
                    -- SET NOCOUNT ON added to prevent extra result sets from
                    -- interfering with SELECT statements.
                    SET NOCOUNT ON

	                -- Обновляем данные по Event-ам
	                SET @updateQuery = N'  
	                UPDATE p 
	                SET p.EventName = temp.EventName, p.EventId = e.Id
	                FROM [Jupiter].' + QUOTENAME('TempEventTestStage'+@RunPipeId) + ' temp
	                INNER JOIN [Jupiter].Promo p ON p.Number = temp.PromoNumber
	                INNER JOIN [Jupiter].[Event] e ON e.Name = temp.EventName'
	
	                EXEC sp_executesql @updateQuery
  
                  --Возвращаем результат для логирования
                  SET @selectQuery = N'
	                SELECT temp.PromoNumber, 
		                CASE WHEN e.Name IS NULL OR p.Id IS NULL THEN ''Error'' ELSE ''Success'' END AS [Status],
		                CASE when p.Id IS NULL  THEN ''Promo not found'' 
		                when e.Id IS NULL THEN ''Event not found''
		                ELSE '''' END AS [Description]
  		                FROM [Jupiter].' + QUOTENAME('TempEventTestStage'+@RunPipeId) + ' temp
	                LEFT JOIN [Jupiter].Promo p ON p.Number = temp.PromoNumber
	                LEFT JOIN [Jupiter].[Event] e ON e.Name = temp.EventName'
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
                    -- SET NOCOUNT ON added to prevent extra result sets from
                    -- interfering with SELECT statements.
                    SET NOCOUNT ON

                    UPDATE 
		                [Jupiter].[RPA]
	                SET 
		                Status = @Status, 
		                FileURL = '<a href=https://tpmuiuxsa.blob.core.windows.net/jupiteruiuxcontainer/RPAFiles/'+@UploadFileName+' download>Download file</a>',
		                LogURL = '<a href=https://tpmuiuxsa.blob.core.windows.net/jupiteruiuxcontainer/RPAFiles/OutputLogFile_'+@RPAId+'.xlsx download>Log file</a>'
	                WHERE 
		                Id = @RPAId

	                SET @dropTableQuery = N'DROP TABLE [Jupiter].' + QUOTENAME('TempEventTestStage'+@RunPipeId)
	                EXEC sp_executesql @dropTableQuery

                END
            ";

    }
}
