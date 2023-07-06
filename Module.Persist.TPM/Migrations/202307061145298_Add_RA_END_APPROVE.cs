namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_RA_END_APPROVE : DbMigration
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
                   ,'RA_END_APPROVE_WEEKS'
                   ,'int'
                   ,'4'
                   ,'Number of weeks for RA period approve')
            GO
            ";
    }
}
