DELETE FROM AccessPointRole
	WHERE RoleId IN (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false')
	AND AccessPointId IN (SELECT ID FROM [AccessPoint] Where [Resource] = 'PromoStatuss' AND [Action] = 'DownloadTemplateXLSX' AND [Disabled] = 'false')
GO

DELETE FROM AccessPointRole
	WHERE RoleId IN (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false')
	AND AccessPointId IN (SELECT ID FROM [AccessPoint] Where [Resource] = 'PromoStatuss' AND [Action] = 'ExportXLSX' AND [Disabled] = 'false')
GO

INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' and [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE ([Resource] = 'PromoStatuss' AND [Action] = 'ExportXLSX' AND [Disabled] = 0);
GO