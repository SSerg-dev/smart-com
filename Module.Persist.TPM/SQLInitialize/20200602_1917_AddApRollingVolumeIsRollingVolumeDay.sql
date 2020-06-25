-- избавляемся от дублей для данной роли
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM ACCESSPOINT WHERE [Resource] IN ('RollingVolumes') AND [Action] IN ('IsRollingVolumeDay') )

DELETE FROM AccessPoint WHERE [Resource] IN ('RollingVolumes') AND [Action] IN ('IsRollingVolumeDay')

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('RollingVolumes', 'IsRollingVolumeDay', 0, NULL);
--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='RollingVolumes' AND [Action] IN ('IsRollingVolumeDay');

--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='RollingVolumes' AND [Action] IN ('IsRollingVolumeDay');

-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='RollingVolumes' AND [Action] IN ('IsRollingVolumeDay');

-- Customer Marketing Manager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='RollingVolumes' AND [Action] IN ('IsRollingVolumeDay');

-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='RollingVolumes' AND [Action] IN ('IsRollingVolumeDay');

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='RollingVolumes' AND [Action] IN ('IsRollingVolumeDay');

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='RollingVolumes' AND [Action] IN ('IsRollingVolumeDay');

-- SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='RollingVolumes' AND [Action] IN ('IsRollingVolumeDay');

-- SupportAdministrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='RollingVolumes' AND [Action] IN ('IsRollingVolumeDay');
