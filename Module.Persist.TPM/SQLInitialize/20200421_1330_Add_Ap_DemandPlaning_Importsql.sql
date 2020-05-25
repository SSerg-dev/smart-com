 
-- избавляемся от дублей для данной роли
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM ACCESSPOINT WHERE [Resource] IN ('ClientDashboardViews') AND [Action] IN ('FullImportXLSX', 'DownloadTemplateXLSX') AND RoleId = (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'));

-- роль DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ClientDashboardViews' AND [Action] IN ('FullImportXLSX', 'DownloadTemplateXLSX');
  