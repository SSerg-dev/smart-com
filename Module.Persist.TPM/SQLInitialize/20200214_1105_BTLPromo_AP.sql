-- точки доступа
-- избавляемся от дублей
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM ACCESSPOINT WHERE [Resource] IN ('BTLPromoes') AND [Action] IN ('GetBTLPromoes', 'GetBTLPromo', 'BTLPromoPost', 'Patch', 'Put', 'Post', 'Delete'));
DELETE FROM AccessPoint WHERE [Resource] IN ('BTLPromoes') AND [Action] IN ('GetBTLPromoes', 'GetBTLPromo', 'BTLPromoPost', 'Patch', 'Put', 'Post', 'Delete') AND [Disabled] = 'false';

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('BTLPromoes', 'GetBTLPromoes', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('BTLPromoes', 'GetBTLPromo', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('BTLPromoes', 'BTLPromoPost', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('BTLPromoes', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('BTLPromoes', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('BTLPromoes', 'Put', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('BTLPromoes', 'Post', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('BTLPromoes', 'Delete', 0, NULL);

-- роли
--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='BTLPromoes' AND [Action] IN ('GetBTLPromoes', 'GetBTLPromo', 'BTLPromoPost', 'Patch', 'Put', 'Post', 'Delete');

--Support Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='BTLPromoes' AND [Action] IN ('GetBTLPromoes', 'GetBTLPromo', 'BTLPromoPost', 'Patch', 'Put', 'Post', 'Delete');

--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='BTLPromoes' AND [Action] IN ('GetBTLPromoes', 'GetBTLPromo', 'BTLPromoPost', 'Patch', 'Put', 'Post', 'Delete');

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='BTLPromoes' AND [Action] IN ('GetBTLPromoes', 'GetBTLPromoe', 'BTLPromoPost', 'Patch', 'Put', 'Post', 'Delete');


-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='BTLPromoes' AND [Action] IN ('GetBTLPromoes', 'GetBTLPromo'); 

-- CMManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='BTLPromoes' AND [Action] IN ('GetBTLPromoes', 'GetBTLPromo'); 


-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='BTLPromoes' AND [Action] IN ('GetBTLPromoes', 'GetBTLPromo'); 


-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='BTLPromoes' AND [Action] IN ('GetBTLPromoes', 'GetBTLPromo'); 


-- SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='BTLPromoes' AND [Action] IN ('GetBTLPromoes', 'GetBTLPromo'); 
