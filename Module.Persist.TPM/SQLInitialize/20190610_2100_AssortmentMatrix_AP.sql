-- точки доступа
-- чтобы не было дублей
DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource] IN ('AssortmentMatrices', 'DeletedAssortmentMatrices', 'HistoricalAssortmentMatrices'))
DELETE FROM AccessPoint WHERE [Resource] IN ('AssortmentMatrices', 'DeletedAssortmentMatrices', 'HistoricalAssortmentMatrices')

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('AssortmentMatrices', 'GetAssortmentMatrices', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('AssortmentMatrices', 'GetAssortmentMatrix', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('DeletedAssortmentMatrices', 'GetDeletedAssortmentMatrix', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('DeletedAssortmentMatrices', 'GetDeletedAssortmentMatrices', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('HistoricalAssortmentMatrices', 'GetHistoricalAssortmentMatrices', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('AssortmentMatrices', 'Put', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('AssortmentMatrices', 'Post', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('AssortmentMatrices', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('AssortmentMatrices', 'Delete', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('AssortmentMatrices', 'ExportXLSX', 0, NULL);

--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('AssortmentMatrices', 'DeletedAssortmentMatrices', 'HistoricalAssortmentMatrices')

--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('AssortmentMatrices', 'DeletedAssortmentMatrices', 'HistoricalAssortmentMatrices')
-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('AssortmentMatrices', 'DeletedAssortmentMatrices', 'HistoricalAssortmentMatrices')

-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('AssortmentMatrices', 'DeletedAssortmentMatrices', 'HistoricalAssortmentMatrices')

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('AssortmentMatrices', 'DeletedAssortmentMatrices', 'HistoricalAssortmentMatrices')

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('AssortmentMatrices', 'DeletedAssortmentMatrices', 'HistoricalAssortmentMatrices')

-- SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('AssortmentMatrices', 'DeletedAssortmentMatrices', 'HistoricalAssortmentMatrices')


