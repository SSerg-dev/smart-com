namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Settings_Metrics : DbMigration
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
            INSERT INTO [DefaultSchemaSetting].[Setting]
           ([Id]
           ,[Name]
           ,[Type]
           ,[Value]
           ,[Description])
             VALUES
                   (NEWID()
                   ,'METRICS_PPA_GREEN'
                   ,'string'
                   ,'95'
                   ,'Metrics PPA settings')
            GO
            INSERT INTO [DefaultSchemaSetting].[Setting]
           ([Id]
           ,[Name]
           ,[Type]
           ,[Value]
           ,[Description])
             VALUES
                   (NEWID()
                   ,'METRICS_PPA_YELLOW'
                   ,'string'
                   ,'90'
                   ,'Metrics PPA settings')
            GO
            INSERT INTO [DefaultSchemaSetting].[Setting]
           ([Id]
           ,[Name]
           ,[Type]
           ,[Value]
           ,[Description])
             VALUES
                   (NEWID()
                   ,'METRICS_PCT_GREEN'
                   ,'string'
                   ,'90'
                   ,'Metrics PCT settings')
            GO
            INSERT INTO [DefaultSchemaSetting].[Setting]
           ([Id]
           ,[Name]
           ,[Type]
           ,[Value]
           ,[Description])
             VALUES
                   (NEWID()
                   ,'METRICS_PCT_YELLOW'
                   ,'string'
                   ,'85'
                   ,'Metrics PCT settings')
            GO
            INSERT INTO [DefaultSchemaSetting].[Setting]
           ([Id]
           ,[Name]
           ,[Type]
           ,[Value]
           ,[Description])
             VALUES
                   (NEWID()
                   ,'METRICS_PAD_MIN'
                   ,'string'
                   ,'0'
                   ,'Metrics PAD settings')
            GO
            INSERT INTO [DefaultSchemaSetting].[Setting]
           ([Id]
           ,[Name]
           ,[Type]
           ,[Value]
           ,[Description])
             VALUES
                   (NEWID()
                   ,'METRICS_PSFA_GREEN'
                   ,'string'
                   ,'80'
                   ,'Metrics PSFA settings')
            GO
            INSERT INTO [DefaultSchemaSetting].[Setting]
           ([Id]
           ,[Name]
           ,[Type]
           ,[Value]
           ,[Description])
             VALUES
                   (NEWID()
                   ,'METRICS_PSFA_YELLOW'
                   ,'string'
                   ,'75'
                   ,'Metrics PSFA settings')
            GO
            ";
    }
}
