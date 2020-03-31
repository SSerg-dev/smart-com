--SuperReader
DELETE FROM AccessPointRole 
WHERE 
	RoleId = (SELECT ID FROM [Role] WHERE SystemName = 'SuperReader' AND [Disabled] = 'false')
	AND AccessPointId IN (SELECT Id FROM ACCESSPOINT WHERE [Resource] = 'NonPromoSupportBrandTeches' AND [Action] IN ('Put', 'Post', 'Delete', 'ModifyNonPromoSupportBrandTechList'));