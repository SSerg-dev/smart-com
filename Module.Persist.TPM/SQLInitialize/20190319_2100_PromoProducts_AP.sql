-- точки доступа
-- избавляемся от дуближа
DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE Resource in ('PromoProducts', 'DeletedPromoProducts', 'HistoricalPromoProducts'));
Delete FROM AccessPoint WHERE ([Resource] = 'PromoProducts' OR [Resource] = 'DeletedPromoProducts' OR [Resource] = 'HistoricalPromoProducts') AND [Disabled] = 'false';

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoProducts', 'GetPromoProduct', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoProducts', 'GetPromoProducts', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoProducts', 'Put', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoProducts', 'Post', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoProducts', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoProducts', 'Delete', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoProducts', 'ExportXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoProducts', 'FullImportXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('DeletedPromoProducts', 'GetDeletedPromoProduct', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('DeletedPromoProducts', 'GetDeletedPromoProducts', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('HistoricalPromoProducts', 'GetHistoricalPromoProducts', 0, NULL);

-- роли
--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE Resource in ('PromoProducts', 'DeletedPromoProducts', 'HistoricalPromoProducts');

--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert'), Id FROM ACCESSPOINT WHERE Resource in ('PromoProducts', 'DeletedPromoProducts', 'HistoricalPromoProducts');

-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing'), Id FROM ACCESSPOINT WHERE Resource  in ('PromoProducts', 'DeletedPromoProducts', 'HistoricalPromoProducts');

-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager'), Id FROM ACCESSPOINT WHERE Resource  in ('PromoProducts', 'DeletedPromoProducts', 'HistoricalPromoProducts');

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance'), Id FROM ACCESSPOINT WHERE Resource  in ('PromoProducts', 'DeletedPromoProducts', 'HistoricalPromoProducts');

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning'), Id FROM ACCESSPOINT WHERE Resource in ('PromoProducts', 'DeletedPromoProducts', 'HistoricalPromoProducts');