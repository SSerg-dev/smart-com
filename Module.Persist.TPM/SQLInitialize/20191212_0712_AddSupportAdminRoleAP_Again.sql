--AP
DELETE FROM [AccessPointRole] WHERE RoleId = (SELECT Id FROM [Role] WHERE SystemName = 'SupportAdministrator' AND Disabled = 0)

INSERT INTO [AccessPointRole]
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT Id FROM [Role] WHERE SystemName = 'SupportAdministrator' AND Disabled = 0), Id FROM [AccessPoint]