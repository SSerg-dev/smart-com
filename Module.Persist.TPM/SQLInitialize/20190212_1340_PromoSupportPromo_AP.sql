-- точки доступа
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoSupportPromoes', 'GetPromoSupportPromo', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoSupportPromoes', 'GetPromoSupportPromoes', 0, NULL);

-- роли
-- избавляемся от дуближа
DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE Resource in ('PromoSupportPromoes'));

--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator'), Id FROM ACCESSPOINT WHERE Resource in ('PromoSupportPromoes');