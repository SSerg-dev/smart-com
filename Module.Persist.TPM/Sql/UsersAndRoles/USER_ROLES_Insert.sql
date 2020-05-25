DECLARE @name NVARCHAR(100);
SET @name='smartcom\anatoliy.soldatov'
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

SET @name='smartcom\vadim.kosarev'
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

SET @name='smartcom\ilia.fedorov'
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

SET @name='smartcom\natalia.aleksandrova'
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

SET @name='smartcom\dmitry.puzikov'
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

SET @name='smartcom\artem.polyanin'
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

SET @name='smartcom\alexey.morozkin'
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

SET @name='SMARTCOM\alexandr.butyrskiy'
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

SET @name='smartcom\ekaterina.kalugina'
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

SET @name='smartcom\andrey.filyushkin'
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

SET @name='smartcom\evgeny.suglobov'
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

SET @name='smartcom\alexey.kondratiev'
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

SET @name='smartcom\andrey.samborsky'
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

SET @name='smartcom\artem.morozov'
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

SET @name='smartcom\marina.kryuchkova'
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

SET @name='smartcom\mikhail.volovich'
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

SET @name='smartcom\alexander.pereponov'
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

SET @name='smartcom\alexander.zinkevich'
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

SET @name='smartcom\maria.belyakova'
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

SET @name='smartcom\alexander.spivak'
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

SET @name='smartcom\denis.moskvitin'
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

SET @name='smartcom\alexander.streltsov'
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

SET @name='smartcom\andrey.vologdin'
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

SET @name='smartcom\evgeny.bondarenko'
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