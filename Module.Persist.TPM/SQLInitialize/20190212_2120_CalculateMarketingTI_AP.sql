-- точки доступа
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('Promoes', 'CalculateMarketingTI', 0, NULL);

--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator'), Id FROM ACCESSPOINT WHERE [Resource]='Promoes' AND [Action]='CalculateMarketingTI';