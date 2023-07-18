namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_Add_ML_RSRA : DbMigration
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
            UPDATE [DefaultSchemaSetting].[Interface]
               SET [Name] = 'ML_CALENDAR_ANAPLAN_RS'
                  ,[Description] = 'Input ML Calendar from Anaplan RS'
             WHERE Name = 'ML_CALENDAR_ANAPLAN'
            GO

            UPDATE [DefaultSchemaSetting].[FileCollectInterfaceSetting]
               SET [SourcePath] = 'ML\RS'
             WHERE InterfaceId = (SELECT [Id] FROM [DefaultSchemaSetting].[Interface] Where Name Like '%ML_CALENDAR_ANAPLAN_RS%')
            GO

            INSERT INTO [DefaultSchemaSetting].[Interface]
                       ([Id]
                       ,[Name]
                       ,[Direction]
                       ,[Description])
                 VALUES
                       (NEWID()
                       ,'ML_CALENDAR_ANAPLAN_RA'
                       ,'INBOUND'
                       ,'Input ML Calendar from Anaplan RA')
            GO
            INSERT INTO [DefaultSchemaSetting].[FileCollectInterfaceSetting]
                       ([Id]
                       ,[InterfaceId]
                       ,[SourcePath]
                       ,[SourceFileMask]
                       ,[CollectHandler])
                 VALUES
                       (NEWID()
                       ,(SELECT [Id] FROM [DefaultSchemaSetting].[Interface] Where Name Like '%ML_CALENDAR_ANAPLAN_RA%')
                       ,'ML\RA'
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
                       ,(SELECT [Id] FROM [DefaultSchemaSetting].[Interface] Where Name Like '%ML_CALENDAR_ANAPLAN_RA%')
                       ,'|'
                       ,0
                       ,''
                       ,'')
            GO
        ";
    }
}
