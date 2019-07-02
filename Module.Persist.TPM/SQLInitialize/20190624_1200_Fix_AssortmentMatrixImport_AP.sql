-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('AssortmentMatrices', 'DeletedAssortmentMatrices', 'HistoricalAssortmentMatrices') AND [Action] IN ( 'DownloadTemplateXLSX', 'FullImportXLSX');