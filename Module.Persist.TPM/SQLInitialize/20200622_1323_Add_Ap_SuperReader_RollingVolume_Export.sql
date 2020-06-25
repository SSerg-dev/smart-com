-- избавляемся от дублей для данной роли
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM ACCESSPOINT WHERE [Resource] IN ('RollingVolumes')) and RoleId IN (select Id from [Role] where SystemName = 'SuperReader')
--SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('RollingVolumes') and [Action] IN('GetRollingVolumes')
 INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('RollingVolumes') and [Action] IN('ExportXLSX')
 