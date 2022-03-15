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
           CREATE OR ALTER PROCEDURE [DefaultSchemaSetting].[SetTaskStatus]
            (@TaskId nvarchar(max), @Status nvarchar(max))
             AS
             BEGIN
	            UPDATE
		            DefaultSchemaSetting.LoopHandler
	            SET
		            [Status] = @Status
	            WHERE 
		            [Id] = @TaskId
            END
        ";
    }
}
