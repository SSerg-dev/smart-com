DECLARE @name NVARCHAR(100);
SET @name='andrey.filyushkin@smartcom.software'
INSERT INTO [UserRole] ([Id], [UserId], [RoleId], [IsDefault])
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

SET @name='alexey.kondratiev@smartcom.software'
INSERT INTO [UserRole] ([Id], [UserId], [RoleId], [IsDefault])
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

SET @name='andrey.vologdin@smartcom.software'
INSERT INTO [UserRole] ([Id], [UserId], [RoleId], [IsDefault])
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

SET @name='alexander.spivak@smartcom.software'
INSERT INTO [UserRole] ([Id], [UserId], [RoleId], [IsDefault])
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

SET @name='artem.morozov@smartcom.software'
INSERT INTO [UserRole] ([Id], [UserId], [RoleId], [IsDefault])
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

SET @name='natalia.aleksandrova@smartcom.software'
INSERT INTO [UserRole] ([Id], [UserId], [RoleId], [IsDefault])
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

SET @name='evgeny.suglobov@smartcom.software'
INSERT INTO [UserRole] ([Id], [UserId], [RoleId], [IsDefault])
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

SET @name='evgeny.bondarenko@smartcom.software'
INSERT INTO [UserRole] ([Id], [UserId], [RoleId], [IsDefault])
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

SET @name='oleg.zazaev@smartcom.software'
INSERT INTO [UserRole] ([Id], [UserId], [RoleId], [IsDefault])
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

SET @name='ekaterina.kalugina@smartcom.software'
INSERT INTO [UserRole] ([Id], [UserId], [RoleId], [IsDefault])
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

SET @name='ekaterina.burlak@smartcom.software'
INSERT INTO [UserRole] ([Id], [UserId], [RoleId], [IsDefault])
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

SET @name='mikhail.volovich@smartcom.software'
INSERT INTO [UserRole] ([Id], [UserId], [RoleId], [IsDefault])
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

SET @name='vadim.kosarev@smartcom.software'
INSERT INTO [UserRole] ([Id], [UserId], [RoleId], [IsDefault])
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

SET @name='ilia.fedorov@smartcom.software'
INSERT INTO [UserRole] ([Id], [UserId], [RoleId], [IsDefault])
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

SET @name='denis.moskvitin@smartcom.software'
INSERT INTO [UserRole] ([Id], [UserId], [RoleId], [IsDefault])
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

SET @name='anatoliy.soldatov@smartcom.software'
INSERT INTO [UserRole] ([Id], [UserId], [RoleId], [IsDefault])
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


SET @name='dmitry.puzikov@smartcom.software'
INSERT INTO [UserRole] ([Id], [UserId], [RoleId], [IsDefault])
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

SET @name='oleg.borisov@smartcom.software'
INSERT INTO [UserRole] ([Id], [UserId], [RoleId], [IsDefault])
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

SET @name='ivan.obraztsov@smartcom.software'
INSERT INTO [UserRole] ([Id], [UserId], [RoleId], [IsDefault])
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

SET @name='ilya.chernoskutov@smartcom.software'
INSERT INTO [UserRole] ([Id], [UserId], [RoleId], [IsDefault])
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