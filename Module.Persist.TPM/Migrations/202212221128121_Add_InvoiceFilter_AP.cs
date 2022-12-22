namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_InvoiceFilter_AP : DbMigration
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
                IF (NOT EXISTS(SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'InvoiceFilter'))
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
                               (NEWID(), 0, NULL, 'Promoes', 'InvoiceFilter', NULL, 1)
                    END
                GO

                IF (NOT EXISTS(SELECT Id FROM [DefaultSchemaSetting].[AccessPointRole] WHERE AccessPointId IN (SELECT Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'InvoiceFilter')))
                    BEGIN
                        INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                        (
                            [Id]
                            ,[RoleId]
                            ,[AccessPointId]
                        )
                        VALUES
                        (NEWID(), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'CMManager'), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'InvoiceFilter')),
                        (NEWID(), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'Administrator'), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'InvoiceFilter')),
                        (NEWID(), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'FunctionalExpert'), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'InvoiceFilter')),
                        (NEWID(), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'DemandPlanning'), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'InvoiceFilter')),
                        (NEWID(), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'KeyAccountManager'), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'InvoiceFilter')),
                        (NEWID(), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'SuperReader'), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'InvoiceFilter')),
                        (NEWID(), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'DemandFinance'), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'InvoiceFilter')),
                        (NEWID(), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'GAManager'), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'InvoiceFilter')),
                        (NEWID(), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'CustomerMarketing'), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'InvoiceFilter')),
                        (NEWID(), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'SupportAdministrator'), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'InvoiceFilter'))
                    END
                GO
            ";
    }
}
