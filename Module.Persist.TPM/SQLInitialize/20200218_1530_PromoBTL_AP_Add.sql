-- избавляемся от дублей
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM ACCESSPOINT WHERE [Resource] IN ('BTLPromoes') AND [Action] IN ('GetPromoesWithBTL'));
DELETE FROM AccessPoint WHERE [Resource] IN ('BTLPromoes') AND [Action] IN ('GetPromoesWithBTL') AND [Disabled] = 'false';

--Точка доступа
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('BTLPromoes', 'GetPromoesWithBTL', 0, NULL);

--Роли
--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('BTLPromoes') AND [Action] = 'GetPromoesWithBTL'; 

--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('BTLPromoes') AND [Action] = 'GetPromoesWithBTL'; 

-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('BTLPromoes') AND [Action] = 'GetPromoesWithBTL'; 

-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('BTLPromoes') AND [Action] = 'GetPromoesWithBTL'; 

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('BTLPromoes') AND [Action] = 'GetPromoesWithBTL'; 

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('BTLPromoes') AND [Action] = 'GetPromoesWithBTL'; 

-- SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('BTLPromoes') AND [Action] = 'GetPromoesWithBTL'; 

-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('BTLPromoes') AND [Action] = 'GetPromoesWithBTL'; 

-- SupportAdministrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('BTLPromoes') AND [Action] = 'GetPromoesWithBTL'; 

GO