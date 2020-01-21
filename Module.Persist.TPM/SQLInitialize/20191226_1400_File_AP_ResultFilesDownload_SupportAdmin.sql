--Роли
--SA
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='File' AND [Action] IN ('DataLakeSyncResultSuccessDownload', 'DataLakeSyncResultWarningDownload', 'DataLakeSyncResultErrorDownload');
