-- избавляемся от дублей для данной роли
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM ACCESSPOINT WHERE [Resource] IN ('PriceLists') AND [Action] IN ('GetPriceLists', 'GetPriceList', 'ExportXLSX', 'Put', 'Post', 'Patch', 'Delete', 'FullImportXLSX', 'DownloadTemplateXLSX') AND RoleId = (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'));

-- роль CMManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='PriceLists' AND [Action] IN ('GetPriceLists', 'GetPriceList', 'ExportXLSX');