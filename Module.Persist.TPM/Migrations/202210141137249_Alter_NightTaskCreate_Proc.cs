namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alter_NightTaskCreate_Proc : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            alterScript = alterScript.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(alterScript);
        }
        
        public override void Down()
        {
        }

        string alterScript = $@"
        ALTER   PROCEDURE [DefaultSchemaSetting].[CreateNightProcessingWaitHandler](
	        @HandlerId nvarchar(100) = N''
        )
        AS

        DECLARE @handlerName VARCHAR(255) = 'Module.Host.TPM.Handlers.NightProcessingWaitHandler'
        DECLARE @NewHandlerId nvarchar(100)
        IF @HandlerId = N'' BEGIN
	        SET @NewHandlerId = NEWID()
        END ELSE BEGIN
	        SET @NewHandlerId = CONVERT(uniqueidentifier, @HandlerId)
        END
	
        INSERT INTO [DefaultSchemaSetting].[LoopHandler] (
	        [Id],
	        [Description],
	        [Name],
	        [ExecutionPeriod],
	        [ExecutionMode],
	        [CreateDate],
	        [LastExecutionDate],
	        [NextExecutionDate],
	        [ConfigurationName],
	        [Status],
	        [RunGroup],
	        [UserId],
	        [RoleId]
        )
        VALUES (
	        @NewHandlerId,
	        N'Wait while night proccess is in progress ',
	        @handlerName,
	        NULL,
	        'SINGLE',
	        SYSDATETIMEOFFSET(),
	        NULL,
	        NULL,
	        'PROCESSING',
	        'WAITING',
	        NULL,
	        NULL,
	        NULL
        )
        ";
    }
}
