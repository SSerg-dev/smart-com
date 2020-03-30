DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource]='ClientDashboardViews' AND [Action] IN ('Patch', 'Update'));
DELETE FROM AccessPoint WHERE [Resource]='ClientDashboardViews' AND [Action] IN ('Patch', 'Update');

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ClientDashboardViews', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ClientDashboardViews', 'Update', 0, NULL);

--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] = 'ClientDashboardViews' AND [Action] IN ('Patch', 'Update');

--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] = 'ClientDashboardViews' AND [Action] IN ('Patch', 'Update');

-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] = 'ClientDashboardViews' AND [Action] IN ('Patch', 'Update');

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] = 'ClientDashboardViews' AND [Action] IN ('Patch', 'Update');
