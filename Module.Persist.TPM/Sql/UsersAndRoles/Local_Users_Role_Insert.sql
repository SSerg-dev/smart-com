DECLARE @name NVARCHAR(100);
SET @name='MSNBH-POLYAART\artem polyanin'
INSERT INTO [dbo].[UserRole] ([Id], [UserId], [RoleId], [IsDefault])
VALUES 
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'CMManager'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'FunctionalExpert'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'DemandPlanning'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'KeyAccountManager'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'SuperReader'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'DemandFinance'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'CustomerMarketing'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'SupportAdministrator'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'Administrator'), 1)

SET @name='msnbh-filyuand\andrey filyushkin'
INSERT INTO [dbo].[UserRole] ([Id], [UserId], [RoleId], [IsDefault])
VALUES 
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'CMManager'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'FunctionalExpert'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'DemandPlanning'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'KeyAccountManager'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'SuperReader'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'DemandFinance'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'CustomerMarketing'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'SupportAdministrator'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'Administrator'), 1)

SET @name='msnbh-zinkeale\alexander zinkevich'
INSERT INTO [dbo].[UserRole] ([Id], [UserId], [RoleId], [IsDefault])
VALUES 
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'CMManager'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'FunctionalExpert'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'DemandPlanning'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'KeyAccountManager'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'SuperReader'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'DemandFinance'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'CustomerMarketing'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'SupportAdministrator'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'Administrator'), 1)

SET @name='msnbh-kondrale\alexey kondratiev'
INSERT INTO [dbo].[UserRole] ([Id], [UserId], [RoleId], [IsDefault])
VALUES 
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'CMManager'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'FunctionalExpert'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'DemandPlanning'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'KeyAccountManager'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'SuperReader'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'DemandFinance'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'CustomerMarketing'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'SupportAdministrator'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'Administrator'), 1)

SET @name='msnbh-vologand\andrey vologdin'
INSERT INTO [dbo].[UserRole] ([Id], [UserId], [RoleId], [IsDefault])
VALUES 
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'CMManager'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'FunctionalExpert'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'DemandPlanning'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'KeyAccountManager'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'SuperReader'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'DemandFinance'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'CustomerMarketing'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'SupportAdministrator'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'Administrator'), 1)

SET @name='msnbh-belyamar\maria belyakova'
INSERT INTO [dbo].[UserRole] ([Id], [UserId], [RoleId], [IsDefault])
VALUES 
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'CMManager'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'FunctionalExpert'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'DemandPlanning'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'KeyAccountManager'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'SuperReader'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'DemandFinance'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'CustomerMarketing'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'SupportAdministrator'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'Administrator'), 1)

SET @name='msnbh-spivaale\alexander spivak'
INSERT INTO [dbo].[UserRole] ([Id], [UserId], [RoleId], [IsDefault])
VALUES 
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'CMManager'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'FunctionalExpert'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'DemandPlanning'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'KeyAccountManager'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'SuperReader'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'DemandFinance'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'CustomerMarketing'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'SupportAdministrator'), 0),
(NEWID(), (SELECT Id FROM [User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM [Role] WHERE SystemName = 'Administrator'), 1)
GO