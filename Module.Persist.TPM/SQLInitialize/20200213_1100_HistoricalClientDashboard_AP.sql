DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource]='HistoricalClientDashboards' AND [Action] IN ('GetHistoricalClientDashboards'));
DELETE FROM AccessPoint WHERE [Resource]='HistoricalClientDashboards' AND [Action] IN ('GetHistoricalClientDashboards');

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('HistoricalClientDashboards', 'GetHistoricalClientDashboards', 0, NULL);

--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] = 'HistoricalClientDashboards' AND [Action] IN ('GetHistoricalClientDashboards');

--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] = 'HistoricalClientDashboards' AND [Action] IN ('GetHistoricalClientDashboards');

-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] = 'HistoricalClientDashboards' AND [Action] IN ('GetHistoricalClientDashboards');

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] = 'HistoricalClientDashboards' AND [Action] IN ('GetHistoricalClientDashboards');

-- FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] = 'HistoricalClientDashboards' AND [Action] IN ('GetHistoricalClientDashboards');

-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] = 'HistoricalClientDashboards' AND [Action] IN ('GetHistoricalClientDashboards');

-- CMManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] = 'HistoricalClientDashboards' AND [Action] IN ('GetHistoricalClientDashboards');

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] = 'HistoricalClientDashboards' AND [Action] IN ('GetHistoricalClientDashboards');

-- SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] = 'HistoricalClientDashboards' AND [Action] IN ('GetHistoricalClientDashboards');
