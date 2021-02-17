namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_AccessPoint_MassApproval : DbMigration
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
                IF (NOT EXISTS(SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'MassApprove'))
                    BEGIN
                        INSERT INTO [DefaultSchemaSetting].[AccessPoint]
                        (
                            [Id]
                            ,[Disabled]
                            ,[DeletedDate]
                            ,[Resource]
                            ,[Action]
                            ,[Description]
                        )
                        VALUES
                        (
                        NEWID(), 0, NULL, 'Promoes', 'MassApprove', NULL
                        )
                    END
                GO

                IF (NOT EXISTS(SELECT Id FROM [DefaultSchemaSetting].[AccessPointRole] WHERE AccessPointId IN (SELECT Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'MassApprove')))
                    BEGIN
                        INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                        (
                            [Id]
                            ,[RoleId]
                            ,[AccessPointId]
                        )
                        VALUES
                        (NEWID(), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'CMManager'), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'MassApprove')),
                        (NEWID(), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'DemandPlanning'), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'MassApprove')),
                        (NEWID(), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'DemandFinance'), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'MassApprove'))
                    END
                GO
            ";
    }
}
