-- точки доступа
-- чтобы не было дублей
DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource] IN ('IncrementalPromoes', 'DeletedIncrementalPromoes', 'HistoricalIncrementalPromoes'))
DELETE FROM AccessPoint WHERE [Resource] IN ('IncrementalPromoes', 'DeletedIncrementalPromoes', 'HistoricalIncrementalPromoes')

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('IncrementalPromoes', 'GetIncrementalPromo', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('IncrementalPromoes', 'GetIncrementalPromoes', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('DeletedIncrementalPromoes', 'GetDeletedIncrementalPromo', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('DeletedIncrementalPromoes', 'GetDeletedIncrementalPromoes', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('HistoricalIncrementalPromoes', 'GetHistoricalIncrementalPromoes', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('IncrementalPromoes', 'Put', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('IncrementalPromoes', 'Post', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('IncrementalPromoes', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('IncrementalPromoes', 'Delete', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('IncrementalPromoes', 'ExportXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('IncrementalPromoes', 'FullImportXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('IncrementalPromoes', 'DownloadTemplateXLSX', 0, NULL);

--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('IncrementalPromoes', 'DeletedIncrementalPromoes', 'HistoricalIncrementalPromoes')

--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('IncrementalPromoes', 'DeletedIncrementalPromoes', 'HistoricalIncrementalPromoes')
-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('IncrementalPromoes', 'DeletedIncrementalPromoes', 'HistoricalIncrementalPromoes')

-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('IncrementalPromoes', 'DeletedIncrementalPromoes', 'HistoricalIncrementalPromoes')

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('IncrementalPromoes', 'DeletedIncrementalPromoes', 'HistoricalIncrementalPromoes')

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('IncrementalPromoes', 'DeletedIncrementalPromoes', 'HistoricalIncrementalPromoes')

-- SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('IncrementalPromoes', 'DeletedIncrementalPromoes', 'HistoricalIncrementalPromoes')


