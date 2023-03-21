namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ML_Time_Setting : DbMigration
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
                   ,'ML_TIME_BLOCK'
                   ,'string'
                   ,'06:00;22:00;23:00'
                   ,'ML the time period when file processing is open')
            GO
            ";
    }
}
