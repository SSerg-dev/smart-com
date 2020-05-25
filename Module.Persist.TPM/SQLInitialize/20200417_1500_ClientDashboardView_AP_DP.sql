DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource]='ClientDashboardViews' AND [Action] IN ('Patch', 'Update') AND 
	RoleId = (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'));

--Demand Planning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] = 'ClientDashboardViews' AND [Action] IN ('Patch', 'Update');
