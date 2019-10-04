DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource] IN ('EventClientTrees'))
DELETE FROM AccessPoint WHERE [Resource] IN ('EventClientTrees')

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('EventClientTrees', 'GetEventClientTree', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('EventClientTrees', 'GetEventClientTrees', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('EventClientTrees', 'Post', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('EventClientTrees', 'Put', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('EventClientTrees', 'Delete', 0, NULL);

--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('EventClientTrees')

--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('EventClientTrees')
-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('EventClientTrees')

-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('EventClientTrees')

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('EventClientTrees')

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('EventClientTrees')

-- SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('EventClientTrees')
