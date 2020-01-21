--Role
DELETE FROM [Role] WHERE SystemName = 'SupportAdministrator'

INSERT INTO [Role]
(Id, Disabled, DeletedDate, SystemName, DisplayName, IsAllow)
VALUES(NEWID(), 0, NULL, 'SupportAdministrator', 'Support Administrator', 1);

--AP
DELETE FROM [AccessPointRole] WHERE RoleId = (SELECT Id FROM [Role] WHERE SystemName = 'SupportAdministrator' AND Disabled = 0)

INSERT INTO [AccessPointRole]
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT Id FROM [Role] WHERE SystemName = 'SupportAdministrator' AND Disabled = 0), Id FROM [AccessPoint]