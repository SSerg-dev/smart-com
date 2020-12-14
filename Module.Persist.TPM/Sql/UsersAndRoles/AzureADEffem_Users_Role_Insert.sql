DECLARE @name NVARCHAR(100);
SET @name='kondratiev.aleksey@effem.com'
INSERT INTO Jupiter.[UserRole] ([Id], [UserId], [RoleId], [IsDefault])
VALUES 
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'CMManager'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'FunctionalExpert'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'DemandPlanning'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'KeyAccountManager'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'SuperReader'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'DemandFinance'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'CustomerMarketing'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'SupportAdministrator'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'Administrator'), 1)

SET @name='bondarenko.eugeny@effem.com'
INSERT INTO Jupiter.[UserRole] ([Id], [UserId], [RoleId], [IsDefault])
VALUES 
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'CMManager'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'FunctionalExpert'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'DemandPlanning'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'KeyAccountManager'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'SuperReader'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'DemandFinance'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'CustomerMarketing'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'SupportAdministrator'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'Administrator'), 1)

SET @name='eugeny.suglobov@effem.com'
INSERT INTO Jupiter.[UserRole] ([Id], [UserId], [RoleId], [IsDefault])
VALUES 
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'CMManager'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'FunctionalExpert'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'DemandPlanning'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'KeyAccountManager'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'SuperReader'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'DemandFinance'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'CustomerMarketing'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'SupportAdministrator'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'Administrator'), 1)

SET @name='artem.x.morozov@effem.com'
INSERT INTO Jupiter.[UserRole] ([Id], [UserId], [RoleId], [IsDefault])
VALUES 
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'CMManager'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'FunctionalExpert'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'DemandPlanning'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'KeyAccountManager'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'SuperReader'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'DemandFinance'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'CustomerMarketing'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'SupportAdministrator'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'Administrator'), 1)

SET @name='andrey.philushkin@effem.com'
INSERT INTO Jupiter.[UserRole] ([Id], [UserId], [RoleId], [IsDefault])
VALUES 
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'CMManager'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'FunctionalExpert'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'DemandPlanning'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'KeyAccountManager'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'SuperReader'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'DemandFinance'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'CustomerMarketing'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'SupportAdministrator'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'Administrator'), 1)

SET @name='mikhail.volovich@effem.com'
INSERT INTO Jupiter.[UserRole] ([Id], [UserId], [RoleId], [IsDefault])
VALUES 
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'CMManager'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'FunctionalExpert'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'DemandPlanning'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'KeyAccountManager'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'SuperReader'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'DemandFinance'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'CustomerMarketing'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'SupportAdministrator'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'Administrator'), 1)


SET @name='marina.kruchkova@effem.com'
INSERT INTO Jupiter.[UserRole] ([Id], [UserId], [RoleId], [IsDefault])
VALUES 
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'CMManager'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'FunctionalExpert'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'DemandPlanning'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'KeyAccountManager'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'SuperReader'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'DemandFinance'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'CustomerMarketing'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'SupportAdministrator'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'Administrator'), 1)


SET @name='alexander.spivak@effem.com'
INSERT INTO Jupiter.[UserRole] ([Id], [UserId], [RoleId], [IsDefault])
VALUES 
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'CMManager'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'FunctionalExpert'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'DemandPlanning'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'KeyAccountManager'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'SuperReader'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'DemandFinance'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'CustomerMarketing'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'SupportAdministrator'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'Administrator'), 1)


SET @name='ekaterina.kalugina@effem.com'
INSERT INTO Jupiter.[UserRole] ([Id], [UserId], [RoleId], [IsDefault])
VALUES 
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'CMManager'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'FunctionalExpert'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'DemandPlanning'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'KeyAccountManager'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'SuperReader'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'DemandFinance'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'CustomerMarketing'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'SupportAdministrator'), 0),
(NEWID(), (SELECT Id FROM Jupiter.[User] WHERE [Name] = @name AND Disabled = 0), (SELECT Id FROM Jupiter.[Role] WHERE SystemName = 'Administrator'), 1)
