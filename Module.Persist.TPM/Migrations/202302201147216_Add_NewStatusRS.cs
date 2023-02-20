namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_NewStatusRS : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.RollingScenario", "RSstatus", c => c.String());
            SqlString = SqlString.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlString);
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropColumn($"{defaultSchema}.RollingScenario", "RSstatus");
        }
        private string SqlString =
        @"
            UPDATE Table_A
               SET [RSstatus] = Table_B.Name
               FROM
                   [DefaultSchemaSetting].[RollingScenario] AS Table_A
                INNER JOIN [DefaultSchemaSetting].[PromoStatus] AS Table_B
                    ON Table_A.PromoStatusId = Table_B.id
            GO
        ";
    }
}
