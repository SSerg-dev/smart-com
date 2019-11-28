DELETE FROM AccessPointRole WHERE [AccessPointId] in (SELECT [Id] FROM [AccessPoint] WHERE [Resource] IN ('PromoProductsViews'))
DELETE FROM [AccessPoint] WHERE [Resource] IN ('PromoProductsViews')

INSERT INTO [AccessPoint] (Resource, Action, Disabled, DeletedDate) VALUES ('PromoProductsViews', 'GetPromoProductsView', 0, NULL);
INSERT INTO [AccessPoint] (Resource, Action, Disabled, DeletedDate) VALUES ('PromoProductsViews', 'GetPromoProductsViews', 0, NULL);
INSERT INTO [AccessPoint] (Resource, Action, Disabled, DeletedDate) VALUES ('PromoProductsViews', 'Put', 0, NULL);
INSERT INTO [AccessPoint] (Resource, Action, Disabled, DeletedDate) VALUES ('PromoProductsViews', 'Patch', 0, NULL);
INSERT INTO [AccessPoint] (Resource, Action, Disabled, DeletedDate) VALUES ('PromoProductsViews', 'ExportXLSX', 0, NULL);
INSERT INTO [AccessPoint] (Resource, Action, Disabled, DeletedDate) VALUES ('PromoProductsViews', 'FullImportXLSX', 0, NULL);
INSERT INTO [AccessPoint] (Resource, Action, Disabled, DeletedDate) VALUES ('PromoProductsViews', 'DownloadTemplateXLSX', 0, NULL);

--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'Administrator' and [Disabled] = 'false'), [Id] FROM [AccessPoint] WHERE [Resource] = ('PromoProductsViews')

--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'FunctionalExpert' and [Disabled] = 'false'), [Id] FROM [AccessPoint] WHERE [Resource] = ('PromoProductsViews')

-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), 
(SELECT [Id] FROM [Role] Where [SystemName] = 'CustomerMarketing' and [Disabled] = 'false'), 
[Id] FROM [AccessPoint] WHERE [Resource] = ('PromoProductsViews') and [Action] not in ('FullImportXLSX', 'DownloadTemplateXLSX')

-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'KeyAccountManager' and [Disabled] = 'false'), 
[Id] FROM [AccessPoint] WHERE [Resource] = ('PromoProductsViews') and [Action] not in ('FullImportXLSX', 'DownloadTemplateXLSX')

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'DemandFinance' and [Disabled] = 'false'), 
[Id] FROM [AccessPoint] WHERE [Resource] = ('PromoProductsViews') and [Action] not in ('FullImportXLSX', 'DownloadTemplateXLSX')

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'DemandPlanning' and [Disabled] = 'false'), [Id] FROM [AccessPoint] WHERE [Resource] = ('PromoProductsViews')

-- SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'SuperReader' and [Disabled] = 'false'), 
[Id] FROM [AccessPoint] WHERE [Resource] = ('PromoProductsViews') and [Action] not in ('FullImportXLSX', 'DownloadTemplateXLSX')

-- CMManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'CMManager' and [Disabled] = 'false'), 
[Id] FROM [AccessPoint] WHERE [Resource] IN ('PromoProductsViews') and [Action] not in ('FullImportXLSX', 'DownloadTemplateXLSX')
