namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PrepareNightCalculateHandler : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.RollingScenario", "IsCreateMLpromo", c => c.Boolean(nullable: false));
            SqlString = SqlString.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlString);
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropColumn($"{defaultSchema}.RollingScenario", "IsCreateMLpromo");
        }
        private string SqlString = @"
            DELETE [DefaultSchemaSetting].[LoopHandler] WHERE [Name] = 'Module.Host.TPM.Handlers.PrepareNightCalculateHandler'
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
                       ,'Prepare Night Calculate'
                       ,'Module.Host.TPM.Handlers.PrepareNightCalculateHandler'
                       ,86400000
                       ,'SCHEDULE'
                       ,GETDATE()
                       ,GETDATE()
                       ,DATEADD(mi,1320,DATEDIFF(d,0,GETDATE()))
                       ,'PROCESSING'
                       ,NULL
                       ,NULL
                       ,NULL
                       ,NULL)
            GO
        ";
    }
}
