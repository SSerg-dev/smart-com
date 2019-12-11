USE TPM_NonPromoCost_Main_Dev_Current
-- точки доступа
-- избавляемся от дуближа
DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource] IN ('NonPromoEquipments', 'HistoricalNonPromoEquipments', 'DeletedNonPromoEquipments') AND [Action] IN ('GetNonPromoEquipments', 'GetNonPromoEquipment', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete', 'GetHistoricalNonPromoEquipments', 'GetDeletedNonPromoEquipments'));
Delete FROM AccessPoint WHERE [Resource] IN ('NonPromoEquipments', 'HistoricalNonPromoEquipments', 'DeletedNonPromoEquipments') AND [Action] IN ('GetNonPromoEquipments', 'GetNonPromoEquipment', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete', 'GetHistoricalNonPromoEquipments', 'GetDeletedNonPromoEquipments') AND [Disabled] = 'false';

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoEquipments', 'GetNonPromoEquipments', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoEquipments', 'GetNonPromoEquipment', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoEquipments', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoEquipments', 'ExportXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoEquipments', 'FullImportXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoEquipments', 'DownloadTemplateXLSX', 0, NULL);INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoEquipments', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoEquipments', 'Put', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoEquipments', 'Post', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoEquipments', 'Delete', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('HistoricalNonPromoEquipments', 'GetHistoricalNonPromoEquipments', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('DeletedNonPromoEquipments', 'GetDeletedNonPromoEquipments', 0, NULL);



-- роли
--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='NonPromoEquipments' AND [Action] IN ('GetNonPromoEquipments', 'GetNonPromoEquipment', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalNonPromoEquipments' AND [Action] IN ('GetHistoricalNonPromoEquipments');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedNonPromoEquipments' AND [Action] IN ('GetDeletedNonPromoEquipments');


--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='NonPromoEquipments' AND [Action] IN ('GetNonPromoEquipments', 'GetNonPromoEquipment', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalNonPromoEquipments' AND [Action] IN ('GetHistoricalNonPromoEquipments');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedNonPromoEquipments' AND [Action] IN ('GetDeletedNonPromoEquipments');

-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='NonPromoEquipments' AND [Action] IN ('GetNonPromoEquipments', 'GetNonPromoEquipment', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalNonPromoEquipments' AND [Action] IN ('GetHistoricalNonPromoEquipments');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedNonPromoEquipments' AND [Action] IN ('GetDeletedNonPromoEquipments');

-- CMManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='NonPromoEquipments' AND [Action] IN ('GetNonPromoEquipments', 'GetNonPromoEquipment', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalNonPromoEquipments' AND [Action] IN ('GetHistoricalNonPromoEquipments');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedNonPromoEquipments' AND [Action] IN ('GetDeletedNonPromoEquipments');


-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='NonPromoEquipments' AND [Action] IN ('GetNonPromoEquipments', 'GetNonPromoEquipment', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalNonPromoEquipments' AND [Action] IN ('GetHistoricalNonPromoEquipments');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedNonPromoEquipments' AND [Action] IN ('GetDeletedNonPromoEquipments');


-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='NonPromoEquipments' AND [Action] IN ('GetNonPromoEquipments', 'GetNonPromoEquipment', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalNonPromoEquipments' AND [Action] IN ('GetHistoricalNonPromoEquipments');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedNonPromoEquipments' AND [Action] IN ('GetDeletedNonPromoEquipments');


-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='NonPromoEquipments' AND [Action] IN ('GetNonPromoEquipments', 'GetNonPromoEquipment', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalNonPromoEquipments' AND [Action] IN ('GetHistoricalNonPromoEquipments');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedNonPromoEquipments' AND [Action] IN ('GetDeletedNonPromoEquipments');


-- SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='NonPromoEquipments' AND [Action] IN ('GetNonPromoEquipments', 'GetNonPromoEquipment', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalNonPromoEquipments' AND [Action] IN ('GetHistoricalNonPromoEquipments');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedNonPromoEquipments' AND [Action] IN ('GetDeletedNonPromoEquipments');
