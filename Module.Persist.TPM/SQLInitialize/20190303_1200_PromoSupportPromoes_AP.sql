-- точки доступа
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoSupportPromoes', 'GetLinkedSubItems', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoSupportPromoes', 'ManageSubItems', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoSupportPromoes', 'GetValuesForItems', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoSupportPromoes', 'PromoSuportPromoPost', 0, NULL);

DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource]='PromoSupportPromoes' AND ([Action]='GetLinkedSubItems' OR [Action]='ManageSubItems' OR [Action]='GetValuesForItems' OR [Action]='PromoSuportPromoPost'));

--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' and [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='PromoSupportPromoes' AND ([Action]='GetLinkedSubItems' OR [Action]='ManageSubItems' OR [Action]='GetValuesForItems' OR [Action]='PromoSuportPromoPost');

-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager'), Id FROM ACCESSPOINT WHERE [Resource]='PromoSupportPromoes' AND ([Action]='GetLinkedSubItems' OR [Action]='ManageSubItems' OR [Action]='GetValuesForItems' OR [Action]='PromoSuportPromoPost');

-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing'), Id FROM ACCESSPOINT WHERE [Resource]='PromoSupportPromoes' AND ([Action]='GetLinkedSubItems' OR [Action]='ManageSubItems' OR [Action]='GetValuesForItems' OR [Action]='PromoSuportPromoPost');

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance'), Id FROM ACCESSPOINT WHERE [Resource]='PromoSupportPromoes' AND ([Action]='GetLinkedSubItems' OR [Action]='ManageSubItems' OR [Action]='GetValuesForItems' OR [Action]='PromoSuportPromoPost');

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning'), Id FROM ACCESSPOINT WHERE [Resource]='PromoSupportPromoes' AND ([Action]='GetLinkedSubItems' OR [Action]='ManageSubItems' OR [Action]='GetValuesForItems' OR [Action]='PromoSuportPromoPost');