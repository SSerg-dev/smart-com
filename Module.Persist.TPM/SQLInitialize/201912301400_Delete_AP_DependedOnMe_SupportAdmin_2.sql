
 DELETE FROM AccessPointRole
	WHERE RoleId IN (SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false')
	AND AccessPointId IN (SELECT ID FROM [AccessPoint]
	Where [Resource] = 'PromoGridViews' AND [Action] = 'GetCanChangeStatePromoGridViews' AND [Disabled] = 'false')
GO