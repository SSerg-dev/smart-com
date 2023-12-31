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
			UpdateRPAStatus = UpdateRPAStatus.Replace("DefaultSchemaSetting", defaultSchema);
			UpdateEventByPromo = UpdateEventByPromo.Replace("DefaultSchemaSetting", defaultSchema);
			UpdateRPA = UpdateRPA.Replace("DefaultSchemaSetting", defaultSchema);

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
			$@"
                CREATE OR ALTER PROCEDURE [DefaultSchemaSetting].[RpaPipeEvent_UpdateRPAStatus]
                (   
	                @RPAId nvarchar(max),
					@Status nvarchar(max)
                )
                AS
                BEGIN

                    SET NOCOUNT ON

                    UPDATE 
		                [Jupiter].[RPA]
	                SET 
		                Status = @Status
	                WHERE 
		                Id = @RPAId
                END
            ";

        private string UpdateEventByPromo =
			@"
                CREATE OR ALTER PROCEDURE [DefaultSchemaSetting].[RpaPipeEvent_UpdateEventByPromo]
                 (
                   @RunPipeId nvarchar(max),
				   @Shema nvarchar(max),
				   @UserRoleName nvarchar(max),
				   @UserId uniqueidentifier
                )
                AS
                BEGIN
					SET NOCOUNT ON
	                
					DECLARE @query nvarchar(max)
					
					SET @query = N'					
					DECLARE @PromoStatus TABLE (Id uniqueidentifier)
					DECLARE @PromoAllowClient TABLE (Id int)
					DECLARE @UserRoleId uniqueidentifier

					SELECT @UserRoleId = us.Id FROM ['+@Shema+'].[UserRole] us 
					INNER JOIN ['+@Shema+'].[Role] r ON r.SystemName = ''' + @UserRoleName +'''
					WHERE us.UserId = ''' + CAST(@UserId AS CHAR(36)) +''' AND us.RoleId = r.Id

					INSERT @PromoStatus (Id) VALUES (''EFCA0CEC-4554-E911-8BC8-08606E18DF3F'')
					INSERT @PromoStatus (Id) VALUES (''D6F20200-4654-E911-8BC8-08606E18DF3F'')
					INSERT @PromoStatus (Id) VALUES (''2305DC07-4654-E911-8BC8-08606E18DF3F'')
					INSERT @PromoStatus (Id) VALUES (''DA5DA702-4754-E911-8BC8-08606E18DF3F'')
					INSERT @PromoStatus (Id) VALUES (''FE7FFE19-4754-E911-8BC8-08606E18DF3F'')

					INSERT @PromoAllowClient(Id) SELECT CAST(Value AS INT) FROM ['+@Shema+'].[Constraint] 
					WHERE UserRoleId = @UserRoleId AND Prefix = ''CLIENT_ID''
					
					DECLARE @ExistConstraint int

					SELECT @ExistConstraint = COUNT(*) FROM @PromoAllowClient

	                UPDATE p 
	                SET p.EventName = temp.EventName, p.EventId = e.Id
	                FROM ['+@Shema+'].' + QUOTENAME('TempEventTestStage'+@RunPipeId) + ' temp
	                INNER JOIN ['+@Shema+'].Promo p ON p.Number = temp.PromoNumber
					INNER JOIN @PromoStatus ps ON ps.Id = p.PromoStatusId
	                INNER JOIN ['+@Shema+'].[Event] e ON e.Name = temp.EventName
	                LEFT JOIN @PromoAllowClient pfc ON pfc.Id = p.ClientTreeId
					WHERE (pfc.Id IS NOT NULL AND @ExistConstraint > 0) OR (pfc.Id IS NULL AND @ExistConstraint = 0)
					
	                SELECT temp.PromoNumber, 
						temp.EventName,
		                CASE WHEN e.Name IS NULL OR p.Id IS NULL OR ps.Id IS NULL OR (pfc.Id IS NULL AND @ExistConstraint > 0) THEN ''Error'' ELSE ''Success'' END AS [Status],
		                CASE WHEN p.Id IS NULL  THEN ''Promo not found''						
		                WHEN e.Id IS NULL THEN ''Event not found''
						WHEN ps.Id IS NULL THEN ''Promo status is not valid''
						WHEN pfc.Id IS NULL AND @ExistConstraint > 0 THEN ''You have constarint for this client''
		                ELSE '''' END AS [Description]
  		                FROM ['+@Shema+'].' + QUOTENAME('TempEventTestStage'+@RunPipeId) + ' temp
	                LEFT JOIN ['+@Shema+'].Promo p ON p.Number = temp.PromoNumber
					LEFT JOIN @PromoStatus ps ON ps.Id = p.PromoStatusId
	                LEFT JOIN ['+@Shema+'].[Event] e ON e.Name = temp.EventName
					LEFT JOIN @PromoAllowClient pfc ON pfc.Id = p.ClientTreeId'
	                
					EXEC sp_executesql @query
                END
            ";

        private string UpdateRPA =
			@"
                CREATE OR ALTER PROCEDURE [DefaultSchemaSetting].[RpaPipeEvent_UpdateRPA]
                (   
	                @RPAId nvarchar(max),
	                @RunPipeId nvarchar(max)
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
		                LogURL = '<a href=https://tpmuiuxsa.blob.core.windows.net/jupiteruiuxcontainer/RPAFiles/OutputLogFile_'+@RPAId+'.xlsx download>Log file</a>'
	                WHERE 
		                Id = @RPAId
				
					SET @dropTableQuery = N'DROP TABLE [Jupiter].' + QUOTENAME('TempEventTestStage'+@RunPipeId)
					EXEC sp_executesql @dropTableQuery

                END
            ";

    }
}
