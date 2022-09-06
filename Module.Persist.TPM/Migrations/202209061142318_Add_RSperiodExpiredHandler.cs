namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_RSperiodExpiredHandler : DbMigration
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
            DELETE [DefaultSchemaSetting].[LoopHandler] WHERE [Name] = 'Module.Host.TPM.Handlers.RSperiodExpiredHandler'
                        GO

            INSERT INTO [DefaultSchemaSetting].[LoopHandler]
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
                       ,'Decline expired periods'
                       ,'Module.Host.TPM.Handlers.RSperiodExpiredHandler'
                       ,86400000
                       ,'SCHEDULE'
                       ,GETDATE()
                       ,GETDATE()
                       ,Convert(DateTime, DATEDIFF(DAY, 0, GETDATE() + .5))
                       ,'PROCESSING'
                       ,NULL
                       ,NULL
                       ,NULL
                       ,NULL)
            GO
        ";
    }
}
