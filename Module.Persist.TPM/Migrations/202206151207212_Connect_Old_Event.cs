namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Connect_Old_Event : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            SqlString = SqlString.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlString);
            DropIndex($"{defaultSchema}.Event", new[] { "EventTypeId" });
            AlterColumn($"{defaultSchema}.Event", "EventTypeId", c => c.Guid(nullable: false));
            CreateIndex($"{defaultSchema}.Event", "EventTypeId");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropIndex($"{defaultSchema}.Event", new[] { "EventTypeId" });
            AlterColumn($"{defaultSchema}.Event", "EventTypeId", c => c.Guid());
            CreateIndex($"{defaultSchema}.Event", "EventTypeId");
        }
        private string SqlString = @"
            IF NOT EXISTS (SELECT * FROM [DefaultSchemaSetting].[EventType] WHERE [Name] = 'Customer')
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
                       ,'Customer'
                       ,0
                       ,NULL)
            END
            GO
            DECLARE @ItemId UNIQUEIDENTIFIER
            SELECT @ItemId = Id FROM [DefaultSchemaSetting].[EventType] WHERE [Name] = 'Customer'
            UPDATE [DefaultSchemaSetting].[Event]
               SET [EventTypeId] = @ItemId
            GO
        ";
    }
}
