DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource]='Security' AND [Action]='SaveGridSettings');

--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' and [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='Security' AND [Action]='SaveGridSettings'

--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' and [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='Security' AND [Action]='SaveGridSettings';
-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' and [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='Security' AND [Action]='SaveGridSettings';

-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' and [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='Security' AND [Action]='SaveGridSettings';

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' and [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='Security' AND [Action]='SaveGridSettings';

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' and [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='Security' AND [Action]='SaveGridSettings';

-- Super Reader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' and [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='Security' AND [Action]='SaveGridSettings';