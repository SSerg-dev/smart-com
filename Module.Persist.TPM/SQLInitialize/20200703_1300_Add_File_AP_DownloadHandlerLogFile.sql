DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM ACCESSPOINT WHERE [Resource] IN ('File') AND [Action] IN ('DownloadHandlerLogFile'));
DELETE FROM AccessPoint WHERE [Resource] IN ('File') AND [Action] IN ('DownloadHandlerLogFile') AND [Disabled] = 'false';

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('File', 'DownloadHandlerLogFile', 0, NULL);

--CMManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), 
(SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), 
Id FROM ACCESSPOINT WHERE [Resource] IN ('File') and [Action] IN('DownloadHandlerLogFile')

--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), 
(SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), 
Id FROM ACCESSPOINT WHERE [Resource] IN ('File') and [Action] IN('DownloadHandlerLogFile')

--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), 
(SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), 
Id FROM ACCESSPOINT WHERE [Resource] IN ('File') and [Action] IN('DownloadHandlerLogFile')

--DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), 
(SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), 
Id FROM ACCESSPOINT WHERE [Resource] IN ('File') and [Action] IN('DownloadHandlerLogFile')

--KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), 
(SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), 
Id FROM ACCESSPOINT WHERE [Resource] IN ('File') and [Action] IN('DownloadHandlerLogFile')

--SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), 
(SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), 
Id FROM ACCESSPOINT WHERE [Resource] IN ('File') and [Action] IN('DownloadHandlerLogFile')

--DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), 
(SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), 
Id FROM ACCESSPOINT WHERE [Resource] IN ('File') and [Action] IN('DownloadHandlerLogFile')

--CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), 
(SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), 
Id FROM ACCESSPOINT WHERE [Resource] IN ('File') and [Action] IN('DownloadHandlerLogFile')

--SupportAdministrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), 
(SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false'), 
Id FROM ACCESSPOINT WHERE [Resource] IN ('File') and [Action] IN('DownloadHandlerLogFile')