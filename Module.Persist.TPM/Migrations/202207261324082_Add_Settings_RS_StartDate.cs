namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Settings_RS_StartDate : DbMigration
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
                   ,'RS_START_WEEKS'
                   ,'int'
                   ,'8'
                   ,'Number of weeks before RS period Start date')
            GO
            ";
    }
}
