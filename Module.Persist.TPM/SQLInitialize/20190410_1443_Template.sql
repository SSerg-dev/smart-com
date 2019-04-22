-- точки доступа
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) (SELECT Resource, 'DownloadTemplateXLSX', 0, NULL FROM AccessPoint WHERE [Action] = 'FullImportXLSX' AND Disabled=0);

-- роли
--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Action]='DownloadTemplateXLSX';

-- роли
--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT 
	WHERE [Action]='DownloadTemplateXLSX' 
	AND [Resource] in (SELECT [Resource] FROM ACCESSPOINT WHERE ID IN (		
		SELECT AccessPointId FROM AccessPointRole WHERE RoleId = (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false') AND [Action]='FullImportXLSX'));

--CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT 
	WHERE [Action]='DownloadTemplateXLSX' 
	AND [Resource] in (SELECT [Resource] FROM ACCESSPOINT WHERE ID IN (		
		SELECT AccessPointId FROM AccessPointRole WHERE RoleId = (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false') AND [Action]='FullImportXLSX'));


--DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT 
	WHERE [Action]='DownloadTemplateXLSX' 
	AND [Resource] in (SELECT [Resource] FROM ACCESSPOINT WHERE ID IN (		
		SELECT AccessPointId FROM AccessPointRole WHERE RoleId = (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false') AND [Action]='FullImportXLSX'));

--KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT 
	WHERE [Action]='DownloadTemplateXLSX' 
	AND [Resource] in (SELECT [Resource] FROM ACCESSPOINT WHERE ID IN (		
		SELECT AccessPointId FROM AccessPointRole WHERE RoleId = (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false') AND [Action]='FullImportXLSX'));

--DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT 
	WHERE [Action]='DownloadTemplateXLSX' 
	AND [Resource] in (SELECT [Resource] FROM ACCESSPOINT WHERE ID IN (		
		SELECT AccessPointId FROM AccessPointRole WHERE RoleId = (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false') AND [Action]='FullImportXLSX'));

--SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT 
	WHERE [Action]='DownloadTemplateXLSX' 
	AND [Resource] in (SELECT [Resource] FROM ACCESSPOINT WHERE ID IN (		
		SELECT AccessPointId FROM AccessPointRole WHERE RoleId = (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false') AND [Action]='FullImportXLSX'));
