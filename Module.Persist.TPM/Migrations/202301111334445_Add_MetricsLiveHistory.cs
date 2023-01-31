namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_MetricsLiveHistory : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            
            CreateTable(
                $"{defaultSchema}.MetricsLiveHistories",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Type = c.Int(nullable: false),
                        Date = c.DateTimeOffset(nullable: false, precision: 7),
                        ClientTreeId = c.Int(nullable: false),
                        Value = c.Double(nullable: false),
                        ValueLSV = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            SqlString = SqlString.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlString);
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropTable($"{defaultSchema}.MetricsLiveHistories");
        }
        private string SqlString = @" 
                IF (NOT EXISTS(SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetMetricsLiveHistories' AND Resource = 'MetricsLiveHistories'))
                    BEGIN
                        INSERT INTO [Jupiter].[AccessPoint]
                               ([Id]
                               ,[Disabled]
                               ,[DeletedDate]
                               ,[Resource]
                               ,[Action]
                               ,[Description]
                               ,[TPMmode])
                         VALUES
                               (NEWID(), 0, NULL, 'MetricsLiveHistories', 'GetMetricsLiveHistories', NULL, 1)
                    END
                GO

                IF (NOT EXISTS(SELECT Id FROM [DefaultSchemaSetting].[AccessPointRole] WHERE AccessPointId IN (SELECT Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetMetricsLiveHistories' AND Resource = 'MetricsLiveHistories')))
                    BEGIN
                        INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                        (
                            [Id]
                            ,[RoleId]
                            ,[AccessPointId]
                        )
                        VALUES
                        (NEWID(), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'Administrator'), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetMetricsLiveHistories' AND Resource = 'MetricsLiveHistories')),
                        (NEWID(), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'FunctionalExpert'), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetMetricsLiveHistories' AND Resource = 'MetricsLiveHistories')),
                        (NEWID(), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'SupportAdministrator'), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetMetricsLiveHistories' AND Resource = 'MetricsLiveHistories'))
                    END
                GO

                IF (NOT EXISTS(SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetFilteredData' AND Resource = 'MetricsLiveHistories'))
                    BEGIN
                        INSERT INTO [Jupiter].[AccessPoint]
                               ([Id]
                               ,[Disabled]
                               ,[DeletedDate]
                               ,[Resource]
                               ,[Action]
                               ,[Description]
                               ,[TPMmode])
                         VALUES
                               (NEWID(), 0, NULL, 'MetricsLiveHistories', 'GetFilteredData', NULL, 1)
                    END
                GO

                IF (NOT EXISTS(SELECT Id FROM [DefaultSchemaSetting].[AccessPointRole] WHERE AccessPointId IN (SELECT Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetFilteredData' AND Resource = 'MetricsLiveHistories')))
                    BEGIN
                        INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                        (
                            [Id]
                            ,[RoleId]
                            ,[AccessPointId]
                        )
                        VALUES
                        (NEWID(), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'Administrator'), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetFilteredData' AND Resource = 'MetricsLiveHistories')),
                        (NEWID(), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'FunctionalExpert'), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetFilteredData' AND Resource = 'MetricsLiveHistories')),
                        (NEWID(), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'SupportAdministrator'), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetFilteredData' AND Resource = 'MetricsLiveHistories'))
                    END
                GO
            ";
    }
}
