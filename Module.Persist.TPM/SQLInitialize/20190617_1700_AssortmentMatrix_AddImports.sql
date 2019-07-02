-- точки доступа
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('AssortmentMatrices', 'DownloadTemplateXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('AssortmentMatrices', 'FullImportXLSX', 0, NULL);

-- роли
--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('AssortmentMatrices', 'DeletedAssortmentMatrices', 'HistoricalAssortmentMatrices') AND [Action] IN ('DownloadTemplateXLSX', 'FullImportXLSX');


--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('AssortmentMatrices', 'DeletedAssortmentMatrices', 'HistoricalAssortmentMatrices') AND [Action] IN ( 'DownloadTemplateXLSX', 'FullImportXLSX');


-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('AssortmentMatrices', 'DeletedAssortmentMatrices', 'HistoricalAssortmentMatrices') AND [Action] IN ( 'DownloadTemplateXLSX', 'FullImportXLSX');


-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('AssortmentMatrices', 'DeletedAssortmentMatrices', 'HistoricalAssortmentMatrices') AND [Action] IN ( 'DownloadTemplateXLSX', 'FullImportXLSX');


-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('AssortmentMatrices', 'DeletedAssortmentMatrices', 'HistoricalAssortmentMatrices') AND [Action] IN ( 'DownloadTemplateXLSX', 'FullImportXLSX');


-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('AssortmentMatrices', 'DeletedAssortmentMatrices', 'HistoricalAssortmentMatrices') AND [Action] IN ( 'DownloadTemplateXLSX', 'FullImportXLSX');


-- SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('AssortmentMatrices', 'DeletedAssortmentMatrices', 'HistoricalAssortmentMatrices') AND [Action] IN ( 'DownloadTemplateXLSX', 'FullImportXLSX');
