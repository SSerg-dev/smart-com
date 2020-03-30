-- точки доступа
-- избавляемся от дублей
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM ACCESSPOINT WHERE [Resource] IN ('BTLs', 'HistoricalBTLs', 'DeletedBTLs') AND [Action] IN ('GetBTLs', 'GetBTL', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete', 'GetHistoricalBTLs', 'GetDeletedBTLs'));
DELETE FROM AccessPoint WHERE [Resource] IN ('BTLs', 'HistoricalBTLs', 'DeletedBTLs') AND [Action] IN ('GetBTLs', 'GetBTL', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete', 'GetHistoricalBTLs', 'GetDeletedBTLs') AND [Disabled] = 'false';

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('BTLs', 'GetBTLs', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('BTLs', 'GetBTL', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('BTLs', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('BTLs', 'ExportXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('BTLs', 'FullImportXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('BTLs', 'DownloadTemplateXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('BTLs', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('BTLs', 'Put', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('BTLs', 'Post', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('BTLs', 'Delete', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('HistoricalBTLs', 'GetHistoricalBTLs', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('DeletedBTLs', 'GetDeletedBTLs', 0, NULL);

-- роли
--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='BTLs' AND [Action] IN ('GetBTLs', 'GetBTL', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalBTLs' AND [Action] IN ('GetHistoricalBTLs');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedBTLs' AND [Action] IN ('GetDeletedBTLs');

--Support Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='BTLs' AND [Action] IN ('GetBTLs', 'GetBTL', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalBTLs' AND [Action] IN ('GetHistoricalBTLs');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedBTLs' AND [Action] IN ('GetDeletedBTLs');

--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='BTLs' AND [Action] IN ('GetBTLs', 'GetBTL', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalBTLs' AND [Action] IN ('GetHistoricalBTLs');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedBTLs' AND [Action] IN ('GetDeletedBTLs');

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='BTLs' AND [Action] IN ('GetBTLs', 'GetBTL', 'ExportXLSX');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalBTLs' AND [Action] IN ('GetHistoricalBTLs');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedBTLs' AND [Action] IN ('GetDeletedBTLs');


-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='BTLs' AND [Action] IN ('GetBTLs', 'GetBTL', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete'); 
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalBTLs' AND [Action] IN ('GetHistoricalBTLs');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedBTLs' AND [Action] IN ('GetDeletedBTLs');

-- CMManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='BTLs' AND [Action] IN ('GetBTLs', 'GetBTL', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete'); 
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalBTLs' AND [Action] IN ('GetHistoricalBTLs');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedBTLs' AND [Action] IN ('GetDeletedBTLs');


-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='BTLs' AND [Action] IN ('GetBTLs', 'GetBTL', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete'); 
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalBTLs' AND [Action] IN ('GetHistoricalBTLs');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedBTLs' AND [Action] IN ('GetDeletedBTLs');

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='BTLs' AND [Action] IN ('GetBTLs', 'GetBTL', 'ExportXLSX'); 
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalBTLs' AND [Action] IN ('GetHistoricalBTLs');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedBTLs' AND [Action] IN ('GetDeletedBTLs');

-- SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='BTLs' AND [Action] IN ('GetBTLs', 'GetBTL', 'ExportXLSX'); 
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalBTLs' AND [Action] IN ('GetHistoricalBTLs');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT 
WHERE [Resource]='DeletedBTLs' AND [Action] IN ('GetDeletedBTLs');

