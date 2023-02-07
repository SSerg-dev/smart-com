namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_MLUpload : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.RollingScenario", "FileBufferId", c => c.Guid());
            SqlString = SqlString.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlString);
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropColumn($"{defaultSchema}.RollingScenario", "FileBufferId");
        }
        private string SqlString =
        @"
            INSERT INTO [DefaultSchemaSetting].[Interface]
                       ([Id]
                       ,[Name]
                       ,[Direction]
                       ,[Description])
                 VALUES
                       (NEWID()
                       ,'ML_CALENDAR_ANAPLAN'
                       ,'INBOUND'
                       ,'Input ML Calendar from Anaplan')
            GO
            INSERT INTO [DefaultSchemaSetting].[FileCollectInterfaceSetting]
                       ([Id]
                       ,[InterfaceId]
                       ,[SourcePath]
                       ,[SourceFileMask]
                       ,[CollectHandler])
                 VALUES
                       (NEWID()
                       ,(SELECT [Id] FROM [DefaultSchemaSetting].[Interface] Where Name Like '%ML_CALENDAR_ANAPLAN%')
                       ,'in/Apollo/ML'
                       ,'*.csv*'
                       ,'')
            GO
            INSERT INTO [DefaultSchemaSetting].[CSVProcessInterfaceSetting]
                       ([Id]
                       ,[InterfaceId]
                       ,[Delimiter]
                       ,[UseQuoting]
                       ,[QuoteChar]
                       ,[ProcessHandler])
                 VALUES
                       (NEWID()
                       ,(SELECT [Id] FROM [DefaultSchemaSetting].[Interface] Where Name Like '%ML_CALENDAR_ANAPLAN%')
                       ,'|'
                       ,0
                       ,''
                       ,'')
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
                       ,'Processing ML files'
                       ,'Module.Host.TPM.Handlers.Interface.Incoming.InputMLProcessHandler'
                       ,1200000
                       ,'PERIOD'
                       ,GETDATE()
                       ,NULL
                       ,NULL
                       ,'PROCESSING'
                       ,NULL
                       ,NULL
                       ,NULL
                       ,NULL)
            GO
        ";
    }
}
