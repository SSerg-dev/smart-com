-- точки доступа
-- избавляемся от дублей
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM ACCESSPOINT WHERE [Resource] IN ('HistoricalClientTreeBrandTeches') AND [Action] IN ('GetHistoricalClientTreeBrandTeches', 'GetFilteredData'));
DELETE FROM AccessPoint WHERE [Resource] IN ('HistoricalClientTreeBrandTeches') AND [Action] IN ('GetHistoricalClientTreeBrandTeches', 'GetFilteredData') AND [Disabled] = 'false';

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('HistoricalClientTreeBrandTeches', 'GetHistoricalClientTreeBrandTeches', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('HistoricalClientTreeBrandTeches', 'GetFilteredData', 0, NULL);


-- роли
--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalClientTreeBrandTeches' AND [Action] IN ('GetHistoricalClientTreeBrandTeches', 'GetFilteredData');

--CMManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalClientTreeBrandTeches' AND [Action] IN ('GetHistoricalClientTreeBrandTeches', 'GetFilteredData');

-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalClientTreeBrandTeches' AND [Action] IN ('GetHistoricalClientTreeBrandTeches', 'GetFilteredData');

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalClientTreeBrandTeches' AND [Action] IN ('GetHistoricalClientTreeBrandTeches', 'GetFilteredData'); 

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalClientTreeBrandTeches' AND [Action] IN ('GetHistoricalClientTreeBrandTeches', 'GetFilteredData'); 

-- FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalClientTreeBrandTeches' AND [Action] IN ('GetHistoricalClientTreeBrandTeches', 'GetFilteredData'); 

-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalClientTreeBrandTeches' AND [Action] IN ('GetHistoricalClientTreeBrandTeches', 'GetFilteredData'); 

-- SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalClientTreeBrandTeches' AND [Action] IN ('GetHistoricalClientTreeBrandTeches', 'GetFilteredData'); 

-- SupportAdministrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalClientTreeBrandTeches' AND [Action] IN ('GetHistoricalClientTreeBrandTeches', 'GetFilteredData'); 
