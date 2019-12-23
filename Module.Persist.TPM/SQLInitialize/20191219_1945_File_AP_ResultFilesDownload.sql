-- точки доступа
-- избавляемся от дуближа
DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource]='File' AND [Action] IN ('DataLakeSyncResultSuccessDownload', 'DataLakeSyncResultWarningDownload', 'DataLakeSyncResultErrorDownload'));
Delete FROM AccessPoint WHERE [Resource]='File' AND [Action] IN ('DataLakeSyncResultSuccessDownload', 'DataLakeSyncResultWarningDownload', 'DataLakeSyncResultErrorDownload') AND [Disabled] = 'false';

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('File', 'DataLakeSyncResultSuccessDownload',  0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('File', 'DataLakeSyncResultWarningDownload', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('File', 'DataLakeSyncResultErrorDownload', 0, NULL);

--Роли
--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='File' AND [Action] IN ('DataLakeSyncResultSuccessDownload', 'DataLakeSyncResultWarningDownload', 'DataLakeSyncResultErrorDownload');


--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='File' AND [Action] IN ('DataLakeSyncResultSuccessDownload', 'DataLakeSyncResultWarningDownload', 'DataLakeSyncResultErrorDownload');


-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='File' AND [Action] IN ('DataLakeSyncResultSuccessDownload', 'DataLakeSyncResultWarningDownload', 'DataLakeSyncResultErrorDownload');

-- Customer Marketing Manager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='File' AND [Action] IN ('DataLakeSyncResultSuccessDownload', 'DataLakeSyncResultWarningDownload', 'DataLakeSyncResultErrorDownload');


-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='File' AND [Action] IN ('DataLakeSyncResultSuccessDownload', 'DataLakeSyncResultWarningDownload', 'DataLakeSyncResultErrorDownload');


-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='File' AND [Action] IN ('DataLakeSyncResultSuccessDownload', 'DataLakeSyncResultWarningDownload', 'DataLakeSyncResultErrorDownload');


-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='File' AND [Action] IN ('DataLakeSyncResultSuccessDownload', 'DataLakeSyncResultWarningDownload', 'DataLakeSyncResultErrorDownload');


-- SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='File' AND [Action] IN ('DataLakeSyncResultSuccessDownload', 'DataLakeSyncResultWarningDownload', 'DataLakeSyncResultErrorDownload');
