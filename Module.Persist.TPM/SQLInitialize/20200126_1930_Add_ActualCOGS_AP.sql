-- точки доступа
-- избавляемся от дублей
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM ACCESSPOINT WHERE [Resource] IN ('ActualCOGSs', 'HistoricalActualCOGSs', 'DeletedActualCOGSs') AND [Action] IN ('GetActualCOGSs', 'GetActualCOGS', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete', 'GetHistoricalActualCOGSs', 'GetDeletedActualCOGSs'));
DELETE FROM AccessPoint WHERE [Resource] IN ('ActualCOGSs', 'HistoricalActualCOGSs', 'DeletedActualCOGSs') AND [Action] IN ('GetActualCOGSs', 'GetActualCOGS', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete', 'GetHistoricalActualCOGSs', 'GetDeletedActualCOGSs') AND [Disabled] = 'false';

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualCOGSs', 'GetActualCOGSs', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualCOGSs', 'GetActualCOGS', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualCOGSs', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualCOGSs', 'ExportXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualCOGSs', 'FullImportXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualCOGSs', 'DownloadTemplateXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualCOGSs', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualCOGSs', 'Put', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualCOGSs', 'Post', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualCOGSs', 'Delete', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('HistoricalActualCOGSs', 'GetHistoricalActualCOGSs', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('DeletedActualCOGSs', 'GetDeletedActualCOGSs', 0, NULL);

-- роли
--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualCOGSs' AND [Action] IN ('GetActualCOGSs', 'GetActualCOGS', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalActualCOGSs' AND [Action] IN ('GetHistoricalActualCOGSs');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedActualCOGSs' AND [Action] IN ('GetDeletedActualCOGSs');

--Support Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualCOGSs' AND [Action] IN ('GetActualCOGSs', 'GetActualCOGS', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalActualCOGSs' AND [Action] IN ('GetHistoricalActualCOGSs');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedActualCOGSs' AND [Action] IN ('GetDeletedActualCOGSs');


--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualCOGSs' AND [Action] IN ('GetActualCOGSs', 'GetActualCOGS', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalActualCOGSs' AND [Action] IN ('GetHistoricalActualCOGSs');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedActualCOGSs' AND [Action] IN ('GetDeletedActualCOGSs');


-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualCOGSs' AND [Action] IN ('GetActualCOGSs', 'GetActualCOGS', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalActualCOGSs' AND [Action] IN ('GetHistoricalActualCOGSs');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedActualCOGSs' AND [Action] IN ('GetDeletedActualCOGSs');


-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualCOGSs' AND [Action] IN ('GetActualCOGSs', 'GetActualCOGS', 'ExportXLSX');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalActualCOGSs' AND [Action] IN ('GetHistoricalActualCOGSs');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedActualCOGSs' AND [Action] IN ('GetDeletedActualCOGSs');

-- CMManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualCOGSs' AND [Action] IN ('GetActualCOGSs', 'GetActualCOGS', 'ExportXLSX');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalActualCOGSs' AND [Action] IN ('GetHistoricalActualCOGSs');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedActualCOGSs' AND [Action] IN ('GetDeletedActualCOGSs');


-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualCOGSs' AND [Action] IN ('GetActualCOGSs', 'GetActualCOGS', 'ExportXLSX'); 
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalActualCOGSs' AND [Action] IN ('GetHistoricalActualCOGSs');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedActualCOGSs' AND [Action] IN ('GetDeletedActualCOGSs');


-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualCOGSs' AND [Action] IN ('GetActualCOGSs', 'GetActualCOGS', 'ExportXLSX');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalActualCOGSs' AND [Action] IN ('GetHistoricalActualCOGSs');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedActualCOGSs' AND [Action] IN ('GetDeletedActualCOGSs');


-- SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualCOGSs' AND [Action] IN ('GetActualCOGSs', 'GetActualCOGS', 'ExportXLSX');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalActualCOGSs' AND [Action] IN ('GetHistoricalActualCOGSs');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedActualCOGSs' AND [Action] IN ('GetDeletedActualCOGSs');
