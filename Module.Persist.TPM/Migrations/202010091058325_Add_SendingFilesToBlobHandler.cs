namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_SendingFilesToBlobHandler : DbMigration
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

		private string SqlString =
        @"
            DELETE [LoopHandler] WHERE [Name] = 'Module.Host.TPM.Handlers.SendingFilesToBlobHandler'
            GO

            SET DATEFIRST 1;
            DECLARE @today INT = DATEPART(dw, GETDATE());
            DECLARE @nextSaturday DATETIMEOFFSET(7) = DATEADD(DAY, (7 % @today - 1), SYSDATETIME());
            INSERT INTO [LoopHandler]
                       ([Id]
                       ,[Description]
                       ,[Name]
                       ,[ExecutionPeriod]
                       ,[ExecutionMode]
                       ,[CreateDate]
                       ,[LastExecutionDate]
                       ,[NextExecutionDate]
                       ,[ConfigurationName]
                       ,[Status]
                       ,[RunGroup]
                       ,[UserId]
                       ,[RoleId])
                 VALUES
                       (NEWID()
                       ,'Sending all old files to blob storage (only Azure version)'
                       ,'Module.Host.TPM.Handlers.SendingFilesToBlobHandler'
                       ,604800000
                       ,'SCHEDULE'
                       ,SYSDATETIME()
                       ,NULL
                       ,DATETIMEOFFSETFROMPARTS ( 
				            YEAR(@nextSaturday), 
				            MONTH(@nextSaturday),
				            DAY(@nextSaturday), 
				            23, 30, 0, 0, 3, 0, 7 )
                       ,'PROCESSING'
                       ,'WAITING'
                       ,NULL
                       ,NULL
                       ,NULL)
            GO
        ";
	}
}
