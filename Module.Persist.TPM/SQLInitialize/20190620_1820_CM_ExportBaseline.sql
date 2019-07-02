DECLARE @CustomerMarketingRoleId UNIQUEIDENTIFIER;
SELECT @CustomerMarketingRoleId = Id FROM [Role] WHERE LOWER([SystemName]) = 'CustomerMarketing' AND [Disabled] = 0;

INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(),  @CustomerMarketingRoleId, Id FROM ACCESSPOINT WHERE Resource + ':' + [Action] in ('BaseLines:ExportXLSX');