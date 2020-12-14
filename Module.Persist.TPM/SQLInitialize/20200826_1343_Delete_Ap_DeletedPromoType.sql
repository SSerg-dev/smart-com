
DELETE FROM AccessPointRole
	where AccessPointId IN (SELECT ID FROM [AccessPoint] Where [Resource] = 'DeletedPromoTypes' AND [Action] = 'GetDeletedPromoTypes' AND [Disabled] = 'false')
GO