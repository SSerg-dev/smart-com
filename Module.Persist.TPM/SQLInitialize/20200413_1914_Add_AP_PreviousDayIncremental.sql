-- точки доступа
-- избавляемся от дублей
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM ACCESSPOINT WHERE [Resource] IN ('PreviousDayIncrementals') AND [Action] IN ('PreviousDayIncrementals', 'GetPreviousDayIncrementals', 'ExportXLSX'));
DELETE FROM AccessPoint WHERE [Resource] IN ('PreviousDayIncrementals') AND [Action] IN ('GetPreviousDayIncrementals', 'ExportXLSX') AND [Disabled] = 'false';

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PreviousDayIncrementals', 'GetPreviousDayIncrementals', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PreviousDayIncrementals', 'ExportXLSX', 0, NULL);

-- роли
--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'Administrator' and [Disabled] = 'false'), [Id] FROM [AccessPoint] WHERE [Resource] = ('PreviousDayIncrementals') AND [Action] IN ('GetPreviousDayIncrementals', 'ExportXLSX');

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'DemandPlanning' and [Disabled] = 'false'), [Id] FROM [AccessPoint] WHERE [Resource] = ('PreviousDayIncrementals') AND [Action] IN ('GetPreviousDayIncrementals', 'ExportXLSX');

-- SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'SuperReader' and [Disabled] = 'false'), 
[Id] FROM [AccessPoint] WHERE [Resource] = ('PreviousDayIncrementals') AND [Action] IN ('GetPreviousDayIncrementals');

-- SupportAdministrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'SupportAdministrator' and [Disabled] = 'false'), 
[Id] FROM [AccessPoint] WHERE [Resource] IN ('PreviousDayIncrementals') AND [Action] IN ('GetPreviousDayIncrementals', 'ExportXLSX');