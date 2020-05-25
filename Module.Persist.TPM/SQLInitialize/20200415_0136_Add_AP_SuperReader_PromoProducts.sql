-- точки доступа
DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE Resource in ('PromoProducts') AND [Action] IN ('GetPromoProducts')) and AccessPointRole.RoleId in (SELECT ID FROM [Role] Where SystemName = 'SuperReader' and [Disabled] = 'false');

INSERT INTO AccessPointRole (Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' and [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='PromoProducts' AND [Action]='GetPromoProducts';
