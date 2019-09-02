-- точки доступа
-- избавляемся от дублей
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM ACCESSPOINT WHERE [Resource]='Products' AND [Action] IN ('Post', 'Patch', 'FullImportXLSX')) 
AND RoleId = (SELECT Id FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false');

-- роли
-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT Id FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='Products' AND [Action] IN ('Post', 'Patch', 'FullImportXLSX');