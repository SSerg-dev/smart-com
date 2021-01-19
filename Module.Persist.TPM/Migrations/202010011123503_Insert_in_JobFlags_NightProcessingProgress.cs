namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Insert_in_JobFlags_NightProcessingProgress : DbMigration
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
            IF NOT EXISTS (SELECT Prefix FROM [DefaultSchemaSetting].[JobFlag] WHERE Prefix = 'NightProcessingProgress') 
                BEGIN
                    INSERT INTO [DefaultSchemaSetting].[JobFlag]
                    (
                        [Prefix], [Value], [Description]
                    )
                    VALUES
                    ( 
                        N'NightProcessingProgress', 0, N'Show if night processing is in progress'
                    )
                END
            GO
        ";
    }
}
