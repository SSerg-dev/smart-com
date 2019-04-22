-- точки доступа
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoSupports', 'GetUserTimestamp', 0, NULL);

DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource]='PromoSupports' AND [Action]='GetUserTimestamp');

--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' and [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='PromoSupports' AND [Action]='GetUserTimestamp';

--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert'), Id FROM ACCESSPOINT WHERE [Resource]='PromoSupports' AND [Action]='GetUserTimestamp';
-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing'), Id FROM ACCESSPOINT WHERE [Resource]='PromoSupports' AND [Action]='GetUserTimestamp';

-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager'), Id FROM ACCESSPOINT WHERE [Resource]='PromoSupports' AND [Action]='GetUserTimestamp';

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance'), Id FROM ACCESSPOINT WHERE [Resource]='PromoSupports' AND [Action]='GetUserTimestamp';

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning'), Id FROM ACCESSPOINT WHERE [Resource]='PromoSupports' AND [Action]='GetUserTimestamp';