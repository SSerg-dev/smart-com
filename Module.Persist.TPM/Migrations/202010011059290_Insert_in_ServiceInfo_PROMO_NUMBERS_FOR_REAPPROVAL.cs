namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Insert_in_ServiceInfo_PROMO_NUMBERS_FOR_REAPPROVAL : DbMigration
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
            IF NOT EXISTS (SELECT Id FROM [DefaultSchemaSetting].[ServiceInfo] WHERE Name = 'PROMO_NUMBERS_FOR_REAPPROVAL') 
                BEGIN
                    INSERT INTO [DefaultSchemaSetting].[ServiceInfo]
                    (
                        [Name], [Description], [Value]
                    )
                    VALUES
                    ( 
                        'PROMO_NUMBERS_FOR_REAPPROVAL', 'Promo numbers to reapproval', ''
                    )
                END
            GO
        ";
    }
}
