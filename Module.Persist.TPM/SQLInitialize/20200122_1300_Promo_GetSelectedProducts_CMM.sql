DECLARE @role_name VARCHAR(255) = 'CMManager';
DECLARE @resource_name VARCHAR(255) = 'Products';
DECLARE @action_name VARCHAR(255) = 'GetSelectedProducts';

DELETE AccessPointRole 
WHERE RoleId = (SELECT ID FROM [Role] Where SystemName = @role_name AND [Disabled] = 'false') AND 
	AccessPointId = (SELECT Id FROM ACCESSPOINT WHERE [Resource]= @resource_name AND [Action]= @action_name AND [Disabled] = 'false');

INSERT INTO AccessPointRole (Id, RoleId, AccessPointId) 
	VALUES(NEWID(), (SELECT ID FROM [Role] Where SystemName = @role_name AND [Disabled] = 'false'), (SELECT Id FROM ACCESSPOINT WHERE [Resource]= @resource_name AND [Action]= @action_name AND [Disabled] = 'false'));