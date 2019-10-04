DELETE FROM AccessPointRole
	WHERE RoleId IN (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false')
	AND AccessPointId IN (SELECT ID FROM [AccessPoint]
	Where [Resource] = 'DeletedPromoSupports' AND [Action] = 'GetDeletedPromoSupports' AND [Disabled] = 'false')
GO
