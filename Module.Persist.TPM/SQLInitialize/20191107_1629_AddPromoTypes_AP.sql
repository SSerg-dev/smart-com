
 DELETE FROM AccessPointRole WHERE [AccessPointId] in (SELECT [Id] FROM [AccessPoint] WHERE [Resource] IN ('PromoTypes'))
DELETE FROM [AccessPoint] WHERE [Resource] IN ('PromoTypes')

INSERT INTO [AccessPoint] (Resource, Action, Disabled, DeletedDate) VALUES ('PromoTypes', 'GetPromoType', 0, NULL);
INSERT INTO [AccessPoint] (Resource, Action, Disabled, DeletedDate) VALUES ('PromoTypes', 'GetPromoTypes', 0, NULL);
INSERT INTO [AccessPoint] (Resource, Action, Disabled, DeletedDate) VALUES ('PromoTypes', 'Put', 0, NULL);
INSERT INTO [AccessPoint] (Resource, Action, Disabled, DeletedDate) VALUES ('PromoTypes', 'Post', 0, NULL);
INSERT INTO [AccessPoint] (Resource, Action, Disabled, DeletedDate) VALUES ('PromoTypes', 'Patch', 0, NULL);
INSERT INTO [AccessPoint] (Resource, Action, Disabled, DeletedDate) VALUES ('PromoTypes', 'Delete', 0, NULL);
 INSERT INTO[AccessPoint] (Resource, Action, Disabled, DeletedDate) VALUES('PromoTypes', 'ExportXLSX', 0, NULL);


--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'Administrator' and [Disabled] = 'false'), [Id] FROM [AccessPoint] WHERE [Resource] = ('PromoTypes')

--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'FunctionalExpert' and [Disabled] = 'false'), [Id] FROM [AccessPoint] WHERE [Resource] = ('PromoTypes')

-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), 
(SELECT [Id] FROM [Role] Where [SystemName] = 'CustomerMarketing' and [Disabled] = 'false'), 
[Id] FROM [AccessPoint] WHERE [Resource] = ('PromoTypes')  

-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'KeyAccountManager' and [Disabled] = 'false'), 
[Id] FROM [AccessPoint] WHERE [Resource] = ('PromoTypes') and [Action] not in ('FullImportXLSX', 'DownloadTemplateXLSX')

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'DemandFinance' and [Disabled] = 'false'), 
[Id] FROM [AccessPoint] WHERE [Resource] = ('PromoTypes') and [Action] not in ('FullImportXLSX', 'DownloadTemplateXLSX')

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'DemandPlanning' and [Disabled] = 'false'), [Id] FROM [AccessPoint] WHERE [Resource] = ('PromoTypes')

-- SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'SuperReader' and [Disabled] = 'false'), 
[Id] FROM [AccessPoint] WHERE [Resource] = ('PromoTypes') and [Action] not in ('FullImportXLSX', 'DownloadTemplateXLSX')

-- CMManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'CMManager' and [Disabled] = 'false'), 
[Id] FROM [AccessPoint] WHERE [Resource] IN ('PromoTypes') and [Action] not in ('FullImportXLSX', 'DownloadTemplateXLSX')

GO