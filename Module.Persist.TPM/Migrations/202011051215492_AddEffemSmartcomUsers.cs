namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEffemSmartcomUsers : DbMigration
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
                UPDATE [DefaultSchemaSetting].[User] SET [Disabled] = 1, [DeletedDate] = GETDATE()
                WHERE [DefaultSchemaSetting].[User].[Name]='kondratiev.aleksey@effem.com'
                OR [DefaultSchemaSetting].[User].[Name]='eugeny.suglobov@effem.com'
                OR [DefaultSchemaSetting].[User].[Name]='artem.x.morozov@effem.com'
                OR [DefaultSchemaSetting].[User].[Name]='andrey.philushkin@effem.com'
                OR [DefaultSchemaSetting].[User].[Name]='mikhail.volovich@effem.com'
                OR [DefaultSchemaSetting].[User].[Name]='bondarenko.eugeny@effem.com'
                GO

                INSERT INTO [DefaultSchemaSetting].[User] ([Id], [DeletedDate], [Disabled], [Email], [Name], [Password], [PasswordSalt], [Sid])
                VALUES 
                (NEWID(), NULL, 0, NULL, 'kondratiev.aleksey@effem.com', NULL, NULL, ''),
                (NEWID(), NULL, 0, NULL, 'eugeny.suglobov@effem.com', NULL, NULL, ''),
                (NEWID(), NULL, 0, NULL, 'artem.x.morozov@effem.com', NULL, NULL, ''),
                (NEWID(), NULL, 0, NULL, 'andrey.philushkin@effem.com', NULL, NULL, ''),
                (NEWID(), NULL, 0, NULL, 'bondarenko.eugeny@effem.com', NULL, NULL, ''),
                (NEWID(), NULL, 0, NULL, 'mikhail.volovich@effem.com', NULL, NULL, '')
                GO

                DECLARE @name NVARCHAR(100);
                SET @name='kondratiev.aleksey@effem.com'
                INSERT INTO [DefaultSchemaSetting].[UserRole] ([Id], [UserId], [RoleId], [IsDefault])
                VALUES 
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'CMManager'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'FunctionalExpert'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'DemandPlanning'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'KeyAccountManager'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'SuperReader'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'DemandFinance'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'CustomerMarketing'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'SupportAdministrator'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'Administrator'), 1)

                SET @name='bondarenko.eugeny@effem.com'
                INSERT INTO [DefaultSchemaSetting].[UserRole] ([Id], [UserId], [RoleId], [IsDefault])
                VALUES 
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'CMManager'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'FunctionalExpert'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'DemandPlanning'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'KeyAccountManager'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'SuperReader'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'DemandFinance'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'CustomerMarketing'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'SupportAdministrator'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'Administrator'), 1)

                SET @name='eugeny.suglobov@effem.com'
                INSERT INTO [DefaultSchemaSetting].[UserRole] ([Id], [UserId], [RoleId], [IsDefault])
                VALUES 
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'CMManager'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'FunctionalExpert'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'DemandPlanning'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'KeyAccountManager'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'SuperReader'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'DemandFinance'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'CustomerMarketing'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'SupportAdministrator'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'Administrator'), 1)

                SET @name='artem.x.morozov@effem.com'
                INSERT INTO [DefaultSchemaSetting].[UserRole] ([Id], [UserId], [RoleId], [IsDefault])
                VALUES 
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'CMManager'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'FunctionalExpert'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'DemandPlanning'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'KeyAccountManager'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'SuperReader'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'DemandFinance'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'CustomerMarketing'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'SupportAdministrator'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'Administrator'), 1)

                SET @name='andrey.philushkin@effem.com'
                INSERT INTO [DefaultSchemaSetting].[UserRole] ([Id], [UserId], [RoleId], [IsDefault])
                VALUES 
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'CMManager'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'FunctionalExpert'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'DemandPlanning'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'KeyAccountManager'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'SuperReader'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'DemandFinance'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'CustomerMarketing'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'SupportAdministrator'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'Administrator'), 1)

                SET @name='mikhail.volovich@effem.com'
                INSERT INTO [DefaultSchemaSetting].[UserRole] ([Id], [UserId], [RoleId], [IsDefault])
                VALUES 
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'CMManager'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'FunctionalExpert'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'DemandPlanning'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'KeyAccountManager'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'SuperReader'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'DemandFinance'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'CustomerMarketing'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'SupportAdministrator'), 0),
                (NEWID(), (SELECT Id FROM [DefaultSchemaSetting].[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'Administrator'), 1)
                GO
            ";
    }
}
