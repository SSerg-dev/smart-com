namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_EventType : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            SqlString = SqlString.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlString);
            AddColumn($"{defaultSchema}.Event", "MarketSegment", c => c.String(maxLength: 255));
            DropColumn($"{defaultSchema}.EventType", "MarketSegment");

        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.EventType", "MarketSegment", c => c.String(maxLength: 255));
            DropColumn($"{defaultSchema}.Event", "MarketSegment");
        }
        private string SqlString = @"
            IF NOT EXISTS (SELECT * FROM [DefaultSchemaSetting].[EventType] WHERE [Name] = 'National')
            BEGIN
            INSERT INTO [DefaultSchemaSetting].[EventType]
                       ([Id]
                       ,[Disabled]
                       ,[DeletedDate]
                       ,[Name]
                       ,[National]
                       ,[MarketSegment])
                 VALUES
                       (NEWID()
                       ,0
                       ,NULL
                       ,'National'
                       ,1
                       ,NULL)
            END
            GO
        ";
    }
}
