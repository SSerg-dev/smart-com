-- избавляемся от дублей для данной роли
delete FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM ACCESSPOINT WHERE [Resource] IN ('CoefficientSI2SOs', 'HistoricalCoefficientSI2SOs', 'DeletedCoefficientSI2SOs') AND [Action] IN ('GetCoefficientSI2SOs', 'GetCoefficientSI2SO', 'ExportXLSX', 'Post', 'Put', 'Patch', 'Delete', 'FullImportXLSX', 'DownloadTemplateXLSX', 'GetHistoricalCoefficientSI2SOs', 'GetDeletedCoefficientSI2SOs') AND RoleId = (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'));

-- роль CMManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('CoefficientSI2SOs') AND [Action] IN ('GetCoefficientSI2SOs', 'GetCoefficientSI2SO', 'ExportXLSX');
--Deleted
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedCoefficientSI2SOs' AND [Action] IN ('GetDeletedCoefficientSI2SOs');
--Historical
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalCoefficientSI2SOs' AND [Action] IN ('GetHistoricalCoefficientSI2SOs');
