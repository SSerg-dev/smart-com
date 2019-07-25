-- точки доступа
DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE Resource in ('PromoProducts') AND [Action] IN ('DownloadTemplateXLSXTLC'));
Delete FROM AccessPoint WHERE ([Resource] = 'PromoProducts') AND [Action] IN ('DownloadTemplateXLSXTLC');

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoProducts', 'DownloadTemplateXLSXTLC', 0, NULL);


	INSERT INTO AccessPointRole
	(Id, RoleId, AccessPointId)
	SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'keyaccountmanager' and [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='PromoProducts' AND [Action]='DownloadTemplateXLSXTLC';

