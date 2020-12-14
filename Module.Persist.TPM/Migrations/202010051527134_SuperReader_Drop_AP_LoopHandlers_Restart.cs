namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SuperReader_Drop_AP_LoopHandlers_Restart : DbMigration
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
            DELETE FROM [DefaultSchemaSetting].[AccessPointRole] WHERE Id = (
                SELECT 
                    [apr].[Id]
                FROM [DefaultSchemaSetting].[AccessPoint] ap
                INNER JOIN [DefaultSchemaSetting].[AccessPointRole] apr ON ap.[Id] = apr.[AccessPointId]
                INNER JOIN [DefaultSchemaSetting].[Role] r ON apr.[RoleId] = r.[Id]
                WHERE   
                    [r].[SystemName] = 'SuperReader'
                    AND [ap].[Resource] = 'LoopHandlers'
                    AND [ap].[Action] = 'Restart'
                    AND [r].[Disabled] = 0
                    AND [ap].[Disabled] = 0
            )
        ";
    }
}
