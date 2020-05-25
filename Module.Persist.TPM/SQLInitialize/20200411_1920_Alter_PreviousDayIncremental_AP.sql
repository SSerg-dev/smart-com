 
-- точки доступа
-- избавляемся от дублей
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM ACCESSPOINT WHERE [Resource] IN ('PreviousDayIncrementals') AND [Action] IN ('GetPreviousDayIncrementals','ExportXLSX'));
DELETE FROM AccessPoint WHERE [Resource] IN ('PreviousDayIncrementals') AND [Action] IN ('GetPreviousDayIncrementals','ExportXLSX') AND [Disabled] = 'false';

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PreviousDayIncrementals', 'GetPreviousDayIncrementals', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PreviousDayIncrementals', 'ExportXLSX', 0, NULL);

--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'Administrator' and [Disabled] = 'false'), [Id] FROM [AccessPoint] WHERE [Resource] = ('PreviousDayIncrementals')

--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'FunctionalExpert' and [Disabled] = 'false'), [Id] FROM [AccessPoint] WHERE [Resource] = ('PreviousDayIncrementals')

-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), 
(SELECT [Id] FROM [Role] Where [SystemName] = 'CustomerMarketing' and [Disabled] = 'false'), 
[Id] FROM [AccessPoint] WHERE [Resource] = ('PreviousDayIncrementals')  

-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'KeyAccountManager' and [Disabled] = 'false'), 
[Id] FROM [AccessPoint] WHERE [Resource] = ('PreviousDayIncrementals') 

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'DemandFinance' and [Disabled] = 'false'), 
[Id] FROM [AccessPoint] WHERE [Resource] = ('PreviousDayIncrementals')

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'DemandPlanning' and [Disabled] = 'false'), 
[Id] FROM [AccessPoint] WHERE [Resource] = ('PreviousDayIncrementals')

-- SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'SuperReader' and [Disabled] = 'false'), 
[Id] FROM [AccessPoint] WHERE [Resource] = ('PreviousDayIncrementals') 

-- CMManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'CMManager' and [Disabled] = 'false'), 
[Id] FROM [AccessPoint] WHERE [Resource] IN ('PreviousDayIncrementals') 
-- SupportAdministrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'SupportAdministrator' and [Disabled] = 'false'), 
[Id] FROM [AccessPoint] WHERE [Resource] IN ('PreviousDayIncrementals') 
GO