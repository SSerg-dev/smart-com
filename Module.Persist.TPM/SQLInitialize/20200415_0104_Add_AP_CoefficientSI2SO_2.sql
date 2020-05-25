-- точки доступа
-- избавляемся от дублей
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM ACCESSPOINT WHERE [Resource] IN ('CoefficientSI2SOs', 'HistoricalCoefficientSI2SOs', 'DeletedCoefficientSI2SOs') AND [Action] IN ('GetCoefficientSI2SOs', 'GetCoefficientSI2SO', 'ExportXLSX', 'Post', 'Put', 'Patch', 'Delete', 'FullImportXLSX', 'DownloadTemplateXLSX', 'GetHistoricalCoefficientSI2SOs', 'GetDeletedCoefficientSI2SOs'));
DELETE FROM AccessPoint WHERE [Resource] IN ('CoefficientSI2SOs', 'HistoricalCoefficientSI2SOs', 'DeletedCoefficientSI2SOs') AND [Action] IN ('GetCoefficientSI2SOs', 'GetCoefficientSI2SO', 'ExportXLSX', 'Post', 'Put', 'Patch', 'Delete', 'FullImportXLSX', 'DownloadTemplateXLSX', 'GetHistoricalCoefficientSI2SOs', 'GetDeletedCoefficientSI2SOs') AND [Disabled] = 'false';

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('CoefficientSI2SOs', 'GetCoefficientSI2SOs', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('CoefficientSI2SOs', 'GetCoefficientSI2SO', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('CoefficientSI2SOs', 'Post', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('CoefficientSI2SOs', 'Put', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('CoefficientSI2SOs', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('CoefficientSI2SOs', 'Delete', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('CoefficientSI2SOs', 'ExportXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('CoefficientSI2SOs', 'FullImportXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('CoefficientSI2SOs', 'DownloadTemplateXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('HistoricalCoefficientSI2SOs', 'GetHistoricalCoefficientSI2SOs', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('DeletedCoefficientSI2SOs', 'GetDeletedCoefficientSI2SOs', 0, NULL);


-- роли
--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='CoefficientSI2SOs' AND [Action] IN ('GetCoefficientSI2SOs', 'GetCoefficientSI2SO', 'ExportXLSX', 'Post', 'Put', 'Patch', 'Delete', 'FullImportXLSX', 'DownloadTemplateXLSX');
--Deleted
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedCoefficientSI2SOs' AND [Action] IN ('GetDeletedCoefficientSI2SOs');
--Historical
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalCoefficientSI2SOs' AND [Action] IN ('GetHistoricalCoefficientSI2SOs');

--Support Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='CoefficientSI2SOs' AND [Action] IN ('GetCoefficientSI2SOs', 'GetCoefficientSI2SO', 'ExportXLSX', 'Post', 'Put', 'Patch', 'Delete', 'FullImportXLSX', 'DownloadTemplateXLSX');
--Deleted
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedCoefficientSI2SOs' AND [Action] IN ('GetDeletedCoefficientSI2SOs');
--Historical
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalCoefficientSI2SOs' AND [Action] IN ('GetHistoricalCoefficientSI2SOs');

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='CoefficientSI2SOs' AND [Action] IN ('GetCoefficientSI2SOs', 'GetCoefficientSI2SO', 'ExportXLSX', 'Post', 'Put', 'Patch', 'Delete', 'FullImportXLSX', 'DownloadTemplateXLSX');
--Deleted
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedCoefficientSI2SOs' AND [Action] IN ('GetDeletedCoefficientSI2SOs');
--Historical
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalCoefficientSI2SOs' AND [Action] IN ('GetHistoricalCoefficientSI2SOs');

-- SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='CoefficientSI2SOs' AND [Action] IN ('GetCoefficientSI2SOs', 'GetCoefficientSI2SO', 'ExportXLSX'); 
--Deleted
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedCoefficientSI2SOs' AND [Action] IN ('GetDeletedCoefficientSI2SOs');
--Historical
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalCoefficientSI2SOs' AND [Action] IN ('GetHistoricalCoefficientSI2SOs');
