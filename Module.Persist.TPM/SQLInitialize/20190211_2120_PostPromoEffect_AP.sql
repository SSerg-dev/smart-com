-- точки доступа
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PostPromoEffects', 'GetPostPromoEffect', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PostPromoEffects', 'GetPostPromoEffects', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('DeletedPostPromoEffects', 'GetDeletedPostPromoEffect', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('DeletedPostPromoEffects', 'GetDeletedPostPromoEffects', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('HistoricalPostPromoEffects', 'GetHistoricalPostPromoEffects', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PostPromoEffects', 'Put', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ImportPostPromoEffects', 'Put', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PostPromoEffects', 'Post', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ImportPostPromoEffects', 'Post', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PostPromoEffects', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ImportPostPromoEffects', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PostPromoEffects', 'Delete', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ImportPostPromoEffects', 'Delete', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PostPromoEffects', 'ExportXLSX', 0, NULL);

-- роли
-- избавляемся от дуближа
DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE Resource in ('PostPromoEffects','DeletedPostPromoEffects', 'HistoricalPostPromoEffects'));

--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator'), Id FROM ACCESSPOINT WHERE Resource in ('PostPromoEffects','DeletedPostPromoEffects', 'HistoricalPostPromoEffects');

--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert'), Id FROM ACCESSPOINT WHERE Resource in ('PostPromoEffects','DeletedPostPromoEffects',  'HistoricalPostPromoEffects');

-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing'), Id FROM ACCESSPOINT WHERE Resource  in ('PostPromoEffects','DeletedPostPromoEffects', 'HistoricalPostPromoEffects')
and ([Action] like 'Get%' or [Action] ='ExportXLSX');

-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager'), Id FROM ACCESSPOINT WHERE Resource  in ('PostPromoEffects','DeletedPostPromoEffects', 'HistoricalPostPromoEffects')
and ([Action] like 'Get%' or [Action] ='ExportXLSX');

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance'), Id FROM ACCESSPOINT WHERE Resource  in ('PostPromoEffects','DeletedPostPromoEffects', 'HistoricalPostPromoEffects')
and ([Action] like 'Get%' or [Action] ='ExportXLSX');

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning'), Id FROM ACCESSPOINT WHERE Resource in ('PostPromoEffects','DeletedPostPromoEffects', 'HistoricalPostPromoEffects')
and ([Action] like 'Get%' or [Action] ='ExportXLSX');