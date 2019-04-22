-- точки доступа
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('Promoes', 'ReadPromoCalculatingLog', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('Promoes', 'CheckPromoCalculatingStatus', 0, NULL);

--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' and [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='Promoes' AND [Action] IN ('ReadPromoCalculatingLog', 'CheckPromoCalculatingStatus');


--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert'), Id FROM ACCESSPOINT WHERE [Resource]='Promoes' AND [Action] IN ('ReadPromoCalculatingLog', 'CheckPromoCalculatingStatus');
-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing'), Id FROM ACCESSPOINT WHERE [Resource]='Promoes' AND [Action] IN ('ReadPromoCalculatingLog', 'CheckPromoCalculatingStatus');

-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager'), Id FROM ACCESSPOINT WHERE [Resource]='Promoes' AND [Action] IN ('ReadPromoCalculatingLog', 'CheckPromoCalculatingStatus');

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance'), Id FROM ACCESSPOINT WHERE [Resource]='Promoes' AND [Action] IN ('ReadPromoCalculatingLog', 'CheckPromoCalculatingStatus');

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning'), Id FROM ACCESSPOINT WHERE [Resource]='Promoes' AND [Action] IN ('ReadPromoCalculatingLog', 'CheckPromoCalculatingStatus');