
DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource]='Products' AND [Action]='Delete') and RoleId in (select Id from Role where SystemName = 'DemandPlanning' and Disabled = 0);

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='Products' AND [Action]='Delete' AND [Disabled] = 0
