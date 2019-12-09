-- точки доступа
-- избавляемся от дуближа
DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource]='NonPromoSupports' AND [Action] IN ('GetNonPromoSupports', 'GetNonPromoSupport', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete', 'GetUserTimestamp', 'DownloadFile', 'UploadFile'));
Delete FROM AccessPoint WHERE [Resource]='NonPromoSupports' AND [Action] IN ('GetNonPromoSupports', 'GetNonPromoSupport', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete', 'GetUserTimestamp', 'DownloadFile', 'UploadFile') AND [Disabled] = 'false';

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoSupports', 'GetNonPromoSupports', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoSupports', 'GetNonPromoSupport', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoSupports', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoSupports', 'ExportXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoSupports', 'FullImportXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoSupports', 'DownloadTemplateXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoSupports', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoSupports', 'Put', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoSupports', 'Post', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoSupports', 'Delete', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoSupports', 'GetUserTimestamp', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('HistoricalNonPromoSupports', 'GetHistoricalNonPromoSupports', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('DeletedNonPromoSupports', 'GetDeletedNonPromoSupports', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoSupports', 'DownloadFile', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoSupports', 'UploadFile', 0, NULL);

--Роли
--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='NonPromoSupports' AND [Action] IN ('GetNonPromoSupports', 'GetNonPromoSupport', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete', 'GetUserTimestamp', 'DownloadFile', 'UploadFile');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalNonPromoSupports' AND [Action] IN ('GetHistoricalNonPromoSupports');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedNonPromoSupports' AND [Action] IN ('GetDeletedNonPromoSupports');


--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='NonPromoSupports' AND [Action] IN ('GetNonPromoSupports', 'GetNonPromoSupport', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete', 'GetUserTimestamp', 'DownloadFile', 'UploadFile');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalNonPromoSupports' AND [Action] IN ('GetHistoricalNonPromoSupports');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedNonPromoSupports' AND [Action] IN ('GetDeletedNonPromoSupports');


-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='NonPromoSupports' AND [Action] IN ('GetNonPromoSupports', 'GetNonPromoSupport', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete', 'GetUserTimestamp', 'DownloadFile', 'UploadFile');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalNonPromoSupports' AND [Action] IN ('GetHistoricalNonPromoSupports');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedNonPromoSupports' AND [Action] IN ('GetDeletedNonPromoSupports');

-- Customer Marketing Manager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='NonPromoSupports' AND [Action] IN ('GetNonPromoSupports', 'GetNonPromoSupport', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete', 'GetUserTimestamp', 'DownloadFile', 'UploadFile');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalNonPromoSupports' AND [Action] IN ('GetHistoricalNonPromoSupports');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedNonPromoSupports' AND [Action] IN ('GetDeletedNonPromoSupports');


-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='NonPromoSupports' AND [Action] IN ('GetNonPromoSupports', 'GetNonPromoSupport', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete', 'GetUserTimestamp', 'DownloadFile', 'UploadFile');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalNonPromoSupports' AND [Action] IN ('GetHistoricalNonPromoSupports');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedNonPromoSupports' AND [Action] IN ('GetDeletedNonPromoSupports');


-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='NonPromoSupports' AND [Action] IN ('GetNonPromoSupports', 'GetNonPromoSupport', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'GetUserTimestamp', 'DownloadFile');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalNonPromoSupports' AND [Action] IN ('GetHistoricalNonPromoSupports');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedNonPromoSupports' AND [Action] IN ('GetDeletedNonPromoSupports');

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='NonPromoSupports' AND [Action] IN ('GetNonPromoSupports', 'GetNonPromoSupport', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'GetUserTimestamp', 'DownloadFile');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalNonPromoSupports' AND [Action] IN ('GetHistoricalNonPromoSupports');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedNonPromoSupports' AND [Action] IN ('GetDeletedNonPromoSupports');


-- SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='NonPromoSupports' AND [Action] IN ('GetNonPromoSupports', 'GetNonPromoSupport', 'ExportXLSX', 'DownloadTemplateXLSX', 'GetUserTimestamp', 'DownloadFile');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalNonPromoSupports' AND [Action] IN ('GetHistoricalNonPromoSupports');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedNonPromoSupports' AND [Action] IN ('GetDeletedNonPromoSupports');
