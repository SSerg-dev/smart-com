/****** Script for SelectTopNRows command from SSMS  ******/

 DELETE FROM AccessPointRole WHERE [AccessPointId] in (SELECT [Id] FROM [AccessPoint] WHERE [Resource] IN ('PromoTypes'))
DELETE FROM [AccessPoint] WHERE [Resource] IN ('PromoTypes')

INSERT INTO [AccessPoint] (Resource, Action, Disabled, DeletedDate) VALUES ('PromoTypes', 'GetPromoType', 0, NULL);
INSERT INTO [AccessPoint] (Resource, Action, Disabled, DeletedDate) VALUES ('PromoTypes', 'GetPromoTypes', 0, NULL);
 


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
[Id] FROM [AccessPoint] WHERE [Resource] = ('PromoTypes')  

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'DemandFinance' and [Disabled] = 'false'), 
[Id] FROM [AccessPoint] WHERE [Resource] = ('PromoTypes')  

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'DemandPlanning' and [Disabled] = 'false'), [Id] FROM [AccessPoint] WHERE [Resource] = ('PromoTypes')

-- SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'SuperReader' and [Disabled] = 'false'), 
[Id] FROM [AccessPoint] WHERE [Resource] = ('PromoTypes')  

-- CMManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] Where [SystemName] = 'CMManager' and [Disabled] = 'false'), 
[Id] FROM [AccessPoint] WHERE [Resource] IN ('PromoTypes')  

GO