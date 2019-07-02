DECLARE @CMManagerRoleId UNIQUEIDENTIFIER;
SELECT @CMManagerRoleId = Id FROM [Role] WHERE LOWER([SystemName]) = 'CMManager' AND [Disabled] = 0;

INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(),  @CMManagerRoleId, Id FROM ACCESSPOINT WHERE Resource + ':' + [Action] in ('BaseLines:ExportXLSX');