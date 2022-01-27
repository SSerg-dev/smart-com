namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_SetTaskStatus_AP : DbMigration
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
           CREATE OR ALTER FUNCTION [DefaultSchemaSetting].[SetTaskStatusCompleted]
           (
	            @TaskId nvarchar(max)
           )
            RETURNS NVARCHAR(255) AS 
                BEGIN
	                DECLARE @query nvarchar(max)
                	SELECT @query = N'UPDATE [DefaultSchemaSetting].[LoopHandler]
                    SET [Status] = ''COMPLETE'' WHERE Scenario.[LoopHandler].[Id] ='+@TaskId+''
	                EXEC sp_executesql @query
                END

          CREATE OR ALTER FUNCTION [DefaultSchemaSetting].[SetTaskStatusError]
            (
	            @TaskId nvarchar(max)
            )
            RETURNS NVARCHAR(255) AS 
            BEGIN
	            DECLARE @query nvarchar(max)
                SELECT @query = N'UPDATE [DefaultSchemaSetting].[LoopHandler]
                SET [Status] = ''ERROR'' WHERE Scenario.[LoopHandler].[Id] ='+@TaskId+''
	            EXEC sp_executesql @query
            END

        ";
    }
}
