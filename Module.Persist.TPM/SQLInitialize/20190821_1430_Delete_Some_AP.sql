DELETE FROM AccessPointRole
	WHERE RoleId IN (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false')
	AND AccessPointId IN (SELECT ID FROM [AccessPoint] Where [Resource] = 'Products' AND [Action] = 'FullImportXLSX' AND [Disabled] = 'false')
GO

DELETE FROM AccessPointRole
	WHERE RoleId IN (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false')
	AND AccessPointId IN (SELECT ID FROM [AccessPoint] Where [Resource] = 'PromoStatuss' AND [Action] = 'FullImportXLSX' AND [Disabled] = 'false')
GO

DELETE FROM AccessPointRole
	WHERE RoleId IN (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false')
	AND AccessPointId IN (SELECT ID FROM [AccessPoint] Where [Resource] = 'Promoes' AND [Action] = 'Post' AND [Disabled] = 'false')
GO