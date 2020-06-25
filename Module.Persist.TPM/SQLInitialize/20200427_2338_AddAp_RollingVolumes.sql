

DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource] IN ('RollingVolumes'))
DELETE FROM AccessPoint WHERE [Resource] IN ('RollingVolumes')

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('RollingVolumes', 'GetRollingVolumes', 0, NULL);

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('RollingVolumes', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('RollingVolumes', 'ExportXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('RollingVolumes', 'GetFilteredData', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('RollingVolumes', 'FullImportXLSX', 0, NULL);
--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('RollingVolumes')
 
-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('RollingVolumes')

 -- SupportAdministrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('RollingVolumes')

 